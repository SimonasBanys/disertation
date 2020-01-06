using Lidgren.Network;
using System.Threading;
using System;
using System.IO;
using System.Net;
using ProtoMessageClient;
using ProtoBuf;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
//using Assets.Scenes;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scenes
{
    public class ConcurrentQueueWithEvent<T> : System.Collections.Concurrent.ConcurrentQueue<T>
    {
        public EventWaitHandle eventWaiter { get; set; }

        public ConcurrentQueueWithEvent() : base()
        {
            eventWaiter = new EventWaitHandle(false, EventResetMode.AutoReset);
        }
        public new void Enqueue(T t)
        {
            base.Enqueue(t);
            eventWaiter.Set();
        }
    }
    class GameOptions
    {
        private NetConnection connection;
        private NetEncryption alg = null;
        public CancellationTokenSource ctSource;
        public bool loggedIn { get; set; }
        public bool autoLogIn { get; set; }
        private byte[] key;
        private GameOptions options;
        private NetClient client;
        public ConcurrentQueueWithEvent<ProtoMessage> protobufMessageQueue;
        public ConcurrentQueueWithEvent<string> stringMessageQueue;
        private string user = "helen";
        private string pass= "potato";
        HashAlgorithm hash = new SHA256Managed();
        public void Send(ProtoMessage message, bool encrypt = true)
        {
            NetOutgoingMessage msg = client.CreateMessage();
            MemoryStream ms = new MemoryStream();
            try
            {
                //Serializer.SerializeWithLengthPrefix<ProtoMessage>(ms, message, ProtoBuf.PrefixStyle.Fixed32);
                Console.Write("Client: Sending");
                msg.Write(ms.GetBuffer());
                if (alg != null && encrypt)
                {
                    Console.Write(" encrypted");
                    msg.Encrypt(alg);
                }
                var result = client.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);
                Console.WriteLine(" message of type " + message.GetType() + " with Action: " + message.ActionType + " with result: " + result.ToString());
                client.FlushSendQueue();

            }
            catch (Exception e)
            {
                Console.WriteLine("CLIENT: Failed to serialise message!");
            }

        }
        public bool ValidateCertificateAndCreateKey(ProtoLogIn login, out byte[] key)
        {
            if (login == null || login.certificate == null)
            {
                key = null;
                return false;
            }
            else
            {
                try
                {
                    // Get certificate
                    X509Certificate2 cert = new X509Certificate2(login.certificate);
                    RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)cert.PublicKey.Key;
#if DEBUG
                    if (this.key != null)
                    {
                        if (this.key.Length == 0)
                        {
                            alg = new NetAESEncryption(client);
                        }
                        else
                        {
                            alg = new NetAESEncryption(client,
                            this.key, 0, this.key.Length);
                        }
                        key = rsa.Encrypt(this.key, false);
                    }
                    else
                    {
                        // If no key, do not use an encryption algorithm
                        alg = null;
                        key = null;
                    }
#else
                        // Create a new symmetric key
                        TripleDES des = TripleDESCryptoServiceProvider.Create();
                        des.GenerateKey();
                        // Encrypt key with server's public key
                        this.key = des.Key;
                        key = rsa.Encrypt(des.Key, false);
                        // Initialise the algoitm
                        alg = new NetAESEncryption(client, des.Key, 0, des.Key.Length);
                        Console.WriteLine("CLIENT: my unencrypted key:");
                        foreach (var bite in des.Key)
                        {
                            Console.Write(bite.ToString());
                        }
#endif
                    // Validate certificate
                    if (!cert.Verify())
                    {
                        X509Chain CertificateChain = new X509Chain();
                        //If you do not provide revokation information, use the following line.
                        CertificateChain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
                        bool IsCertificateChainValid = CertificateChain.Build(cert);
                        if (!IsCertificateChainValid)
                        {
                            for (int i = 0; i < CertificateChain.ChainStatus.Length; i++)
                            {
                            }
                            // TODO change to false after testing
                            return true;
                        }

                    }
                    // temporary certificate validation fix
                    return true;
                    //return cert.Verify();
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine("A problem occurred when parsing certificate from bytes: \n" + "type: " + e.GetType().FullName + "\n " + ", source: " + e.Source + "\n message: " + e.Message);
                    key = null;
                    return false;
                }
            }
        }

        public void read()
        {
            Application.Quit();
            while (client.Status == NetPeerStatus.Running && !ctSource.Token.IsCancellationRequested)
            {
                
                WaitHandle.WaitAny(new WaitHandle[] { client.MessageReceivedEvent, ctSource.Token.WaitHandle });
                NetIncomingMessage im;
                while ((im = client.ReadMessage()) != null && !ctSource.IsCancellationRequested)
                {

                    switch (im.MessageType)
                    {
                        case NetIncomingMessageType.DebugMessage:
                        case NetIncomingMessageType.ErrorMessage:
                        case NetIncomingMessageType.WarningMessage:
                        case NetIncomingMessageType.VerboseDebugMessage:
                        case NetIncomingMessageType.Data:
                            try
                            {
                                if (alg != null)
                                {
                                    im.Decrypt(alg);
                                }
                                MemoryStream ms = new MemoryStream(im.Data);
                                ProtoMessage m = null;
                                try
                                {
                                    m = Serializer.DeserializeWithLengthPrefix<ProtoMessage>(ms, PrefixStyle.Fixed32);

                                }
                                catch (Exception e)
                                {
                                    // Attempt to read string and add to message queue
                                    string s = im.ReadString();
                                    if (!string.IsNullOrEmpty(s))
                                    {
                                        options.stringMessageQueue.Enqueue(s);
                                    }
                                }
                                if (m != null)
                                {
                                    if (m.ResponseType == DisplayMessages.LogInSuccess)
                                    {
                                        loggedIn = true;
                                        options.protobufMessageQueue.Enqueue(m);
                                    }
                                    else
                                    {
                                        if (m.ActionType == Actions.Update)
                                        {
                                            // Don't do anything at the moment for updates
                                        }
                                        else
                                        {
                                            options.protobufMessageQueue.Enqueue(m);
                                            if (m.ActionType == Actions.LogIn && m.ResponseType == DisplayMessages.None)
                                            {
                                                byte[] key = null;
                                                if (ValidateCertificateAndCreateKey(m as ProtoLogIn, out key))
                                                {
                                                    ComputeAndSendHashAndKey(m as ProtoLogIn, key);
                                                }
                                            }
                                            else
                                            {
                                                // Attempt to read string and add to message queue
                                                string s = im.ReadString();
                                                if (!string.IsNullOrEmpty(s))
                                                {

                                                    options.stringMessageQueue.Enqueue(s);

                                                }
                                            }
                                        }

                                    }


                                }
                            }
                            catch (Exception e)
                            {
                                Globals_Server.logError("Error in reading data: " + e.GetType() + " :" + e.Message + "; Stack Trace: " + e.StackTrace);
                            }
                            break;
                        case NetIncomingMessageType.StatusChanged:

                            NetConnectionStatus status = (NetConnectionStatus)im.ReadByte();
                            //MemoryStream ms2 = new MemoryStream(im.SenderConnection.RemoteHailMessage.Data);
                            if (status == NetConnectionStatus.Connected)
                            {

                                if (im.SenderConnection.RemoteHailMessage != null)
                                {
                                    try
                                    {
                                        MemoryStream ms2 = new MemoryStream(im.SenderConnection.RemoteHailMessage.Data);
                                        ProtoMessage m = Serializer.DeserializeWithLengthPrefix<ProtoMessage>(ms2, PrefixStyle.Fixed32);
                                        if (m != null)
                                        {
                                            options.protobufMessageQueue.Enqueue(m);
                                            if (m.ActionType == Actions.LogIn && m.ResponseType == DisplayMessages.None)
                                            {
                                                byte[] key = null;
                                                if (ValidateCertificateAndCreateKey(m as ProtoLogIn, out key))
                                                {
                                                    if (autoLogIn)
                                                    {
                                                        ComputeAndSendHashAndKey(m as ProtoLogIn, key);
                                                    }
                                                }
                                                else
                                                {
                                                    client.Disconnect("Invalid Certificate");
                                                }

                                            }

                                        }
                                    }
                                    catch (Exception e)
                                    {

                                    }
                                }
                                break;
                            }
                            else if (status == NetConnectionStatus.Disconnected)
                            {
                                string reason = im.ReadString();
                                if (!string.IsNullOrEmpty(reason))
                                {

                                    options.stringMessageQueue.Enqueue(reason);



                                }
                            }
                            if (im.SenderConnection.RemoteHailMessage != null && (NetConnectionStatus)im.ReadByte() == NetConnectionStatus.Connected)
                            {

                            }
                            break;
                        case NetIncomingMessageType.ConnectionLatencyUpdated:
                            break;
                        default:
                            break;
                    }
                    client.Recycle(im);
                }
            }
#if DEBUG
            Globals_Server.logEvent("Client listening thread ends");
#endif
        }

        public void ComputeAndSendHashAndKey(ProtoLogIn salts, byte[] key)
        {

            string hashstring = "";
            foreach (byte b in salts.userSalt)
            {
                hashstring += b.ToString();
            }
            string sessSalt = "";
            foreach (byte b in salts.sessionSalt)
            {
                sessSalt += b.ToString();
            }
            byte[] passBytes = Encoding.UTF8.GetBytes(pass);
            byte[] hashPassword = ComputeHash(passBytes, salts.userSalt);
            string passHash = "";
            foreach (byte b in hashPassword)
            {
                passHash += b.ToString();
            }
            byte[] hashFull = ComputeHash(hashPassword, salts.sessionSalt);
            string fullHash = "";
            foreach (byte b in hashFull)
            {
                fullHash += b.ToString();
            }
            var response = new ProtoLogIn
            {
                userSalt = hashFull,
                ActionType = Actions.LogIn,
                Key = key
            };
            Send(response, false);
        }
        public byte[] ComputeHash(byte[] toHash, byte[] salt)
        {
            byte[] fullHash = new byte[toHash.Length + salt.Length];
            toHash.CopyTo(fullHash, 0);
            salt.CopyTo(fullHash, toHash.Length);
            byte[] hashcode = hash.ComputeHash(fullHash);
            return hashcode;
        }
        public ProtoFief FiefDetails(TextTestClient client)
        {
            ProtoPlayerCharacter protoMessage = new ProtoPlayerCharacter();
            protoMessage.Message = "Char_158";
            protoMessage.ActionType = Actions.ViewChar;
            client.net.Send(protoMessage);
            var locReply = GetActionReply(Actions.ViewChar, client);
            var locResult = (ProtoPlayerCharacter)locReply;
            ProtoFief protoFief = new ProtoFief();
            protoFief.Message = locResult.location;
            protoFief.ActionType = Actions.ViewFief;
            client.net.Send(protoFief);
            var reply = GetActionReply(Actions.ViewFief, client);
            return (ProtoFief)reply;
        }
        public ProtoMessage GetActionReply(Actions action, TextTestClient client)
        {
            ProtoMessage responseTask = client.GetReply();
            //Thread t = new Thread(new ThreadStart(client.GetReply));
            //ThreadPool.QueueUserWorkItem(new WaitCallback(client.GetReply));
            //responseTask.Wait();
            while (responseTask.ActionType != action)
            {
                responseTask = client.GetReply();

            }
            client.ClearMessageQueues();
            return responseTask;
        }

        public void Connect(string username, string password)
        {

            client.Start();
            string host = "localhost";
            // remember to encrypt the bloody thing in the final
            //Application.Quit();
            if (username != null)
            {

                NetOutgoingMessage msg = client.CreateMessage(username);
                msg.Write("TestString");
                NetConnection c = client.Connect(host, 8000, msg);
            }
            else
            {
                connection = client.Connect(host, 8000);
            }
            // Start listening for responses

            //Thread t_reader = new Thread(new ThreadStart(this.go.read));
            //t_reader.Start();

        }
    }
}

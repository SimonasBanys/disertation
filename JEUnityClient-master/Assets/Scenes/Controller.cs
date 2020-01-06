
using UnityEngine;
using UnityEngine.UI;
using Lidgren.Network;
using System.Net;
using UnityEngine.SceneManagement;
using ProtoMessageClient;
using System.Threading.Tasks;
using System.Threading;
using System;
//using Assets.Scenes;
public class Controller : MonoBehaviour
{
    //login sence

    //Other value
    protected static TextTestClient tclient;
    protected static ProtoFief mf;
    protected static ProtoGenericArray<ProtoArmyOverview> ma;
    protected static ProtoGenericArray<ProtoDetachment> dl;
    protected static ProtoPlayerCharacter c;
    protected static ProtoMessage hr;
    protected static ProtoMessage sd;
    protected static ProtoMessage knp;
    protected static ProtoSiegeDisplay sc;
    protected static ProtoMessage sm;
    protected static ProtoGenericArray<ProtoSiegeOverview> sl;
    public Text m_MyText;

    public void Start()
    {
        tclient = new TextTestClient();

    }

    protected void Login(string username, string password)
    {
       
       
        tclient.LogInAndConnect(username, password, "localhost");

        while (!tclient.IsConnectedAndLoggedIn())
        {
            Thread.Sleep(0);
            
        }

       
        if (tclient.IsConnectedAndLoggedIn())
        {
            mf = FiefDetails(tclient);

            SceneManager.LoadScene(1);
            // FiefControl f = new FiefControl(tclient);
        }
       
    }
    protected ProtoMessage GetActionReply(Actions action, TextTestClient client)
    {
        ProtoMessage responseTask = client.GetReply();

        while (responseTask.ActionType != action)
        {
            responseTask = client.GetReply();

        }
        client.ClearMessageQueues();
        return responseTask;
    }
    /*
     
         note all the function within "Char_158" is for demo use 
         */
    protected ProtoPlayerCharacter GetArmyID(TextTestClient client)
    {
        ProtoPlayerCharacter ProtoPlayerCharacter = new ProtoPlayerCharacter();
        ProtoPlayerCharacter.Message = "Char_158";
        ProtoPlayerCharacter.ActionType = Actions.ViewChar;
        client.net.Send(ProtoPlayerCharacter);
        var armyReply = GetActionReply(Actions.ViewChar, client);
        var armyResult = (ProtoPlayerCharacter)armyReply;
        return armyResult;
    }
    protected ProtoMessage GetPlayers(TextTestClient client)
    {
        ProtoMessage protoMessage = new ProtoMessage();
        protoMessage.Message = "Char_158";
        protoMessage.ActionType = Actions.GetPlayers;
        client.net.Send(protoMessage);
        ProtoMessage reply = GetActionReply(Actions.GetPlayers, client);
        return reply;
    }
    protected ProtoGenericArray<ProtoSiegeOverview> SiegeList(TextTestClient client)
    {
        ProtoMessage protoMessage = new ProtoMessage();
        protoMessage.ActionType = Actions.SiegeList;
        protoMessage.Message = "Char_158";
        client.net.Send(protoMessage);
        var reply = GetActionReply(Actions.SiegeList, client);
        return (ProtoGenericArray<ProtoSiegeOverview>)reply;
    }
    protected ProtoPlayerCharacter Profile(TextTestClient client)
    {
        ProtoPlayerCharacter protoMessage = new ProtoPlayerCharacter();
        protoMessage.Message = "Char_158";
        protoMessage.ActionType = Actions.ViewChar;
        client.net.Send(protoMessage);
        var reply = GetActionReply(Actions.ViewChar, client);
        return (ProtoPlayerCharacter)reply;
    }
    protected ProtoPlayerCharacter GetNPCList(TextTestClient client)
    {
        ProtoPlayerCharacter protoMessage = new ProtoPlayerCharacter();
        protoMessage.Message = "Char_158";
        protoMessage.ActionType = Actions.GetNPCList;
        client.net.Send(protoMessage);
        var reply = GetActionReply(Actions.GetNPCList, client);
        return (ProtoPlayerCharacter)reply;
    }
    /// <summary>
    /// fief functions
    /// </summary>
    /// <param name="client"></param>
    /// <returns></returns>
    protected ProtoFief FiefDetails(TextTestClient client)
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
    
    protected ProtoGenericArray<ProtoFief> ViewMyFiefs(TextTestClient client)
    {
        ProtoPlayerCharacter protoMessage = new ProtoPlayerCharacter();
        protoMessage.Message = "Char_158";
        protoMessage.ActionType = Actions.ViewChar;
        client.net.Send(protoMessage);
        var locReply = GetActionReply(Actions.ViewChar, client);
        var locResult = (ProtoPlayerCharacter)locReply;
        ProtoFief protoFief = new ProtoFief();
        protoFief.Message = locResult.location;
        protoFief.ActionType = Actions.ViewMyFiefs;
        client.net.Send(protoFief);
        var reply = GetActionReply(Actions.ViewMyFiefs, client);
        var fiefs = (ProtoGenericArray<ProtoFief>)reply;
        return fiefs;
    }

    /// <summary>
    /// army function
    /// </summary>
    /// <param name="client"></param>
    /// <returns></returns>
    protected ProtoGenericArray<ProtoArmyOverview> ArmyStatus(TextTestClient client)
    {
        ProtoArmy proto = new ProtoArmy();
        proto.ownerID = "Char_158";
        proto.ActionType = Actions.ListArmies;
        client.net.Send(proto);
        var reply = GetActionReply(Actions.ListArmies, client);
        var armies = (ProtoGenericArray<ProtoArmyOverview>)reply;
        return armies;
    }
    protected ProtoMessage MaintainArmy(string armyID, TextTestClient client)
    {
       // ProtoPlayerCharacter armyResult = GetArmyID(client);
        ProtoMessage protoMessage = new ProtoMessage();
        protoMessage.ActionType = Actions.MaintainArmy;
        protoMessage.Message = armyID;
        client.net.Send(protoMessage);
        var reply = GetActionReply(Actions.MaintainArmy, client);
        return reply;
    }
    protected ProtoMessage AppointLeader(string armyID,string charID, TextTestClient client)
    {
        //ProtoPlayerCharacter armyResult = GetArmyID(client);
        ProtoMessage protoMessage = new ProtoMessage();
        protoMessage.ActionType = Actions.AppointLeader;
        protoMessage.Message = armyID;
        protoMessage.MessageFields = new string[] { charID };
        client.net.Send(protoMessage);
        var reply = GetActionReply(Actions.AppointLeader, client);
        return reply;
    }

    protected ProtoGenericArray<ProtoDetachment> ListDetachments(TextTestClient client)
    {
        ProtoMessage protoMessage = new ProtoMessage();
        protoMessage.ActionType = Actions.ListDetachments;
        protoMessage.Message = "Char_158";
        client.net.Send(protoMessage);
        var reply = GetActionReply(Actions.ListDetachments, client);
        var Detachments = (ProtoGenericArray<ProtoDetachment>)reply;
        return Detachments;
    }
    protected ProtoMessage DropOffTroops(uint[] Troops, TextTestClient client)
    {
        ProtoPlayerCharacter armyResult = GetArmyID(client);
        ProtoDetachment protoDetachment = new ProtoDetachment();
        protoDetachment.ActionType = Actions.DropOffTroops;
        protoDetachment.troops = Troops;
        protoDetachment.armyID = armyResult.armyID;
        protoDetachment.leftFor = "Char_158";
        client.net.Send(protoDetachment);
        var reply = GetActionReply(Actions.DropOffTroops, client);
        return reply;
    }
    protected ProtoMessage PickUpTroops(string armyID, string[] detachmentIDs, TextTestClient client)
    {
        ProtoPlayerCharacter armyResult = GetArmyID(client);
        ProtoMessage protoMessage = new ProtoMessage();
        protoMessage.ActionType = Actions.PickUpTroops;
        protoMessage.Message = armyResult.armyID;
        protoMessage.MessageFields = detachmentIDs;
        client.net.Send(protoMessage);
        var reply = GetActionReply(Actions.PickUpTroops, client);
        return reply;
    }
    protected ProtoMessage HireTroops(int amount, TextTestClient client)
    {
        ProtoPlayerCharacter armyResult = GetArmyID(client);
        ProtoRecruit protoRecruit = new ProtoRecruit();
        protoRecruit.ActionType = Actions.RecruitTroops;
        if (amount > 0)
        {
            protoRecruit.amount = (uint)amount;
        }
        protoRecruit.armyID = armyResult.armyID;
        protoRecruit.isConfirm = true;
        client.net.Send(protoRecruit);
        var reply = GetActionReply(Actions.RecruitTroops, client);
        return reply;
    }

    protected ProtoMessage PillageFief(TextTestClient client)
    {
        ProtoPlayerCharacter armyResult = GetArmyID(client);
        ProtoMessage protoMessage = new ProtoMessage();
        protoMessage.ActionType = Actions.PillageFief;
        protoMessage.Message = armyResult.armyID;
        
        client.net.Send(protoMessage);
        var reply = GetActionReply(Actions.PillageFief, client);
        return reply;
    }
    protected ProtoMessage DisbandArmy(string armyID, TextTestClient client)
    {
        ProtoMessage protoMessage = new ProtoMessage();
        protoMessage.ActionType = Actions.DisbandArmy;
        protoMessage.Message = armyID;
        client.net.Send(protoMessage);
        var reply = GetActionReply(Actions.DisbandArmy, client);
        return reply;
    }
    /// <summary>
    /// family functions
    /// </summary>
    /// <param name="client"></param>
    /// <param name="brideID"></param>
    /// <returns></returns>
    protected ProtoMessage Marry(TextTestClient client, string brideID)
    {
        ProtoMessage protoMessage = new ProtoMessage();
        protoMessage.ActionType = Actions.ProposeMarriage;
        protoMessage.Message = "Char_158";
        protoMessage.MessageFields = new string[] { brideID };
        client.net.Send(protoMessage);
        var reply = GetActionReply(Actions.ProposeMarriage, client);
        return reply;
    }

    protected ProtoMessage AcceptRejectProposal(TextTestClient client, bool AcceptOrReject)
    {
        ProtoMessage protoMessage = new ProtoMessage();
        protoMessage.ActionType = Actions.AcceptRejectProposal;
        protoMessage.Message = "Char_158";
        protoMessage.MessageFields = new string[] { Convert.ToString(AcceptOrReject) };
        client.net.Send(protoMessage);
        var reply = GetActionReply(Actions.AcceptRejectProposal, client);
        return reply;
    }

    protected ProtoMessage TryForChild(TextTestClient client)
    {
        ProtoMessage protoMessage = new ProtoMessage();
        protoMessage.ActionType = Actions.TryForChild;
        protoMessage.Message = "Char_158";
        client.net.Send(protoMessage);
        var reply = GetActionReply(Actions.TryForChild, client);
        return reply;
    }
/*
    protected ProtoMessage TryForChild(string charID, TextTestClient client)
    {
        ProtoMessage protoMessage = new ProtoMessage();
        protoMessage.ActionType = Actions.TryForChild;
        protoMessage.Message = charID;
        client.net.Send(protoMessage);
        var reply = GetActionReply(Actions.TryForChild, client);
        return reply;
    }
    */
    /// <summary>
    /// siege functions
    /// </summary>
    /// <param name="client"></param>
    /// <returns></returns>
    protected ProtoMessage SiegeCurrentFief(TextTestClient client)
    {
        ProtoPlayerCharacter protoMessage = new ProtoPlayerCharacter();
        protoMessage.Message = "Char_158";
        protoMessage.ActionType = Actions.ViewChar;
        client.net.Send(protoMessage);
        var locReply = GetActionReply(Actions.ViewChar, client);
        var locResult = (ProtoPlayerCharacter)locReply;
        ProtoMessage protoSiegeStart = new ProtoMessage();
        protoSiegeStart.ActionType = Actions.BesiegeFief;
        protoSiegeStart.Message = locResult.armyID;
        client.net.Send(protoSiegeStart);
        var reply = GetActionReply(Actions.BesiegeFief, client);
        if (reply.GetType() == typeof(ProtoSiegeDisplay))
        {
            return reply as ProtoSiegeDisplay;
        }
        else
        {
            return reply;
        }
    }
    protected ProtoMessage ViewSiege(string siegeID, TextTestClient client)
    {
       
        ProtoMessage ViewSiege = new ProtoMessage();
        ViewSiege.ActionType = Actions.SiegeList;
        ViewSiege.Message = siegeID;
        client.net.Send(ViewSiege);
        var reply = GetActionReply(Actions.SiegeList, client);
        return reply;
    }
    protected ProtoSiegeDisplay SiegeRoundStorm(string siegeID, TextTestClient client)
    {
       
        ProtoMessage protoMessage = new ProtoMessage();
        protoMessage.ActionType = Actions.SiegeRoundStorm;
        protoMessage.Message = siegeID;
        client.net.Send(protoMessage);
        var reply = GetActionReply(Actions.SiegeRoundStorm, client);
        return (ProtoSiegeDisplay)reply;
    }

    protected ProtoSiegeDisplay SiegeRoundReduction(string siegeID, TextTestClient client)
    {
       
        ProtoMessage protoMessage = new ProtoMessage();
        protoMessage.ActionType = Actions.SiegeRoundReduction;
        protoMessage.Message = siegeID;
        client.net.Send(protoMessage);
        var reply = GetActionReply(Actions.SiegeRoundReduction, client);
        return (ProtoSiegeDisplay)reply;
    }
    protected ProtoSiegeDisplay SiegeRoundNegotiate(string siegeID, TextTestClient client)
    {
       
        ProtoMessage protoMessage = new ProtoMessage();
        protoMessage.ActionType = Actions.SiegeRoundNegotiate;
        protoMessage.Message = siegeID;
        client.net.Send(protoMessage);
        var reply = GetActionReply(Actions.SiegeRoundNegotiate, client);
        return (ProtoSiegeDisplay)reply;
    }
    protected ProtoMessage EndSiege(string siegeID,TextTestClient client)
    {  
        ProtoMessage protoMessage = new ProtoMessage();
        protoMessage.ActionType = Actions.EndSiege;
        protoMessage.Message = siegeID;
        client.net.Send(protoMessage);
        var reply = GetActionReply(Actions.EndSiege, client);
        return  reply;
    }
    /// <summary>
    /// spy funcations
    /// </summary>
    /// <param name="fiefID"></param>
    /// <param name="client"></param>
    /// <returns></returns>
    protected ProtoMessage SpyFief(string fiefID, string charID, TextTestClient client)
    {
        ProtoMessage protoMessage = new ProtoMessage();
        protoMessage.ActionType = Actions.SpyFief;
        protoMessage.Message = charID;
        protoMessage.MessageFields = new string[] { fiefID };
        client.net.Send(protoMessage);
        var reply = GetActionReply(Actions.SpyFief, client);
        return reply;
    }
    protected ProtoMessage SpyCharacter(string targetID, string charID, TextTestClient client)
    {
        ProtoMessage protoMessage = new ProtoMessage();
        protoMessage.ActionType = Actions.SpyCharacter;
        protoMessage.Message = charID;
        protoMessage.MessageFields = new string[] { targetID };
        client.net.Send(protoMessage);
        var reply = GetActionReply(Actions.SpyCharacter, client);
        return reply;
    }

    protected ProtoMessage SpyArmy(string armyID, string charID, TextTestClient client)
    {
        ProtoMessage protoMessage = new ProtoMessage();
        protoMessage.ActionType = Actions.SpyArmy;
        protoMessage.Message = charID;
        protoMessage.MessageFields = new string[] { armyID };
        client.net.Send(protoMessage);
        var reply = GetActionReply(Actions.SpyArmy, client);
        return reply;
    }
    /// <summary>
    ///  Journal Entry
    /// </summary>
    /// <param name="scope"></param>
    /// <param name="client"></param>
    /// <returns></returns>
    protected ProtoGenericArray<ProtoJournalEntry> ViewJournalEntries(string scope, TextTestClient client)
    {
        ProtoMessage protoMessage = new ProtoMessage();
        protoMessage.ActionType = Actions.ViewJournalEntries;
        protoMessage.Message = "Char_158";
        protoMessage.MessageFields = new string[] { scope };
        client.net.Send(protoMessage);
        var reply = GetActionReply(Actions.ViewJournalEntries, client);
        var entries = (ProtoGenericArray<ProtoJournalEntry>)reply;
        return entries;
    }

    protected ProtoJournalEntry ViewJournalEntry(uint journalID, TextTestClient client)
    {
        ProtoMessage protoMessage = new ProtoMessage();
        protoMessage.ActionType = Actions.ViewJournalEntry;
        protoMessage.Message = "Char_158";
        protoMessage.MessageFields = new string[] { Convert.ToString(journalID) };
        client.net.Send(protoMessage);
        var reply = GetActionReply(Actions.ViewJournalEntry, client);
        return (ProtoJournalEntry)reply;
    }
    /// <summary>
    /// kidnapping functions
    /// </summary>
    /// <param name="targetID"></param>
    /// <param name="client"></param>
    /// <returns></returns>
    protected ProtoMessage Kidnap(string targetID,string kidnapperID, TextTestClient client)
    {
        ProtoMessage protoMessage = new ProtoMessage();
        protoMessage.ActionType = Actions.Kidnap;
        protoMessage.Message = targetID;
        protoMessage.MessageFields = new string[] { kidnapperID };
        client.net.Send(protoMessage);

        var reply = GetActionReply(Actions.Kidnap, client);
        return reply;
    }
    protected ProtoMessage ViewCaptives(string captiveLocation, TextTestClient client)
    {
        ProtoMessage protoMessage = new ProtoMessage();
        protoMessage.ActionType = Actions.ViewCaptives;
        protoMessage.Message = captiveLocation;   
        client.net.Send(protoMessage);
        var reply = GetActionReply(Actions.ViewCaptives, client);
        return reply;
    }
    protected ProtoMessage ViewCaptive(string charID, TextTestClient client)
    {
        ProtoMessage protoMessage = new ProtoMessage();
        protoMessage.ActionType = Actions.ViewCaptive;
        protoMessage.Message = charID;
        client.net.Send(protoMessage);
        var reply = GetActionReply(Actions.ViewCaptive, client);
        return reply;
    }
    protected ProtoMessage RansomCaptive(string charID, TextTestClient client)
    {
        ProtoMessage protoMessage = new ProtoMessage();
        protoMessage.ActionType = Actions.RansomCaptive;
        protoMessage.Message = charID;
        client.net.Send(protoMessage);
        var reply = GetActionReply(Actions.RansomCaptive, client);
        return reply;
    }
    protected ProtoMessage ReleaseCaptive(string charID, TextTestClient client)
    {
        ProtoMessage protoMessage = new ProtoMessage();
        protoMessage.ActionType = Actions.ReleaseCaptive;
        protoMessage.Message = charID;
        client.net.Send(protoMessage);
        var reply = GetActionReply(Actions.ReleaseCaptive, client); 
        return reply;
    }
    protected ProtoMessage ExecuteCaptive(string charID, TextTestClient client)
    {
        ProtoMessage protoMessage = new ProtoMessage();
        protoMessage.ActionType = Actions.ExecuteCaptive;
        protoMessage.Message = charID;
        client.net.Send(protoMessage);
        var reply = GetActionReply(Actions.ExecuteCaptive, client);
        return reply;
    }
    protected ProtoMessage RespondRansom(uint jEntryID, bool pay, TextTestClient client)
    {
        ProtoMessage protoMessage = new ProtoMessage();
        protoMessage.ActionType = Actions.ExecuteCaptive;
        protoMessage.Message = Convert.ToString(jEntryID);
        protoMessage.MessageFields = new string[] { Convert.ToString(pay)};
        client.net.Send(protoMessage);
        var reply = GetActionReply(Actions.ExecuteCaptive, client);
        return reply;
    }
    protected ProtoMessage UseChar(TextTestClient client)
    {
        ProtoMessage protoMessage = new ProtoMessage();
        protoMessage.ActionType = Actions.UseChar;
        protoMessage.Message = "Char_158";
        client.net.Send(protoMessage);
        var reply = GetActionReply(Actions.UseChar, client);
        return reply;
    }
    /// <summary>
    /// NPC functions
    /// </summary>
    /// <param name="NPCID"></param>
    /// <param name="client"></param>
    /// <returns></returns>
    protected ProtoMessage hireNPC(string NPCID, TextTestClient client)
    {
        ProtoMessage protoMessage = new ProtoMessage();
        protoMessage.ActionType = Actions.HireNPC;
        protoMessage.Message = NPCID;
        client.net.Send(protoMessage);
        var reply = GetActionReply(Actions.HireNPC, client);
        return reply;
    }
    protected ProtoMessage fireNPC(string NPCID, TextTestClient client)
    {
        ProtoMessage protoMessage = new ProtoMessage();
        protoMessage.ActionType = Actions.FireNPC;
        protoMessage.Message = NPCID;
        client.net.Send(protoMessage);
        var reply = GetActionReply(Actions.FireNPC, client);
        return reply;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="fiefID"></param>
    /// <param name="client"></param>
    /// <returns></returns>
    protected ProtoMessage AppointBailiff(string fiefID, TextTestClient client)
    {
        ProtoMessage protoMessage = new ProtoMessage();
        protoMessage.ActionType = Actions.AppointBailiff;
        protoMessage.Message = fiefID;
        protoMessage.MessageFields = new string[] { "Char_158" };
        client.net.Send(protoMessage);
        var reply = GetActionReply(Actions.AppointBailiff, client);
        return reply;
    }
    protected ProtoMessage RemoveBailiff(string fiefID, TextTestClient client)
    {
        ProtoMessage protoMessage = new ProtoMessage();
        protoMessage.ActionType = Actions.RemoveBailiff;
        protoMessage.Message = fiefID;
        protoMessage.MessageFields = new string[] { "Char_158" };
        client.net.Send(protoMessage);
        var reply = GetActionReply(Actions.RemoveBailiff, client);
        return reply;
    }
    protected ProtoMessage BarCharacters(string fiefID, string[] charIDs, TextTestClient client)
    {
        ProtoMessage protoMessage = new ProtoMessage();
        protoMessage.ActionType = Actions.BarCharacters;
        protoMessage.Message = fiefID;
        protoMessage.MessageFields = charIDs;
        client.net.Send(protoMessage);
        var reply = GetActionReply(Actions.BarCharacters, client);
        return reply;
    }
    protected ProtoMessage UnbarCharacters(string fiefID, string[] charIDs, TextTestClient client)
    {
        ProtoMessage protoMessage = new ProtoMessage();
        protoMessage.ActionType = Actions.UnbarCharacters;
        protoMessage.Message = fiefID;
        protoMessage.MessageFields = charIDs;
        client.net.Send(protoMessage);
        var reply = GetActionReply(Actions.UnbarCharacters, client);
        return reply;
    }
    protected ProtoMessage BarNationalities(string fiefID, string[] netIDs, TextTestClient client)
    {
        ProtoMessage protoMessage = new ProtoMessage();
        protoMessage.ActionType = Actions.BarNationalities;
        protoMessage.Message = fiefID;
        protoMessage.MessageFields = netIDs;
        client.net.Send(protoMessage);
        var reply = GetActionReply(Actions.BarNationalities, client);
        return reply;
    }
    protected ProtoMessage UnbarNationalities(string fiefID, string[] netIDs, TextTestClient client)
    {
        ProtoMessage protoMessage = new ProtoMessage();
        protoMessage.ActionType = Actions.UnbarNationalities;
        protoMessage.Message = fiefID;
        protoMessage.MessageFields = netIDs;
        client.net.Send(protoMessage);
        var reply = GetActionReply(Actions.UnbarNationalities, client);
        return reply;
    }
    protected ProtoMessage GrantFiefTitle(string fiefID, TextTestClient client)
    {
        ProtoMessage protoMessage = new ProtoMessage();
        protoMessage.ActionType = Actions.GrantFiefTitle;
        protoMessage.Message = fiefID;
        protoMessage.MessageFields = new string[] { "Char_158" };
        client.net.Send(protoMessage);
        var reply = GetActionReply(Actions.GrantFiefTitle, client);
        return reply;
    }
    protected ProtoMessage AdjustExpenditure(string fiefID, double[] adjustedValues, TextTestClient client)
    {
        ProtoGenericArray<double> newExpenses = new ProtoGenericArray<double>();
        newExpenses.ActionType = Actions.AdjustExpenditure;
        newExpenses.Message = fiefID;
        newExpenses.fields = adjustedValues;
        client.net.Send(newExpenses);
        var reply = GetActionReply(Actions.AdjustExpenditure, client);
        return reply;
    }
    protected ProtoMessage AutoAdjustExpenditure(string fiefID, TextTestClient client)
    {
        ProtoGenericArray<double> newExpenses = new ProtoGenericArray<double>();
        newExpenses.Message = fiefID;
        newExpenses.ActionType = Actions.AdjustExpenditure;
        client.net.Send(newExpenses);
        var reply = GetActionReply(Actions.AdjustExpenditure, client);
        return reply;
    }
    protected ProtoMessage TransferFunds(string fiefFromID, string fiefToID, int amount, TextTestClient client)
    {
        ProtoTransfer ProtoTransfer = new ProtoTransfer();
        ProtoTransfer.fiefFrom = fiefFromID;
        ProtoTransfer.fiefTo = fiefToID;
        ProtoTransfer.amount = amount;
        ProtoTransfer.ActionType = Actions.AdjustExpenditure;
        client.net.Send(ProtoTransfer);
        var reply = GetActionReply(Actions.AdjustExpenditure, client);
        return reply;
    }
    protected ProtoMessage TransferFundsToPlayer(string playerTo, int amount, TextTestClient client)
    {
        ProtoTransferPlayer ProtoTransferPlayer = new ProtoTransferPlayer();
        ProtoTransferPlayer.playerTo = playerTo;
        ProtoTransferPlayer.amount = amount;
        ProtoTransferPlayer.ActionType = Actions.AdjustExpenditure;
        client.net.Send(ProtoTransferPlayer);
        var reply = GetActionReply(Actions.AdjustExpenditure, client);
        return reply;
    }
    protected ProtoMessage AdjustCombatValues(string armyID, byte aggression, byte odds, TextTestClient client)
    {
        ProtoCombatValues ProtoCombatValues = new ProtoCombatValues();
        ProtoCombatValues.armyID = armyID;
        ProtoCombatValues.aggression = aggression;
        ProtoCombatValues.odds = odds;
        ProtoCombatValues.ActionType = Actions.AdjustCombatValues;
        client.net.Send(ProtoCombatValues);
        var reply = GetActionReply(Actions.AdjustCombatValues, client);
        return reply;
    }
    protected ProtoMessage Attack(string armyID, string targetID, TextTestClient client)
    {
        ProtoMessage attack = new ProtoMessage();
        attack.ActionType = Actions.Attack;
        attack.Message = armyID;
        attack.MessageFields = new string[] { targetID };
        client.net.Send(attack);
        var reply = GetActionReply(Actions.Attack, client);
        return reply;
    }
    protected ProtoMessage EnterExitKeep(string charID, TextTestClient client)
    {
        ProtoMessage protoMessage = new ProtoMessage();
        protoMessage.ActionType = Actions.GrantFiefTitle;
        protoMessage.Message = charID;
        client.net.Send(protoMessage);
        var reply = GetActionReply(Actions.GrantFiefTitle, client);
        return reply;
    }
    protected ProtoMessage ListCharsInMeetingPlace(string placeType, string charID, TextTestClient client)
    {
        ProtoMessage protoMessage = new ProtoMessage();
        protoMessage.ActionType = Actions.GrantFiefTitle;
        protoMessage.Message = placeType;
        protoMessage.MessageFields = new string[] { "charID" };
        client.net.Send(protoMessage);
        var reply = GetActionReply(Actions.GrantFiefTitle, client);
        return reply;
    }
    protected ProtoMessage Camp(string charID, byte days, TextTestClient client)
    {
        ProtoMessage protoMessage = new ProtoMessage();
        protoMessage.ActionType = Actions.GrantFiefTitle;
        protoMessage.Message = charID;
        protoMessage.MessageFields = new string[] { "days" };
        client.net.Send(protoMessage);
        var reply = GetActionReply(Actions.GrantFiefTitle, client);
        return reply;
    }
    protected ProtoMessage AddRemoveEntourage(string charID, TextTestClient client)
    {
        ProtoMessage protoMessage = new ProtoMessage();
        protoMessage.ActionType = Actions.GrantFiefTitle;
        protoMessage.Message = charID;
        client.net.Send(protoMessage);
        var reply = GetActionReply(Actions.GrantFiefTitle, client);
        return reply;
    }
}

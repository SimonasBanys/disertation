using Assets.Scenes;
using ProtoMessageClient;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class fiefControl : Controller
{
    //IDs
    public Text mID;
    //public Text nID;
    public Text wID;
    // public Text sID;
    public Text eID;
    public Text nwID;
    public Text neID;
    public Text swID;
    public Text seID;
    //Buttons
    public Button mB;
    //public Button nB;
    public Button wB;
    //public Button sB;
    public Button eB;
    public Button nwB;
    public Button neB;
    public Button swB;
    public Button seB;
    
    //Other value
    // public static TextTestClient tclient;



    void Start()
    {

        mB.onClick.AddListener(DetailButtonListener);
        // nB.onClick.AddListener(MoveButtonListenerN);
        wB.onClick.AddListener(MoveButtonListenerW);
        //sB.onClick.AddListener(MoveButtonListenerS);
        eB.onClick.AddListener(MoveButtonListenerE);
        nwB.onClick.AddListener(MoveButtonListenerNW);
        neB.onClick.AddListener(MoveButtonListenerNE);
        swB.onClick.AddListener(MoveButtonListenerSW);
        seB.onClick.AddListener(MoveButtonListenerSE);
        initFiefSence();
    }

    // Update is called once per frame
    void Update()
    {

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

        while (responseTask.ActionType != action)
        {
            responseTask = client.GetReply();

        }
        client.ClearMessageQueues();
        return responseTask;
    }
    public enum MoveDirections
    {
        E, W, Se, Sw, Ne, Nw
    }
    public ProtoFief Move(MoveDirections directions)
    {

        ProtoTravelTo protoTravel = new ProtoTravelTo();
        protoTravel.travelVia = new[] { directions.ToString() };
        protoTravel.characterID = "Char_158";
        tclient.net.Send(protoTravel);
        var reply = GetActionReply(Actions.TravelTo, tclient);
        var travel = (ProtoFief)reply;
        if (mf.fiefID.Equals(travel.fiefID))
        {
            return null;
        }
        return travel;
    }
    public void setFiefID()
    {
        mf = FiefDetails(tclient);
        mID.text = mf.fiefID;
        eID.text = Move(MoveDirections.E).fiefID;
        Move(MoveDirections.W);
        wID.text = Move(MoveDirections.W).fiefID;
        Move(MoveDirections.E);
        seID.text = Move(MoveDirections.Se).fiefID;
        Move(MoveDirections.Nw);
        swID.text = Move(MoveDirections.Sw).fiefID;
        Move(MoveDirections.Ne);
        nwID.text = Move(MoveDirections.Ne).fiefID;
        Move(MoveDirections.Sw);
        nwID.text = Move(MoveDirections.Nw).fiefID;
        Move(MoveDirections.Se);
        ;

    }
    public void initFiefSence()
    {
        mID.text = "wocaonima";
        setFiefID();
    }

    void MoveTo(MoveDirections directions)
    {
        Move(directions);
        setFiefID();
    }
    void MoveButtonListenerE()
    {
        MoveTo(MoveDirections.E);
    }

    void MoveButtonListenerW()
    {
        MoveTo(MoveDirections.W);
    }
    /*
    void MoveButtonListenerS()
    {
        MoveTo(MoveDirections.S);
    }
   
    void MoveButtonListenerN()
    {
        MoveTo(MoveDirections.N);
    }
     */
    void MoveButtonListenerSW()
    {
        MoveTo(MoveDirections.Sw);
    }
    void MoveButtonListenerNE()
    {
        MoveTo(MoveDirections.Ne);
    }
    void MoveButtonListenerNW()
    {
        MoveTo(MoveDirections.Nw);
    }
    void MoveButtonListenerSE()
    {
        MoveTo(MoveDirections.Se);
    }
    void DetailButtonListener()
    {
        SceneManager.LoadScene(2);
    }
    void backlistner()
    {
        SceneManager.LoadScene(1);
    }
}

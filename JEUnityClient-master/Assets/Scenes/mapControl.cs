using Assets.Scenes;
using ProtoMessageClient;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class mapControl : Controller
{
    //IDs
    public Text mName;
    //public Text nID;
    public Text wName;
   // public Text sID;
    public Text eName;
    public Text nwName;
    public Text neName;
    public Text swName;
    public Text seName;
    //Buttons
    public Button mButton;
    //public Button nB;
    public Button wButton;
    //public Button sB;
    public Button eButton;
    public Button nwButton;
    public Button neButton;
    public Button swButton;
    public Button seButton;

    //Other value
   // public static TextTestClient tclient;
   


    void Start () {
        
        mButton.onClick.AddListener(DetailButtonListener);
       // nB.onClick.AddListener(MoveButtonListenerN);
        wButton.onClick.AddListener(MoveButtonListenerW);
        //sB.onClick.AddListener(MoveButtonListenerS);
        eButton.onClick.AddListener(MoveButtonListenerE);
        nwButton.onClick.AddListener(MoveButtonListenerNW);
        neButton.onClick.AddListener(MoveButtonListenerNE);
        swButton.onClick.AddListener(MoveButtonListenerSW);
        seButton.onClick.AddListener(MoveButtonListenerSE);
        initFiefSence();
    }

    // Update is called once per frame
    void Update () {

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
        ma = ArmyStatus(tclient);
        mName.text = mf.fiefID;
        //eName.text = Move(MoveDirections.E).fiefID;
      //  Move(MoveDirections.W);
      //  wName.text = Move(MoveDirections.W).fiefID;
     //   seName.text = Move(MoveDirections.Se).fiefID;
     //   Move(MoveDirections.Nw);
    //    swName.text = Move(MoveDirections.Sw).fiefID;
    //    Move(MoveDirections.Ne);
    //    neName.text = Move(MoveDirections.Ne).fiefID;
     //   Move(MoveDirections.Sw);
     //   nwName.text = Move(MoveDirections.Nw).fiefID;
     //   Move(MoveDirections.Se);
 ;

    }
    public void initFiefSence()
    {
        mName.text = "wocaonima";
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
        SceneManager.LoadScene(1);
    }
}

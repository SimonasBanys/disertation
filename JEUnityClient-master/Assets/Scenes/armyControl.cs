using ProtoMessageClient;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class armyControl : Controller
{

    // Use this for initialization
    public Text title;
    public Text report;

    public Button hire;
    public Button pickUp;
    public Button dropOff;
    public Button maintain;
    public Button disbaned;
    public Button appleader;
    public Button pillagefief;
    public Button back;
    public Button detachments;
    public InputField amount;
    public InputField armyID;
    public InputField charID;
    void Start()
    {
        displayArmy();
        pickUp.onClick.AddListener(pickUpListener);
        dropOff.onClick.AddListener(dropOffListener);
        maintain.onClick.AddListener(maintainListener);
        disbaned.onClick.AddListener(disbanedListener);
        appleader.onClick.AddListener(appleaderListener);
        pillagefief.onClick.AddListener(PillageFiefListener);

        detachments.onClick.AddListener(DetachmentsListener);
        back.onClick.AddListener(backListener);
        hire.onClick.AddListener(hireListener);
        


    }
    void displayArmy()      
    {
        report.text += mf.fiefID + "\n";
        ma = ArmyStatus(tclient);
        report.text = "";
        var counter = 0;
        foreach (var army in ma.fields)
        {
            counter++;
            report.text += ("Army " + counter + " ");
            report.text += ("Army ID: " + army.armyID + "\n");
            report.text += ("Owner: " + army.ownerName + "\n");
            report.text += ("Size: " + army.armySize + "\n");
            report.text += ("Location : " + army.locationID + "\n");
            report.text += ("Leader : " + army.leaderName + "\n");
            report.text += ("-----------------------------" + "\n");
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
    void hireListener()
    {
        string num = amount.text;


        hr = HireTroops(int.Parse(num), tclient);
        SceneManager.LoadScene(7);

      //  hireDisplay();
    }
    void backListener()
    {
        SceneManager.LoadScene(1);
    }

    void pickUpListener()
    {
        string num = amount.text;
        string ID = armyID.text;
       // hr = PickUpTroops(ID, , tclient);
       // SceneManager.LoadScene(7);
       // dropOffDisplay();
    }
    void dropOffListener()
    {
       // string num = amount.text;
       //hr = DropOffTroops(, tclient);

        
    }
    void maintainListener()
    {
        string ID = armyID.text;
        hr = MaintainArmy(ID, tclient);
        //maintianDisplay();
        SceneManager.LoadScene(7);
    }
    void disbanedListener()
    {
        string ID = armyID.text;
        if (ID == null)
        {
            return;
        }
        hr = DisbandArmy(ID, tclient);
        displayArmy();
    }
    void appleaderListener()
    {
        string ID = armyID.text;
        string leader = charID.text;

        if (ID == null)
        {
            return;
        }
        hr = AppointLeader(ID, leader, tclient);
        displayArmy();
    }
    void PillageFiefListener()
    {
        string num = amount.text;
        hr = PillageFief(tclient);
        SceneManager.LoadScene(7);
      //  displayPillage();
    }
    void DetachmentsListener()
    {
        string num = amount.text;
        dl = ListDetachments( tclient);
        SceneManager.LoadScene(7);
//displayDetachments();
    }


}
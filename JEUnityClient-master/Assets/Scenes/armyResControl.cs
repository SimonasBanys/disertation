using ProtoMessageClient;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class armyResControl : Controller
{
    public Text resTitle;
    public Text resReport;
    public Button Backbutton;

   
    // Start is called before the first frame update
    void Start()
    {
        Backbutton.onClick.AddListener(backListener);

        if (hr.ActionType == Actions.RecruitTroops)
        {
            hireDisplay();
        }
        else if (hr.ActionType == Actions.MaintainArmy)
        {
            maintianDisplay();
        }
        else if (hr.ActionType == Actions.ListDetachments)
        {
            displayDetachments();
        }
        else if (hr.ActionType == Actions.PillageFief)
        {
            displayPillage();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void backListener()
    {
        SceneManager.LoadScene(3);
    }

    void hireDisplay()
    {

        if (hr.ResponseType == DisplayMessages.CharacterRecruitOwn)
        {
            resTitle.text = "Hire filed";
            resReport.text = ("Recruit from a fief you own!\n");
        }
        else if (hr.ResponseType == DisplayMessages.CharacterRecruitAlready)
        {
            resTitle.text = "Hire filed";
            resReport.text = ("You have already recruited!\n");
        }
        else if (hr.ResponseType == DisplayMessages.CharacterRecruitInsufficientFunds)
        {
            resTitle.text = "Hire filed";
            resReport.text = ("Insufficient recruitment funds!\n");
        }
        else
        {
            resTitle.text = "Hire Success";
            var recruitProtoBufCast = (ProtoRecruit)hr;
            resReport.text = ("----------------------------- \n");
            resReport.text += ("Recruit Report\n");
            resReport.text += ("-----------------------------\n");
            resReport.text += ("Army ID: " + recruitProtoBufCast.armyID + "\n");
            resReport.text += ("Recruitment Cost: " + recruitProtoBufCast.cost + "\n");
            resReport.text += ("Amount of Recruits: " + recruitProtoBufCast.amount + "\n");
            resReport.text += ("-----------------------------\n");
        }

    }
   
    void maintianDisplay()
    {
        if (hr.ResponseType == DisplayMessages.ArmyMaintainedAlready)
        {
            resTitle.text = "Maintain filed";
            resReport.text = ("Army Maintained Already\n");
        }
        else if (hr.ResponseType == DisplayMessages.ArmyMaintainInsufficientFunds)
        {
            resTitle.text = "Maintain filed";
            resReport.text = ("Army Maintain Insufficient Funds\n");
        }
        else
        {
            resTitle.text = "Maintain success";

        }
    }

    void displayDetachments()
    {

        var counter = 0;
        foreach (var d in dl.fields)
        {
            counter++;
            resReport.text += ("Detachment " + counter + " ");
            resReport.text += ("Army ID: " + d.armyID + "\n");
            resReport.text += ("Owner: " + d.id + "\n");
            resReport.text += ("Days: " + d.days + "\n");
            resReport.text += ("Left By : " + d.leftBy + "\n");
            resReport.text += ("Left For : " + d.leftFor + "\n");
            resReport.text += ("-----------------------------" + "\n");
        }
    }
    void displayPillage()
    {
        resReport.text += hr.Message;
    }

}

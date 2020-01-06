using ProtoMessageClient;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class hireControl : Controller
{

    // Use this for initialization
    public Text title;
    public Text report;

    public Button back;

    void Start()
    {
        back.onClick.AddListener(backListener);
        if (hr.ResponseType == DisplayMessages.CharacterRecruitOwn)
        {
            report.text += ("Recruit from a fief you own!\n");
        }
        else if (hr.ResponseType == DisplayMessages.CharacterRecruitAlready)
        {
            report.text += ("You have already recruited!\n");
        }
        else if (hr.ResponseType == DisplayMessages.CharacterRecruitInsufficientFunds)
        {
            report.text += ("Insufficient recruitment funds!\n");
        }
        else
        {
            var recruitProtoBufCast = (ProtoRecruit)hr;
            report.text += ("----------------------------- \n");
            report.text += ("Recruit Report\n");
            report.text += ("-----------------------------\n");
            report.text += ("Army ID: " + recruitProtoBufCast.armyID + "\n");
            report.text += ("Recruitment Cost: " + recruitProtoBufCast.cost + "\n");
            report.text += ("Amount of Recruits: " + recruitProtoBufCast.amount + "\n");
            report.text += ("-----------------------------\n");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    void backListener()
    {
        SceneManager.LoadScene(1);
    }
}
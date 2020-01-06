using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class profileControl : Controller
{

    // Use this for initialization
    public Text title;
    public Text report;

    public Button back;

    void Start()
    {
        back.onClick.AddListener(backListener);
        report.text += ("-----------------------------\n");
        report.text += ("Player Profile\n");
        report.text += ("-----------------------------\n");
        report.text += ("Player ID: " + c.playerID+"\n");
        report.text += ("Player Name: " + c.firstName + " " + c.familyName+"\n");
        report.text += ("-----------------------------\n");
        report.text += ("Owned Fiefs: \n");
        bool written = false;
        foreach (var fief in c.ownedFiefs)
        {
            if (written == false)
            {
                report.text += (fief);
                written = true;
            }
            else
                report.text += (" , " + fief);
        }
        report.text += ("\n");
        report.text += ("-----------------------------\n");
        report.text += ("Location: " + c.location + "\n");
        report.text += ("Army: " + c.armyID + "\n");
        report.text += ("Purse: " + c.purse + "\n");
        report.text += ("-----------------------------\n");
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
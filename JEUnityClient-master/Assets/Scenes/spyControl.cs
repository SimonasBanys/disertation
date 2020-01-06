using ProtoMessageClient;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class spyControl : Controller
{
    // Start is called before the first frame update
    public Button spyArmy;
    public Button spyFief;
    public Button spyChar;
    public Button back;
    public InputField armyID;
    public InputField fiefID;
    public InputField charID;
    public InputField spyID;
    public Text report;

    void Start()
    {
        back.onClick.AddListener(backListener);
        spyArmy.onClick.AddListener(spyArmyListener);
        spyChar.onClick.AddListener(spyCharListener);
        spyFief.onClick.AddListener(spyFiefListener);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void backListener()
    {
        SceneManager.LoadScene(1);
    }
    void spyArmyListener()
    {
        string ID = armyID.text;
        string spy = spyID.text;
        if (ID == null)
        {
            report.text = "enter armyID you want to spy";
        }
        if (spy == null)
        {
            report.text = "enter spy you want to send";
        }
        sm = SpyArmy(ID, spy, tclient);

        if (sm.ResponseType == DisplayMessages.Success)
        {
            ProtoArmy sa = (ProtoArmy)sm;
            report.text = ("Army ID: " + sa.armyID + "\n");
            report.text += ("OwnerID: " + sa.ownerID + "\n");
            report.text += ("Owner: " + sa.owner + "\n");
            report.text += ("Owner: " + sa.owner + "\n");
            report.text += ("nationality : " + sa.nationality + "\n");
            report.text += ("location: " + sa.location + "\n");
            report.text += ("Maintained : " + sa.isMaintained + "\n");
            report.text += ("maintCost: " + sa.maintCost + "\n");
            report.text += ("Leader : " + sa.leader + "\n");
            report.text += ("combatOdds : " + sa.combatOdds + "\n");
            report.text += ("siegeStatus : " + sa.siegeStatus + "\n");
            report.text += ("aggression : " + sa.aggression + "\n");
            report.text += ("-----------------------------" + "\n");
        }
        else
        {
            report.text = "fail to spy";

        }

    }
    void spyCharListener()
    {
        string ID = charID.text;
        string spy = spyID.text;
        if (ID == null)
        {
            report.text = "enter charID you want to spy";
        }
        if (spy == null)
        {
            report.text = "enter spy you want to send";
        }
        sm = SpyCharacter(ID, spy, tclient);
        if (sm.ResponseType == DisplayMessages.Success)
        {
            ProtoNPC sn = (ProtoNPC)sm;
            report.text = ("char ID: " + sn.charID + "\n");
            report.text += ("family Name: " + sn.familyName + "\n");
            report.text += ("family ID: " + sn.familyID + "\n");
            report.text += ("father: " + sn.father + "\n");
            report.text += ("mother: " + sn.mother + "\n");
            report.text += ("language: " + sn.language + "\n");
            report.text += ("birth Year: " + sn.birthYear + "\n");
            report.text += ("birth Season: " + sn.birthSeason + "\n");
            report.text += ("health: " + sn.health + "\n");
            report.text += ("ailments: " + sn.ailments + "\n");
            report.text += ("nationality: " + sn.nationality + "\n");

        }
        else
        {
            report.text = "fail to spy";

        }
    }
    void spyFiefListener()
    {
        string ID = fiefID.text;
        string spy = spyID.text;
        if (ID == null)
        {
            report.text = "enter fiefID you want to spy";
        }
        if (spy == null)
        {
            report.text = "enter spy you want to send";
        }
        sm = SpyFief(ID, spy, tclient);
        if (sm.ResponseType == DisplayMessages.Success)
        {
            ProtoFief sf = (ProtoFief)sm;
            report.text = ("fief ID: " + sf.fiefID + "\n");
            report.text += ("owner: " + sf.owner + "\n");
            report.text += ("owner ID: " + sf.ownerID + "\n");
            report.text += ("Industry Level: " + sf.industry + "\n");
            //TODO to display army and NPC in this fief

        }
        else
        {
            report.text = "fail to spy";


        }
    }
}
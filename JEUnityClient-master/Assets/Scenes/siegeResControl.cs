using ProtoMessageClient;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class siegeResControl : Controller
{
    // Start is called before the first frame update
    public Text resTitle;
    public Text resReport;
    public Button back;

    void Start()
    {
        back.onClick.AddListener(backListener);
      
            //displayEnd();
     
            DisplaySiege(sc);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void backListener()
    {
        SceneManager.LoadScene(6);
    }

    void DisplaySiege(ProtoMessage siegeDisplayProtoBuf)
    {
        if (sd.GetType() == typeof(ProtoSiegeDisplay))
        {
            var siegeDisplay = (ProtoSiegeDisplay)sd;
            if (siegeDisplay.besiegerWon)
            {
                resTitle.text = ("Success");
            }
            else
            {
                resTitle.text = ("Fail");
            }
            resReport.text += ("-----------------------------\n");
            resReport.text += ("Siege Report\n");
            resReport.text += ("-----------------------------\n");
            resReport.text += ("Besieged Fief: " + siegeDisplay.besiegedFief + "\n");
            resReport.text += ("Besieged Army: " + siegeDisplay.besiegerArmy + "\n");
            resReport.text += ("Siege Successful: " + siegeDisplay.besiegerWon + "\n");
            resReport.text += ("Siege Length: " + siegeDisplay.days + " days\n");
            resReport.text += ("Loot Lost: " + siegeDisplay.lootLost + "\n");

            resReport.text += ("-----------------------------\n");

        }
        else
        {
            switch (sd.ResponseType)
            {
                case DisplayMessages.PillageSiegeAlready:
                    resReport.text += ("Already sieged this turn!");
                    break;
                case DisplayMessages.PillageUnderSiege:
                    resReport.text += ("Already under siege!");
                    break;
                case DisplayMessages.ArmyNoLeader:
                    resReport.text += ("Army has no leader!");
                    break;
                case DisplayMessages.SiegeNotBesieger:
                    resReport.text += ("Siege Not Besieger");
                    break;
                default:
                    resReport.text += (sd.ResponseType);
                    break;
            }
        }

    }
}

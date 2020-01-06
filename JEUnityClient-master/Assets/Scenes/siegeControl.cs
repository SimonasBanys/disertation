using ProtoMessageClient;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class siegeControl : Controller
{

    // Use this for initialization
    public Text title;
    public Text report;

    public Button siegeCF;
    public Button Storm;
    public Button Reduction;
    public Button Negotiate;
    public Button resBack;
    public Button end;
    public Button back;
    public InputField siegeID;      

    void Start()
    {
        back.onClick.AddListener(backListener);
        
        Storm.onClick.AddListener(stromListener);
        siegeCF.onClick.AddListener(siegeCFListener);
        Reduction.onClick.AddListener(ReductionListener);
        Negotiate.onClick.AddListener(NegotiateListener);
        end.onClick.AddListener(endListener);
        dispalySiegeList();
       


    }

    // Update is called once per frame
    void Update()
    {

    }
    void backListener()
    {
        SceneManager.LoadScene(1);
    }

    void resBackListener()
    {
        SceneManager.LoadScene(6);
        dispalySiegeList();
    }
    void siegeCFListener()
    {

        sd = SiegeCurrentFief(tclient);
 

        SceneManager.LoadScene(11);
       // DisplaySiege(sd);
    }
    void stromListener()
    {
        string ID = siegeID.text;
        if(ID == null)
        {
            return;
        }
        sc = SiegeRoundStorm(ID, tclient);
        SceneManager.LoadScene(11);
       // DisplaySiege(sc);
    }
    void ReductionListener()
    {
        string ID = siegeID.text;
        if (ID == null)
        {
            return;
        }
        sc = SiegeRoundReduction(ID, tclient);
        SceneManager.LoadScene(11);
       // DisplaySiege(sc);
    }
    void NegotiateListener()
    {
        string ID = siegeID.text;
        if (ID == null)
        {
            return;
        }
        sc = SiegeRoundNegotiate(ID, tclient);
        SceneManager.LoadScene(11);
        //DisplaySiege(sc);
    }

    void dispalySiegeList()
    {
        sl = SiegeList(tclient);

        foreach (ProtoSiegeOverview psd in sl.fields)
        {
            report.text += "SiegeID: " + psd.siegeID + "\n";
            report.text += "Be Sieged Fief: " + psd.besiegedFief + "\n";
            report.text += "Be Sieged Player: " + psd.besiegingPlayer + "\n";
            report.text += "Defending Player : " + psd.defendingPlayer + "\n";
            report.text += "------------------------------- \n";
        }
    }
    void endListener()
    {
        string ID = siegeID.text;
        if (ID == null)
        {
            return;
        }
        sd = EndSiege(ID, tclient);
        SceneManager.LoadScene(11);

    }
}
using ProtoMessageClient;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class kidnapControl : Controller
{
    // Start is called before the first frame update
    public Button kidnap;
    public Button ransom;
    public Button release;
    public Button execute;
    public Button back;
    public InputField charID;
    public InputField targetID;
    public InputField kidnapperID;
    public Text report;
    

    void Start()
    {
        kidnap.onClick.AddListener(kidnapListener);
        ransom.onClick.AddListener(ransomListener);
        release.onClick.AddListener(releaseListener);
        execute.onClick.AddListener(executeListener);
        back.onClick.AddListener(backListener);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void backListener()
    {
        SceneManager.LoadScene(1);
    }
    void kidnapListener()
    {
        string target = targetID.text;
        string kidnapper = kidnapperID.text;
        report.text += "1";
        if (kidnapper == null)
        {
            report.text = "enter target";
            //return;
            report.text += "2";
        }
        if (target == null)
        {
            report.text = "enter kidnapper";
            //return;
            report.text += "3";
        }
        knp = Kidnap(target, kidnapper, tclient);
        report.text += "4";
        if (knp.ResponseType == DisplayMessages.KidnapDead)
        {
            report.text = "Kidnap Dead";

        }
        else if(knp.ResponseType == DisplayMessages.CharacterHeldCaptive)
        {
            report.text = "Character Held Captive";

        }
        else if(knp.ResponseType == DisplayMessages.KidnapNoPlayer)
        {
            report.text = "Kidnap No Player";

        }
        else if(knp.ResponseType == DisplayMessages.KidnapOwnCharacter)
        {
            report.text = "Kidnap Own Character";

        }
        else if(knp.ResponseType == DisplayMessages.Success)
        {
            report.text = "Success kidnap" + target;
        }
    }
    void ransomListener()
    {
        string p = charID.text;
        if (p == null)
        {
            report.text = "enter charID you want to ransom";
        }
        knp = RansomCaptive(p,tclient);
        if(knp.ResponseType == DisplayMessages.NotCaptive)
        {
            report.text = "Not Captive";
        }
        else if (knp.ResponseType == DisplayMessages.RansomAlready)
        {
            report.text = "Ransom Already";
        }
        else
        {
            report.text = "Success Ransom Captive";
        }

    }
    void releaseListener()
    {
        string p = charID.text;
        if (p == null)
        {
            report.text = "enter charID you want to Release";
        }
        knp = ReleaseCaptive(p, tclient);
        if (knp.ResponseType == DisplayMessages.NotCaptive)
        {
            report.text = "Not Captive";
        }
        else
        {
            report.text = "Success  Release Captive";
        }
    }
    void executeListener()
    {
        string p = charID.text;
        if (p == null)
        {
            report.text = "enter charID you want to Execut";
        }
        knp = ExecuteCaptive(p, tclient);
        if (knp.ResponseType == DisplayMessages.NotCaptive)
        {
            report.text = "Not Captive";
        }
        else
        {
            report.text = "Success  Execut Captive";
        }
    }
}

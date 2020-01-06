using ProtoMessageClient;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class familyControl : Controller
{
    // Start is called before the first frame update
    public Button marry;
    public Button Back;
    public Button accpect;
    public Button reject;
    public Button child;
    public InputField brideID;
    public Text groomID;

    void Start()
    {
        marry.onClick.AddListener(marryListener);
        Back.onClick.AddListener(backListener);
        child.onClick.AddListener(childListener);
        accpect.onClick.AddListener(accpectListener);
        reject.onClick.AddListener(rejectListener);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void marryListener()
    {
        string ID = brideID.text;
        Marry(tclient ,ID);
    }
    void backListener()
    {
        SceneManager.LoadScene(1);
    }
    void accpectListener()
    {
        AcceptRejectProposal(tclient, true);
    }
    void rejectListener()
    {
        AcceptRejectProposal(tclient, false);
    }
    void childListener()
    {
        TryForChild(tclient);
    }
}

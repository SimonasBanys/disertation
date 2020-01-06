using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DetailController : Controller
{

    // Use this for initialization
    public Text FiefId;
    public Text Owner;
    public Text OwnerId;
    public Text IndustryLevel;
    public Button map;
    public Button army;
    public Button siege;
    public Button profile;
    public Button spy;
    public Button kidnapB;
    public Button familyB;
    public Button NPC;


    void Start () {
        map.onClick.AddListener(mapListener);
        army.onClick.AddListener(armyListener);
        siege.onClick.AddListener(siegeListener);
        profile.onClick.AddListener(profileListener);
        spy.onClick.AddListener(spyListener);
        kidnapB.onClick.AddListener(kidnapListener);
        familyB.onClick.AddListener(famliyListener);
        FiefId.text += mf.fiefID;
        Owner.text += mf.owner;
        OwnerId.text += mf.ownerID;
        IndustryLevel.text += mf.industry;

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    void mapListener()
    {
        SceneManager.LoadScene(2);
    }
    void armyListener()
    {

        SceneManager.LoadScene(3);

    }

    void siegeListener()
    {
        // sd = SiegeCurrentFief(tclient);
            
        SceneManager.LoadScene(6);
    }
    void profileListener()
    {
        c = Profile(tclient);
        SceneManager.LoadScene(4);
    }
    void kidnapListener()
    {
        SceneManager.LoadScene(8);

    }
    void spyListener()
    {
        SceneManager.LoadScene(9);

    }
    void famliyListener()
    {
        SceneManager.LoadScene(10);

    }
}

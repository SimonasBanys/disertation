using Assets.Scenes;
using ProtoMessageClient;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class loginControl : Controller
{
    public InputField inputUsername;
    public InputField inputPassword;
    public Button loginButton;
    public Text buttonText;
    // Start is called before the first frame update
    void Start()
    {
        loginButton.onClick.AddListener(LoginButton);
    }

    private void LoginButton()
    {
        string username = inputUsername.text;
        string password = inputPassword.text;
        Login(username, password);
      //  Login(username, password);
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

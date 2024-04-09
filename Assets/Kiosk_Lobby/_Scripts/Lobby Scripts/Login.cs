using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    public GameObject Loading, LoginPanel,TempBars;
    public InputField LoginField, PassField;
    public AudioSource bttnsound,keysound;
    public LobbyManager _lobbymanager;
    public static int bar1, bar2,isLogin;

    private void Start()
    {
        if (isLogin == 1)
        {
            TempBars.SetActive(false);
            LoginPanel.SetActive(false);
            //  Loading.SetActive(true);
            _lobbymanager.enabled = true;
        }
    }
    public void ButtonInput(int number)
    {       
        if (bar1==1)
        {
            LoginField.text += number;
            keysound.Play();
        }
        if (bar2==1)
        {
            PassField.text += number;
            keysound.Play();
        }
    }
    public void ClearText1()
    {
        keysound.Play();
        LoginField.text = LoginField.text.Substring(0, LoginField.text.Length - 1);
    }
    public void ClearText2()
    {
        keysound.Play();
        PassField.text = PassField.text.Substring(0, PassField.text.Length - 1);
    }
    public void EnterBttn()
    {
        bttnsound.Play();
        if (LoginField.text == "1234" && PassField.text == "1234")
        {
         //   isLogin = 1;
            TempBars.SetActive(false);
            LoginPanel.SetActive(false);
            Loading.SetActive(true);
            _lobbymanager.enabled = true;
        }
    }
}

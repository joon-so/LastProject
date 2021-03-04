using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    public InputField id;
    public InputField password;
    public Text message;

    void Start()
    {
        message.text = "";        
    }
    
    public void SaveUserData()
    {
        if(CheckInputField(id.text, password.text))
        {
            message.text = "Please enter your ID and PW";
            return;
        }

        if (!PlayerPrefs.HasKey(id.text))
        {
            PlayerPrefs.SetString(id.text, password.text);
            message.text = "The ID and PW is save";
        }
        else
        {
            message.text = "The same ID exists";
        }
    }
    
    public void CheckUserData()
    {
        if (CheckInputField(id.text, password.text))
        {
            message.text = "Please enter your ID and PW";
            return;
        }

        string pass = PlayerPrefs.GetString(id.text);

        if (pass == password.text)
        {
            SceneManager.LoadScene("Main");
        }
        else
        {
            message.text = "The ID or PW is incorrect";
        }
    }

    bool CheckInputField(string id, string pwd)
    {
        if (id == "" || pwd == "")
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
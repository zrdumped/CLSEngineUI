using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Account : MonoBehaviour {

    public GameObject accountInput;
    public GameObject passwordInput;
    public GameObject emailInput;

    public AudioSource alarmSound;
    public Animator wrongAnimator;

    private GM.GM_Core gm;

    // Use this for initialization
    void Start () {
        gm = GameObject.Find("GameManager").GetComponent<GM.GM_Core>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void loginConfirm()
    {
        string account = accountInput.GetComponent<InputField>().text;
        string password = passwordInput.GetComponent<InputField>().text;
        if(gm == null || gm.Login(account, password) == false)
        {
            wrongAnimator.Play("Notification In");
            alarmSound.Play();
        }
    }

    public void signupConfirm()
    {
        string account = accountInput.GetComponent<InputField>().text;
        string password = passwordInput.GetComponent<InputField>().text;
        string email = emailInput.GetComponent<InputField>().text;
        gm.Signup(account, password, email);
    }

    public void signupCancle()
    {
        gm.SwitchToScene("LoginScene");
    }

    public void signupFromLogin()
    {
        gm.SwitchToScene("SignupScene");
    }
}

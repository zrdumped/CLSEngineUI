using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Account : MonoBehaviour
{

    public GameObject accountInput;
    public GameObject passwordInput;
    public GameObject emailInput;

    public AudioSource alarmSound;
    public Animator wrongAnimator;

    public string account = "";
    public string password = "";
    private string defaultInvite = "87";

    private GM.GM_Core gm;

    // Use this for initialization
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GM.GM_Core>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void loginConfirm()
    {
        account = accountInput.GetComponent<InputField>().text;
        password = passwordInput.GetComponent<InputField>().text;
        gm.Login(account, password, this);
    }

    public void loginFallback(bool result)
    {
        if (result)
        {
            gm.SwitchToScene("MainMenu");
        }
        else
        {
            wrongAnimator.Play("Notification In");
            alarmSound.Play();
        }
    }

    public void signupConfirm()
    {
        account = accountInput.GetComponent<InputField>().text;
        password = passwordInput.GetComponent<InputField>().text;
        string email = emailInput.GetComponent<InputField>().text;
        gm.Signup(account, password, email, this);
    }

    public void signupFallback(bool result)
    {
        if (result)
            gm.SwitchToScene("LoginScene");
        else
        {
            wrongAnimator.Play("Notification In");
            alarmSound.Play();
        }
    }

    public void signupCancle()
    {
        gm.SwitchToScene("LoginScene");
    }

    public void signupFromLogin()
    {
        gm.SwitchToScene("SignupScene");
    }

    public void guestLogin()
    {
        gm.IsGuest = false;
        gm.Account = "guest";
        gm.Password = "guest";
        gm.SwitchToScene("MainMenu");
    }

    public void DemoMode()
    {
        WWWForm form = new WWWForm();
        form.AddField("invite", defaultInvite);
        Chemix.Network.NetworkManager.Instance.Post(form, "scene/invite",
                                                    (success, reply) =>
        {
            if (success)
            {
                gm.Invite = defaultInvite;
                gm.experimentalSetup = JsonUtility.FromJson<Chemix.GameManager.ExperimentalSetup>(reply.Detail);
                gm.QuestionnaireMemo = gm.experimentalSetup.questionnaire;
                gm.SwitchToScene("CustomLab");

            }
            else
            {
                //wrongAnimator.Play("Notification In");
            }
        }
                                                   );
    }
}

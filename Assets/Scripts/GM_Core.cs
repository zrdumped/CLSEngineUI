using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Chemix.Network;
using static Chemix.GameManager;
using Chemix;

namespace GM
{
    public class GM_Core : MonoBehaviour
    {

        public static GM_Core instance = null;

        public string testAccount = "a";
        public string testPassword = "b";

        private string curSceneName = "BaseScene";
        private GM_Settings settings;
        private UI_Account accountManager;

		public bool IsGuest = true;
		public string Account = "";
		public string Password = "";
		public string Invite = "";

        void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);

            //DontDestroyOnLoad(gameObject);

            InitGame();
        }

        void InitGame()
        {

            SwitchToScene("LoginScene");
        }

        // Use this for initialization
        void Start()
        {
            Debug.Log("GM_CORE: start");
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SwitchToScene(string sceneName)
        {
            //unload
            if (SceneManager.GetSceneByName(curSceneName).isLoaded)
                SceneManager.UnloadSceneAsync(curSceneName);

            //load async
            StartCoroutine(LoadSceneJob(sceneName));
            curSceneName = sceneName;

        }

        public void ExitGame()
        {
            Application.Quit();
        }

        IEnumerator LoadSceneJob(string sceneName)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            while (!asyncLoad.isDone)
            {
                yield return null;
            }
            
            //update lightings
            if (GameObject.Find("GM_Settings") != null)
            {
                settings = GameObject.Find("GM_Settings").GetComponent<GM.GM_Settings>();
                RenderSettings.skybox = settings.skybox;
            }

        }

        public bool Login(string un, string pwd, UI_Account acc)
        {
            WWWForm form = new WWWForm();
            form.AddField("account", un);
            form.AddField("password", pwd);
            accountManager = acc;
            NetworkManager.Instance.Post(form, "login", OnLoginComplete);
            return true;
        }

        public void OnLoginComplete(bool success, Reply reply)
        {
            if (success)
            {
                Debug.LogFormat("Login success! {0}", reply.Detail);
                SwitchToScene("MainMenu");
            }
            else
            {
                accountManager.loginFallback(false);
                Debug.Log("Login failed!");
            }
        }

        public void Signup(string un, string pwd, string email, UI_Account acc)
        {
            WWWForm form = new WWWForm();
            form.AddField("account", un);
            form.AddField("password", pwd);
            form.AddField("email", email);
            accountManager = acc;
            NetworkManager.Instance.Post(form, "signup", OnSignupComplete);
            //SwitchToScene("LoginScene");
        }

        public void OnSignupComplete(bool success, Reply reply)
        {
            if (success)
            {
                Debug.LogFormat("Signup success! {0}", reply.Detail);
                SwitchToScene("LoginScene");
            }
            else
            {
                accountManager.signupFallback(false);
                Debug.Log("Signup failed!");
            }
        }

        #region ChemixExtension
        public ExperimentalSetup experimentalSetup
        {
            get;
            set;
        } = new ExperimentalSetup();

        public InstrumentsListAsset instrumentListAsset
        {
            get { return m_InstrumentListAsset; }
        }

        [SerializeField]
        private InstrumentsListAsset m_InstrumentListAsset;
        #endregion
    }
}

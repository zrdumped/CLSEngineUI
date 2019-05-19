﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace GM
{
    public class GM_Core : MonoBehaviour
    {

        public static GM_Core instance = null;

        public string testAccount = "a";
        public string testPassword = "b";

        private string curSceneName = "BaseScene";
        private GM_Settings settings;

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

        public bool Login(string un, string pwd)
        {
            if(un == testAccount && pwd == testPassword)
            {
                SwitchToScene("MainMenu");
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Signup(string un, string pwd, string email)
        {
            SwitchToScene("LoginScene");
        }
    }
}

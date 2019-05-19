using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace Chemix.Network
{
    public class Reply
    {
        public bool Success;
        public string Detail;
    }

    public class GameReply
    {
        public bool Success;
        public string[] Values;
    }

    public class NetworkManager : Singleton<NetworkManager>
    {
        public delegate void OnReply(bool success, Reply reply);
        public delegate void OnGameReply(bool success, GameReply gameReply);

        public void Ping()
        {
            Debug.Log("NetworkManager: try ping...");
            StartCoroutine(PingRequest());
        }

        IEnumerator PingRequest()
        {
            UnityWebRequest uwr = UnityWebRequest.Get(hosturl);
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError)
            {
                Debug.Log("NetworkManager/Fail: " + uwr.error);
            }
            else
            {
                Debug.Log("NetworkManager/Success: " + uwr.downloadHandler.text);
            }
        }

        public void Post(WWWForm form, string suburl, OnReply onReply)
        {
            StartCoroutine(PostRequest(form, suburl, onReply));
        }

        IEnumerator PostRequest(WWWForm form, string suburl, OnReply onReply)
        {
            UnityWebRequest uwr = UnityWebRequest.Post(hosturl + "/" + suburl, form);
            yield return uwr.SendWebRequest();
            
            if (uwr.isNetworkError)
            {
                Debug.Log("NMPOST/ERROR: " + uwr.error);
                onReply(false, new Reply());
            }
            else
            {
                Debug.Log("NMPOST: " + uwr.downloadHandler.text);
                var reply = JsonUtility.FromJson<Reply>(uwr.downloadHandler.text);
                if (reply.Success)
                {
                    onReply(true, reply);
                }
                else
                {
                    onReply(false, reply);
                }
            }
        }
        /*
        public void Signup()
        {
            WWWForm form = new WWWForm();
            form.AddField("account", account);
            form.AddField("password", password);
            form.AddField("email", email);
            StartCoroutine(PostRequest(form, "signup"));
        }

        public void Login()
        {
            WWWForm form = new WWWForm();
            form.AddField("account", account);
            form.AddField("password", password);
            StartCoroutine(PostRequest(form, "login"));
        }

        public void SaveData()
        {
            WWWForm form = new WWWForm();
            form.AddField("account", account);
            form.AddField("password", password);
            form.AddField("key", key);
            form.AddField("value", value);
            StartCoroutine(PostRequest(form, "scene/save"));
        }

        public void LoadData()
        {
            WWWForm form = new WWWForm();
            form.AddField("account", account);
            form.AddField("password", password);
            form.AddField("key", key);
            StartCoroutine(PostRequest(form, "scene/load"));
        }
        */
        IEnumerator GetRequest(string uri)
        {
            UnityWebRequest uwr = UnityWebRequest.Get(uri);
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError)
            {
                Debug.Log("Error While Sending: " + uwr.error);
            }
            else
            {
                Debug.Log("Received: " + uwr.downloadHandler.text);
            }
        }


        [SerializeField]
        private string hosturl = "localhost:8080";
        [Header("User")]
        [SerializeField]
        private string account;
        [SerializeField]
        private string password;
        [SerializeField]
        private string email;
        [Header("SaveLoad")]
        [SerializeField]
        private string key;
        [SerializeField]
        private string value;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class UI_Main : MonoBehaviour
    {
        private GM.GM_Core gm;


        // Use this for initialization
        void Start()
        {
            gm = GameObject.Find("GameManager").GetComponent<GM.GM_Core>();
        }

        public void CreateNewExperiment_OnClick()
        {
            gm.SwitchToScene("BuildExperiment");
        }

        public void TestTracing_OnClick()
        {
            gm.SwitchToScene("TracingScene");
        }
    }
}

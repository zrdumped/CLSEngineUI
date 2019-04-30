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

        // Update is called once per frame
        void Update()
        {

        }

        public void CreateNewExperiment_OnClick()
        {
            gm.SwitchToScene("BuildExperiment");
        }
    }
}

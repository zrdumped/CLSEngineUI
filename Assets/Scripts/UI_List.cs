using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace UI {
    public class UI_List : MonoBehaviour {

        public GameObject Bag;
        


        private RectTransform typePanel;


        // Use this for initialization
        void Start() {
            typePanel = gameObject.transform.Find("TypePanel").gameObject.GetComponent<RectTransform>();
            Bag.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {

        }



        public void ClickOnRootButton()
        {
            this.GetComponent<UI_Button>().MoveAndChangeColor();
            if(Bag.activeSelf)
                Bag.GetComponent<UI_Bag>().CloseBag();
        }

        public void ClickOnType(string BagType)
        {
            Bag.GetComponent<UI_Bag>().OpenBag((UI_Bag.BagType)Enum.Parse(typeof(UI_Bag.BagType), BagType));
        }
    }
}

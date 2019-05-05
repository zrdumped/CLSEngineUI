using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace UI {
    public class UI_List : MonoBehaviour {
        public Color[] panelColor = {new Color(200, 200, 200, 100) / 255,
            new Color(0, 0, 0, 100) / 255};
        public float[] borderA = {0, 1};
        public float[] linkContentA = { 0.5f, 1 };
        public GameObject screen;
        public GameObject Bag;
        

        private GameObject border;
        private GameObject linkContent;
        private GameObject panel;
        private RectTransform typePanel;
        private bool activated = false;
        private bool moving = false;
        private int startPanelX = -190;

        // Use this for initialization
        void Start() {
            border = gameObject.transform.Find("Border").gameObject;
            linkContent = gameObject.transform.Find("Link Content").gameObject;
            panel = gameObject.transform.Find("Panel").gameObject;
            typePanel = gameObject.transform.Find("TypePanel").gameObject.GetComponent<RectTransform>();
            startPanelX = (int)typePanel.anchoredPosition.x;
            screen.SetActive(false);
            Bag.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (moving)
            {
                if (activated)
                {
                    //Debug.Log(typePanel.anchoredPosition.x);
                    float step = 1000 * Time.deltaTime;
                    typePanel.anchoredPosition = Vector2.MoveTowards(typePanel.anchoredPosition, new Vector2(0, typePanel.anchoredPosition.y), step);
                    if (typePanel.anchoredPosition.x == 0)
                        moving = false;
                }
                else
                {
                    float step = 1000 * Time.deltaTime;
                    typePanel.anchoredPosition = Vector2.MoveTowards(typePanel.anchoredPosition, new Vector2(startPanelX, typePanel.anchoredPosition.y), step);
                    if (typePanel.anchoredPosition.x == startPanelX)
                        moving = false;
                }
            }
        }

        void UpdateColor()
        {
            int id = activated ? 1 : 0;
            Color srcColor = border.GetComponent<Image>().color;
            srcColor.a = borderA[id];
            border.GetComponent<Image>().color = srcColor;
            linkContent.GetComponent<CanvasGroup>().alpha = linkContentA[id];
            panel.GetComponent<Image>().color = panelColor[id];
        }

        public void ClickOnRootButton()
        {
            //change color
            activated = !activated;
            moving = true;
            UpdateColor();
            if(activated)
                screen.SetActive(true);
            else
                screen.SetActive(false);
            if(Bag.activeSelf)
                Bag.GetComponent<UI_Bag>().CloseBag();
        }

        public void ClickOnType(string BagType)
        {
            Bag.GetComponent<UI_Bag>().OpenBag((UI_Bag.BagType)Enum.Parse(typeof(UI_Bag.BagType), BagType));
        }
    }
}

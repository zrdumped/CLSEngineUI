using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace UI
{
    public class UI_Button : MonoBehaviour
    {

        public Color[] panelColor = {new Color(200, 200, 200, 100) / 255,
            new Color(0, 0, 0, 100) / 255};
        public float[] borderA = { 0, 1 };
        public float[] linkContentA = { 0.5f, 1 };
        public GameObject screen;

        private GameObject border;
        private GameObject linkContent;
        private GameObject panel;

        public int startPanelX = -190;
        public int endPanelX = 0;
        public int movingSpeed = 1000;
        public RectTransform targetPanel;

        private bool activated = false;
        private bool moving = false;


        // Use this for initialization
        void Start()
        {
            border = gameObject.transform.Find("Border").gameObject;
            linkContent = gameObject.transform.Find("Link Content").gameObject;
            panel = gameObject.transform.Find("Panel").gameObject;
            screen.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (moving)
            {
                if (activated)
                {
                    //Debug.Log(typePanel.anchoredPosition.x);
                    float step = movingSpeed * Time.deltaTime;
                    targetPanel.anchoredPosition = Vector2.MoveTowards(targetPanel.anchoredPosition, new Vector2(endPanelX, targetPanel.anchoredPosition.y), step);
                    if (targetPanel.anchoredPosition.x == endPanelX)
                        moving = false;
                }
                else
                {
                    float step = movingSpeed * Time.deltaTime;
                    targetPanel.anchoredPosition = Vector2.MoveTowards(targetPanel.anchoredPosition, new Vector2(startPanelX, targetPanel.anchoredPosition.y), step);
                    if (targetPanel.anchoredPosition.x == startPanelX)
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

        public void MoveAndChangeColor()
        {

            //change color
            activated = !activated;
            moving = true;
            UpdateColor();
            if (activated)
            {
                screen.SetActive(true);
                Camera.main.gameObject.GetComponent<Lab.Lab_Controller>().enabled = false;
            }
            else
            {
                screen.SetActive(false);
                Camera.main.gameObject.GetComponent<Lab.Lab_Controller>().enabled = true;
            }
        }
    }
}

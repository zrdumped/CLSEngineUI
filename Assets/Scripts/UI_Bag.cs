using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UI_Bag : MonoBehaviour
    {
        public enum BagType { TEST, TOOL, CLOSED };
        public string[][] names = { new string[]{ "Test", "Bottle" },
            new string[] { "Bottle" } };
        public GameObject itemPrefab;

        private BagType curBt = BagType.CLOSED;
        private GameObject BagItems;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void AddItems(BagType bt)
        {
            for(int i = 0; i < names[(int)bt].Length; i++)
            {
                GameObject item = Instantiate(itemPrefab);
                item.GetComponent<Image>().sprite = Resources.Load("Images/" + names[(int)bt][i] + ".png", typeof(Sprite)) as Sprite;
                item.transform.parent = BagItems.transform;
            }
        }

        public void CloseBag()
        {
            curBt = BagType.CLOSED;
            gameObject.SetActive(false);
        }

        public void OpenBag(BagType bt)
        {
            if (bt == curBt)
            {
                CloseBag();
            }
            else
            {
                BagItems = transform.Find("BagItems").gameObject;
                AddItems(bt);
                curBt = bt;
                gameObject.SetActive(true);
                Debug.Log("hi");
            }
        }
    }
}

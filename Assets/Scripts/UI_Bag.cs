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
                item.GetComponent<Image>().sprite = Resources.Load("Images/" + names[(int)bt][i], typeof(Sprite)) as Sprite;
                item.transform.parent = BagItems.transform;
                item.transform.localScale = new Vector3(1, 1, 1);
            }
        }

        public void CloseBag()
        {
            //Debug.Log("Closed");
            //BagItems = transform.Find("BagItems").gameObject;
            //int childCount = BagItems.transform.childCount;
            //for (int i = 0; i < childCount; i++)
            //{
            //    Destroy(BagItems.transform.GetChild(0).gameObject);
            //}
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
                gameObject.SetActive(true);
                BagItems = transform.Find("BagItems").gameObject;
                int childCount = BagItems.transform.childCount;
                //Debug.Log(childCount);
                for (int i = childCount-1; i >= 0; i--)
                {
                    Destroy(BagItems.transform.GetChild(i).gameObject);
                }
                AddItems(bt);
                curBt = bt;
            }
        }
    }
}

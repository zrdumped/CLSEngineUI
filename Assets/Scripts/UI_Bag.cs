using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UI_Bag : MonoBehaviour
    {
        public enum BagType { CONTAINER, TOOL, SUBSTANCE, PRESET, CLOSED };
        public string[][] names = { new string[]{ "Test", "Bottle", "Flask", "Beaker" },
            new string[] { "Tripod", "Dropper", "Burner", "Match", "Spoon", "Thermometer" },
            new string[] { "Copper"},
            new string[] {"IronStand", "IronStandComplex"},
        };
        //public static Dictionary<string, float> padding = new Dictionary<string, float>{
        //    {"Test", 0}, {"Bottle", 0}, {"Flask", 0}, {"Beaker", 0.338f},
        //    {"Tripod", 0}, {"Dropper", 0},
        //    {"IronStand", 0 }
        //    };

        public GameObject itemPrefab;

        private BagType curBt = BagType.CLOSED;
        private GameObject BagItems;

        private Lab.Lab_Table table;
        // Use this for initialization
        void Start()
        {
            table = GameObject.Find("Table").GetComponent<Lab.Lab_Table>();
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
                item.transform.SetParent(BagItems.transform, false);//  parent = BagItems.transform;
                item.transform.localScale = new Vector3(1, 1, 1);
                Debug.Log(names[(int)bt][i]);
                item.name = names[(int)bt][i];
                item.GetComponent<Button>().onClick.AddListener(delegate ()
                {
                    this.AddItem(item);
                });
            }
        }

        public void AddItem(GameObject sender)
        {
            GameObject newItem = Instantiate(Resources.Load(sender.name) as GameObject);
            if (table.isFull())
                newItem.transform.position = table.addAnchor.transform.position;
            else
                table.addObject(0, newItem);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lab
{
    public class Lab_Anchor : MonoBehaviour
    {
        private int anchorID;
        private GameObject curObj;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void setID(int i)
        {
            anchorID = i;
        }

        public int getID()
        {
            return anchorID;
        }

        public void setObj(GameObject obj)
        {
            curObj = obj;
            //Debug.Log(gameObject.transform.position);
            curObj.transform.position = gameObject.transform.position;
        }

        public void removeObj()
        {
            curObj = null;
        }

        public GameObject getObj()
        {
            return curObj;
        }
    }
}

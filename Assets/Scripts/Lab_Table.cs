using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lab
{
    public class Lab_Table : MonoBehaviour
    {
        private int anchorNum = 0;
        private int[] anchorState = new int[5];
        private GameObject[] anchors = new GameObject[5];
        // Use this for initialization
        void Start()
        {
            for(int i = 0; i < gameObject.transform.childCount; i++)
            {
                anchors[i] = gameObject.transform.GetChild(i).gameObject;
                anchors[i].GetComponent<Lab_Anchor>().setID(i);
                anchorState[i] = 0;
                anchorNum++;
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public bool addObject(int anchorId, GameObject obj)
        {
            int leftCount = 0, rightCount = 0;
            for(int i = anchorId - 1; i >= 0; i--)
            {
                if (anchorState[i] == 1)
                    leftCount++;
                else
                    break;
            }
            for (int i = anchorId + 1; i < anchorNum; i++)
            {
                if (anchorState[i] == 1)
                    rightCount++;
                else
                    break;
            }

            if (leftCount > rightCount)
            {
                for(int i = anchorId; i < anchorNum; i++)
                {
                    if(anchorState[i] == 0)
                    {
                        anchorState[i] = 1;
                        for (int j = i; j > anchorId; j--)
                        {
                            anchors[j].GetComponent<Lab_Anchor>().setObj(anchors[j - 1].GetComponent<Lab_Anchor>().getObj());
                        }
                        anchors[anchorId].GetComponent<Lab_Anchor>().setObj(obj);
                        return true;
                    }
                }
                for (int i = anchorId - 1; i >= 0; i--)
                {
                    if (anchorState[i] == 0)
                    {
                        anchorState[i] = 1;
                        for (int j = i; j < anchorId; j++)
                        {
                            anchors[j].GetComponent<Lab_Anchor>().setObj(anchors[j + 1].GetComponent<Lab_Anchor>().getObj());
                        }
                        anchors[anchorId].GetComponent<Lab_Anchor>().setObj(obj);
                        return true;
                    }
                }
            }
            else
            {
                for (int i = anchorId; i >= 0; i--)
                {
                    if (anchorState[i] == 0)
                    {
                        anchorState[i] = 1;
                        for (int j = i; j < anchorId; j++)
                        {
                            anchors[j].GetComponent<Lab_Anchor>().setObj(anchors[j + 1].GetComponent<Lab_Anchor>().getObj());
                        }
                        anchors[anchorId].GetComponent<Lab_Anchor>().setObj(obj);
                        return true;
                    }
                }
                for (int i = anchorId + 1; i < anchorNum; i++)
                {
                    if (anchorState[i] == 0)
                    {
                        anchorState[i] = 1;
                        for (int j = i; j > anchorId; j--)
                        {
                            anchors[j].GetComponent<Lab_Anchor>().setObj(anchors[j - 1].GetComponent<Lab_Anchor>().getObj());
                        }
                        anchors[anchorId].GetComponent<Lab_Anchor>().setObj(obj);
                        return true;
                    }
                }
            }
            return false;
        }


        public void replaceObject(int anchorId, GameObject obj)
        {
            anchors[anchorId].GetComponent<Lab_Anchor>().setObj(obj);
        }

        public void removeObject(int anchorId)
        {
            int i; 
            int leftCount = 0, rightCount = 0;
            for (i = anchorId - 1; i >= 0; i--)
            {
                if (anchorState[i] == 1)
                    leftCount++;
                else
                    break;
            }
            for (i = anchorId + 1; i < anchorNum; i++)
            {
                if (anchorState[i] == 1)
                    rightCount++;
                else
                    break;
            }
            i = anchorId;

            if (leftCount > rightCount)
            {
                for (; i < anchorNum - 1; i++)
                {
                    if(anchorState[i + 1] == 1)
                    {
                        anchors[i].GetComponent<Lab_Anchor>().setObj(anchors[i + 1].GetComponent<Lab_Anchor>().getObj());
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                for (; i > 0; i--)
                {
                    if (anchorState[i - 1] == 1)
                    {
                        anchors[i].GetComponent<Lab_Anchor>().setObj(anchors[i - 1].GetComponent<Lab_Anchor>().getObj());
                    }
                    else
                    {
                        break;
                    }
                }
            }
            anchorState[i] = 0;
            anchors[i].GetComponent<Lab_Anchor>().removeObj();
            return;
        }

        public int onAnchor(GameObject obj)
        {
            for(int i = 0; i < anchorNum; i++)
            {
                if (anchors[i].GetComponent<Lab_Anchor>().getObj() == obj)
                    return anchors[i].GetComponent<Lab_Anchor>().getID();
            }
            return -1;
        }
    }
}

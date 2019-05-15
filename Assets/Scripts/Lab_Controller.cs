using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lab
{
    public class Lab_Controller : MonoBehaviour
    {
        public GameObject table;
        private GameObject Substitude;
        private Material SubsMaterial; 
        private string holdingName = "";
        private GameObject holdingObject = null;
        private int curAnchor = -1;
        // Use this for initialization
        void Start()
        {
            SubsMaterial= new Material(Shader.Find("Unlit/TransparentColor"));
            SubsMaterial.color = new Color(255, 0, 255, 96) / 255;
        }

        // Update is called once per frame
        void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (holdingObject == null && Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(ray, out hit, 1000))
                {
                    Debug.Log(hit.transform.name);
                    if (hit.transform.tag == "Object")
                    {
                        holdingName = hit.transform.name;
                        holdingObject = GameObject.Find(holdingName);
                        //angle = target.transform.rotation.eulerAngles.y;
                        //target.GetComponent<SphereCollider> ().enabled = false;
                        holdingObject.GetComponent<MeshCollider>().enabled = false;
                        
                        if ((curAnchor = table.GetComponent<Lab_Table>().onAnchor(hit.transform.gameObject)) >= 0)
                        {
                            //curAnchor = hit.transform.gameObject.GetComponent<Lab_Anchor>().getID();
                            Substitude = Instantiate(holdingObject);
                            Substitude.GetComponent<Renderer>().material = SubsMaterial;
                            table.GetComponent<Lab_Table>().replaceObject(curAnchor, Substitude);
                        }
                    }
                }
            }

            if (holdingObject != null && Input.GetMouseButtonUp(0))
            {
                if (Substitude)
                {
                    //holdingObject.transform.position = Substitude.transform.position;
                    table.GetComponent<Lab_Table>().replaceObject(curAnchor, holdingObject);
                    curAnchor = -1;
                    Destroy(Substitude);
                }
                else
                {
                    holdingObject.transform.Translate(Vector3.down * 0.5f, Space.Self);
                }
                holdingObject.GetComponent<MeshCollider>().enabled = true;
                holdingName = "";
                holdingObject = null;
            }

            if (holdingObject != null && Physics.Raycast(ray, out hit, 1000))
            {
                holdingObject.transform.position = hit.point;


                holdingObject.transform.up = hit.normal;
                //target.transform.eulerAngles = new Vector3(0, angle, 0);
                holdingObject.transform.Translate(Vector3.up * 0.5f, Space.Self);

                if (Substitude == null && hit.transform.tag == "Anchor")
                {
                    curAnchor = hit.transform.gameObject.GetComponent<Lab_Anchor>().getID();
                    Substitude = Instantiate(holdingObject);
                    Substitude.GetComponent<Renderer>().material = SubsMaterial;
                        if (!table.GetComponent<Lab_Table>().addObject(curAnchor, Substitude))
                        {
                            Debug.LogError("No Enough Space");
                        }
                    //Substitude.transform.position = hit.transform.position;
                }
                else if (Substitude != null && hit.transform.tag != "Anchor")
                {
                    table.GetComponent<Lab_Table>().removeObject(curAnchor);
                    curAnchor = -1;
                    Destroy(Substitude);
                }

            }
        }
    }
}

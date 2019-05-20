using Chemix;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        private float timer = 0;
        private bool pressed;
        public float internals = 0.3f;

        public GameObject substanceEditor;
        public GameObject textEditor;
        public GameObject screen;
        private Container container;

        public GameObject textSizeSlider;
        public GameObject textRSlider;
        public GameObject textGSlider;
        public GameObject textBSlider;


        //public List<string> substanceType = new List<string> { "空" };

        // Use this for initialization
        void Start()
        {
            SubsMaterial= new Material(Shader.Find("Unlit/TransparentColor"));
            SubsMaterial.color = new Color(255, 0, 255, 96) / 255;

            

            screen.SetActive(false);
            substanceEditor.SetActive(false);
            textEditor.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                pressed = true;
                timer = 0;
                //FetchObject();
            }

            if (Input.GetMouseButtonUp(0))
            {
                Debug.Log(timer + " " + internals);
                if (timer  < internals)
                {
                    //single click
                    EditObject();
                }
                else
                {
                    ReleaseObject();
                }
                timer = 0;
                pressed = false;
            }

            if (pressed)
            {
                timer += Time.deltaTime;
                if (timer > internals)
                {
                    FetchObject();
                    pressed = false;
                }
            }

            if (holdingObject != null)
            {
                MoveObject();
            }
        }

        void FetchObject()
        {
            if (holdingObject != null) return;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000))
            {
                //Debug.Log(hit.transform.name);
                if (hit.transform.tag == "Object")
                {
                    holdingName = hit.transform.name;
                    holdingObject = hit.transform.gameObject;
                    //angle = target.transform.rotation.eulerAngles.y;
                    //target.GetComponent<SphereCollider> ().enabled = false;
                    if(holdingObject.GetComponent<MeshCollider>() != null)
                        holdingObject.GetComponent<MeshCollider>().enabled = false;
                    else if(holdingObject.GetComponent<BoxCollider>() != null)
                        holdingObject.GetComponent<BoxCollider>().enabled = false;

                    if ((curAnchor = table.GetComponent<Lab_Table>().onAnchor(hit.transform.gameObject)) >= 0)
                    {
                        //curAnchor = hit.transform.gameObject.GetComponent<Lab_Anchor>().getID();
                        Substitude = Instantiate(holdingObject);
                        if (Substitude.GetComponent<Renderer>() != null)
                        {
                            Substitude.GetComponent<Renderer>().material = SubsMaterial;
                        }
                        if (Substitude.GetComponentInChildren<TextMesh>() != null)
                        {
                            Substitude.GetComponentInChildren<TextMesh>().gameObject.SetActive(false);
                        }
                        //for (int i = 0; i < Substitude.GetComponentsInChildren<Renderer>().Length; i++)
                        //{
                        //    Material[] ms = Substitude.GetComponentsInChildren<Renderer>()[i].materials;
                        //    for (int j = 0; j < ms.Length; j++)
                        //    {
                        //        Substitude.GetComponentsInChildren<Renderer>()[i].materials[j] = SubsMaterial;
                        //    }
                        //}
                        foreach (Renderer r in Substitude.GetComponentsInChildren<Renderer>())
                        {
                            r.material = SubsMaterial;
                        }
                        table.GetComponent<Lab_Table>().replaceObject(curAnchor, Substitude);
                    }
                }
                else if (hit.transform.tag == "Text")
                {
                    holdingName = hit.transform.name;
                    holdingObject = hit.transform.gameObject;
                    holdingObject.GetComponent<BoxCollider>().enabled = false;
                }
            }
        }

        void ReleaseObject()
        {
            if (holdingObject == null) return;
            if (holdingObject.tag == "Object")
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
                if (holdingObject.GetComponent<MeshCollider>() != null)
                    holdingObject.GetComponent<MeshCollider>().enabled = true;
                else if (holdingObject.GetComponent<BoxCollider>() != null)
                    holdingObject.GetComponent<BoxCollider>().enabled = true;
            }
            else if (holdingObject.tag == "Text")
            {
                holdingObject.GetComponent<BoxCollider>().enabled = true;
            }
            holdingName = "";
            holdingObject = null;
        }

        void MoveObject()
        {
            if (holdingObject == null) return;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000))
            {
                if (holdingObject.tag == "Object")
                {
                    holdingObject.transform.position = hit.point;
                    holdingObject.transform.up = hit.normal;
                    //target.transform.eulerAngles = new Vector3(0, angle, 0);
                    holdingObject.transform.Translate(Vector3.up * 0.5f, Space.Self);



                    if (Substitude == null)
                    {
                        if (hit.transform.tag == "Anchor")
                        {
                            curAnchor = hit.transform.gameObject.GetComponent<Lab_Anchor>().getID();
                        }
                        //else if (hit.transform.tag == "Object")
                        //{
                        //    curAnchor = table.GetComponent<Lab_Table>().onAnchor(hit.transform.gameObject);
                        //    if (curAnchor == -1) return;
                        //}
                        else
                        {
                            return;
                        }
                        if (table.GetComponent<Lab_Table>().isFull()) return;
                        Substitude = Instantiate(holdingObject);
                        if (Substitude.GetComponent<Renderer>() != null) {
                            Substitude.GetComponent<Renderer>().material = SubsMaterial;
                        }
                        if (Substitude.GetComponentInChildren<TextMesh>() != null)
                        {
                            Substitude.GetComponentInChildren<TextMesh>().gameObject.SetActive(false);
                        }
                        //for (int i = 0; i < Substitude.GetComponentsInChildren<Renderer>().Length; i++)
                        //{
                        //    Material[] ms = Substitude.GetComponentsInChildren<Renderer>()[i].materials;
                        //    for (int j = 0; j < ms.Length; j++)
                        //    {
                        //        ms[j] = SubsMaterial;
                        //    }
                        //}
                        foreach(Renderer r in Substitude.GetComponentsInChildren<Renderer>())
                        {
                            r.material = SubsMaterial;
                        }
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
                else if (holdingObject.tag == "Text")
                {
                    if (hit.transform.tag == "BlackBoard")
                    {
                        holdingObject.transform.position = hit.point + new Vector3(0, 0, -0.5f);
                        //holdingObject.transform.Translate(Vector3.up * 0.5f, Space.Self);
                    }
                    else
                    {
                        holdingObject.GetComponent<BoxCollider>().enabled = true;
                        holdingName = "";
                        holdingObject = null;
                    }
                }

            }
        }

        void EditObject()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000))
            {
                if(hit.transform.gameObject.GetComponent<Container>() != null
                    && hit.transform.gameObject.GetComponent<Container>().enabled == true)
                {
                    substanceEditor.SetActive(true);
                    substanceEditor.transform.position = Input.mousePosition;
                    container = hit.transform.gameObject.GetComponent<Container>();
                    screen.SetActive(true);
                    Camera.main.gameObject.GetComponent<Lab.Lab_Controller>().enabled = false;

                    substanceEditor.GetComponentInChildren<Dropdown>().value = container.type;
                    substanceEditor.GetComponentInChildren<InputField>().text = container.quantity.ToString();

                }
                else if(hit.transform.tag == "Text")
                {
                    textEditor.SetActive(true);
                    textEditor.transform.position = Input.mousePosition;
                    foreach(UI.UI_Slider slider in textEditor.GetComponentsInChildren<UI.UI_Slider>())
                    {
                        slider.targetText = hit.transform.gameObject;
                    }
                    textSizeSlider.GetComponent<Slider>().value = hit.transform.localScale.x / hit.transform.gameObject.GetComponent<Lab_Text>().srcScale.x;
                    textRSlider.GetComponent<Slider>().value = hit.transform.gameObject.GetComponent<Renderer>().material.GetColor("_Color").r * 255;
                    textGSlider.GetComponent<Slider>().value = hit.transform.gameObject.GetComponent<Renderer>().material.GetColor("_Color").g * 255;
                    textBSlider.GetComponent<Slider>().value = hit.transform.gameObject.GetComponent<Renderer>().material.GetColor("_Color").b * 255;
                    screen.SetActive(true);
                    Camera.main.gameObject.GetComponent<Lab.Lab_Controller>().enabled = false;
                }
            }
        }

        public void ConfirmSubstance()
        {
            int result = substanceEditor.GetComponentInChildren<Dropdown>().value;
            if (result == 0)
            {
                substanceEditor.GetComponentInChildren<InputField>().text = 0.ToString();
                substanceEditor.GetComponentInChildren<InputField>().interactable = false;
            }
            else
            {
                substanceEditor.GetComponentInChildren<InputField>().interactable = true;
            }
            container.type = result;
            container.typeName = substanceEditor.GetComponentInChildren<Dropdown>().options[result].text;//substanceType[result];
        }

        public void ConfirmQuantity()
        {
            float result = float.Parse(substanceEditor.GetComponentInChildren<InputField>().text);
            container.quantity = result;
        }

        public void Close()
        {
            screen.SetActive(false);
            Camera.main.gameObject.GetComponent<Lab.Lab_Controller>().enabled = true;
            if (substanceEditor.activeSelf)
            {
                substanceEditor.SetActive(false);
                container.GetComponentInChildren<TextMesh>().text = container.typeName.ToString() + " " + container.quantity + "mol";
                container = null;
            }else if (textEditor.activeSelf)
            {
                textEditor.SetActive(false);
            }
        }
    }
}

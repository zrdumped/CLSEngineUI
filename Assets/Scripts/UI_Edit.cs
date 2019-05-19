using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chemix;

public class UI_Edit : MonoBehaviour
{
    public GameObject thisPanel;

    public UI_Step steps;

    public GameManager.ExperimentalSetup output;
    public GameObject titleText;
    public GameObject detailText;
    public GameObject objectList;


    // Use this for initialization
    void Start()
    {
        output = GameObject.Find("GameManager").GetComponent<GM.GM_Core>().experimentalSetup;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Open()
    {
        thisPanel.SetActive(true);
        Camera.main.gameObject.GetComponent<Lab.Lab_Controller>().enabled = false;
    }

    public void Complete()
    {
        output.title.color = titleText.GetComponent<Renderer>().material.GetColor("_Color");
        output.title.position = titleText.transform.position;
        output.title.size = titleText.transform.localScale.x / titleText.GetComponent<Lab_Text>().srcScale.x;

        output.detail.color = detailText.GetComponent<Renderer>().material.GetColor("_Color");
        output.detail.position = detailText.transform.position;
        output.detail.size = detailText.transform.localScale.x / detailText.GetComponent<Lab_Text>().srcScale.x;

        for(int i = 0; i < objectList.transform.childCount; i++)
        {
            GameObject obj = objectList.transform.GetChild(i).gameObject;
            GameManager.InstrumentInfo info = new GameManager.InstrumentInfo();
            info.type = obj.name;
            info.quaternion = obj.transform.rotation;
            info.position = obj.transform.position;
            if(obj.GetComponent<Container>() != null)
            {
                Container c = obj.GetComponent<Container>();
                info.formula = c.typeName;
                info.mass = c.quantity;
            }
            output.instrumentInfos.Add(info);
        }
    }
}

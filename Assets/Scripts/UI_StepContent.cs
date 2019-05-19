using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_StepContent : MonoBehaviour {
    
    public int stepID = 0;
    public string stepTitle = "请输入标题";

    public bool isBig = true;

    public GameObject controller;

    //public enum eventName { NONE, TEST1, TEST2};
    //public int eID = 0;
    public string eName;
    public enum eventType { NORMAL, TRUE, FALSE};
    public eventType tName = eventType.NORMAL;



    // Use this for initialization
    void Start () {
        eName = "默认";
        //Debug.Log("HELLO: " + eName);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ClickOnMenu()
    {
        if(isBig)
            controller.GetComponent<UI_Step>().updateFromBigStep(stepID);
        else
            controller.GetComponent<UI_Step>().updateFromSmallStep(stepID);
    }
}

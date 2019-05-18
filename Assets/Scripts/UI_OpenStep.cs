using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_OpenStep : MonoBehaviour {
    public GameObject thisPanel;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Open()
    {
        thisPanel.SetActive(true);
        Camera.main.gameObject.GetComponent<Lab.Lab_Controller>().enabled = false;
    }
}

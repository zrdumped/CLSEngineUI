using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Button : MonoBehaviour {
    public GameObject stepPanel;
    public GameObject objectPanel;
    public GameObject settingsPanel;
    public GameObject completeButton;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OpenObjectMenu()
    {
        Camera.main.gameObject.GetComponent<Lab.Lab_Controller>().enabled = false;
        objectPanel.GetComponent<UI.UI_List>().ClickOnRootButton();
    }

    public void OpenSettingsMenu()
    {
        Camera.main.gameObject.GetComponent<Lab.Lab_Controller>().enabled = false;
        settingsPanel.GetComponent<UI.UI_Menu>().MoveAndChangeColor();
    }

    public void FinishEditing()
    {
        Camera.main.gameObject.GetComponent<Lab.Lab_Controller>().enabled = false;
        completeButton.GetComponent<UI_Edit>().Complete();
    }

    public void OpenStepMenu()
    {
        stepPanel.SetActive(true);
        Camera.main.gameObject.GetComponent<Lab.Lab_Controller>().enabled = false;
    }
}

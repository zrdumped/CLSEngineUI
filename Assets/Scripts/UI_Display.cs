using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Display : MonoBehaviour {

    public GameObject caveCameras;
    public GameObject caveLookCamera;
    public GameObject mainCamera;
    public GameObject VRCameras;
    public GameObject VRLookCamera;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Back()
    {
        caveLookCamera.SetActive(false);
        VRLookCamera.SetActive(false);
        mainCamera.SetActive(true);
    }

    public void toCave()
    {
        VRLookCamera.SetActive(false);
        mainCamera.SetActive(false);
        caveLookCamera.SetActive(true);
    }

    public void toVR()
    {
        caveLookCamera.SetActive(false);
        mainCamera.SetActive(false);
        VRLookCamera.SetActive(true);
    }
}

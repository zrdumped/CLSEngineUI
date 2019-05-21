using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Display : MonoBehaviour {

    public GameObject caveCameras;
    public GameObject caveLookCamera;
    public GameObject mainCamera;
    public GameObject VRCameras;
    public GameObject VRLookCamera;

    public Vector3 camInitPos;

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
        
        if (camInitPos.x != 0 || camInitPos.y != 0)
        {
            Chemix.ChemixEngine.Instance.mainCamera.transform.position = camInitPos;
            camInitPos = new Vector3();
        }
    }

    public void toCave()
    {
        VRLookCamera.SetActive(false);
        mainCamera.SetActive(false);
        caveLookCamera.SetActive(true);

        MoveAwayMainCamera();
    }

    public void toVR()
    {
        caveLookCamera.SetActive(false);
        mainCamera.SetActive(false);
        VRLookCamera.SetActive(true);

        MoveAwayMainCamera();
    }

    public void MoveAwayMainCamera()
    {
        if (camInitPos.x == 0 && camInitPos.y == 0)
        {
            camInitPos = Chemix.ChemixEngine.Instance.mainCamera.transform.position;
            Chemix.ChemixEngine.Instance.mainCamera.transform.position = new Vector3(1000, 1000, 1000);
        }
    }
}

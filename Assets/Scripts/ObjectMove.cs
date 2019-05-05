using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMove : MonoBehaviour {

    private string holdingName = "";
    private GameObject holdingObject = null;
	// Use this for initialization
	void Start () {
		
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
                if (hit.transform.name.Contains("bottle"))
                {
                    holdingName = hit.transform.name;
                    holdingObject = GameObject.Find(holdingName);
                    //angle = target.transform.rotation.eulerAngles.y;
                    //target.GetComponent<SphereCollider> ().enabled = false;
                    holdingObject.GetComponent<MeshCollider>().enabled = false;
                }
            }
        }

        if (holdingObject != null && Input.GetMouseButtonUp(0))
        {
            holdingObject.GetComponent<MeshCollider>().enabled = true;
            holdingObject.transform.Translate(Vector3.down * 0.5f, Space.Self);
            holdingName = "";
            holdingObject = null;
        }

        if (holdingObject != null && Physics.Raycast(ray, out hit, 1000))
        {
            holdingObject.transform.position = hit.point;


            holdingObject.transform.up = hit.normal;
            //target.transform.eulerAngles = new Vector3(0, angle, 0);
            holdingObject.transform.Translate(Vector3.up * 0.5f, Space.Self);

        }
    }
}

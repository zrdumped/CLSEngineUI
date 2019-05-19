using Chemix;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour {
    public int type;
    public string typeName = "空";
    public float quantity = 0;

    //private GM.GM_Core gm;

    // Use this for initialization
    void Start () {
        //gm = GameObject.Find("GameManager").gameObject.GetComponent<GM.GM_Core>();
        type = 0;
        //Debug.Log(this.name);
        if (gameObject.GetComponentInChildren<TextMesh>() != null)
		    gameObject.GetComponentInChildren<TextMesh>().text = this.typeName.ToString() + " " + this.quantity + "mol";

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

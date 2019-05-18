using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour {

    public enum substanceType { EMPTY, HCL, H2SO4};
    public substanceType type = substanceType.EMPTY;
    public float quantity = 0;

	// Use this for initialization
	void Start () {
        //Debug.Log(this.name);
        if(gameObject.GetComponentInChildren<TextMesh>() != null)
		    gameObject.GetComponentInChildren<TextMesh>().text = this.type.ToString() + " " + this.quantity + "mol";
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

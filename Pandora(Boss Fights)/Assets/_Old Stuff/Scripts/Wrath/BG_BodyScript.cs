using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG_BodyScript : MonoBehaviour {

    //Rotate the gameobject either clockwise or counterclockwise.

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(0.0f, 0.0f, -0.5f));
	}

    //public void RaiseCoils() {  }
}

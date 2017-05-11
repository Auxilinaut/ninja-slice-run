using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundCheck : MonoBehaviour {

    public bool ninjaGrounded;
	// Use this for initialization
	void Start () {
        ninjaGrounded = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionStay2D(Collision2D col)
    {
        ninjaGrounded = true;
    }

    void OnCollisionExit2D(Collision2D col)
    {
        ninjaGrounded = false;
    }
}

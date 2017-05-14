using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileCollision : MonoBehaviour {

    private DrawPhaseController drawPhaseScript;

	// Use this for initialization
	void Start () {
        drawPhaseScript = GameObject.FindObjectOfType<DrawPhaseController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
            drawPhaseScript.missileHit = true;
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
            drawPhaseScript.missileHit = false;
    }
}

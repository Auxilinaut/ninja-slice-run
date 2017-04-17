using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour {

    private GameObject ninja;
    private Animator anim;

    // Use this for initialization
    void Start () {
        ninja = GameObject.FindWithTag("Player");
        anim = ninja.GetComponent<Animator>();
        anim.Play("running");
    }
	
    // Update is called once per frame
    void Update () {
		
    }

    public void LoadDrawPhase() {
        SceneManager.LoadScene("DrawPhase", LoadSceneMode.Single);
        //print("Play Button Pressed");
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour {

    private GameObject ninja;
    private Animator anim;
    private CharacterController controller;
    private GameObject playButton;
    private bool playButtonClicked = false;
    private Vector3 moveDirection = Vector3.zero;

    public float speed = 6.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    

    // Use this for initialization
    void Start () {
        ninja = GameObject.FindWithTag("Player");
        anim = ninja.GetComponent<Animator>();
        anim.Play("running");
        ninja.AddComponent<CharacterController>();
        controller = ninja.GetComponent<CharacterController>();
        playButton = GameObject.Find("PlayButton");
    }
	
    // Update is called once per frame
    void Update () {
        if (playButtonClicked) {
            
            
            if (ninja.transform.position.x > 10)
            {
                anim.Play("idle");
                GameObject.FindGameObjectWithTag("Background").GetComponent <BGScroller> ().enabled = false;
                SceneManager.LoadScene("DrawPhase", LoadSceneMode.Single);
            }
            else
            {
                moveDirection = new Vector3(1, 0, 0);
                moveDirection = transform.TransformDirection(moveDirection);
                moveDirection *= speed;
                /*if (Input.GetButton("Jump"))
                    moveDirection.y = jumpSpeed;*/
                //moveDirection.y -= gravity * Time.deltaTime;
                controller.Move(moveDirection * Time.deltaTime);
            }

        }
    }



    public void LoadDrawPhase() {
        playButton.SetActive(false);
        playButtonClicked = true;
        //print("Play Button Pressed");
    }
}
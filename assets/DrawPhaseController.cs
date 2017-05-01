using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DrawPhaseController : MonoBehaviour
{
    private GameObject ninja;
    private GameObject mob;
    private Animator anim;
    private CharacterController controller;

    private DrawLine lines;

    private Button done;

    private int[] angles;

    private string[] moves = new string[3];
    private int currentMove = 0;

    private bool fighting = false;

    private Vector3 moveDirection = Vector3.zero;
    public float speed = 6.0f;
    public float jumpSpeed = 10000.0f;
    public float gravity = 5.0f;

    private BoxCollider2D floor;

    private void doneButton() //done button on click
    {
        fighting = true;
        moves = getMoves();
        done.GetComponentInChildren<Image>().enabled = false;
        done.enabled = false;
    }

    private void Start()
    {
        floor = new BoxCollider2D();
        ninja = GameObject.FindWithTag("Player");
        ninja.AddComponent<CharacterController>();
        controller = ninja.GetComponent<CharacterController>();
        anim = ninja.GetComponent<Animator>();

        lines = GameObject.Find("Main Camera").GetComponent<DrawLine>();
        done = GameObject.Find("Done").GetComponentInChildren<Button>();
        done.onClick.AddListener(doneButton);
    }

    private void Update()
    {
        if (fighting == true) {
            string currMove = moves[currentMove];
            if (currMove == "up")
            {
                moveDirection.y = jumpSpeed;
            }
            moveDirection.y -= gravity;
            controller.Move(moveDirection * Time.deltaTime);
        }
        else{

            angles = lines.angles;

        }

        Debug.Log("angles " + angles[0] + " " + angles[1] + " " + angles[2]);
        
    }

    private void fight()
    {

    }

    private string[] getMoves()
    {
        string[] m = new string[angles.Length];

        for (int i=0; i < angles.Length; i++)
        {
            if (angles[i] > 45 && angles[i] <= 135)
            {
                m[i] = "up";
            } else if (angles[i] > 135 && angles[i] <= 225)
            {
                m[i] = "left";
            }
            else if (angles[i] > 225 && angles[i] < 315)
            {
                m[i] = "down";
            }
            else if (angles[i] <= 45 || angles[i] >= 315)
            {
                m[i] = "right";
            }
        }

        return m;
    }
}
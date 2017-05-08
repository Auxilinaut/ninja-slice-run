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
    private Rigidbody2D bodyNinja;
    private CircleCollider2D collider;
    RaycastHit2D hit;
    private bool groundedNinja = false;
    private Collider2D floor;
    private Transform groundCheck;

    private DrawLine lines;

    private Button done;

    private int[] angles = new int[3];

    private string[] moves = new string[3];
    private int currentMove = 0;

    private bool fighting = false;

    private Vector3 moveDirection = Vector3.zero;
    public float speed = 6.0f;
    public float jumpSpeed = 100.0f;
    public float gravity = .5f;

    //private GameObject floor;

    private void doneButton() //done button on click
    {
        fighting = true;
        moves = getMoves();
        done.GetComponentInChildren<Image>().enabled = false;
        done.enabled = false;
    }

    private void Start()
    {
        floor = GameObject.Find("groundCheck").GetComponent<Collider2D>();
        ninja = GameObject.FindWithTag("Player");
        //ninja.AddComponent<CharacterController>();
        bodyNinja = ninja.GetComponent<Rigidbody2D>();
        anim = ninja.GetComponent<Animator>();
        groundCheck = GameObject.Find("groundCheck").transform;
        lines = Camera.main.GetComponent<DrawLine>();
        done = GameObject.Find("Done").GetComponentInChildren<Button>();
        done.onClick.AddListener(doneButton);
        collider = ninja.GetComponent<CircleCollider2D>();
    }

    private void Update()
    {

        if (fighting == true) {
            fight();
        } else {
            angles = lines.angles;
        }

        

        groundedNinja = Physics2D.Linecast(bodyNinja.transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("ground"));

        //Debug.Log("angles " + angles[0] + " " + angles[1] + " " + angles[2]);

        //Debug.Log("ninjaPos " + ninja.transform.position.x + " " + ninja.transform.position.y);
        //Debug.Log("ninjaGrd " + groundedNinja);
    }

    private void fight()
    {
        string currMove = moves[currentMove];
        if (currMove == "up")
        {
            if (collider.IsTouching(floor))
            bodyNinja.velocity = new Vector2(0, jumpSpeed);
        }
        else if (currMove == "down")
        {

        }
        else if (currMove == "left")
        {

        }
        else if (currMove == "right")
        {

        }
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
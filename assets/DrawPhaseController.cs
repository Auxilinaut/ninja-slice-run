using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DrawPhaseController : MonoBehaviour
{
    private GameObject ninja;
    private int ninjaHP = 100;

    private Animator anim;
    private Rigidbody2D bodyNinja;

    private GameObject mob;
    private int mobHP = 100;

    private bool groundedNinja = false;
    private GameObject groundCheck;
    private groundCheck groundScript;

    private DrawLine lines;
    private GameObject[] arrows;

    Canvas canvas;
    private Button done;

    private int[] angles;

    private string[] moves;
    private int currentMove = 0;

    private bool fighting = false;
    public bool inMove = false;

    private float prevTime;

    private Vector3 moveDirection = Vector3.zero;
    public float speed;
    public float jumpSpeed;
    public float gravity;

    AudioSource audio;
    private Button mute;
    public Sprite muteOnSprite;
    public Sprite muteOffSprite;

    private void Start()
    {
        ninja = GameObject.FindWithTag("Player");
        anim = ninja.GetComponent<Animator>();
        bodyNinja = ninja.GetComponent<Rigidbody2D>();

        mob = GameObject.Find("mob");

        groundCheck = GameObject.Find("groundCheck");
        groundScript = groundCheck.GetComponent<groundCheck>();

        lines = Camera.main.GetComponent<DrawLine>(); //the script, not an array
        arrows = new GameObject[3];

        canvas = GameObject.FindObjectOfType<Canvas>();
        done = GameObject.Find("Done").GetComponentInChildren<Button>();
        done.onClick.AddListener(doneButton);

        angles = new int[arrows.Length];
        moves = new string[arrows.Length];

        audio = GetComponent<AudioSource>();
        mute = GameObject.Find("MuteButton").GetComponentInChildren<Button>();
        mute.onClick.AddListener(muteButton);
    }

    private void Update()
    {

        groundedNinja = groundScript.ninjaGrounded;

        if (fighting == true)
        {
            fight();
        }
        else
        {
            angles = lines.angles;
        }
        
        Debug.Log("groundedNinja: " + groundedNinja);
        Debug.Log("currMove: " + currentMove);

        //Debug.Log("mousePos " + Input.mousePosition);
    }

    private void muteButton() //mute button on click
    {
        audio.mute = !audio.mute;

        if (!audio.mute)
            mute.image.sprite = muteOffSprite;
        else
            mute.image.sprite = muteOnSprite;
    }


    private void doneButton() //done button on click
    {
        fighting = true;
        moves = getMoves();
        canvasEnabled(false);
    }

    private void canvasEnabled(bool enabled)
    {
        canvas.enabled = enabled;

        mute.GetComponentInChildren<Image>().enabled = true;
        mute.enabled = true;

        if (fighting)
        {
            arrows[currentMove].SetActive(true);

        }
    }

    private void fight() // when they are fighting
    {

        string currMove = moves[currentMove];

        if (currMove == "up")
        {
            if (inMove) { 

                if (ninja.transform.position.y >= -2.4f)
                {
                    anim.Play("inair");
                }
                else if (ninja.transform.position.y > -2.6f)
                {
                    anim.Play("landing");
                }else if (groundedNinja)
                {
                    //arrows[currentMove].GetComponentInChildren<Image>().enabled = true;
                    currentMove++;
                    inMove = false;
                    anim.Play("idle");
                }
            }else if (groundedNinja){
                anim.Play("jump");
                bodyNinja.transform.position = bodyNinja.transform.position + new Vector3(0f, .5f, 0f);
                bodyNinja.AddForce(new Vector2(bodyNinja.velocity.x, jumpSpeed), ForceMode2D.Impulse);
                inMove = true;
            }
        }
        else if (currMove == "down")
        {
            if (inMove) {

            } else {

            }
        }
        else if (currMove == "left")
        {
            if (inMove)
            {
                if (bodyNinja.position.x < -5.0f || bodyNinja.position.y > 0.5f)
                {
                    bodyNinja.position = new Vector2(bodyNinja.position.x + 0.1f, bodyNinja.position.y - 0.1f);
                    bodyNinja.velocity = new Vector2(0.0f, 0.0f);
                    moveDirection = new Vector2(3f, -2f);
                    bodyNinja.AddRelativeForce(moveDirection, ForceMode2D.Impulse);
                }else if(bodyNinja.position.x > -2.87f)
                {
                    bodyNinja.velocity = new Vector2(0.0f, 0.0f);
                    bodyNinja.position = new Vector3(-2.87f, -2.76f);
                    currentMove++;
                    inMove = false;
                    anim.Play("idle");
                }
            }
            else
            {
                bodyNinja.gravityScale = 0.0f;
                bodyNinja.AddForce(new Vector2(-2.0f, 2.0f), ForceMode2D.Impulse);
                anim.Play("gliding");
                inMove = true;
            }
        }
        else if (currMove == "right")
        {
            if (inMove)
            {
                if (bodyNinja.position.x > 5.8f)
                {
                    Vector3 stand = new Vector3(5.8f, bodyNinja.position.y);
                    bodyNinja.position = stand;
                    bodyNinja.velocity = new Vector2(0.0f, 0.0f);
                    anim.Play("attack");
                }
                else if (bodyNinja.position.x < -2.87f)
                {
                    ninja.GetComponent<SpriteRenderer>().flipX = false;
                    Vector3 stand = new Vector3(-2.87f, bodyNinja.position.y);
                    bodyNinja.position = stand;
                    bodyNinja.velocity = new Vector2(0.0f, 0.0f);

                    currentMove++;
                    inMove = false;
                    anim.Play("idle");
                }
                else
                {
                    if (prevTime != 0 && Time.time >= prevTime + 2.0f)
                    {
                        bodyNinja.GetComponent<SpriteRenderer>().flipX = true;
                        bodyNinja.AddForce(new Vector2(-12.0f, bodyNinja.velocity.y), ForceMode2D.Impulse);
                        anim.Play("running");
                        prevTime = 0;
                    }
                }
            }
            else
            {
                bodyNinja.AddForce(new Vector2(12.0f, bodyNinja.velocity.y), ForceMode2D.Impulse);
                prevTime = Time.time;
                anim.Play("running");
                inMove = true;
            }
            
            
        }

        if (currentMove > 2)
        {
            fighting = false;
            currentMove = 0;
        }
    }
    
    private string[] getMoves() // angles from DrawLine script into moveset
    {
        string[] m = new string[angles.Length];

        for (int i=0; i < angles.Length; i++)
        {

            arrows[i] = GameObject.Find("Line" + i);

            if (angles[i] > 45 && angles[i] <= 135)
            {
                arrows[i].transform.SetPositionAndRotation(arrows[i].transform.position, new Quaternion(0.707f, 0, 0, 0.707f));
                m[i] = "up";
            } else if (angles[i] > 135 && angles[i] <= 225)
            {
                arrows[i].transform.SetPositionAndRotation(arrows[i].transform.position, new Quaternion(0, 0, 0, 1));
                m[i] = "left";
            }
            else if (angles[i] > 225 && angles[i] < 315)
            {
                arrows[i].transform.SetPositionAndRotation(arrows[i].transform.position, new Quaternion(-0.707f, 0, 0, 0.707f));
                m[i] = "down";
            }
            else if (angles[i] <= 45 || angles[i] >= 315)
            {
                arrows[i].transform.SetPositionAndRotation(arrows[i].transform.position, new Quaternion(1, 0, 0, 0));
                m[i] = "right";
            }
        }

        return m;
    }
}
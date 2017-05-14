using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DrawPhaseController : MonoBehaviour
{
    private GameObject ninja;
    private int maxHP = 100;
    private int ninjaHP;
    private RectTransform ninjaHPBar;

    private Animator anim;
    private Rigidbody2D bodyNinja;
    private float prevTime;

    public GameObject mob;
    private int mobHP;
    private RectTransform mobHPBar;

    private bool groundedNinja = false;
    private GameObject groundCheck;
    private groundCheck groundScript;
    private MissileCollision missileScript;

    Canvas canvas;
    private Button done;
    private GameObject panel;
    private GameObject text;
    private GameObject holder1, holder2, holder3;
    private GameObject[] arrows;
    private DrawLine lines;

    private int[] angles;

    private string[] moves;
    private int currentMove = 0;

    private bool fighting = false;
    public bool inMove = false;
    private GameObject missile;
    private Rigidbody2D bodyMissile;
    private GameObject bullet = null;
    public bool missileHit;
    private float prevMissileTime = 0;
    

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
        ninjaHP = maxHP;
        ninjaHPBar = GameObject.Find("NinjaHP").GetComponent<RectTransform>();

        mob = GameObject.Find("mob");
        Debug.Log(mob.transform.position);
        mobHP = maxHP;
        mobHPBar = GameObject.Find("MobHP").GetComponent<RectTransform>();

        groundCheck = GameObject.Find("groundCheck");
        groundScript = groundCheck.GetComponent<groundCheck>();

        lines = Camera.main.GetComponent<DrawLine>(); //the script, not an array
        arrows = new GameObject[3];

        canvas = GameObject.FindObjectOfType<Canvas>();
        done = GameObject.Find("Done").GetComponentInChildren<Button>();
        done.onClick.AddListener(doneButton);
        panel = GameObject.Find("Panel");
        text = GameObject.Find("Text");
        holder1 = GameObject.Find("Holder1");
        holder2 = GameObject.Find("Holder2");
        holder3 = GameObject.Find("Holder3");

        angles = new int[arrows.Length];
        moves = new string[arrows.Length];

        missile = GameObject.Find("Missile");
        missileScript = missile.GetComponent<MissileCollision>();
        bodyMissile = missile.GetComponentInChildren<Rigidbody2D>();

        audio = GetComponent<AudioSource>();
        mute = GameObject.Find("MuteButton").GetComponentInChildren<Button>();
        mute.onClick.AddListener(muteButton);
    }

    private void Update()
    {

        groundedNinja = groundScript.ninjaGrounded;

        if (ninja.transform.position.x < -7.0f)
        {
            ninja.transform.position = new Vector3(-2.87f, -2.81f, 0);
        }

        if (ninjaHP > 0 && mobHP > 0)
        {
            if (fighting == true)
            {
                fight();

            }
            else if (currentMove > 2)
            {
                selectiveCanvas();
                moves = new string[3];
                currentMove = 0;
            }
            else
            {
                angles = lines.angles;
            }
        }else
        {
            if (ninjaHP <= 0)
            {
                anim.Play("dead");
                AutoFade.LoadLevel("MainMenu", 1, 1, Color.red);
            }
            else
            {
                Destroy(mob);
                AutoFade.LoadLevel("MainMenu", 1, 1, Color.black);
            }
        }
        
        Debug.Log("MissileHit: " + missileHit);
        //Debug.Log("currMove: " + currentMove);

        //Debug.Log("mousePos " + Input.mousePosition);
    }

    // Mute button on click
    private void muteButton()
    {
        audio.mute = !audio.mute;

        if (!audio.mute)
            mute.image.sprite = muteOffSprite;
        else
            mute.image.sprite = muteOnSprite;
    }

    // Done button on click
    private void doneButton()
    {
        fighting = true;
        moves = getMoves();
        selectiveCanvas();
        lines.angles = new int[3];
        lines.currLines = 0;
    }

    private void selectiveCanvas()
    {
        panel.SetActive(!panel.activeSelf);
        text.SetActive(!text.activeSelf);
        holder1.SetActive(!holder1.activeSelf);
        holder2.SetActive(!holder2.activeSelf);
        holder3.SetActive(!holder3.activeSelf);

        for (int i = 0; i < 3; i++)
        {
            GameObject dLine = GameObject.Find("Line" + i);
            Destroy(dLine);

            arrows[i].transform.rotation = new Quaternion();
            arrows[i].GetComponentInChildren<Image>().enabled = false;
            arrows[i].SetActive(!arrows[i].activeSelf);
        }
    }

    // All moves for player and mob
    private void fight()
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
            if (inMove)
            {
                if (bodyNinja.position.x > 6.0f)
                {
                    Vector3 stand = new Vector3(6.0f, bodyNinja.position.y);
                    bodyNinja.position = stand;
                    bodyNinja.velocity = new Vector2(0.0f, 0.0f);
                    decreaseHealth("Mob");
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
                    if (prevTime != 0 && Time.time >= prevTime + 1.0f)
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
                anim.Play("sliding");
                inMove = true;
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
                    bodyNinja.position = new Vector3(-2.87f, -2.81f);
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
                    decreaseHealth("Mob");
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

        // Mob Shooting
        if (Time.time >= prevMissileTime + Random.Range(5.0f, 10.0f))
        {
            mobShoot();
            prevMissileTime = Time.time;
        }

        if (missileHit)
        {
            decreaseHealth("Ninja");
            Destroy(bullet);
            missileHit = false;
        }
        else if (bullet && bullet.transform.position.x < -5)
        {
            Destroy(bullet);
        }

        // Stop fighting on 3 moves
        if (currentMove > 2)
        {
            fighting = false;
        }
    }

    // Angles from DrawLine script into moveset
    private string[] getMoves()
    {
        string[] m = new string[angles.Length];

        for (int i=0; i < angles.Length; i++)
        {

            arrows[i] = GameObject.Find("Arrow" + (i+1));

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

    void decreaseHealth(string character)
    {
        if (character.Equals("Ninja")){
            ninjaHP -= 5;
            setHPBar(character, ninjaHP);
        }
        else
        {
            mobHP -= 10;
            setHPBar(character, mobHP);
        }
    }

    void setHPBar(string character, float HP)
    {
        if (character.Equals("Ninja"))
        {
            ninjaHPBar.sizeDelta = new Vector2(HP, 12);
            Debug.Log("ninjaHP " + ninjaHP);
        }
        else
        {
            mobHPBar.sizeDelta = new Vector2(HP, 12);
        }
    }

    void mobShoot()
    {
        if (bullet == null)
        {
            bullet = Instantiate(missile, this.mob.transform.position, this.mob.transform.rotation);
            bullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(-20.0f, 0), ForceMode2D.Impulse);
        }
    }
}
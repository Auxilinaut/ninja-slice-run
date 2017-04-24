using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DrawPhaseController : MonoBehaviour
{
    private GameObject ninja;
    private Animator anim;
    private CharacterController controller;

    private DrawLine lines;

    private int[] angles;

    private void Start()
    {
        lines = GameObject.Find("Main Camera").GetComponent<DrawLine>();
        
    }

    private void Update()
    {
        angles = lines.angles;

        Debug.Log("angles " + angles[0] + " " + angles[1] + " " + angles[2]);
        
    }
}
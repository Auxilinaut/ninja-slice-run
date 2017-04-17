using UnityEngine;
using System.Collections;

public class BGScroller : MonoBehaviour
{

    public float scrollSpeed;
    private Vector2 offset;
    private Vector2 savedOffset;
    private Color clr;
    private float repeater;
    private float pingpong;
    private string ninjaState;
    //private GameObject ninja;

    void Start()
    {
        savedOffset = GetComponent<Renderer>().sharedMaterial.GetTextureOffset("_MainTex");
        clr.r = 0.0f;
        clr.g = 0.0f;
        clr.b = 0.0f;
        clr.a = 0.8f;
        offset = new Vector2(0.0f,0.0f);
        //ninja = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        repeater = Mathf.Repeat(Time.time * scrollSpeed, 1);
        pingpong = Mathf.PingPong(Time.time * 0.1f, 0.5f) + 0.5f;

        offset.x = repeater;
        offset.y = savedOffset.y;

        GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_MainTex", offset);

        clr.r = pingpong * 0.5f;
        clr.g = pingpong * 0.25f;
        clr.b = pingpong;

        GetComponent<Renderer>().sharedMaterial.SetColor("_TintColor", clr);
    }

    void OnDisable()
    {
        GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_MainTex", savedOffset);
    }
}
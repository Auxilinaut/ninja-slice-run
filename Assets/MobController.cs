using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

namespace PixelSpriteGenerator
{
    public class MobController : MonoBehaviour
    {

        public GameObject mob;

        //mob floating movement
        public float floatAmplitude; 
        public float floatSpeed;
        private Vector2 tempPos;

        [HideInInspector]
        float timer = 2; // 2 sec since last


        [HideInInspector]
        public int width = 8;

        [HideInInspector]
        public int height = 8;

        [HideInInspector]
        public bool mirrorX;

        [HideInInspector]
        public bool mirrorY;

        public enum SpriteTemplate
        {
            spaceShipColored,
            spaceShipColoredLowSat,
            spaceShipManyColor,
            treeColored,
            dragonColored,
            shrubColored,
            robotBW
        }

        public SpriteTemplate SpriteTemplateSelection = SpriteTemplate.spaceShipColored;

        private SpriteRenderer sr;

        private PsgOptions options;

        private PsgMask mask;

        // Initialization
        void Start()
        {
            GenerateSprite();
            mob = GameObject.Find("mob");
            /*mob.AddComponent<Rigidbody2D>();
            mob.GetComponent<Rigidbody2D>().gravityScale = 0f;*/
            tempPos = new Vector3();
            floatAmplitude = 0.25f;
            floatSpeed = 2.5f;
        }

        private void GetTemplate()
        {
            mask = new PsgMask(new int[] {
                    0, 0, 0, 0, 0, 0,
                    0, 0, 0, 0, 1, 1,
                    0, 0, 0, 0, 1, -1,
                    0, 0, 0, 1, 1, -1,
                    0, 0, 0, 1, 1, -1,
                    0, 0, 1, 1, 1, -1,
                    0, 1, 1, 1, 2, 2,
                    0, 1, 1, 1, 2, 2,
                    0, 1, 1, 1, 2, 2,
                    0, 1, 1, 1, 1, -1,
                    0, 0, 0, 1, 1, 1,
                    0, 0, 0, 0, 0, 0
                }, 6, 12, true, false);

            options = new PsgOptions()
            {
                Colored = true,
                EdgeBrightness = 0.3f,
                ColorVariations = 0.2f,
                BrightnessNoise = 0.3f,
                Saturation = 0.5f
            };
        }

        private void GenerateSprite()
        {
            GetTemplate();

            var psgSprite = new PsgSprite(mask, options);

            if (mask.mirrorX)
            {
                width = mask.width * 2;

            }
            else
            {
                width = mask.width;
            }

            if (mask.mirrorY)
            {
                height = mask.height * 2;
            }
            else
            {
                height = mask.height;
            }

            mirrorX = mask.mirrorX;
            mirrorY = mask.mirrorY;

            var tex = psgSprite.texture;
            tex.wrapMode = TextureWrapMode.Clamp;
            tex.filterMode = FilterMode.Point;

            var cmpts = new Type[1] { typeof(SpriteRenderer) };
            var go = new GameObject("mob", cmpts);
            var theSr = go.GetComponent<SpriteRenderer>();

            theSr.sprite = Sprite.Create(tex, new Rect(0, 0, (float)width, (float)height), new Vector2(0.5f, 0.5f), 32f);

            theSr.transform.localScale = new Vector3(-10.0f, -10.0f, -10.0f);
            theSr.transform.position = new Vector2(9.4f, -1);
        }

        // They'll be moving around a lot
        void Update()
        {
            tempPos.Set(9.4f, -1 + floatAmplitude * Mathf.Sin(floatSpeed * Time.time));
            mob.transform.position = tempPos;
            //Debug.Log("mobPos " + mob.transform.position.x + " " + mob.transform.position.y);
        }

    }
}
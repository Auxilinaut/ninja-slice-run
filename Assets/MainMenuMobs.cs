using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

namespace PixelSpriteGenerator
{
    public class MainMenuMobs : MonoBehaviour
    {

        private GameObject[] mobs = new GameObject[15];

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

        private float spritePadding;

        // Initialization
        void Start()
        {
            GenerateSprites();
            for (int i = 1; i <= 15; i++)
            {
                mobs[i-1] = GameObject.Find(i.ToString());
                mobs[i-1].AddComponent<Rigidbody2D>();
                mobs[i-1].GetComponent<Rigidbody2D>().gravityScale = 0f;
                mobs[i-1].GetComponent<Rigidbody2D>().velocity = new Vector2(-(1.5f * UnityEngine.Random.Range(0.5f, 1.0f)), mobs[i-1].GetComponent<Rigidbody2D>().velocity.y);
            }
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

            spritePadding = 1.2f;

            options = new PsgOptions()
            {
                Colored = true,
                EdgeBrightness = 0.3f,
                ColorVariations = 0.2f,
                BrightnessNoise = 0.3f,
                Saturation = 0.5f
            };
        }

        private void GenerateSprites()
        {
            GetTemplate();

            for (var y = 0; y < 3; y++)
            {
                for (var x = 0; x < 5; x++)
                {
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
                    var go = new GameObject(((x * 3 + y)+1).ToString(), cmpts);
                    var theSr = go.GetComponent<SpriteRenderer>();

                    theSr.sprite = Sprite.Create(tex, new Rect(0, 0, (float)width, (float)height), new Vector2(0.5f, 0.5f), 32f);

                    theSr.transform.localScale = new Vector3(-2.5f, -2.5f, -1 * UnityEngine.Random.Range(1.5f, 2.5f));
                    theSr.transform.position = new Vector2(x * spritePadding * 2, y * spritePadding + UnityEngine.Random.Range(1.0f, 2.0f));
                }
            }
        }

        // They'll be moving around a lot
        void Update()
        {
            timer -= Time.deltaTime;
            for (int i=0; i<15; i++)
            {
                if (timer <= 0)
                {
                    mobs[i].GetComponent<Rigidbody2D>().velocity = new Vector2(-1*(2.0f * UnityEngine.Random.Range(0.5f,1.0f)), mobs[i].GetComponent<Rigidbody2D>().velocity.y);
                }

                if (mobs[i].transform.position.x < -6.0f)
                {
                    mobs[i].transform.position = new Vector2(15, mobs[i].transform.position.y);
                }
            }

            if (timer <= 0)
            {
                timer = 2;
            }

        }

    }
}
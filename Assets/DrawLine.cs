using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DrawLine : MonoBehaviour
{
    //reference to LineRenderer component
    private LineRenderer line;
    //car to store mouse position on the screen
    private Vector3 mousePos;
    //assign a material to the Line Renderer in the Inspector
    public Material material;

    private static int MAX_LINES = 3;

    //current number of lines drawn
    private int currLines = 0;

    public int[] angles = new int[3];

    private Button back;
    private Button done;
    private bool backClicked = false;

    private void backButton() //back button on click
    {
        currLines--; 
        angles[currLines] = 0;

        //disable and reset arrow
        GameObject currArrow = GameObject.Find("Arrow" + (currLines + 1));
        currArrow.GetComponentInChildren<UnityEngine.UI.Image>().enabled = false;
        currArrow.transform.SetPositionAndRotation(currArrow.transform.position, new Quaternion());

        GameObject currLine = GameObject.Find("Line" + currLines); //lock and load
        Destroy(currLine); //sorry, RAM
    }

    private void Start()
    {
        back = GameObject.Find("Back").GetComponentInChildren<Button>();
        back.onClick.AddListener(backButton);

        done = GameObject.Find("Done").GetComponentInChildren<Button>();
        done.GetComponentInChildren<Image>().enabled = false;
        done.enabled = false;
    }

    private void Update()
    {
        if (currLines == 0)
        {
            back.GetComponentInChildren<Image>().enabled = false;
            back.enabled = false;
        }else{
            back.GetComponentInChildren<Image>().enabled = true;
            back.enabled = true;
        }

        if (currLines < MAX_LINES) //enough space for lines?
        {
            //get the mouse position
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //Debug.Log("mousePos " + mousePos.x + " " + mousePos.y);

            //set the z coordinate to 0 as we are only interested in the xy axes
            mousePos.z = 0;

            done.GetComponentInChildren<Image>().enabled = false;
            done.enabled = false;

            //keep in UI panel
            if (mousePos.x < 6.45f && mousePos.x > -0.05f && mousePos.y < 3.2f && mousePos.y > -3.12f)
            {
                //Create new Line on left mouse click(down)
                if (Input.GetMouseButtonDown(0))
                {
                    if (line == null)//check if there is no line renderer created
                    {
                        //create the line
                        createLine();
                    }

                    //set the start point and end point of the line renderer
                    line.SetPosition(0, mousePos);
                    line.SetPosition(1, mousePos);
                }
                //if line renderer exists and left mouse button is click exited (up)
                else if (Input.GetMouseButtonUp(0) && line)
                {
                    //set the end point of the line renderer to current mouse position
                    line.SetPosition(1, mousePos);

                    //calc angle of line
                    angles[currLines] = angle(line.GetPosition(0).x, line.GetPosition(0).y, line.GetPosition(1).x, line.GetPosition(1).y);
                    //Debug.Log(angles[currLines]);
                    GameObject arrow = GameObject.Find("Arrow" + (currLines + 1));
                    arrow.GetComponentInChildren<UnityEngine.UI.Image>().enabled = true;
                    arrow.transform.Rotate(0, 0, angles[currLines], Space.World);

                    //set line as null once the line is created
                    line = null;
                    currLines++;
                }
                //if mouse button is held clicked and line exists
                else if (Input.GetMouseButton(0) && line)
                {
                    //set the end position as current position but dont set line as null as the mouse click is not exited
                    line.SetPosition(1, mousePos);
                }
            }
        }else{
            done.GetComponentInChildren<Image>().enabled = true;
            done.enabled = true;
        }
    }

    private int angle(float bx, float by, float ex, float ey)
    {
        var dy = ey - by;
        var dx = ex - bx;
        int theta = (int)(System.Math.Atan2(dy, dx) * 180 / System.Math.PI); //range (-180, 180]
        if (theta < 0) theta = 360 + theta; // range [0, 360)
        return theta;
    }

    //method to create line
    private void createLine()
    {
        //create a new empty gameobject and line renderer component
        line = new GameObject("Line" + currLines).AddComponent<LineRenderer>();
        //assign the material to the line
        line.material = material;
        //set the number of points to the line
        line.positionCount = 2;
        //set the width
        line.startWidth = 0.15f;
        line.endWidth = 0.15f;
        //render line to the world origin and not to the object's position
        line.useWorldSpace = true;

    }
}
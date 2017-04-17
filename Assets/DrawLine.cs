using UnityEngine;
using System.Collections;

public class DrawLine : MonoBehaviour
{
    //reference to LineRenderer component
    private LineRenderer line;
    //car to store mouse position on the screen
    private Vector3 mousePos;
    //assign a material to the Line Renderer in the Inspector
    public Material material;
    //number of lines drawn
    private int currLines = 0;

    void Update()
    {
        //Create new Line on left mouse click(down)
        if (Input.GetMouseButtonDown(0))
        {
            //check if there is no line renderer created
            if (line == null)
            {
                //create the line
                createLine();
            }
            //get the mouse position
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //set the z co ordinate to 0 as we are only interested in the xy axes
            mousePos.z = 0;
            //set the start point and end point of the line renderer
            line.SetPosition(0, mousePos);
            line.SetPosition(1, mousePos);
        }
        //if line renderer exists and left mouse button is click exited (up)
        else if (Input.GetMouseButtonUp(0) && line)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            //set the end point of the line renderer to current mouse position
            line.SetPosition(1, mousePos);
            //set line as null once the line is created
            line = null;
            currLines++;
        }
        //if mouse button is held clicked and line exists
        else if (Input.GetMouseButton(0) && line)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            //set the end position as current position but dont set line as null as the mouse click is not exited
            line.SetPosition(1, mousePos);
        }
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
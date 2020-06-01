using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class SwipeTrail : MonoBehaviour
{
    public LineManager lineManager = new LineManager();

    public int LineCounter = -1;

    public GameObject PlaneToDrawOn;

    public float lineWidth = 0.75f;

    public Material TrailMaterial;
    public bool TouchedAlready = false;
    private TrailRenderer TrailRenderer;


    public bool FirstTouch = true;


    public Color Color;


    public static String LastMarkerName;

    private Vector3 StandardPosition;

    void Awake()
    {

        StandardPosition = transform.position;

        TrailRenderer = GetComponentInChildren<TrailRenderer>();
        TrailRenderer.startWidth = lineWidth;
        TrailRenderer.endWidth = lineWidth;

        // Disabled to get rid of the accidental line in the first frame
        TrailRenderer.enabled = false;


    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.LogError("ENTERED");
    }

    private void OnTriggerStay(Collider other)
    {
        
        if (other.transform.gameObject.name == PlaneToDrawOn.name)
        {
            TrailRenderer.enabled = true;
        }
    }


    private void OnTriggerExit(Collider other)
    {

        if (other.transform.gameObject.name == PlaneToDrawOn.name)
        {
            TrailRenderer.enabled = false;
            StoreRay();
            CreateLineObject();
        }

    }


    //! The created object is used for retracing the line
    private void CreateLineObject()
    {
        LineCounter++;
        GameObject newLine = new GameObject();
        newLine.transform.parent = PlaneToDrawOn.transform;
        newLine.name = "Line segment " + LineCounter;
        newLine.AddComponent<LineRenderer>();

        RetraceLine(newLine.GetComponent<LineRenderer>(), LineCounter);

    }


    private void RetraceLine(LineRenderer line, int lineNumber)
    {
        Line currentLine = lineManager.AllLines[lineNumber];
        // Set the width of the Line Renderer
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
        // Set the number of vertex for the Line Renderer
        line.positionCount = currentLine.Positions.Length;
        line.material = TrailMaterial;
        setTrailColour(Color, null, line);
        line.SetPositions(currentLine.Positions);
        line.useWorldSpace = false;
    }



    private void StoreRay()
    {
        print("store ray");
        int arrayLength = TrailRenderer.positionCount;
        Color currentColour = TrailRenderer.endColor;
        Vector3[] rayPositions = new Vector3[arrayLength];
        GetComponentInChildren<TrailRenderer>().GetPositions(rayPositions);

        lineManager.AllLines.Add(new Line(currentColour, rayPositions));


        // Clears the "stage" so you can draw a new line
        TrailRenderer.Clear();
    }





    /// <summary>
    /// Changes the Colour of the passed Renderer component
    /// </summary>
    /// <param name="color"></param>
    /// <param name="tRenderer">Optional. Changes the colour of the passed trail renderer</param>
    /// <param name="lRenderer">Optional. Changes the colour of the passed line renderer</param>
    public void setTrailColour(Color color, TrailRenderer tRenderer = null, LineRenderer lRenderer = null)
    {
        if (tRenderer != null)
        {
            tRenderer.startColor = color;
            tRenderer.endColor = color;
        }
        else if (lRenderer != null)
        {
            lRenderer.startColor = color;
            lRenderer.endColor = color;
        }
    }

    public void SelectColor(String language)
    {


        setTrailColour(Color, TrailRenderer);


        TrailRenderer.enabled = false;
        transform.position = StandardPosition;

    }

}

[System.Serializable]
public class LineManager
{
    public List<Line> AllLines = new List<Line>();
}
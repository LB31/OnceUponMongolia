using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawTest : MonoBehaviour
{
    public Texture2D TextureToDrawOn;
    public Color ColorToFill = Color.blue;
    public Color ColorToDraw = Color.red;

    public int pixelsAround = 5;

    private Texture2D backupTex;

    private int width;
    private int height;
    private bool[,] usedPositions;

    void Start()
    {
        width = TextureToDrawOn.width;
        height = TextureToDrawOn.height;
        usedPositions = new bool[width, height];

        backupTex = new Texture2D(width, height);
        Graphics.CopyTexture(TextureToDrawOn, backupTex);
    }

    void Update()
    {
        if (!Input.GetMouseButton(0))
            return;

        RaycastHit hit;
        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            return;

        if (!hit.transform.Equals(transform))
            return;

        Vector2 pixelUV = hit.textureCoord;
        pixelUV.x *= width;
        pixelUV.y *= height;

        SetPixel(TextureToDrawOn, pixelUV);
        //TextureToDrawOn.SetPixel((int)pixelUV.x, (int)pixelUV.y, ColorToDraw);
        //usedPositions[(int)pixelUV.x, (int)pixelUV.y] = true;
        TextureToDrawOn.Apply();
    }

    private void SetPixel(Texture2D tex, Vector2 pixel)
    {
        // Also color the pixels arond the selected
        for (int x = -pixelsAround; x <= pixelsAround; x++)
        {
            for (int y = -pixelsAround; y <= pixelsAround; y++)
            {         
                try
                {
                    tex.SetPixel((int)pixel.x - x, (int)pixel.y - y, ColorToDraw);
                    usedPositions[(int)pixel.x - x, (int)pixel.y - y] = true;
                }
                catch (System.IndexOutOfRangeException) { }    
            }
        }
    }

    [ContextMenu("Fill Color")]
    private void SetFullColor()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (usedPositions[x, y])
                    continue;
                TextureToDrawOn.SetPixel(x, y, ColorToFill);
            }
        }

        TextureToDrawOn.Apply();
    }

    private void OnApplicationQuit()
    {
        Graphics.CopyTexture(backupTex, TextureToDrawOn);
    }

}

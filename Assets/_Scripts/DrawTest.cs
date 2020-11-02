using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class DrawTest : MonoBehaviour
{
    public Texture2D TextureToDrawOn;
    public Color ColorToFill = Color.blue;
    public Color ColorToDraw = Color.red;
    public Material BrushTipMat;
    public int pixelsAround = 5;
    public float distancteToDraw = 0.2f;
    public bool FillingWall;

    private Texture2D backupTex;

    private int width;
    private int height;
    private static bool[,] usedPositions;

    private int layerMask = 1 << 9;
    private bool selectingColor;
    private Color returnColor;
    private bool fillHit;

    void Start()
    {
        width = TextureToDrawOn.width;
        height = TextureToDrawOn.height;
        usedPositions = new bool[width, height];

        backupTex = new Texture2D(width, height);
        Graphics.CopyTexture(TextureToDrawOn, backupTex);

        ColorToDraw = BrushTipMat.color;
        returnColor = BrushTipMat.color;
    }

    private async void Update()
    {
        if (selectingColor) return;


        RaycastHit hit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        if (!Physics.Raycast(transform.position, fwd, out hit, distancteToDraw, layerMask))
        {
            print("outstide");

            fillHit = true;
            return;
        }
            

        if (FillingWall)
        {
            if (fillHit)
            {
                fillHit = false;
                StartCoroutine(SetFullColor());
            }
            return;
        }

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

    private IEnumerator SetFullColor()
    {
        for (int x = 0; x < TextureToDrawOn.mipmapCount; ++x)
        {
            Color[] colors = TextureToDrawOn.GetPixels(x);
            for (int y = 0; y < colors.Length; ++y)
            {
                //if (usedPositions[x, y])
                //    colors[y] = TextureToDrawOn.GetPixel(x, y);
                //else
                    colors[y] = ColorToDraw;
                //TextureToDrawOn.SetPixel(x, y, ColorToDraw);               
            }
            TextureToDrawOn.SetPixels(colors, x);
        }

        TextureToDrawOn.Apply(false);
        //FillingWall = true;
        yield return null;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("color"))
        {
            Material material = other.GetComponent<Renderer>().material;
            ColorToDraw = material.color;
            BrushTipMat.color = material.color;
        }

        if (other.name.ToLower().Contains("table"))
        {
            selectingColor = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name.ToLower().Contains("table"))
        {
            selectingColor = false;
        }
    }


    private void OnApplicationQuit()
    {
        Graphics.CopyTexture(backupTex, TextureToDrawOn);
        BrushTipMat.color = returnColor;
    }

}

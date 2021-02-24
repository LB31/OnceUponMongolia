using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class BrushDrawer : MonoBehaviour
{
    public Texture2D TextureToDrawOn;
    public Color ColorToFill = Color.blue;
    public Color ColorToDraw = Color.red;
    public Material BrushTipMat;
    public int PixelsAround = 5;
    public float DistancteToDraw = 0.2f;
    public bool FillingWall;

    private Texture2D backupTex;

    private int width;
    private int height;
    private static Color[,] usedPositions;
    //private static bool[,] usedPositions;

    private int layerMask = 1 << 9; // Draw Layer
    private bool selectingColor;
    private Color returnColor;
    private bool fillHit;

    void Start()
    {
        width = TextureToDrawOn.width;
        height = TextureToDrawOn.height;
        //usedPositions = new bool[width, height];
        usedPositions = new Color[width, height];

        backupTex = new Texture2D(width, height);
        Graphics.CopyTexture(TextureToDrawOn, backupTex);

        ColorToDraw = BrushTipMat.color;
        returnColor = BrushTipMat.color;
    }

    private void Update()
    {
        if (selectingColor) return;


        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        if (!Physics.Raycast(transform.position, fwd, out RaycastHit hit, DistancteToDraw, layerMask))
        {
            fillHit = true;
            return;
        }

        Vector2 pixelUV = hit.textureCoord;
        pixelUV.x *= width;
        pixelUV.y *= height;

        if (FillingWall)
        {
            if (fillHit)
            {
                fillHit = false;
                //StartCoroutine(SetFullColor());
                StartCoroutine(FillColor(pixelUV));
            }
            return;
        }



        SetPixel(pixelUV, ColorToDraw);
        //TextureToDrawOn.SetPixel((int)pixelUV.x, (int)pixelUV.y, ColorToDraw);
        //usedPositions[(int)pixelUV.x, (int)pixelUV.y] = true;
        TextureToDrawOn.Apply();
    }

    private void SetPixel(Vector2 pixel, Color col)
    {
        // Also color the pixels arond the selected
        for (int x = -PixelsAround; x <= PixelsAround; x++)
        {
            for (int y = -PixelsAround; y <= PixelsAround; y++)
            {
                try
                {
                    TextureToDrawOn.SetPixel((int)pixel.x - x, (int)pixel.y - y, col);
                    usedPositions[(int)pixel.x - x, (int)pixel.y - y] = col;
                }
                catch (System.IndexOutOfRangeException) { }
            }
        }
    }

    private IEnumerator SetFullColor()
    {
        print(TextureToDrawOn.mipmapCount + " mip");
        for (int x = 0; x < TextureToDrawOn.mipmapCount; ++x)
        {
            Color[] colors = TextureToDrawOn.GetPixels(x);
            print(colors.Length + " cl");
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

    private IEnumerator FillColor(Vector2 pixel)
    {
        Color pixelColor = usedPositions[(int)pixel.x, (int)pixel.y];

        if (pixelColor == null)
            yield return null;

        for (int x = 0; x < usedPositions.GetLength(0); x++)
        {
            for (int y = 0; y < usedPositions.GetLength(1); y++)
            {
                if (usedPositions[x, y] != pixelColor) continue;

                TextureToDrawOn.SetPixel(x, y, ColorToDraw);
                usedPositions[x, y] = ColorToDraw;
            }
        }

        TextureToDrawOn.Apply();
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



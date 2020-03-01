using System;
using UnityEngine;

[Serializable]
public class PageContent
{
    [SerializeField] private Texture2D backgroundTexture;

    public Texture2D BackgroundTexture => backgroundTexture;
}
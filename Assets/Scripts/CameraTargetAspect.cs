using System;
using UnityEngine;

public class CameraTargetAspect : MonoBehaviour
{
    [SerializeField] private float initialCameraSize = 5f;
    
    [SerializeField] private Camera cam = null;
    [SerializeField] private RectTransform watchedTransform = null;

    private void Start()
    {
        Recalculate();
    }

    public void Recalculate()
    {
        if (cam == null || watchedTransform == null)
        {
            Debug.LogWarning("ENCYCLOPEDIA NOT WORKING! Please set camera and watchedTransform");
            return;
        }
        
        var targetTexture = cam.targetTexture;
            
        float targetWidth = targetTexture.width;
        float targetHeight = targetTexture.height;

        if (targetWidth <= 0f || targetHeight <= 0f)
            return;
        
        var aspectRatio = targetWidth / targetHeight;
        var size = initialCameraSize * 2f;

        watchedTransform.sizeDelta = aspectRatio > 1f
            ? new Vector2(size, size / aspectRatio)
            : new Vector2(size * aspectRatio, size);

        cam.orthographicSize = aspectRatio > 1f 
            ? initialCameraSize / aspectRatio
            : initialCameraSize;
        cam.ResetAspect();
    }
}
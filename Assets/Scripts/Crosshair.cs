using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    public Image aimPointReticle;
    public Image hitPointReticle;

    public float smoothTime = 0.2f;
    
    private Camera screenCamera;
    private RectTransform CrossHairRectTransform;

    private Vector2 currentHitPointVelocity;
    private Vector2 targetPoint;

    private void Awake()
    {
        screenCamera = Camera.main;
        CrossHairRectTransform = hitPointReticle.GetComponent<RectTransform>();
    }

    public void SetActiveCrosshair(bool active)
    {
        hitPointReticle.enabled = active;
        aimPointReticle.enabled = active;
    }

    public void UpdatePosition(Vector3 worldPoint)
    {
        targetPoint = screenCamera.WorldToScreenPoint(worldPoint);
    }

    private void Update()
    {
        if(!hitPointReticle.enabled) return;

        CrossHairRectTransform.position = Vector2.SmoothDamp(CrossHairRectTransform.position, targetPoint,
            ref currentHitPointVelocity, smoothTime);
    }
}

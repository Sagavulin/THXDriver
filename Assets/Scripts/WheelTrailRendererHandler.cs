using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelTrailRendererHandler : MonoBehaviour
{
    TopDownCarController topDownCarController;
    TrailRenderer trailRenderer;

    private void Awake()
    {
        topDownCarController = GetComponentInParent<TopDownCarController>();

        trailRenderer = GetComponentInParent<TrailRenderer>();

        trailRenderer.emitting = false;
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (topDownCarController.IsTireScreeching(out float lateralVelocity, out bool isBraking))
            trailRenderer.emitting = true;
        else trailRenderer.emitting = false;
    }
}

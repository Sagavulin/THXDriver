using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelTrailRendererHandler : MonoBehaviour
{
    // making drifting marks
	TopDownCarController topDownCarController;
    TrailRenderer trailRenderer;

    void Awake()
    {
        topDownCarController = GetComponentInParent<TopDownCarController>();

        trailRenderer = GetComponentInParent<TrailRenderer>();

        trailRenderer.emitting = false;
    }

    void Update()
    {
        if (topDownCarController.IsTireScreeching(out float lateralVelocity, out bool isBraking))
            trailRenderer.emitting = true;
        else trailRenderer.emitting = false;
    }
}

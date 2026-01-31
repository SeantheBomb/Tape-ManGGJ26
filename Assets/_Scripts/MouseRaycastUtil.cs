using UnityEngine;

public static class MouseRaycastUtil
{
    public static bool TryGetMouseColliderHit(
        Vector2 mousePosition,
        out RaycastHit hit,
        Camera camera = null,
        float maxDistance = Mathf.Infinity,
        int layerMask = Physics.DefaultRaycastLayers,
        QueryTriggerInteraction triggerInteraction = QueryTriggerInteraction.Ignore)
    {
        hit = default;

        if (camera == null)
            camera = Camera.main;

        Ray ray = camera.ScreenPointToRay(mousePosition);
        return Physics.Raycast(
            ray,
            out hit,
            maxDistance,
            layerMask,
            triggerInteraction
        );
    }

    public static bool TryGetMouseColliderHit(
        out RaycastHit hit,
        Camera camera = null,
        float maxDistance = Mathf.Infinity,
        int layerMask = Physics.DefaultRaycastLayers,
        QueryTriggerInteraction triggerInteraction = QueryTriggerInteraction.Ignore)
    {
        hit = default;

        if (camera == null)
            camera = Camera.main;

        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(
            ray,
            out hit,
            maxDistance,
            layerMask,
            triggerInteraction
        );
    }
}
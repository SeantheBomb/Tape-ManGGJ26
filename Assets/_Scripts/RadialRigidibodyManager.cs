using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialRigidibodyManager : MonoBehaviour
{
    static RadialRigidibodyManager _instance;
    public static RadialRigidibodyManager instance
    {
        get 
        {
            if (_instance == null)
                _instance = FindObjectOfType<RadialRigidibodyManager>();
            return _instance;
        }
    }

    public float radius;
    public float height;

    public float minY => transform.position.y;
    public float maxY => transform.position.y + height;

    public Vector2 debugPos;
    public Vector3 debugResult;


    // Start is called before the first frame update
    void Awake()
    {
        _instance = this; 
    }

    public Vector3 GetRadialPosition(Vector2 pos)
    {
        Vector3 result = Vector3.zero;
        result.y = Mathf.Clamp(pos.y, minY, maxY);
        result.x = Mathf.Cos(pos.x * Mathf.Deg2Rad) * radius;
        result.z = Mathf.Sin(pos.x * Mathf.Deg2Rad) * radius;
        debugResult = result;
        return result;
    }

    public Vector3 GetRadialVector(Vector2 pos, Vector2 vector)
    {
        Vector3 realPos = GetRadialPosition(pos);
        Vector3 normal = Vector3.ProjectOnPlane(realPos - transform.position, Vector3.up).normalized;
        Vector3 realDir = Vector3.Cross(normal, Vector3.up) * vector.x;
        realDir.y = vector.y;
        return realDir;
    }

    public Vector2 GetInverseRadialVector(Vector3 realPos, Vector3 realDir)
    {
        // Radial direction (from center to point) in XZ plane
        Vector3 radial = Vector3.ProjectOnPlane(realPos - transform.position, Vector3.up);

        // Degenerate safety (if you're exactly at the center for some reason)
        if (radial.sqrMagnitude < 1e-8f)
            return new Vector2(0f, realDir.y);

        radial.Normalize();

        // Tangent direction (the direction you used for vector.x)
        Vector3 tangent = Vector3.Cross(radial, Vector3.up).normalized;

        Vector2 vector;
        vector.x = Vector3.Dot(Vector3.ProjectOnPlane(realDir, Vector3.up), tangent);
        vector.y = realDir.y;
        return vector;
    }

    public Vector3 GetRadialPosition(Vector2 pos, float radius)
    {
        Vector3 result = Vector3.zero;
        result.y = Mathf.Clamp(pos.y, minY, maxY);
        result.x = Mathf.Cos(pos.x * Mathf.Deg2Rad) * radius;
        result.z = Mathf.Sin(pos.x * Mathf.Deg2Rad) * radius;
        debugResult = result;
        return result;
    }

    public Vector2 GetInverseRadialPosition(Vector3 pos)
    {
        Vector2 result = Vector2.zero;
        result.y = pos.y;
        result.x = Mathf.Atan2(pos.z, pos.x) * Mathf.Rad2Deg;
        return result;
    }

    public bool TryGetRadialPositionFromScreen(
        Vector2 screenPos,
        out Vector2 radialPos,
        Camera cam = null,
        float maxDistance = 10000f,
        bool requireWithinHeight = true)
    {
        cam ??= Camera.main;

        radialPos = default;
        if (!cam) return false;

        Ray ray = cam.ScreenPointToRay(screenPos);

        if (!TryIntersectRayWithCylinder(ray, radius, out Vector3 hitWorld, maxDistance, requireWithinHeight))
            return false;

        radialPos = GetInverseRadialPosition(hitWorld);

        // Keep angle stable (optional): map to [0, 360)
        radialPos.x = (radialPos.x % 360f + 360f) % 360f;

        // Clamp y to cylinder bounds (optional, matches GetRadialPosition behavior)
        radialPos.y = Mathf.Clamp(radialPos.y, minY, maxY);

        return true;
    }

    /// <summary>
    /// Ray/cylinder intersection with an infinite vertical cylinder (center at transform.position, radius r),
    /// then optionally rejects hits outside [minY, maxY].
    /// </summary>
    private bool TryIntersectRayWithCylinder(
        Ray ray,
        float r,
        out Vector3 hit,
        float maxDistance,
        bool requireWithinHeight)
    {
        hit = default;

        // Put the ray in cylinder-local coordinates (XZ plane around transform.position)
        Vector3 o = ray.origin - transform.position;
        Vector3 d = ray.direction;

        // Cylinder equation in XZ: x^2 + z^2 = r^2
        float A = d.x * d.x + d.z * d.z;

        // If A is ~0, ray is parallel to cylinder axis (straight up/down), no side hit.
        if (A < 1e-8f)
            return false;

        float B = 2f * (o.x * d.x + o.z * d.z);
        float C = (o.x * o.x + o.z * o.z) - (r * r);

        float disc = B * B - 4f * A * C;
        if (disc < 0f)
            return false;

        float sqrtDisc = Mathf.Sqrt(disc);

        // Two intersections; we want the closest positive t
        float t0 = (-B - sqrtDisc) / (2f * A);
        float t1 = (-B + sqrtDisc) / (2f * A);

        float t = float.PositiveInfinity;

        if (t0 > 0f) t = t0;
        if (t1 > 0f && t1 < t) t = t1;

        if (!float.IsFinite(t))
            return false;

        if (t > maxDistance)
            return false;

        Vector3 worldHit = ray.origin + ray.direction * t;

        if (requireWithinHeight)
        {
            if (worldHit.y < minY || worldHit.y > maxY)
                return false;
        }

        hit = worldHit;
        return true;
    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + (Vector3.up * radius), radius);
        Gizmos.DrawWireSphere(transform.position + (Vector3.up * (height - radius)), radius);
        Gizmos.DrawSphere(GetRadialPosition(debugPos), 1f);
    }
}

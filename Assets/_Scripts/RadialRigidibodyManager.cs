using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialRigidibodyManager : MonoBehaviour
{

    public static RadialRigidibodyManager instance;

    public float radius;
    public float height;

    public float minY => transform.position.y;
    public float maxY => transform.position.y + height;

    public Vector2 debugPos;
    public Vector3 debugResult;


    // Start is called before the first frame update
    void Awake()
    {
        instance = this; 
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


    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + (Vector3.up * radius), radius);
        Gizmos.DrawWireSphere(transform.position + (Vector3.up * (height - radius)), radius);
        Gizmos.DrawSphere(GetRadialPosition(debugPos), 1f);
    }
}

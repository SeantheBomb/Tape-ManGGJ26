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

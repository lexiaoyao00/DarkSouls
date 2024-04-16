using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGroundSensor : MonoBehaviour
{
    public CapsuleCollider capcol;
    public float offset = 0.1f;

    private Vector3 point1;
    private Vector3 point2;
    private float radius;

    private void Awake()
    {
        radius = capcol.radius - 0.05f;

    }

    private void FixedUpdate()
    { 
        point1 = transform.position + transform.up * (radius - offset);
        point2 = transform.position + transform.up * (capcol.height - radius - offset);
        Collider[] outputCols = Physics.OverlapCapsule(point1, point2, radius,LayerMask.GetMask("Ground"));
        if (outputCols.Length != 0)
        {
            //foreach (Collider col in outputCols)
            //{
            //    print("Collider:"+ col.name);
            //}
            SendMessageUpwards("IsGround");
        }
        else
        {
            SendMessageUpwards("IsNotGround");
        }
    }
}

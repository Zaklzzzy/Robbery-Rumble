using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragable : MonoBehaviour
{
    public Transform PointLeft;
    public Transform PointRight;

    private void OnDrawGizmosSelected()
    {
        if (PointLeft != null && PointRight != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(PointLeft.position, 0.1f);
            Gizmos.DrawSphere(PointRight.position, 0.1f);
        }
    }
}

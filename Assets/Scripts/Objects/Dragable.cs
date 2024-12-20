using Mirror;
using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NetworkIdentity))]
[RequireComponent(typeof(NetworkTransformReliable))]
public class Dragable : NetworkBehaviour
{
    [Header("Grab Points")]
    public Transform PointLeft; //Left Connect Point
    public Transform PointRight; //Right Connect Point

    [Header("Need This Object to Win?")]
    public bool required;

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

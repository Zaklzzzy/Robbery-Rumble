using Mirror;
using UnityEngine;

public class Window : NetworkBehaviour
{
    [SerializeField] private GameObject windowPart;
    [SerializeField] private float breakForce = 5f;

    private void OnCollisionEnter(Collision collision)
    {
        if (isServer && collision.relativeVelocity.magnitude > breakForce)
        {
            RpcBreakWindow(transform.position, transform.rotation);
        }
    }

    [ClientRpc]
    private void RpcBreakWindow(Vector3 position, Quaternion rotation)
    {
        GameObject brokenGlass = Instantiate(windowPart, position, rotation);

        //Spawn glass parts on all clients
        NetworkServer.Spawn(brokenGlass);

        //Add force for every parts
        foreach (Transform fragment in brokenGlass.transform)
        {
            Rigidbody rb = fragment.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 forceDirection = (fragment.position - position).normalized + Vector3.up * 0.5f;
                forceDirection.Normalize();
                rb.AddForce(forceDirection * Random.Range(2f, 5f), ForceMode.Impulse);
            }
        }
        
        //Broke glass on all clients
        NetworkServer.Destroy(gameObject);
    }
}

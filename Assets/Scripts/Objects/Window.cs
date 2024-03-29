using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
    [SerializeField] private GameObject windowPart;
    [SerializeField] private float breakForce = 5f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > breakForce)
        {
            BreakWindow();
        }
    }

    private void BreakWindow()
    {
        GameObject brokenGlass = Instantiate(windowPart, transform.position, transform.rotation);

        foreach (Transform fragment in brokenGlass.transform)
        {
            Rigidbody rb = fragment.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 forceDirection = (fragment.position - transform.position).normalized + Vector3.up * 0.5f;
                forceDirection.Normalize();
                rb.AddForce(forceDirection * Random.Range(2f, 5f), ForceMode.Impulse);
            }
        }
        Destroy(gameObject);
    }
}

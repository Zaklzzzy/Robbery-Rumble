using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Dragable : MonoBehaviour
{
    [Header("Grab Prefs")]
    
    [SerializeField] private float requiredHoldDuration = 0.5f; //Time for object grab
    [SerializeField] private bool IsHeld;
    private float holdTimer = 0f;

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

    private void Update()
    {
        // Простая логика таймера для демонстрации
        // В реальном проекте здесь будет логика проверки захвата объекта игроками
        if (holdTimer >= requiredHoldDuration)
        {
            IsHeld = true;
        }
    }

    public void AttemptHold()
    {
        holdTimer += Time.deltaTime;
    }

    public void Release()
    {
        IsHeld = false;
        holdTimer = 0f;
    }
}

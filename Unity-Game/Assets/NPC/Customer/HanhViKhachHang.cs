using UnityEngine;

public class Customer : MonoBehaviour
{
    public Vector3 targetPosition; // Vị trí mục tiêu khách cần đi tới
    public float moveSpeed = 5f;

    void Update()
    {
        // Luôn di chuyển về phía vị trí mục tiêu
        if (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }
}
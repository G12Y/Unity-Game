using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 2f;

    [Header("Mouse Settings")]
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private Transform cameraHolder;

    private CharacterController controller;
    private float rotationX;

    private void Start()
    {
        controller = GetComponent<CharacterController>();

        // Kiểm tra xem bạn đã kéo CameraHolder vào chưa
        if (cameraHolder == null)
        {
            Debug.LogError("LỖI: Bạn chưa kéo cái Camera (hoặc Transform rỗng) vào ô Camera Holder trong Inspector!");
        }

        // Khóa chuột
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        Look();
        Move();
    }

    private void Look()
    {
        if (cameraHolder == null) return;

        // Lấy dữ liệu chuột trực tiếp từ Input System
        Vector2 mouse = Mouse.current.delta.ReadValue();

        // Xoay Player trái phải
        transform.Rotate(Vector3.up * mouse.x * (mouseSensitivity / 4));

        // Xoay Camera lên xuống
        rotationX -= mouse.y * (mouseSensitivity / 4);
        rotationX = Mathf.Clamp(rotationX, -80f, 80f);

        cameraHolder.localRotation = Quaternion.Euler(rotationX, 0, 0);
    }

    private void Move()
    {
        // Lấy Input trực tiếp tại đây, không cần file GInput nữa
        Vector3 input = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) input += Vector3.forward;
        if (Input.GetKey(KeyCode.S)) input += Vector3.back;
        if (Input.GetKey(KeyCode.A)) input += Vector3.left;
        if (Input.GetKey(KeyCode.D)) input += Vector3.right;

        input.Normalize();

        // Di chuyển
        Vector3 moveDirection = transform.TransformDirection(input) * speed;
        
        if (controller != null)
        {
            controller.Move(moveDirection * Time.deltaTime);
        }
    }
}
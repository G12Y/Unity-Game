using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    public event EventHandler<OnSelectedInteractableChangedEventArgs> OnSelectedInteractableChanged;

    public class OnSelectedInteractableChangedEventArgs : EventArgs
    {
        public VatTuongTac selectedInteractable;
    }

    [Header("Movement")]
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float crouchSpeed = 1.5f;
    private float currentSpeed;

    [Header("Crouching")]
    [SerializeField] private float standingHeight = 2f;
    [SerializeField] private float crouchHeight = 1f;
    private bool isCrouching = false;
    private float velocityY; // Thêm trọng lực cơ bản

    [Header("Mouse")]
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private Transform cameraHolder;

    [Header("Interaction")]
    [SerializeField] private LayerMask interactLayer;
    [SerializeField] private float interactDistance = 2f;

    [Header("Cooking System (Hệ thống cầm nắm)")]
    public Transform handPoint;      // Kéo một GameObject rỗng đại diện cho bàn tay vào đây
    public GameObject itemInHand;    // Lưu trữ nguyên liệu/món ăn đang cầm trên tay

    private CharacterController controller;
    private float rotationX;
    private VatTuongTac selectedInteractable;

    private void Awake()
    {
        if (Instance != null) Debug.LogWarning("Có nhiều PlayerController trong Scene!");
        Instance = this;
    }

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        currentSpeed = walkSpeed;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (cameraHolder == null) Debug.LogError("Chưa gán Camera Holder!");
    }

    private void Update()
    {
        Look();
        Move();
        HandleInteractions();
    }

    private void Look()
    {
        if (cameraHolder == null) return;

        Vector2 mouse = Mouse.current.delta.ReadValue();

        transform.Rotate(Vector3.up * mouse.x * mouseSensitivity * 0.25f);

        rotationX -= mouse.y * mouseSensitivity * 0.25f;
        rotationX = Mathf.Clamp(rotationX, -80f, 80f);

        cameraHolder.localRotation = Quaternion.Euler(rotationX, 0, 0);
    }

    private void Move()
    {
        // 1. Xử lý Input di chuyển
        Vector3 input = Vector3.zero;
        if (Keyboard.current.wKey.isPressed) input += Vector3.forward;
        if (Keyboard.current.sKey.isPressed) input += Vector3.back;
        if (Keyboard.current.aKey.isPressed) input += Vector3.left;
        if (Keyboard.current.dKey.isPressed) input += Vector3.right;
        input.Normalize();

        // 2. Xử lý Ngồi (Crouch) bằng phím C hoặc Left Ctrl
        if (Keyboard.current.cKey.isPressed || Keyboard.current.leftCtrlKey.isPressed)
        {
            isCrouching = true;
            controller.height = crouchHeight;
            currentSpeed = crouchSpeed;
        }
        else
        {
            isCrouching = false;
            controller.height = standingHeight;
            currentSpeed = walkSpeed;
        }

        // 3. Di chuyển Character
        Vector3 move = transform.TransformDirection(input);
        
        // 4. Áp dụng trọng lực (Gravity) để lúc đứng lên ngồi xuống mượt hơn
        if (controller.isGrounded)
        {
            velocityY = -2f; // Dính chặt xuống sàn
        }
        else
        {
            velocityY += -9.81f * Time.deltaTime; // Trọng lực
        }

        Vector3 finalMovement = move * currentSpeed;
        finalMovement.y = velocityY;

        controller.Move(finalMovement * Time.deltaTime);
    }

    private void HandleInteractions()
    {
        if (cameraHolder == null) return;

        Vector3 origin = cameraHolder.position;

        // 1. Kiểm tra tia Raycast
        if (Physics.Raycast(origin, cameraHolder.forward, out RaycastHit hit, interactDistance, interactLayer))
        {
            VatTuongTac interactable = hit.collider.GetComponent<VatTuongTac>();

            if (interactable == null)
                interactable = hit.collider.GetComponentInParent<VatTuongTac>();

            // Nếu tia Raycast trúng vật gì đó, in tên vật đó ra Console
            // Debug.Log("Raycast đang nhìn vào: " + hit.collider.gameObject.name);

            if (interactable != selectedInteractable)
            {
                SetSelectedInteractable(interactable);
            }
        }
        else
        {
            SetSelectedInteractable(null);
        }

        // 2. Kiểm tra phím E
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {   
            Debug.Log(">> ĐÃ BẤM PHÍM E"); // Kiểm tra xem phím có nhận không

            if (selectedInteractable != null)
            {
                Debug.Log(">> Đang gọi hàm Interact của: " + selectedInteractable.gameObject.name);
                selectedInteractable.Interact(this); 
            }
            else
            {
                Debug.Log(">> Bấm E nhưng KHÔNG nhìn thấy vật nào có script VatTuongTac!");
            }
        }
    }
    private void SetSelectedInteractable(VatTuongTac interactable)
    {
        selectedInteractable = interactable;
        OnSelectedInteractableChanged?.Invoke(this, new OnSelectedInteractableChangedEventArgs
        {
            selectedInteractable = selectedInteractable
        });
    }
}
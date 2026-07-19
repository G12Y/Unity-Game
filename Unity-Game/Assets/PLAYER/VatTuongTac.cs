using UnityEngine;

public abstract class VatTuongTac : MonoBehaviour
{
    // Cập nhật hàm Interact để truyền vào PlayerController
    public abstract void Interact(PlayerController player);
}
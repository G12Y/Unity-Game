using System.Collections.Generic;
using UnityEngine;

public class IngredientShelf : VatTuongTac
{
    [Header("Cài đặt Kệ Hàng")]
    public IngredientType typeToDispense;
    public GameObject itemPrefab;

    [Header("Hiển thị trực quan (Visuals)")]
    [Tooltip("Kéo các Object trống đại diện cho vị trí đặt đồ vào đây")]
    public List<Transform> displaySlots; 
    
    // Dùng Stack để lưu các đồ vật đang nằm trên kệ (Lấy ra theo thứ tự từ trên xuống / từ ngoài vào trong)
    private Stack<GameObject> spawnedVisualItems = new Stack<GameObject>();

    private void Start()
    {
        // Khởi tạo đồ ăn trên kệ ngay khi vào game
        UpdateShelfVisuals();
    }

    // Hàm này sinh ra các mô hình đồ ăn đặt lên kệ
    public void UpdateShelfVisuals()
    {
        if (displaySlots == null || displaySlots.Count == 0) 
        {
            Debug.LogWarning("Chú ý: Kệ hàng " + gameObject.name + " chưa được gán Slot hiển thị nào!");
            return; 
        }
        // Nếu không có GameManager hoặc chưa gán Slot thì bỏ qua
        if (GameManager.Instance == null || displaySlots.Count == 0) return;

        // Xóa các đồ vật trực quan cũ nếu có
        foreach (GameObject obj in spawnedVisualItems)
        {
            Destroy(obj);
        }
        spawnedVisualItems.Clear();

        // Lấy số lượng nguyên liệu trong kho
        int inventoryCount = GameManager.Instance.inventory[typeToDispense];
        
        // Chỉ hiển thị tối đa bằng số lượng slot đang có trên kệ
        int displayCount = Mathf.Min(inventoryCount, displaySlots.Count);

        for (int i = 0; i < displayCount; i++)
        {
            // Sinh ra vật phẩm tại vị trí của Slot
            GameObject visualItem = Instantiate(itemPrefab, displaySlots[i].position, displaySlots[i].rotation);
            visualItem.transform.SetParent(displaySlots[i]);

            // QUAN TRỌNG: Tắt script HoldableItem và Collider của cục nằm trên kệ 
            // để nó không rớt xuống đất hay chắn tia Raycast của người chơi
            HoldableItem holdable = visualItem.GetComponent<HoldableItem>();
            if (holdable != null) Destroy(holdable);
            
            Collider col = visualItem.GetComponent<Collider>();
            if (col != null) col.enabled = false;
            
            Rigidbody rb = visualItem.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = true;

            // Đưa vào danh sách để quản lý
            spawnedVisualItems.Push(visualItem);
        }
    }

    public override void Interact(PlayerController player)
    {
        if (GameManager.Instance == null || itemPrefab == null) return;

        // 1. CHẶN KHÔNG CHO LẤY NẾU TAY ĐANG CẦM ĐỒ
        if (player.itemInHand != null)
        {
            Debug.Log("<color=yellow>Tay đang cầm đồ rồi, không thể lấy thêm!</color>");
            return; // Dừng hàm lại ngay lập tức
        }

        // 2. Kiểm tra kho còn đồ không
        if (GameManager.Instance.inventory[typeToDispense] > 0)
        {
            // Trừ số lượng trong kho
            GameManager.Instance.inventory[typeToDispense]--;

            // Xóa bớt 1 mô hình hiển thị trên kệ
            if (spawnedVisualItems.Count > 0)
            {
                GameObject itemToRemove = spawnedVisualItems.Pop();
                Destroy(itemToRemove);
            }

            // Sinh ra cục nguyên liệu THẬT trên tay người chơi
            GameObject newItem = Instantiate(itemPrefab);
            HoldableItem holdable = newItem.GetComponent<HoldableItem>();
            
            if (holdable != null)
            {
                holdable.PickUp(player.handPoint);
                player.itemInHand = newItem; // Đánh dấu là tay đã có đồ
                
                Debug.Log("<color=green>Đã lấy " + typeToDispense + "</color>. Còn lại trong kho: " + GameManager.Instance.inventory[typeToDispense]);
            }
        }
        else
        {
            Debug.Log("<color=red>Đã hết " + typeToDispense + " trong kho!</color>");
        }
    }
}
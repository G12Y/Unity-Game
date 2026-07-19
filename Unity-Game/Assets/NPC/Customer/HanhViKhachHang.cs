using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Customer : VatTuongTac
{
    public Vector3 targetPosition;
    public float moveSpeed = 5f;

    [Header("Cấu hình Order")]
    public List<IngredientType> mandatoryIngredients; // List bắt buộc (Bánh mì)
    public List<IngredientType> optionalPool;      // List chọn ngẫu nhiên
    public int minExtras = 1;
    public int maxExtras = 2;

    [Header("Đơn hàng hiện tại")]
    public List<IngredientType> currentOrder = new List<IngredientType>();
    
    private bool isServed = false;

    
    [Header("Thời gian chờ")]
    public float maxWaitTime = 30f; // Bạn có thể chỉnh 10s, 30s tùy ý trong Inspector
    private float currentWaitTime;
    public Slider patienceBar; // Kéo Slider UI trên đầu khách vào đây
    private bool isLeaving = false;

    private void Start()
    {
        currentWaitTime = maxWaitTime;
        if (patienceBar != null) patienceBar.maxValue = maxWaitTime;
        GenerateOrder();
    }
    void GenerateOrder()
    {
        currentOrder.Clear();
        // 1. Thêm món bắt buộc
        currentOrder.AddRange(mandatoryIngredients);

        // 2. Thêm món ngẫu nhiên
        int extraCount = Random.Range(minExtras, maxExtras + 1);
        for (int i = 0; i < extraCount; i++)
        {
            if (optionalPool.Count > 0)
            {
                currentOrder.Add(optionalPool[Random.Range(0, optionalPool.Count)]);
            }
        }
        
        Debug.Log($"Khách hàng yêu cầu: {string.Join(", ", currentOrder)}");
    }

    private void Update()
    {
        // 1. Di chuyển NPC
        if (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }

        // 2. Tính thời gian chờ (chỉ giảm khi đã đứng đúng vị trí chờ)
        if (!isLeaving && Vector3.Distance(transform.position, targetPosition) < 0.2f)
        {
            currentWaitTime -= Time.deltaTime;
            if (patienceBar != null) patienceBar.value = currentWaitTime;

            if (currentWaitTime <= 0)
            {
                CustomerLeavesDueToTimeout();
            }
        }
    }
    
    void CustomerLeavesDueToTimeout()
    {
        isLeaving = true;
        Debug.Log("<color=red>Khách đã mất kiên nhẫn và bỏ đi!</color>");
        // Gọi manager xóa khách khỏi hàng để người sau tiến lên
        FindObjectOfType<QueueManager>().ServeCustomerByObject(this.gameObject);
    }
    public override void Interact(PlayerController player)
    {   
        if (isLeaving) return;
        if (isServed) return;

        // KIỂM TRA 1: Phải có đồ trên tay
        if (player.itemInHand == null)
        {
            Debug.Log("Bạn không cầm gì cả, khách không quan tâm!");
            return;
        }

        // KIỂM TRA 2: Đồ trên tay phải là Món ăn hoàn chỉnh (AssembledDish)
        AssembledDish dish = player.itemInHand.GetComponent<AssembledDish>();
        if (dish == null)
        {
            Debug.Log("Cầm nguyên liệu sống khách không lấy đâu!");
            return;
        }

        // KIỂM TRA 3: Kiểm tra công thức món ăn
        if (IsDishMatching(dish))
        {
            PayAndLeave(dish, player);
        }
        else
        {
            Debug.Log("<color=red>Khách: Cái này không đúng món tôi gọi!</color>");
        }
    }

    bool IsDishMatching(AssembledDish dish)
    {
        // 1. Kiểm tra số lượng nguyên liệu có khớp không
        if (dish.containedIngredients.Count != currentOrder.Count) return false;

        // 2. Kiểm tra xem các loại nguyên liệu có trùng khớp không
        // Dùng bản sao để so sánh tránh làm hỏng data gốc
        List<IngredientType> checkList = new List<IngredientType>(dish.containedIngredients);
        
        foreach (IngredientType required in currentOrder)
        {
            // Nếu tìm thấy nguyên liệu yêu cầu trong đĩa
            if (checkList.Contains(required))
            {
                checkList.Remove(required); // Xóa khỏi danh sách kiểm tra để tránh trùng lặp
            }
            else
            {
                return false; // Thiếu nguyên liệu khách cần
            }
        }
        return true; // Khớp hoàn toàn
    }

    void PayAndLeave(AssembledDish dish, PlayerController player)
    {
        isServed = true;

        // Tính tiền: 130% - 150% giá trị gốc
        float tipMultiplier = Random.Range(1.3f, 1.5f);
        int finalPrice = Mathf.RoundToInt(dish.totalBaseCost * tipMultiplier);

        GameManager.Instance.AddMoney(finalPrice);
        Debug.Log($"<color=green>Đã bán món ăn! Nhận: {finalPrice} (Gốc: {dish.totalBaseCost})</color>");

        // Xóa món ăn trên tay
        Destroy(player.itemInHand);
        player.itemInHand = null;

        // Gọi Manager để đẩy hàng chờ
        FindObjectOfType<QueueManager>().ServeCustomerByObject(this.gameObject);
    }
}
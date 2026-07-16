using UnityEngine;

public enum IngredientType
{
    Bread,
    Tomato,
    Meat,
    Lettuce,
    Egg,
}

public class Ingredient : MonoBehaviour
{
    [Header("Thông tin nguyên liệu")]
    public string itemName;
    public IngredientType ingredientType;

    [Header("Số lượng còn lại trên kệ")]
    public int amount = 5;

    [Header("Prefab cầm trên tay")]
    public GameObject heldPrefab;

    // Có thể nhặt không?
    public bool CanPick()
    {
        return amount > 0;
    }

    // Nhặt 1 nguyên liệu
    public void PickOne()
    {
        if (amount > 0)
            amount--;
    }

    // Trả lại 1 nguyên liệu (nếu cần)
    public void ReturnOne()
    {
        amount++;
    }
}
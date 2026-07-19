using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    [Header("Điểm cầm đồ")]
    public Transform handPoint;

    private Ingredient heldIngredient;
    private FoodInfo heldFood;

    private GameObject heldObject;

    //==================================================
    // Kiểm tra đang cầm gì không
    //==================================================
    public bool IsHolding()
    {
        return heldIngredient != null || heldFood != null;
    }

    //==================================================
    // Cầm nguyên liệu
    //==================================================
    public void Hold(Ingredient ingredient)
    {
        if (IsHolding())
            return;

        heldIngredient = ingredient;

        heldObject = Instantiate(
            ingredient.heldPrefab,
            handPoint);

        heldObject.transform.localPosition = Vector3.zero;
        heldObject.transform.localRotation = Quaternion.identity;
    }

    //==================================================
    // Cầm món ăn
    //==================================================
    public void Hold(FoodInfo food)
    {
        if (IsHolding())
            return;

        heldFood = food;

        // Nếu FoodInfo có heldPrefab thì dùng nó,
        // còn không thì dùng chính prefab món ăn
        GameObject prefab = food.heldPrefab != null
            ? food.heldPrefab
            : food.recipe.resultPrefab;

        heldObject = Instantiate(
            prefab,
            handPoint);

        heldObject.transform.localPosition = Vector3.zero;
        heldObject.transform.localRotation = Quaternion.identity;
    }

    //==================================================
    // Xóa vật đang cầm
    //==================================================
    public void ClearHand()
    {
        if (heldObject != null)
            Destroy(heldObject);

        heldObject = null;
        heldIngredient = null;
        heldFood = null;
    }

    //==================================================
    // Lấy nguyên liệu đang cầm
    //==================================================
    public Ingredient GetHeldIngredient()
    {
        return heldIngredient;
    }

    //==================================================
    // Lấy món ăn đang cầm
    //==================================================
    public FoodInfo GetHeldFood()
    {
        return heldFood;
    }

    //==================================================
    // Kiểm tra đang cầm nguyên liệu
    //==================================================
    public bool IsHoldingIngredient()
    {
        return heldIngredient != null;
    }

    //==================================================
    // Kiểm tra đang cầm món ăn
    //==================================================
    public bool IsHoldingFood()
    {
        return heldFood != null;
    }
}
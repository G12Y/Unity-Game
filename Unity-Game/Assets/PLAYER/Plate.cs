using UnityEngine;
using System.Collections.Generic;

public class Plate : VatTuongTac
{
    public Transform stackStartPoint; // Vị trí tâm cái đĩa
    public float stackYOffset = 0.05f; 
    
    private List<HoldableItem> itemsOnPlate = new List<HoldableItem>();

    public override void Interact(PlayerController player)
    {
        if (player.itemInHand != null)
        {
            HoldableItem item = player.itemInHand.GetComponent<HoldableItem>();
            if (item != null)
            {
                // XỬ LÝ VỊ TRÍ ĐẶT ĐỒ (FIX LỖI BỊ CHÌM)
                PlaceItemOnPlate(item);
                player.itemInHand = null;
            }
        }
        else if (player.itemInHand == null && itemsOnPlate.Count > 0)
        {
            AssembleFinalDish(player);
        }
    }

    void PlaceItemOnPlate(HoldableItem newItem)
    {
        newItem.transform.SetParent(stackStartPoint);
        
        // 1. Tính toán độ cao xếp chồng
        float currentY = itemsOnPlate.Count * stackYOffset;

        // 2. Logic đặc biệt cho bánh mì rỗng (Fix lỗi đặt vào lòng bánh mì)
        // Nếu đĩa chưa có gì và món đầu tiên là Bánh mì
        if (itemsOnPlate.Count > 0)
        {
            // Kiểm tra xem miếng đầu tiên có phải là Bánh mì (để lấy cái lòng rỗng) không
            HoldableItem firstItem = itemsOnPlate[0];
            if (firstItem.ingredientType == IngredientType.BanhMi)
            {
                // Bạn có thể nâng nhẹ Y lên một chút để nhân bánh nằm trên miếng bánh mì dưới
                currentY += 0.03f; 
            }
        }

        // 3. Ép vị trí (X, Z là 0 để vào giữa, Y là chiều cao tầng)
        newItem.transform.localPosition = new Vector3(0, currentY, 0);
        newItem.transform.localRotation = Quaternion.identity;
        newItem.transform.localScale = Vector3.one;

        itemsOnPlate.Add(newItem);
    }

    void AssembleFinalDish(PlayerController player)
    {
        GameObject finalDishObj = new GameObject("Dish_Finished");
        finalDishObj.transform.position = stackStartPoint.position;
        AssembledDish dishScript = finalDishObj.AddComponent<AssembledDish>();

        foreach (HoldableItem i in itemsOnPlate)
        {
            dishScript.containedIngredients.Add(i.ingredientType);
            dishScript.totalBaseCost += GameManager.Instance.itemPrices[i.ingredientType];
            
            i.transform.SetParent(finalDishObj.transform, true);
            Destroy(i);
        }
        
        itemsOnPlate.Clear();
        dishScript.PickUpToHand(player);

        // Add BoxCollider để sau này còn tương tác lại
        BoxCollider col = finalDishObj.AddComponent<BoxCollider>();
        col.center = new Vector3(0, 0.1f, 0);
        col.size = new Vector3(0.5f, 0.5f, 0.5f);
    }
}
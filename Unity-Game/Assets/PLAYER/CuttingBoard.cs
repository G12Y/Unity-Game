using UnityEngine;

public class CuttingBoard : VatTuongTac
{
    public override void Interact(PlayerController player)
    {
        if (player.itemInHand != null)
        {
            HoldableItem item = player.itemInHand.GetComponent<HoldableItem>();

            if (item != null && item.currentState == ItemState.Raw)
            {
                // Kiểm tra các món có thể thái: Bánh mì, Dưa chuột, Chả...
                if (item.ingredientType == IngredientType.BanhMi || 
                    item.ingredientType == IngredientType.DuaChuot || 
                    item.ingredientType == IngredientType.Cha)
                {
                    // QUAN TRỌNG: Gọi ChangeState để đổi Mesh sang mẫu đã thái
                    item.ChangeState(ItemState.Chopped);
                    Debug.Log("<color=yellow>Thớt: Đã thái xong " + item.ingredientType + "</color>");
                }
            }
        }
    }
}
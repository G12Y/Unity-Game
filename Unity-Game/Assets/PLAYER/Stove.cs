using UnityEngine;

public class Stove : VatTuongTac
{
    public override void Interact(PlayerController player)
{
    if (player.itemInHand != null)
    {
        HoldableItem item = player.itemInHand.GetComponent<HoldableItem>();

        if (item != null)
        {
            // Kiểm tra trạng thái và Loại
            if (item.currentState == ItemState.Raw && 
               (item.ingredientType == IngredientType.Thit || item.ingredientType == IngredientType.Trung))
            {
                item.ChangeState(ItemState.Cooked);
                Debug.Log("<color=orange>Bếp: Đã nấu chín " + item.ingredientType + "</color>");
            }
            else {
                Debug.Log("<color=red>Bếp: Vật này không nấu được trên bếp!</color>");
            }
        }
    }
}
}

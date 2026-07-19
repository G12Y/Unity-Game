using UnityEngine;

public class TrashCan : VatTuongTac
{
    public override void Interact(PlayerController player)
    {
        if (player.itemInHand != null)
        {
            Debug.Log("<color=red>Thùng rác:</color> Đã vứt bỏ " + player.itemInHand.name);

            // Lưu tham chiếu để xóa
            GameObject itemToBeDestroyed = player.itemInHand;

            // 1. Reset biến trên tay người chơi về null NGAY LẬP TỨC
            player.itemInHand = null;

            // 2. Xóa Object đó khỏi thế giới
            Destroy(itemToBeDestroyed);
        }
        else
        {
            Debug.Log("Tay trống, không có gì để vứt.");
        }
    }
}
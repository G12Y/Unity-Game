using System.Collections.Generic;
using UnityEngine;

public class AssembledDish : VatTuongTac
{
    public List<IngredientType> containedIngredients = new List<IngredientType>();
    public int totalBaseCost = 0;

    public override void Interact(PlayerController player)
    {
        // Khi bấm E vào món ăn (lúc nó đang ở trên bàn), nó sẽ được cầm lên tay
        if (player.itemInHand == null)
        {
            PickUpToHand(player);
        }
    }

    public void PickUpToHand(PlayerController player)
    {
        player.itemInHand = this.gameObject;
        
        transform.SetParent(player.handPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        // BƯỚC 1: Đổi layer sang 2 (Ignore Raycast) để tia Raycast xuyên qua được món ăn
        SetLayerRecursively(this.gameObject, 2); 

        // BƯỚC 2: Tắt mọi collider (cho chắc chắn)
        foreach (Collider col in GetComponentsInChildren<Collider>())
        {
            col.enabled = false;
        }

        if (GetComponent<Rigidbody>()) GetComponent<Rigidbody>().isKinematic = true;
        
        Debug.Log("<color=cyan>Đã cầm món ăn, layer đổi sang Ignore Raycast.</color>");
    }

    // Hàm phụ trợ để đổi layer cho toàn bộ con bên trong món ăn
    private void SetLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
}
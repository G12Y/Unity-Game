using UnityEngine;
using TMPro;

public class ItemUI : MonoBehaviour
{
    [Header("UI")]
    public GameObject panel;

    public TMP_Text itemNameText;
    public TMP_Text amountText;
    public TMP_Text pressEText;

    // Hiện thông tin nguyên liệu
    public void Show(Ingredient ingredient)
    {
        panel.SetActive(true);

        itemNameText.text = "Tên: " + ingredient.itemName;
        amountText.text = "Số lượng: " + ingredient.amount;
        pressEText.text = "Nhấn E để nhặt";
    }

    // Ẩn UI
    public void Hide()
    {
        panel.SetActive(false);
    }

    // Cập nhật số lượng sau khi nhặt
    public void Refresh(Ingredient ingredient)
    {
        amountText.text = "Số lượng: " + ingredient.amount;
    }
}
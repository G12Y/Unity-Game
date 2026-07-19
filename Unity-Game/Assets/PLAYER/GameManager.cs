using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int startingIngredientCount = 10;
    public static GameManager Instance;

    public int money = 1000;
    
    // số lượng nguyên liệu đã mua
    public Dictionary<IngredientType, int> inventory = new Dictionary<IngredientType, int>();
    
    public Dictionary<IngredientType, int> itemPrices = new Dictionary<IngredientType, int>()
    {
        { IngredientType.Rau, 10 },
        { IngredientType.BanhMi, 15 },
        { IngredientType.Cha, 20 },
        { IngredientType.Trung, 12 },
        { IngredientType.Thit, 30 },
        { IngredientType.DuaChuot, 8 }
    };

    private void Awake()
    {
        if (Instance == null) Instance = this;
        
        
        foreach (IngredientType type in System.Enum.GetValues(typeof(IngredientType)))
        {
            if(type != IngredientType.None) inventory[type] = startingIngredientCount;
        }
    }

    public void BuyIngredient(IngredientType type)
    {
        int price = itemPrices[type];
        if (money >= price)
        {
            money -= price;
            inventory[type]++;
            Debug.Log("Đã mua: " + type + " | Số lượng hiện tại: " + inventory[type]);
        }
        else
        {
            Debug.Log("Không đủ tiền!");
        }
    }
    public void AddMoney(int amount)
    {
        money += amount;
        Debug.Log("<color=green>Đã cộng tiền: +" + amount + " | Tổng tiền: " + money + "</color>");
    }
}
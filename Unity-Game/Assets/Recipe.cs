using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Cooking/Recipe")]
public class Recipe : ScriptableObject
{
    [Header("Tên món ăn")]
    public string recipeName;

    [Header("Nguyên liệu cần")]
    public List<IngredientType> ingredients = new List<IngredientType>();

    [Header("Prefab món ăn")]
    public GameObject resultPrefab;
}
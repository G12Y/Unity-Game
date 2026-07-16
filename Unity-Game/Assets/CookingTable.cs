using System.Collections.Generic;
using UnityEngine;

public class CookingTable : MonoBehaviour
{
    [Header("Các vị trí đặt nguyên liệu")]
    public List<Transform> placePoints = new List<Transform>();

    [Header("Quản lý công thức")]
    public RecipeManager recipeManager;

    private List<Ingredient> currentIngredients = new List<Ingredient>();
    private List<GameObject> placedObjects = new List<GameObject>();

    public bool AddIngredient(Ingredient ingredient)
    {
        // Bàn đầy
        if (currentIngredients.Count >= placePoints.Count)
            return false;

        currentIngredients.Add(ingredient);

        // Hiện nguyên liệu lên bàn
        GameObject obj = Instantiate(
            ingredient.heldPrefab,
            placePoints[currentIngredients.Count - 1]);

        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;

        placedObjects.Add(obj);

        Debug.Log("Đã đặt: " + ingredient.itemName);

        CheckRecipe();

        return true;
    }

    void CheckRecipe()
    {
        if (recipeManager == null)
        {
            Debug.LogWarning("Chưa gán RecipeManager!");
            return;
        }

        Recipe recipe = recipeManager.FindRecipe(currentIngredients);

        if (recipe == null)
            return;

        Debug.Log("Đã tạo món: " + recipe.recipeName);

        CreateFood(recipe);
    }

    void CreateFood(Recipe recipe)
    {
        // Xóa nguyên liệu trên bàn
        foreach (GameObject obj in placedObjects)
        {
            Destroy(obj);
        }

        placedObjects.Clear();
        currentIngredients.Clear();

        // Tạo món ăn
        GameObject food = Instantiate(
            recipe.resultPrefab,
            transform.position + Vector3.up * 1f,
            Quaternion.identity);

        Debug.Log("Spawn thành công: " + food.name);
    }
}
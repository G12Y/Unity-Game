using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    public List<Recipe> recipes = new List<Recipe>();

    public Recipe FindRecipe(List<Ingredient> ingredients)
    {
        foreach (Recipe recipe in recipes)
        {
            if (IsMatch(recipe, ingredients))
                return recipe;
        }

        return null;
    }

    private bool IsMatch(Recipe recipe, List<Ingredient> ingredients)
    {
        if (recipe.ingredients.Count != ingredients.Count)
            return false;

        List<IngredientType> input = new List<IngredientType>();

        foreach (Ingredient ingredient in ingredients)
        {
            input.Add(ingredient.ingredientType);
        }

        input.Sort();

        List<IngredientType> target = new List<IngredientType>(recipe.ingredients);
        target.Sort();

        for (int i = 0; i < input.Count; i++)
        {
            if (input[i] != target[i])
                return false;
        }

        return true;
    }
}
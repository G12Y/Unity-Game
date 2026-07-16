using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    public Transform handPoint;

    private Ingredient heldIngredient;
    private GameObject heldObject;

    public bool IsHolding()
    {
        return heldIngredient != null;
    }

    public void Hold(Ingredient ingredient)
    {
        if (IsHolding())
            return;

        heldIngredient = ingredient;

        heldObject = Instantiate(
            ingredient.heldPrefab,
            handPoint);

        heldObject.transform.localPosition = Vector3.zero;
        heldObject.transform.localRotation = Quaternion.identity;
    }

    public void ClearHand()
    {
        if (heldObject != null)
            Destroy(heldObject);

        heldObject = null;
        heldIngredient = null;
    }

    public Ingredient GetHeldIngredient()
    {
        return heldIngredient;
    }
}
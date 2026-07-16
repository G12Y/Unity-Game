using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Header("Raycast")]
    public float interactDistance = 3f;

    [Header("References")]
    public PlayerHand playerHand;
    public ItemUI itemUI;

    private Ingredient currentIngredient;

    void Update()
    {
        DetectIngredient();

        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    void DetectIngredient()
    {
        currentIngredient = null;

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            Ingredient ingredient = hit.collider.GetComponent<Ingredient>();

            if (ingredient != null)
            {
                currentIngredient = ingredient;
                itemUI.Show(ingredient);
                return;
            }
        }

        itemUI.Hide();
    }

    void Interact()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit, interactDistance))
            return;

        // ======================
        // Nhặt nguyên liệu
        // ======================
        Ingredient ingredient = hit.collider.GetComponent<Ingredient>();

        if (ingredient != null)
        {
            if (!playerHand.IsHolding() && ingredient.CanPick())
            {
                ingredient.PickOne();
                playerHand.Hold(ingredient);
                itemUI.Refresh(ingredient);
            }

            return;
        }

        // ======================
        // Đặt lên bàn
        // ======================
        CookingTable table = hit.collider.GetComponent<CookingTable>();

        if (table != null)
        {
            if (playerHand.IsHolding())
            {
                Ingredient held = playerHand.GetHeldIngredient();

                if (table.AddIngredient(held))
                {
                    playerHand.ClearHand();
                }
            }
        }
    }
}
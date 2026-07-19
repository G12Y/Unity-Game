using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Header("Raycast")]
    public float interactDistance = 3f;

    [Header("References")]
    public PlayerHand playerHand;
    public ItemUI itemUI;

    private Ingredient currentIngredient;
    private FoodInfo currentFood;

    void Update()
    {
        DetectObject();

        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    void DetectObject()
    {
        currentIngredient = null;
        currentFood = null;

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            //=====================
            // Nguyên liệu
            //=====================
            Ingredient ingredient = hit.collider.GetComponent<Ingredient>();

            if (ingredient != null)
            {
                currentIngredient = ingredient;
                itemUI.Show(ingredient);
                return;
            }

            //=====================
            // Món ăn
            //=====================
            FoodInfo food = hit.collider.GetComponent<FoodInfo>();

            if (food != null)
            {
                currentFood = food;
                itemUI.ShowFood(food);
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

        //=====================
        // Nhặt nguyên liệu
        //=====================
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

        //=====================
        // Nhặt món ăn
        //=====================
        FoodInfo food = hit.collider.GetComponent<FoodInfo>();

        if (food != null)
        {
            if (!playerHand.IsHolding())
            {
                playerHand.Hold(food);
                Debug.Log("Đang nhặt: " + food.gameObject.name);
                Destroy(food.gameObject);
                itemUI.Hide();
            }

            return;
        }

        //=====================
        // Đặt nguyên liệu lên bàn
        //=====================
        CookingTable table = hit.collider.GetComponent<CookingTable>();

        if (table != null)
        {
            if (playerHand.IsHoldingIngredient())
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
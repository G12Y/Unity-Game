using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class HoldableItem : MonoBehaviour
{
    public IngredientType ingredientType;
    public ItemState currentState = ItemState.Raw;

    [Header("Kéo File FBX từ Assets vào đây")]
    public GameObject rawAsset;
    public GameObject choppedAsset; // Ô dành cho đồ đã thái (Bánh mì kẹp, dưa chuột lát)
    public GameObject cookedAsset;  // Ô dành cho đồ đã nấu (Thịt chín)

    private MeshFilter myFilter;
    private MeshRenderer myRenderer;

    private void Awake()
    {
        myFilter = GetComponent<MeshFilter>();
        myRenderer = GetComponent<MeshRenderer>();
    }

    private void Start() { UpdateVisuals(); }

    public void ChangeState(ItemState newState)
    {
        currentState = newState;
        UpdateVisuals();
    }

    public void UpdateVisuals()
    {
        // Chọn Asset tương ứng với trạng thái
        GameObject targetAsset = null;
        if (currentState == ItemState.Raw) targetAsset = rawAsset;
        else if (currentState == ItemState.Chopped) targetAsset = choppedAsset;
        else if (currentState == ItemState.Cooked) targetAsset = cookedAsset;

        if (targetAsset != null)
        {
            MeshFilter assetFilter = targetAsset.GetComponentInChildren<MeshFilter>();
            MeshRenderer assetRenderer = targetAsset.GetComponentInChildren<MeshRenderer>();

            if (assetFilter != null) myFilter.sharedMesh = assetFilter.sharedMesh;
            if (assetRenderer != null) myRenderer.sharedMaterials = assetRenderer.sharedMaterials;

            myRenderer.enabled = true;
            //transform.localScale = Vector3.one;
        }
    }
    
    // Giữ nguyên hàm PickUp cũ của bạn bên dưới...
    public void PickUp(Transform handTransform)
    {
        transform.SetParent(handTransform);
        transform.localPosition = new Vector3(0, 0, 0.2f);
        transform.localRotation = Quaternion.identity;
        if (GetComponent<Collider>()) GetComponent<Collider>().enabled = false;
        if (GetComponent<Rigidbody>()) GetComponent<Rigidbody>().isKinematic = true;
    }
}
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ItemData;

public class MachineUiMgr : MonoBehaviour
{
    [SerializeField]
    Inventory inventory;

    [SerializeField]
    GameObject productListObj;
    RectTransform productListRc;
    GridLayoutGroup productListGridLayout;

    [SerializeField]
    GameObject VendingMachineItemBtn;

    void Awake()
    {
        productListRc = productListObj.GetComponent<RectTransform>();
        productListGridLayout = productListObj.GetComponent<GridLayoutGroup>();

        if (inventory == null)
            inventory = FindFirstObjectByType<Inventory>();
    }

    public void SetProductListBtn(
        Dictionary<int, ProductData> products,
        IReadOnlyDictionary<int, Sprite> productImages,
        Inventory inventoryRef = null)
    {
        if (products == null || VendingMachineItemBtn == null || productListObj == null)
            return;

        ClearProductList();

        var targetInventory = inventoryRef != null ? inventoryRef : inventory;

        foreach (var product in products.Values)
        {
            var productBtn = Instantiate(VendingMachineItemBtn, productListObj.transform);
            var productBtnComp = productBtn.GetComponent<ProductBtn>();
            productBtnComp.SetProductData(product, productImages[product.id] ?? null);

            if (targetInventory != null)
                targetInventory.RegisterBuyCallback(productBtnComp);
            else
                Debug.LogWarning("[MachineUiMgr] Inventory is not assigned. Buy callback was not registered.");
            productBtn.transform.SetParent(productListObj.transform);        }

        SetProductListRc();
    }

    void ClearProductList()
    {
        for (int i = productListObj.transform.childCount - 1; i >= 0; i--)
            Destroy(productListObj.transform.GetChild(i).gameObject);
    }

    public void SetProductListRc()
    {
        if (productListRc == null || productListGridLayout == null)
            return;

        int childCount = productListObj.transform.childCount;
        if (childCount == 0)
        {
            productListRc.sizeDelta = Vector2.zero;
            return;
        }

        Vector2 cellSize = productListGridLayout.cellSize;
        Vector2 spacing = productListGridLayout.spacing;
        RectOffset padding = productListGridLayout.padding;

        int columnCount = GetColumnCount(childCount);
        int rowCount = Mathf.CeilToInt((float)childCount / columnCount);

        float width = padding.horizontal + columnCount * cellSize.x + Mathf.Max(0, columnCount - 1) * spacing.x;
        float height = padding.vertical + rowCount * cellSize.y + Mathf.Max(0, rowCount - 1) * spacing.y;

        bool stretchWidth = !Mathf.Approximately(productListRc.anchorMin.x, productListRc.anchorMax.x);
        productListRc.sizeDelta = stretchWidth
            ? new Vector2(productListRc.sizeDelta.x, height)
            : new Vector2(width, height);
    }

    int GetColumnCount(int childCount)
    {
        switch (productListGridLayout.constraint)
        {
            case GridLayoutGroup.Constraint.FixedColumnCount:
                return Mathf.Max(1, productListGridLayout.constraintCount);
            case GridLayoutGroup.Constraint.FixedRowCount:
                return Mathf.CeilToInt((float)childCount / Mathf.Max(1, productListGridLayout.constraintCount));
            default:
                return 1;
        }
    }


}

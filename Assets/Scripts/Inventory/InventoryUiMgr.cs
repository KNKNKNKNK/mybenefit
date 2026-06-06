using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ItemData;

public class InventoryUiMgr : MonoBehaviour
{
    [SerializeField]
    Inventory inventory;

    [SerializeField]
    GameObject inventoryItemBtn;

    [SerializeField]
    GameObject inventoryItemListObj;

    [SerializeField]
    ProductImageMgr productImageMgr;

    RectTransform inventoryItemListRc;
    GridLayoutGroup inventoryItemListGridLayout;

    readonly List<InventoryItemBtn> inventoryItemBtnList = new List<InventoryItemBtn>();

    public IReadOnlyList<InventoryItemBtn> InventoryItemBtnList => inventoryItemBtnList;

    void Awake()
    {
        inventoryItemListRc = inventoryItemListObj.GetComponent<RectTransform>();
        inventoryItemListGridLayout = inventoryItemListObj.GetComponent<GridLayoutGroup>();

        if (inventory == null)
            inventory = FindFirstObjectByType<Inventory>();

        if (inventory != null)
        {
            inventory.ItemAdded += OnInventoryItemAdded;
            inventory.ItemRemoved += OnInventoryItemRemoved;
        }

        SetInventoryItemListRc();
    }

    void OnDestroy()
    {
        if (inventory == null)
            return;

        inventory.ItemAdded -= OnInventoryItemAdded;
        inventory.ItemRemoved -= OnInventoryItemRemoved;
    }

    void OnInventoryItemAdded(ProductDataLow itemData)
    {
        AddInventoryItemView(itemData);
    }

    void AddInventoryItemView(ProductDataLow productDataLow)
    {
        if (productDataLow == null || inventoryItemBtn == null || inventoryItemListObj == null)
            return;

        var itemBtn = Instantiate(inventoryItemBtn, inventoryItemListObj.transform);
        var itemBtnComp = itemBtn.GetComponent<InventoryItemBtn>();

        itemBtnComp.SetProductDataLow(productDataLow);

        if (productImageMgr != null)
            itemBtnComp.SetProductImage(productImageMgr.GetProductImage(productDataLow.id));

        inventoryItemBtnList.Add(itemBtnComp);
        inventory?.RegisterUseCallback(itemBtnComp);
        SetInventoryItemListRc();
    }

    void OnInventoryItemRemoved(InventoryItemBtn itemBtn)
    {
        if (itemBtn == null)
            return;

        inventoryItemBtnList.Remove(itemBtn);
        Destroy(itemBtn.gameObject);
        SetInventoryItemListRc();
    }

    public void SetInventoryItemListRc()
    {
        if (inventoryItemListRc == null || inventoryItemListGridLayout == null || inventoryItemListObj == null)
            return;

        int childCount = inventoryItemListObj.transform.childCount;
        if (childCount == 0)
        {
            inventoryItemListRc.sizeDelta = Vector2.zero;
            return;
        }

        Vector2 cellSize = inventoryItemListGridLayout.cellSize;
        Vector2 spacing = inventoryItemListGridLayout.spacing;
        RectOffset padding = inventoryItemListGridLayout.padding;

        int columnCount = GetColumnCount(childCount);
        int rowCount = Mathf.CeilToInt((float)childCount / columnCount);

        float width = padding.horizontal + columnCount * cellSize.x + Mathf.Max(0, columnCount - 1) * spacing.x;
        float height = padding.vertical + rowCount * cellSize.y + Mathf.Max(0, rowCount - 1) * spacing.y;

        bool stretchWidth = !Mathf.Approximately(inventoryItemListRc.anchorMin.x, inventoryItemListRc.anchorMax.x);
        inventoryItemListRc.sizeDelta = stretchWidth
            ? new Vector2(inventoryItemListRc.sizeDelta.x, height)
            : new Vector2(width, height);
    }

    int GetColumnCount(int childCount)
    {
        switch (inventoryItemListGridLayout.constraint)
        {
            case GridLayoutGroup.Constraint.FixedColumnCount:
                return Mathf.Max(1, inventoryItemListGridLayout.constraintCount);
            case GridLayoutGroup.Constraint.FixedRowCount:
                return Mathf.CeilToInt((float)childCount / Mathf.Max(1, inventoryItemListGridLayout.constraintCount));
            default:
                return 1;
        }
    }
}

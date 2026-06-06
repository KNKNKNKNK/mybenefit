using System.Collections.Generic;
using UnityEngine;

using MachineData;
using ItemData;

public class DataParsingMgr : MonoBehaviour
{
    [SerializeField]
    MachineState machineState;

    [SerializeField]
    MachineUiMgr machineUiMgr;

    [SerializeField]
    ProductImageMgr productImageMgr;

    [SerializeField]
    Inventory inventory;

    const string ItemsResourcePath = "Items";

    public string MachineId { get; private set; }
    public MachineStatus Status { get; private set; }
    public Dictionary<int, ProductData> Products { get; private set; }

    void Awake()
    {
        ParseItems();

        productImageMgr?.LoadProductImages(Products);
        machineState.SetMachineState(MachineId, Status, Products);
        machineUiMgr?.SetProductListBtn(Products, productImageMgr.ProductImages, inventory);
    }

    public void ParseItems()
    {
        var textAsset = Resources.Load<TextAsset>(ItemsResourcePath);
        if (textAsset == null)
        {
            Debug.LogError($"[DataParsingMgr] Resources/{ItemsResourcePath}.json not found.");
            return;
        }

        var root = JsonUtility.FromJson<ItemsJsonRoot>(textAsset.text);
        if (root == null)
        {
            Debug.LogError("[DataParsingMgr] Failed to parse Items.json.");
            return;
        }

        MachineId = root.machineId;
        Status = MachineStatusParser.Parse(root.status);
        Products = BuildProductDictionary(root.products);
    }

    static Dictionary<int, ProductData> BuildProductDictionary(ProductData[] products)
    {
        var dictionary = new Dictionary<int, ProductData>();
        if (products == null)
            return dictionary;

        foreach (var product in products)
        {
            if (product == null)
                continue;

            product.SyncProductDataLow();
            dictionary[product.id] = product;
        }

        return dictionary;
    }
}


using System;
using UnityEngine;
using ItemData;
using TMPro;
using UnityEngine.UI;

public class InventoryItemBtn : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI productNameText;

    [SerializeField]
    Image productImage;

    ProductDataLow productDataLow;
    Func<InventoryItemBtn, bool> onUseItem;

    public ProductDataLow ProductDataLow => productDataLow;

    public void SetUseCallback(Func<InventoryItemBtn, bool> callback)
    {
        onUseItem = callback;
    }

    public void SetProductDataLow(ProductDataLow productDataLow_)
    {
        productDataLow = productDataLow_;

        if (productDataLow == null)
            return;

        productNameText.text = productDataLow.name;
    }

    public void SetProductImage(Sprite sprite)
    {
        productImage.sprite = sprite;
    }

    public void UseItme()
    {
        if (productDataLow == null)
            return;

        onUseItem?.Invoke(this);
    }
}

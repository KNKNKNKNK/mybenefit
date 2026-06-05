using System;
using UnityEngine;
using ItemData;
using TMPro;
using UnityEngine.UI;

public class ProductBtn : MonoBehaviour
{    [SerializeField]
    TextMeshProUGUI productNameText;
    [SerializeField]
    Image productImage;
    [SerializeField]
    TextMeshProUGUI productPriceText;
    [SerializeField]
    TextMeshProUGUI productStockText;

    ProductData productData;
    Func<ProductData, bool> onBuyProduct;

    public void SetBuyCallback(Func<ProductData, bool> callback)
    {
        onBuyProduct = callback;
    }

    public void SetProductData(ProductData productData_, Sprite productImage_)    {
        productData = productData_;

        productNameText.text = productData.name;
        productPriceText.text = productData.price.ToString();
        productStockText.text = productData.stock.ToString();
        productImage.sprite = productImage_;
    }

    public void BuyProduct()
    {
        if (onBuyProduct != null && onBuyProduct(productData))
            UpdateStockText();
    }

    void UpdateStockText()
    {
        productStockText.text = productData.stock.ToString();
    }
}
using System.Collections.Generic;
using UnityEngine;
using ItemData;

public class ProductImageMgr : MonoBehaviour
{
    readonly Dictionary<int, Sprite> productImages = new Dictionary<int, Sprite>();

    public IReadOnlyDictionary<int, Sprite> ProductImages => productImages;

    public void LoadProductImages(Dictionary<int, ProductData> products)
    {
        productImages.Clear();

        if (products == null)
            return;

        foreach (var product in products.Values)
        {
            if (product == null || string.IsNullOrEmpty(product.imageUrl))
                continue;

            var sprite = Resources.Load<Sprite>(product.imageUrl);
            if (sprite == null)
            {
                Debug.LogWarning($"[ProductImageMgr] Sprite not found at Resources/{product.imageUrl} (id: {product.id})");
                continue;
            }

            productImages[product.id] = sprite;
        }
    }

    public Sprite GetProductImage(int productId)
    {
        productImages.TryGetValue(productId, out var sprite);
        return sprite;
    }
}

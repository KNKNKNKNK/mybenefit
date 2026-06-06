namespace ItemData
{
    [System.Serializable]
    public class ItemsJsonRoot
    {
        public string machineId;
        public string status;
        public string updatedAt;
        public ProductData[] products;
    }

    [System.Serializable]
    public class ProductDataLow
    {
        public int id;
        public string name;
        public string type;
        public string imageUrl;
    }

    [System.Serializable]
    public class ProductData : ProductDataLow
    {
        public ProductDataLow productDataLow;

        public int price;
        public int stock;

        public void SyncProductDataLow()
        {
            productDataLow = new ProductDataLow
            {
                id = id,
                name = name,
                type = type,
                imageUrl = imageUrl
            };
        }

        public ProductDataLow ToProductDataLow()
        {
            if (productDataLow != null)
                return productDataLow;

            return new ProductDataLow
            {
                id = id,
                name = name,
                type = type,
                imageUrl = imageUrl
            };
        }
    }
}

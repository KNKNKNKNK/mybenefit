using UnityEngine;
using TMPro;
using ItemData;

public class Inventory : MonoBehaviour
{    const int MaxMoney = 10000;

    const int Money100 = 100;
    const int Money1000 = 1000;

    [SerializeField]
    TextMeshProUGUI moneyText;
    int money;

    public int Money => money;

    public void RegisterBuyCallback(ProductBtn productBtn)
    {
        productBtn?.SetBuyCallback(TryBuyProduct);
    }

    bool TryBuyProduct(ProductData productData)
    {
        if (productData == null || productData.stock <= 0)
            return false;

        if (money < productData.price)
            return false;

        money -= productData.price;
        productData.stock--;
        
        UpdateMoneyText();
        return true;
    }

    public void AddMoney100()    {
        if (money + Money100 > MaxMoney)
            return;

        money += Money100;
        UpdateMoneyText();
    }

    public void AddMoney1000()
    {
        if (money + Money1000 > MaxMoney)
            return;

        money += Money1000;
        UpdateMoneyText();
    }

    void UpdateMoneyText()
    {
        string formattedMoney = money >= 1000
            ? money.ToString("#,##0")
            : money.ToString();

        moneyText.text = "money : " + formattedMoney + " won";
    }
}

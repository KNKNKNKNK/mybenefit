using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ItemData;
using LogData;

public class Inventory : MonoBehaviour
{
    const int MaxMoney = 10000;
    const int Money100 = 100;
    const int Money1000 = 1000;

    [SerializeField]
    TextMeshProUGUI moneyText;

    [SerializeField]
    MachineState machineState;

    int money;
    readonly List<ProductDataLow> inventoryItemDataLowList = new List<ProductDataLow>();

    public int Money => money;
    public IReadOnlyList<ProductDataLow> InventoryItemDataLowList => inventoryItemDataLowList;

    public event Action<ProductDataLow> ItemAdded;
    public event Action<InventoryItemBtn> ItemRemoved;

    void Awake()
    {
        if (machineState == null)
            machineState = FindFirstObjectByType<MachineState>();
    }

    bool IsMachineInactive()
    {
        if (machineState == null)
            machineState = FindFirstObjectByType<MachineState>();

        return machineState == null || !machineState.IsActive;
    }

    bool BlockIfMachineInactive(LogEventType blockedEventType, string detail = "")
    {
        if (!IsMachineInactive())
            return false;

        LogMgr.Instance?.AddInactiveBlockedLog(blockedEventType, detail);
        return true;
    }

    bool BlockIfMachineInactive(LogEventType blockedEventType, int amount)
    {
        if (!IsMachineInactive())
            return false;

        LogMgr.Instance?.AddInactiveBlockedLog(blockedEventType, amount);
        return true;
    }

    public void RegisterBuyCallback(ProductBtn productBtn)
    {
        productBtn?.SetBuyCallback(TryBuyProduct);
    }

    public bool TryBuyProduct(ProductData productData)
    {
        if (productData == null)
            return false;

        if (BlockIfMachineInactive(LogEventType.ProductPurchased, productData.name))
            return false;

        if (productData.stock <= 0)
        {
            LogMgr.Instance?.AddLog(LogEventType.ProductPurchaseFailedOutOfStock, productData.name);
            return false;
        }

        if (money < productData.price)
        {
            LogMgr.Instance?.AddLog(LogEventType.ProductPurchaseFailedInsufficientFunds, productData.name);
            return false;
        }

        money -= productData.price;
        productData.stock--;
        productData.SyncProductDataLow();

        UpdateMoneyText();
        AddItem(productData.ToProductDataLow());
        LogMgr.Instance?.AddLog(LogEventType.ProductPurchased, productData.name);
        return true;
    }

    public void AddItem(ProductDataLow itemData)
    {
        if (itemData == null)
            return;

        inventoryItemDataLowList.Add(itemData);
        ItemAdded?.Invoke(itemData);
    }

    public void RegisterUseCallback(InventoryItemBtn inventoryItemBtn)
    {
        inventoryItemBtn?.SetUseCallback(TryUseItem);
    }

    public bool TryUseItem(InventoryItemBtn itemBtn)
    {
        if (itemBtn == null)
            return false;

        var itemData = itemBtn.ProductDataLow;
        if (BlockIfMachineInactive(LogEventType.ProductConsumed, itemData?.name ?? string.Empty))
            return false;

        if (itemData == null)
            return false;

        if (!inventoryItemDataLowList.Remove(itemData))
            return false;

        ItemRemoved?.Invoke(itemBtn);
        LogMgr.Instance?.AddLog(LogEventType.ProductConsumed, itemData.name);
        return true;
    }

    public void AddMoney100()
    {
        if (BlockIfMachineInactive(LogEventType.CurrencyEarned, Money100))
            return;

        if (money + Money100 > MaxMoney)
            return;

        money += Money100;
        UpdateMoneyText();
        LogMgr.Instance?.AddLog(LogEventType.CurrencyEarned, Money100);
    }

    public void AddMoney1000()
    {
        if (BlockIfMachineInactive(LogEventType.CurrencyEarned, Money1000))
            return;

        if (money + Money1000 > MaxMoney)
            return;

        money += Money1000;
        UpdateMoneyText();
        LogMgr.Instance?.AddLog(LogEventType.CurrencyEarned, Money1000);
    }

    void UpdateMoneyText()
    {
        string formattedMoney = money >= 1000
            ? money.ToString("#,##0")
            : money.ToString();

        moneyText.text = "money : " + formattedMoney + " won";
    }
}

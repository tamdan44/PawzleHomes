using UnityEngine;
using System;
using UnityEngine.Purchasing;

[Serializable]
public class ConsumableItem
{
    public string name;
    public string id;
    public string desc;
    public float price;
    public ConsumableItem(string name, string id, string desc, float price)
    {
        this.name = name;
        this.id = id;
        this.desc = desc;
        this.price = price;
    }
}

[Serializable]
public class NonConsumableItem
{
    public string name;
    public string id;
    public string desc;
    public float price;
    public NonConsumableItem(string name, string id, string desc, float price)
    {
        this.name = name;
        this.id = id;
        this.desc = desc;
        this.price = price;
    }
}

public class IAP : MonoBehaviour, IStoreListener
{
    [SerializeField] private MoneyBar moneyBar;

    [HideInInspector]
    public ConsumableItem citem1;
    public ConsumableItem citem2;
    public NonConsumableItem nitem;

    IStoreController m_storeController;

    void Start()
    {
        SetupBuilder();
        SetShopItems();
    }

    void SetShopItems()
    {
        citem1 = new ConsumableItem("99 Diamonds", "dia_99", "Get 99 diamonds!", 39);
        citem2 = new ConsumableItem("399 Diamonds", "dia_399", "Get 399 diamonds!", 139);
        nitem = new NonConsumableItem("Ad Block", "ad_block", "Get rid of pop-up ads. Only get ads if you choose to.", 29);
    }

    void SetupBuilder()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.AddProduct(citem1.id, ProductType.Consumable);
        builder.AddProduct(nitem.id, ProductType.NonConsumable);

        UnityPurchasing.Initialize(this, builder);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        // retrieve the purchased product
        var product = purchaseEvent.purchasedProduct;

        Debug.Log("purchase successful");

        if (product.definition.id == citem1.id)
        {
            AddBigCoins(99);
        }
        else if (product.definition.id == citem2.id)
        {
            AddBigCoins(399);
        }
        else if (product.definition.id == nitem.id)
        {
            //remove add
        }

        return PurchaseProcessingResult.Complete;
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        CheckNonConsumable(nitem.id);
        m_storeController = controller;
    }


    public void RemoveAdsBtnClicked()
    {
        m_storeController.InitiatePurchase(nitem.id);
        // GameData.AdBlock = true;
    }

    public void Buy20BtnClicked()
    {
        AddBigCoins(20);
    }

    public void Buy99BtnClicked()
    {
        // AddBigCoins(99);
        m_storeController.InitiatePurchase(citem1.id);
    }

    public void Buy399BtnClicked()
    {
        // AddBigCoins(399);
        m_storeController.InitiatePurchase(citem2.id);
    }
    void AddBigCoins(int bigcoins)
    {
        GameData.playerBigCoins += bigcoins;
        moneyBar.UpdateCoinNum();
        SaveSystem.SavePlayer();
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        print("purchase fail" + error);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        print("purchase fail" + error + message);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        print("purchase fail");
    }

    void CheckNonConsumable(string id)
    {
        if (m_storeController != null)
        {
            var product = m_storeController.products.WithID(id);
            if (product != null)
            {
                if (product.hasReceipt)
                {
                    //remove ads
                    GameData.AdBlock = true;
                }
                else
                {
                    //show ads
                    GameData.AdBlock = false;
                }
            }
        }
    }
}

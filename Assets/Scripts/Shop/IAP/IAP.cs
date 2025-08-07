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
    public ConsumableItem citem3;
    public NonConsumableItem nitem;

    IStoreController m_storeController;

    void Start()
    {
        SetupBuilder();
        SetShopItems();
    }

    void SetShopItems()
    {
        citem1 = new ConsumableItem("200 Diamonds", "dia_200", "Get 200 diamonds!", 39);
        citem2 = new ConsumableItem("800 Diamonds", "dia_800", "Get 800 diamonds!", 129);
        citem2 = new ConsumableItem("5000 Coins", "coin_5000", "Get 5000 coins!", 29);
        nitem = new NonConsumableItem("Ad Block", "ad_block", "Get rid of pop-up ads. Only get ads if you choose to.", 19);
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
            GameEvents.AddBigCoins(200);
        }
        else if (product.definition.id == citem2.id)
        {
            GameEvents.AddBigCoins(800);
        }
        else if (product.definition.id == citem3.id)
        {
            GameEvents.AddCoins(5000);
        }
        else if (product.definition.id == nitem.id)
        {
            //remove add
            CheckNonConsumable(nitem.id);  
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

    public void BuyBtn1Clicked()
    {
        // AddBigCoins(99);
        m_storeController.InitiatePurchase(citem1.id);
    }

    public void BuyBtn2Clicked()
    {
        // AddBigCoins(399);
        m_storeController.InitiatePurchase(citem2.id);
    }

    public void BuyBtn3Clicked()
    {
        m_storeController.InitiatePurchase(citem3.id);
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

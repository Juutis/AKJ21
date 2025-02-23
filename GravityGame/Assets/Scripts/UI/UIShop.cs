using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class UIShop : MonoBehaviour
{
    [SerializeField]
    private UIShopItem uiShopItemPrefab;

    [SerializeField]
    private Transform elementContainer;

    private List<UIShopItem> shopItems = new();
    [SerializeField]
    private Transform container;

    [SerializeField]
    private Animator animator;

    public bool IsShown = false;

    [SerializeField]
    private Image imgCursor;

    [SerializeField]
    private UIShopItemDetails uiShopItemDetailsPrefab;

    [SerializeField]
    private Transform shopItemDetailsContainer;

    private ShopItem selectedShopItem;

    [SerializeField]
    private UIButton buyButton;

    private List<List<ShopItem>> itemGroups = new();

    [SerializeField]
    private TextMeshProUGUI messageDisplay;

    private UIShopItemDetails shopItemDetails;

    [SerializeField]
    private Transform containerPrefab;

    private bool canClose = false;
    private bool canShow = true;

    void Start()
    {
        if (!IsShown) {
            container.gameObject.SetActive(false);
            imgCursor.enabled = false;
        }
    }

    public bool CanClose() {
        return canClose;
    }
    public bool CanShow() {
        return canShow;
    }

    public void Show()
    {
        if (IsShown) {
            return;
        }
        canShow = false;
        canClose = false;
        if (shopItemDetails != null)
        {
            Destroy(shopItemDetails.gameObject);
        }
        string message = Shop.main.TransferShipResourcesToBase();
        ShopMessage(message);
        //animator.Play("shopShow");
        imgCursor.enabled = true;
        buyButton.gameObject.SetActive(false);
        container.gameObject.SetActive(true);
        //Debug.Log("Showing");
    }

    public void FinishShowing() {
        canClose = true;
        IsShown = true;
        //Debug.Log("finished showing");
    }
    public void FinishHiding() {
        IsShown = false;
        canShow = true;
        //Debug.Log("finished hiding");
    }

    public void ShopMessage(string message) {
        messageDisplay.text = $"{message}";
    }

     public void Hide() {
        //animator.Play("shopHide");
        canShow = false;
        canClose = false;
        imgCursor.enabled = false;
        container.gameObject.SetActive(false);
        //Debug.Log("hiding");
    }

    public void Initialize(List<ShopItem> items)
    {
        buyButton.gameObject.SetActive(false);
        itemGroups = items.GroupBy(i => i.UpgradeType).Select(g => g.ToList()).ToList();

        foreach (var itemGroup in itemGroups)
        {
            var item = itemGroup.OrderBy(i => i.UpgradeTier).First();
            var itemContainer = Instantiate(containerPrefab, elementContainer);
            var shopItem = Instantiate(uiShopItemPrefab, itemContainer);
            shopItem.Initialize(item);
            shopItems.Add(shopItem);
        }
    }
    public void MarkShopItemAsBought(ShopItem shopItem) {
        var item = shopItems.FirstOrDefault(i => i.ShopItem == shopItem);
        if (item != null) {
            item.MarkAsBought();
            var child = itemGroups.FirstOrDefault(g => g.Contains(shopItem)).FirstOrDefault(i => i.UpgradeTier == shopItem.UpgradeTier + 1);
            if (child != null) {
                var uiShopItem = Instantiate(uiShopItemPrefab, item.transform.parent);
                uiShopItem.Initialize(child);
                shopItems.Add(uiShopItem);
            }
        }
    }

    public void BuySelectedItem() {
        if (selectedShopItem == null) {
            return;
        }
        if (Shop.main.Buy(selectedShopItem)) {
            ShopMessage($"Bought \"{selectedShopItem.Name}\"!");
            shopItemDetails.Bought();
            buyButton.gameObject.SetActive(false);
        } else {
            ShopMessage($"Can't afford \"{selectedShopItem.Name}\" ");
        }
    }

    public void ShowItem(ShopItem shopItem) {
        buyButton.gameObject.SetActive(true);
        if (shopItemDetails != null) {
            Destroy(shopItemDetails.gameObject);
        }
        if(Shop.main.CanBuy(shopItem)) {
            buyButton.CanBuy(true);
        } else {
            buyButton.CanBuy(false);
        }
        shopItemDetails = Instantiate(uiShopItemDetailsPrefab, shopItemDetailsContainer);
        shopItemDetails.Initialize(shopItem);
        selectedShopItem = shopItem;
    }

    void Update()
    {
        if (!IsShown) {
            return;
        }
        imgCursor.transform.position = Input.mousePosition;
    }
}

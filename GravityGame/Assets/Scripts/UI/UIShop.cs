using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

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

    private UIShopItemDetails shopItemDetails;

    [SerializeField]
    private Transform containerPrefab;

    void Start()
    {
        if (!IsShown) {
            container.gameObject.SetActive(false);
            imgCursor.enabled = false;
        }
    }

    public void Show()
    {
        if (shopItemDetails != null)
        {
            Destroy(shopItemDetails.gameObject);
        }
        IsShown = true;
        Shop.main.TransferShipResourcesToBase();
        animator.Play("shopShow");
        imgCursor.enabled = true;
        buyButton.gameObject.SetActive(false);
        container.gameObject.SetActive(true);
    }

    public void ShowFinished() {
        elementContainer.gameObject.SetActive(true);
    }

    public void Hide() {
        animator.Play("shopHide");
        imgCursor.enabled = false;
        container.gameObject.SetActive(false);
        IsShown = false;
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
            UIManager.main.ShowMessage($"Bought {selectedShopItem.Name}!");
            shopItemDetails.Bought();
            buyButton.gameObject.SetActive(false);
        } else {
            UIManager.main.ShowMessage($"Can't afford it!");
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

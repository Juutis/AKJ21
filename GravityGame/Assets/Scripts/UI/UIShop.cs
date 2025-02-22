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

    void Start()
    {
        if (!IsShown) {
            container.gameObject.SetActive(false);
            imgCursor.enabled = false;
        }
    }

    public void Show()
    {
        IsShown = true;
        animator.Play("shopShow");
        imgCursor.enabled = true;
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
        foreach (var item in items)
        {
            var shopItem = Instantiate(uiShopItemPrefab, elementContainer);
            shopItem.Initialize(item);
            shopItems.Add(shopItem);
        }
    }
    public void MarkShopItemAsBought(ShopItem shopItem) {
        var item = shopItems.FirstOrDefault(i => i.ShopItem == shopItem);
        if (item != null) {
            item.MarkAsBought();
        }
    }

    public void BuySelectedItem() {
        if (selectedShopItem == null) {
            return;
        }
        if (Shop.main.Buy(selectedShopItem)) {
            Debug.Log("Wow!");
        }
    }

    public void ShowItem(ShopItem shopItem) {
        buyButton.gameObject.SetActive(true);
        foreach (Transform child in shopItemDetailsContainer) {
            Destroy(child.gameObject);
        }
        UIShopItemDetails shopItemDetails = Instantiate(uiShopItemDetailsPrefab, shopItemDetailsContainer);
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

using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class UIShopItem : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private ShopItem shopItem;
    public ShopItem ShopItem { get { return shopItem; } }

    [SerializeField]
    private Image imgIcon;
    [SerializeField]
    private TextMeshProUGUI txtName;

    private bool isBought = false;
    public void Initialize(ShopItem newShopItem)
    {
        shopItem = newShopItem;
        imgIcon.sprite = shopItem.Icon;
        txtName.text = shopItem.Name;
    }

    public void MarkAsBought() {
        imgIcon.color = Color.red;
        isBought = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
        UIManager.main.ShowShopItem(shopItem);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
    }
}

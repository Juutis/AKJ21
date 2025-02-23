using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class UIShopItem : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private ShopItem shopItem;
    public ShopItem ShopItem { get { return shopItem; } }

    [SerializeField]
    private Image imgIcon;
    [SerializeField]
    private Outline imgBorders;
    [SerializeField]
    private TextMeshProUGUI txtName;
    [SerializeField]
    private Color HighlightColor;

    [SerializeField]
    private Color boughtColor;
    private Color normalColor;


    private bool isBought = false;
    public void Initialize(ShopItem newShopItem)
    {
        normalColor = imgBorders.effectColor;
        shopItem = newShopItem;
        imgIcon.sprite = shopItem.Icon;
        txtName.text = shopItem.Name;
    }

    public void MarkAsBought() {
        imgIcon.color = boughtColor;
        imgBorders.effectColor = Color.black;
        isBought = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isBought)
        {
            return;
        }
        imgBorders.effectColor = HighlightColor;
        //throw new System.NotImplementedException();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
        if (isBought) {
            return;
        }
        UIManager.main.ShowShopItem(shopItem);
        imgBorders.effectColor = normalColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isBought)
        {
            return;
        }
        imgBorders.effectColor = normalColor;
        //throw new System.NotImplementedException();
    }
}

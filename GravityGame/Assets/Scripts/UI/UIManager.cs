using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIManager: MonoBehaviour {
    public static UIManager main;

    [SerializeField]
    private UIInventory uiBaseInventory;
    [SerializeField]
    private UIInventory uiShipInventory;
    [SerializeField]
    private UIShop uiShop;

    [SerializeField]
    private UICurtainTransition uiCurtainTransition;
    [SerializeField]
    private GameObject shopIndicator;
    [SerializeField]
    private GameObject storageFullIndicator;

    [SerializeField]
    private UIMessage uiMessagePrefab;

    [SerializeField]
    private Transform uiMessageContainer;

    private bool storageFullMessage = false;

    public bool InvertY = false;

    void Awake()
    {
        main = this;
    } 

    void Start() {
        Cursor.visible = false;
    }

    void Update()
    {
        ProcessMessageBuffer();
        if (uiShop.IsShown && Input.GetKeyDown(KeyCode.B))
        {
            HideShop();
        }
        else if (Shop.main.IsInRangeOfShop()) {
            shopIndicator.SetActive(true);
            if (Input.GetKeyDown(KeyCode.B))
            {
                if (!uiShop.IsShown) {
                    ShowShop();
                }
            }
        } else {
            shopIndicator.SetActive(false);
        }

    }

    public void ShowCurtains(UnityAction showCallback) {
        uiCurtainTransition.Show(delegate
        {
            showCallback?.Invoke();
        }
        );
    }

    public void HideCurtains(UnityAction hideCallback)
    {
        uiCurtainTransition.Hide(delegate
        {
            hideCallback?.Invoke();
        }
        );
    }
    public void CurtainTransition(UnityAction showCallback, UnityAction hideCallback) {
        uiCurtainTransition.Show(delegate {
            showCallback?.Invoke();
            uiCurtainTransition.Hide(delegate{
                hideCallback?.Invoke();
            });
        });
    }

    public void ShowShop() {
        if (uiShop.IsShown) {
            return;
        }
        CurtainTransition(
            delegate{
                Time.timeScale = 0f;
                uiShop.Show();
            },
            delegate{
                Cursor.lockState = CursorLockMode.None;
            }
        );
    }
    public void HideShop()
    {
        CurtainTransition(
            delegate
            {
                uiShop.Hide();
            },
            delegate
            {
                Time.timeScale = 1f;
                Cursor.lockState = CursorLockMode.Locked;
            }
        );
    }


    public void InitializeBaseInventory(Inventory inventory) {
        uiBaseInventory.Initialize(inventory, false);
    }

    public void InitializeShipInventory(Inventory inventory) {
        uiShipInventory.Initialize(inventory, true);
    }

    public void InitializeShop(List<ShopItem> items) {
        uiShop.Initialize(items);
    }


    public void HideStorageIsFull() {
        storageFullIndicator.SetActive(false);
    }
    public void ShowStorageIsFull() {
        storageFullIndicator.SetActive(true);
    }


    private float messageInterval = 0.5f;
    private float mostRecentMessageTime;
    private List<string> messages = new();
    public void ShowMessage(string message)
    {
        messages.Add(message);
    }

    private void ProcessMessageBuffer()
    {
        if (messages.Count == 0)
        {
            return;
        }
        if (messageInterval > (Time.unscaledTime - mostRecentMessageTime))
        {
            return;
        }
        mostRecentMessageTime = Time.unscaledTime;
        var message = messages[0];
        messages.RemoveAt(0);
        DisplayMessage(message);
    }


    private void DisplayMessage(string message)
    {
        UIMessage uiMessage = Instantiate(uiMessagePrefab, uiMessageContainer);
        uiMessage.Initialize(message);
    }

    public void AddResourceToBaseInventory(InventoryResource resource) {
        uiBaseInventory.AddResource(resource);
    }

    public void AddResourceToShipInventory(InventoryResource resource)
    {
        uiShipInventory.AddResource(resource);
    }

    public void UpdateBaseResourceView(InventoryResource resource) {
        uiBaseInventory.UpdateResourceView(resource);
    }
    public void UpdateInventoryResourceView(InventoryResource resource) {
        uiShipInventory.UpdateResourceView(resource);
    }

    public void ShowShopItem(ShopItem shopItem) {
        uiShop.ShowItem(shopItem);
    }

    public void MarkShopItemAsBought(ShopItem shopItem) {
        uiShop.MarkShopItemAsBought(shopItem);
    }

}
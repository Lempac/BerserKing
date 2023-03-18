using UnityEngine;

public class MenuHandeler : MonoBehaviour
{
    public GameObject ChestMenu;
    public GameObject ItemMenu;
    public delegate void ShowItem(Item item);
    public delegate void ShowChest();
    public delegate void TakeItem(Item item);
    public delegate void TakeChest();

    public static event ShowItem OnShowItem;
    public static event ShowChest OnShowChest;
    public static event TakeItem OnTakeItem;
    public static event TakeChest OnTakeChest;

    public static MenuHandeler Instance { get; private set; }
    public void Awake()
    {
        if (Instance != null) Debug.LogError("Only one Menu Handeler!");
        else Instance = this;
    }
    public void ShowItemMenu(Item item)
    {
        OnShowItem?.Invoke(item);
        GameObject menu = Instantiate(ItemMenu);
        menu.transform.SetParent(transform, false);
        menu.GetComponent<ItemMenu>().ItemName.text = item.ItemName;
        menu.GetComponent<ItemMenu>().ItemDescription.text = item.ItemDescription;
        menu.GetComponent<ItemMenu>().ItemIcon.sprite = item.ItemSprite;
        menu.GetComponent<ItemMenu>().Take.onClick.AddListener(() =>
        {
            OnTakeItem?.Invoke(item);
            Destroy(menu);
        });
    }
    public void ShowChestMenu()
    {
        OnShowChest?.Invoke();
        GameObject menu = Instantiate(ChestMenu);
        menu.transform.SetParent(transform, false);
        menu.GetComponent<ChestMenu>().Take.onClick.AddListener(() =>
        {
            OnTakeChest?.Invoke();
            Destroy(menu);
        });
    }
}

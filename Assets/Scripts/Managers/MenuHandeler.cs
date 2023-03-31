using UnityEngine;

public class MenuHandeler : MonoBehaviour
{
    [SerializeField] private GameObject ChestMenu;
    [SerializeField] private GameObject ItemMenu;
    [SerializeField] private GameObject ItemSlot;
 
    public delegate void ShowItem(Item item);
    public delegate void ShowChest();
    public delegate void ShowNewItemSlot(Attribute attribute);
    public delegate void TakeItem(Item item);
    public delegate void TakeChest();
    

    public static event ShowItem OnShowItem;
    public static event ShowChest OnShowChest;
    public static event ShowNewItemSlot OnShowNewItemSlot;
    public static event TakeItem OnTakeItem;
    public static event TakeChest OnTakeChest;
    

    public static MenuHandeler Instance { get; private set; }
    public void Awake()
    {
        if (Instance != null) Debug.LogError("Only one Menu Handeler!");
        else Instance = this;
    }

    private void Pause()
    {
        Time.timeScale = 0f;
    }
    private void Resume()
    {
        Time.timeScale = 1f;
    }

    public void Start()
    {
        OnShowItem += (Item _) => { Pause(); };
        OnShowChest += () => { Pause(); };
        OnTakeItem += (Item _) => { Resume(); };
        OnTakeChest += () => { Resume(); };
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
    public void ShowItemSlot(Attribute attribute)
    {
        OnShowNewItemSlot?.Invoke(attribute);
        GameObject slot = Instantiate(ItemSlot);
        ItemSlot slotdata = slot.GetComponent<ItemSlot>();
        slotdata.ItemIcon.sprite = attribute.ItemSprite;
        slotdata.ItemLevel.text = "0";
        slot.transform.SetParent(transform, false);
        slot.transform.position = new Vector3((Screen.width/2)/ItemManager.Instance.Items.Length, transform.position.x / (Screen.width / 4)*3);
    }
}

using UnityEngine;

public class MenuHandler : MonoBehaviour
{
    [SerializeField] private GameObject ChestMenu;
    [SerializeField] private GameObject ItemMenu;
    [SerializeField] private GameObject ItemSlot;
    [SerializeField] private GameObject PauseMenu;
 
    public delegate void ShowItem(Item item);
    public delegate void ShowChest();
    public delegate void ShowPause();
    public delegate void ClosePause();
    public delegate void NewItemSlot(Attribute attribute);

    public delegate void TakeItem(Item item);
    public delegate void TakeChest();


    public static event ShowPause OnShowPauseMenu;
    public static event ClosePause OnClosePauseMenu;
    public static event ShowItem OnShowItem;
    public static event ShowChest OnShowChest;
    public static event NewItemSlot OnNewItemSlot;
    public static event TakeItem OnTakeItem;
    public static event TakeChest OnTakeChest;
    

    public static MenuHandler Instance { get; private set; }
    public void Awake()
    {
        if (Instance != null) Debug.LogError("Only one Menu Handler!");
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
        OnClosePauseMenu += () => { Pause(); };
        OnShowPauseMenu += () => { Resume(); };
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {

        }
    }
    
    public void ShowPauseMenu()
    {

    }

    public void ClosePauseMenu()
    {

    }

    public void ShowItemMenu(Item item)
    {
        
        OnShowItem?.Invoke(item);
        GameObject menu = Instantiate(ItemMenu);
        ItemMenu itemDataLayout = menu.GetComponent<ItemMenu>();
        itemDataLayout.ItemFound.text = $"Found {item.GetType()}!";
        itemDataLayout.ItemName.text = item.ItemName;
        itemDataLayout.ItemDescription.text = item.ItemDescription;
        itemDataLayout.ItemIcon.sprite = item.ItemSprite;
        itemDataLayout.Take.onClick.AddListener(() =>
        {
            OnTakeItem?.Invoke(item);
            Destroy(menu);
        });
        menu.transform.SetParent(transform, false);
    }

    public void ShowChestMenu()
    {
        OnShowChest?.Invoke();
        GameObject menu = Instantiate(ChestMenu);
        ChestMenu chestMenuData = menu.GetComponent<ChestMenu>();
        menu.transform.SetParent(transform, false);
        chestMenuData.Take.onClick.AddListener(() =>
        {
            OnTakeChest?.Invoke();
            Destroy(menu);
        });
    }
    public void NowItemSlot(Attribute attribute)
    {
        OnNewItemSlot?.Invoke(attribute);
        GameObject slot = Instantiate(ItemSlot);
        ItemSlot slotdata = slot.GetComponent<ItemSlot>();
        slotdata.ItemIcon.sprite = attribute.ItemSprite;
        slotdata.ItemLevel.text = "0";
        slot.transform.SetParent(transform, false);
        slot.transform.position = new Vector3((Screen.width/2)/Item.Items.Count, transform.position.x / (Screen.width / 4)*3);
    }
}

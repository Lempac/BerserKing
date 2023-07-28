using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class MenuHandler : MonoBehaviour
{
    [SerializeField] private GameObject PickupMenu;
    [SerializeField] private GameObject ItemMenu;
    [SerializeField] private GameObject ItemSlot;
    [SerializeField] private GameObject PauseMenu;

    //public delegate void ShowItem(Item item);
    //public delegate void ShowChest();
    //public delegate void ShowPause();
    //public delegate void ClosePause();
    //public delegate void NewItemSlot(Attribute attribute);
    //public delegate void TakeItem(Item item);
    //public delegate void TakeChest();


    //public static event ShowPause OnShowPauseMenu;
    //public static event ClosePause OnClosePauseMenu;
    //public static event ShowItem OnShowItem;
    //public static event ShowChest OnShowChest;
    //public static event NewItemSlot OnNewItemSlot;
    //public static event TakeItem OnTakeItem;
    //public static event TakeChest OnTakeChest;

    //public UnityEvent OnShowPauseMenu;
    //public UnityEvent OnClosePauseMenu;
    //public UnityEvent<Item> OnShowItem;
    //public UnityEvent OnShowChest;
    //public UnityEvent<Attribute> OnNewItemSlot;
    //public UnityEvent<Item> OnTakeItem;
    //public UnityEvent OnTakeChest;

    [SerializeField] GameEvent OnShowItem;
    [SerializeField] GameEvent OnShowPickup;
    [SerializeField] GameEvent OnNewItemSlot;
    [SerializeField] GameEvent OnTakeItem;
    [SerializeField] GameEvent OnTakePickup;
    [SerializeField] GameEvent OnShowMenu;
    [SerializeField] int DefaultMenuElementCount = 0;

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

    //public void Start()
    //{
    //    //OnShowItem.AddListener((Item _) => { Pause(); });
    //    //OnShowChest.AddListener(() => { Pause(); });
    //    //OnTakeItem.AddListener((Item _) => { Resume(); });
    //    //OnTakeChest.AddListener(() => { Resume(); });
    //    //OnClosePauseMenu.AddListener(() => { Pause(); });
    //    //OnShowPauseMenu.AddListener(() => { Resume(); });
    //}

    private void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {

        }
    }

    public void ShowMenu(Item item)
    {
        
        GameObject menu = null;
        //if (item.GetType().Name == nameof(Pickup))
        //{
        //    PickupMenu pickup = Instantiate(PickupMenu).GetComponent<PickupMenu>();
        //    pickup.Take.onClick.AddListener(() =>
        //    {
        //        OnTakePickup.Raise(this, item);
        //        Destroy(menu);
        //    });
        //    menu = pickup.MakeMenu(item);
        //}
        //else
        //{
        ItemMenu itemMenu = Instantiate(ItemMenu).GetComponent<ItemMenu>();
        itemMenu.Take.onClick.AddListener(() =>
        {
            if(transform.childCount == DefaultMenuElementCount + 1)
            {
                Resume();
            }
            OnTakeItem.Raise(this, item);
            Destroy(menu);
        });
        menu = itemMenu.MakeMenu(item);
        //}
        menu.transform.SetParent(transform, false);
        Pause();
        OnShowMenu.Raise(this, item);
        if (item.GetType().Name == nameof(Pickup)) OnShowPickup.Raise(this, item);
        else OnShowItem.Raise(this, item);
    }

    //public void ShowItemMenu(Item item)
    //{
    //    OnShowItem.Raise(this, item);
    //    GameObject menu = Instantiate(ItemMenu);
    //    ItemMenu itemDataLayout = menu.GetComponent<ItemMenu>();
    //    itemDataLayout.Found.text = $"Found {item.GetType()}!";
    //    itemDataLayout.Name.text = item.Name;
    //    itemDataLayout.Description.text = item.Description;
    //    itemDataLayout.Icon.sprite = item.Sprite;
    //    itemDataLayout.Take.onClick.AddListener(() =>
    //    {
    //        OnTakeItem.Raise(this, item);
    //        Destroy(menu);
    //    });
    //    menu.transform.SetParent(transform, false);
    //}

    //public void ShowChestMenu()
    //{
    //    OnShowChest.Raise(this, null);
    //    GameObject menu = Instantiate(ChestMenu);
    //    ChestMenu chestMenuData = menu.GetComponent<ChestMenu>();
    //    menu.transform.SetParent(transform, false);
    //    chestMenuData.Take.onClick.AddListener(() =>
    //    {
    //        OnTakeChest.Raise(this, null);
    //        Destroy(menu);
    //    });
    //}

    public void NewItemSlot(Attribute attribute)
    {
        OnNewItemSlot.Raise(this, attribute);
        GameObject slot = Instantiate(ItemSlot);
        ItemSlot slotdata = slot.GetComponent<ItemSlot>();
        slotdata.Icon.sprite = attribute.Sprite;
        slotdata.Level.text = "0";
        slot.transform.SetParent(transform, false);
        slot.transform.position = new Vector3(Screen.width / 2 / ItemManager.Instance.Items.Length, transform.position.x / (Screen.width / 4) * 3);
    }
}

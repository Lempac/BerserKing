using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public delegate void ItemSpawn(GameObject item);
    public event ItemSpawn OnItemSpawn;
    public delegate void ItemDespawn(Item item);
    public event ItemDespawn OnItemDespawn;
    private GameObject ItemInChunk;
    [SerializeField] private Item[] Items;
    private List<(int, int, byte)> ItemsData;
    private Dictionary<(int, int), List<(int, int, byte)>> SpawnedItems = new();
    int ChunkX, ChunkY;
    public static ItemManager Instance { get; private set; }
    public void Awake()
    {
        if (Instance != null) Debug.LogError("Only one Item Manager!");
        else Instance = this;
    }
    private void Start()
    {
        ChunkX = GenerateTileMap.Instance.ChunkX;
        ChunkY = GenerateTileMap.Instance.ChunkY;
        GenerateTileMap.OnLoadChunk += OnLoadChunk;
        GenerateTileMap.OnUnloadChunk += OnUnloadChunk;
    }

    private void OnUnloadChunk(int x, int y)
    {
        if (SpawnedItems.TryGetValue((x,y), out ItemsData))
        {
            foreach (var item in ItemsData)
            {
                if (OnItemDespawn != null) OnItemDespawn(Items[item.Item3]);
            }
            Destroy(transform.Find(x + "-" + y).gameObject);
        }
    }

    private void Spawn(int x, int y, Item item)
    {
        
        GameObject newItem = new GameObject(item.ItemName, new System.Type[] { typeof(SpriteRenderer) });
        SpriteRenderer newItemSprite = newItem.GetComponent<SpriteRenderer>();
        newItemSprite.sprite = item.ItemSprite;
        newItem.AddComponent<PolygonCollider2D>();
        newItem.transform.SetParent(ItemInChunk.transform, false);
        newItem.transform.position = new Vector3(x, y);
        if (OnItemSpawn != null) OnItemSpawn(newItem);
    }

    private void OnLoadChunk(int x, int y, byte data)
    {
        ItemInChunk = new GameObject(x+"-"+y);
        ItemInChunk.transform.SetParent(transform, false);
        if(SpawnedItems.TryGetValue((x,y), out ItemsData))
        {
            foreach (var item in ItemsData)
            {
                Debug.Log(item.Item1 + " "+ item.Item2+" "+item.Item3);
                Spawn(item.Item1, item.Item2, Items[item.Item3]);
            }
        }
        else
        {
            ItemsData = new List<(int, int, byte)>();
            int index = 0;
            foreach (var item in Items)
            {
                index++;
                if (Random.Range(0f, 1f) > item.SpawnRate) continue;
                int PositionX = x * ChunkX +Random.Range(0, ChunkX);
                int PositionY = y * ChunkY + Random.Range(0, ChunkY);
                Spawn(PositionX, PositionY, item);
                ItemsData.Add((PositionX, PositionY, (byte)(index-1)));
            }
            SpawnedItems.Add((x, y), ItemsData);
        }
    }
}

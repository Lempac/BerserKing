using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemManager : MonoBehaviour
{
    public delegate void ItemSpawned(GameObject item);
    public event ItemSpawned OnItemSpawned;
    public delegate void NewItemSpawned(Item item);
    public event NewItemSpawned OnNewItemSpawned;
    public delegate void ItemDespawned(GameObject item);
    public event ItemDespawned OnItemDespawned;
    public delegate void ItemPickUp(GameObject item, Collider2D howPickUp);
    public event ItemPickUp OnItemPickUp;
    public Item[] Items;

    private delegate void DeleteItemInChunk(int x, int y);
    private DeleteItemInChunk OnDeleteItemInChunk;
    private List<(int, int, byte)> ItemsData;
    readonly private Dictionary<(int, int), List<(int, int, byte)>> SpawnedItems = new();
    private int ChunkX, ChunkY;
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
        GenerateTileMap.OnNewLoadChunk += OnNewLoadChunk;
        GenerateTileMap.OnLoadChunk += OnLoadChunk;
        GenerateTileMap.OnUnloadChunk += OnUnloadChunk;

        //Logic for what happens on pickup
        OnItemPickUp += (item, other) =>
        {
            if (!other.TryGetComponent(typeof(ItemLevel), out var otherLevel)) otherLevel = other.gameObject.AddComponent<ItemLevel>();
            Attribute data = item.GetComponent<ItemData>().itemdata;
            item.GetComponent<AudioSource>().PlayOneShot(data.PlayOnPickUp);
            void OnTakeItem (Item itemdata)
            {
                Attribute.Level[] levels = data.Levels;
                if (levels == null || levels.Length == 0)
                {   
                    Destroy(item);
                    return;
                }
                Dictionary<int, int> otherLevels = ((ItemLevel)otherLevel).ItemLevels;
                if (!otherLevels.TryGetValue(data.ID, out int levelIndex)) { 
                    otherLevels[data.ID] = 0;
                    levelIndex = 0;
                }
                Attribute.Level levelData = levels[levelIndex];
                foreach (var stat in levelData.States)
                {
                    Component component = other.GetComponent(Type.GetType(stat.ComponentName, false));
                    FieldInfo property = component.GetType().GetField(stat.StatName);
                    property.SetValue(component, stat.OverWrite ? Convert.ChangeType(stat.Value, property.GetValue(component).GetType()) : stat.Value + property.GetValue(component));
                }
                if(levelIndex + 1 < levels.Length) otherLevels[data.ID] = 1+levelIndex;
                MenuHandler.OnTakeItem -= OnTakeItem;
                SpawnedItems[(Mathf.FloorToInt(item.transform.position.x / ChunkX), Mathf.FloorToInt(item.transform.position.y / ChunkX))].Remove((Mathf.FloorToInt(item.transform.position.x), Mathf.FloorToInt(item.transform.position.y), (byte)itemdata.ID));
                Destroy(item);
            }
            MenuHandler.OnTakeItem += OnTakeItem;
            MenuHandler.Instance.ShowItemMenu(data);
        };
    }

    private void OnUnloadChunk(int x, int y)
    {
        if (SpawnedItems.TryGetValue((x,y), out ItemsData))
        {
            foreach (var item in ItemsData)
            {
                //FIX: NOT THE BEST SOLUSION, BECAUSE IT BEING CALLED PER ITEM PER CHUNK
                OnDeleteItemInChunk(item.Item1, item.Item2);
                //OnItemDespawned?.Invoke(Items[item.Item3]);
            }
        }
    }

    private class ItemData : MonoBehaviour
    {
        public Attribute itemdata;
    }

    private class ItemTrigger : MonoBehaviour
    {
        private void OnTriggerStay2D(Collider2D collision)
        {
            Instance.OnItemPickUp(gameObject, collision);
        }
    }
    protected class ItemLevel : MonoBehaviour
    {
        public Dictionary<int, int> ItemLevels = new();
    }

    private GameObject Spawn(int x, int y, Item item)
    {
        
        GameObject newItem = new(item.ItemName, new Type[] { typeof(SpriteRenderer), typeof(ItemTrigger), typeof(AudioSource) });
        SpriteRenderer newItemSprite = newItem.GetComponent<SpriteRenderer>();
        newItemSprite.sprite = item.ItemSprite;
        newItemSprite.sortingOrder = 100;
        PolygonCollider2D newItemPolygonCollider = newItem.AddComponent<PolygonCollider2D>();
        newItemPolygonCollider.isTrigger = true;
        newItem.transform.SetParent(transform, false);
        newItem.transform.position = new Vector3(x, y);
        if(item.GetType().IsSubclassOf(typeof(Attribute)) || item.GetType() == typeof(Attribute)) newItem.AddComponent<ItemData>().itemdata = (Attribute)item;
        void delete (int PositionX, int PositionY)
        {
            Debug.Log($"This should be only one! : {item.ItemName}");
            if(PositionX == x && PositionY == y) {
                //Debug.Log($"Item Despawned x:{x} y:{y} x/chunkX:{Mathf.FloorToInt(x / ChunkX)} y/ChunkY:{Mathf.FloorToInt(y / ChunkX)} PositionX:{PositionX} PositionY:{PositionY} {Mathf.FloorToInt(x / ChunkX) == PositionX && Mathf.FloorToInt(y / ChunkX) == PositionY}");
                //if (Mathf.FloorToInt(x / ChunkX) == PositionX && Mathf.FloorToInt(y / ChunkX) == PositionY) {
                OnDeleteItemInChunk -= delete;
                OnItemDespawned?.Invoke(newItem);
                Destroy(newItem);
            }
        };
        OnDeleteItemInChunk -= delete;
        OnDeleteItemInChunk += delete;
        //Debug.Log($"Item spawned x:{x} y:{y} x/chunkX:{Mathf.FloorToInt(x / ChunkX)} y/ChunkY:{Mathf.FloorToInt(y / ChunkX)}");
        return newItem;
    }
    private void OnNewLoadChunk(int x, int y, byte data)
    {
        TileMapData tileMapData = GenerateTileMap.Instance.baseTiles[data].GetComponent<TileMapData>();
        ItemsData = new List<(int, int, byte)>();
        foreach (var item in Items)
        {
            if (tileMapData.MaxItemAmount >= ItemsData.Count) break;
            if (Random.Range(0f, 1f) > item.SpawnRate) continue;
            int PositionX = x * ChunkX + Random.Range(0, ChunkX);
            int PositionY = y * ChunkY + Random.Range(0, ChunkY);
            //GameObject itemObject = Spawn(PositionX, PositionY, item);
            //Debug.Log($"With Position x:{x} y:{y} PositionX:{PositionX} PositionY:{PositionY}");
            OnNewItemSpawned?.Invoke(item);
            //OnItemSpawned?.Invoke(itemObject);
            ItemsData.Add((PositionX, PositionY, (byte)(item.ID)));
        }
        SpawnedItems[(x, y)] = ItemsData;
    }

    private void OnLoadChunk(int x, int y, byte _)
    {
        if (SpawnedItems.TryGetValue((x, y), out ItemsData))
        {
            foreach (var item in ItemsData)
            {
                GameObject itemObject = Spawn(item.Item1, item.Item2, Items[item.Item3]);
                OnItemSpawned?.Invoke(itemObject);
            }
        }
    }
}
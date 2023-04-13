using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
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
    private Dictionary<(int, int), List<(int, int, byte)>> SpawnedItems = new();
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
            Item data = item.GetComponent<ItemData>().itemdata;
            item.GetComponent<AudioSource>().PlayOneShot(data.PlayOnPickUp);
            //BUG: DOESNT UNSUBCRIBE, I think?
            MenuHandeler.OnTakeItem += (itemdata) =>
            {

                //Item.IState[] data = item.GetComponent<ItemData>()?.itemdata?.State;
                //if (data == null) return;
                //foreach (var stat in data)
                //{
                //    Component component = other.GetComponent(Type.GetType(stat.ComponentName, false));
                //    FieldInfo property = component.GetType().GetField(stat.StatName);
                //    property.SetValue(component, stat.OverWrite ? Convert.ChangeType(stat.Value, property.GetValue(component).GetType()) : stat.Value + property.GetValue(component));
                //}
                Destroy(item);
            };
            MenuHandeler.Instance.ShowItemMenu(data);
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
        public Item itemdata;
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
        Dictionary<int, int> Levels = new();
    }

    private GameObject Spawn(int x, int y, Item item)
    {
        
        GameObject newItem = new GameObject(item.ItemName, new Type[] { typeof(SpriteRenderer), typeof(ItemTrigger), typeof(AudioSource) });
        SpriteRenderer newItemSprite = newItem.GetComponent<SpriteRenderer>();
        newItemSprite.sprite = item.ItemSprite;
        PolygonCollider2D newItemPolygonCollider = newItem.AddComponent<PolygonCollider2D>();
        newItemPolygonCollider.isTrigger = true;
        newItem.transform.SetParent(transform, false);
        newItem.transform.position = new Vector3(x, y);
        newItem.AddComponent<ItemData>().itemdata = item;
        OnDeleteItemInChunk += (PositionX, PositionY) =>
        {
            if(PositionX == x && PositionY == y) {
            //Debug.Log($"Item Despawned x:{x} y:{y} x/chunkX:{Mathf.FloorToInt(x / ChunkX)} y/ChunkY:{Mathf.FloorToInt(y / ChunkX)} PositionX:{PositionX} PositionY:{PositionY} {Mathf.FloorToInt(x / ChunkX) == PositionX && Mathf.FloorToInt(y / ChunkX) == PositionY}");
            //if (Mathf.FloorToInt(x / ChunkX) == PositionX && Mathf.FloorToInt(y / ChunkX) == PositionY) {
                OnItemDespawned?.Invoke(newItem);
                Destroy(newItem);
            }
        };
        //Debug.Log($"Item spawned x:{x} y:{y} x/chunkX:{Mathf.FloorToInt(x / ChunkX)} y/ChunkY:{Mathf.FloorToInt(y / ChunkX)}");
        return newItem;
    }
    private void OnNewLoadChunk(int x, int y, byte data)
    {

        ItemsData = new List<(int, int, byte)>();
        int index = 0;
        foreach (var item in Items)
        {
            index++;
            if (Random.Range(0f, 1f) > item.SpawnRate) continue;
            int PositionX = x * ChunkX + Random.Range(0, ChunkX);
            int PositionY = y * ChunkY + Random.Range(0, ChunkY);
            //GameObject itemObject = Spawn(PositionX, PositionY, item);
            //Debug.Log($"With Position x:{x} y:{y} PositionX:{PositionX} PositionY:{PositionY}");
            OnNewItemSpawned?.Invoke(item);
            //OnItemSpawned?.Invoke(itemObject);
            ItemsData.Add((PositionX, PositionY, (byte)(index - 1)));
        }
        SpawnedItems.Add((x, y), ItemsData);
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
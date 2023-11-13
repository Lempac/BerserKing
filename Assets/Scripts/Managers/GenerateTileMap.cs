using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class GenerateTileMap : MonoBehaviour
{
    public Grid[] baseTiles;
    readonly private Dictionary<(int, int), byte> data = new();
    public int ChunkX = 16, ChunkY = 16, LoadRange = 1, UnloadAfterChunk = 1;
    [SerializeField] float delta = 0.01f;

    public static GenerateTileMap Instance { get; private set; }

    //public delegate void OnNewLoadChunkEvent(int x, int y, byte data);
    //public delegate void LoadChunkEvent(int x, int y, byte data);
    //public delegate void UnloadChunkEvent(int x, int y);

    //public static event LoadChunkEvent OnNewLoadChunk;
    //public static event LoadChunkEvent OnLoadChunk;
    //public static event UnloadChunkEvent OnUnloadChunk;

    //public UnityEvent<int, int, byte> OnNewLoadChunk;
    //public UnityEvent<int, int, byte> OnLoadChunk;
    //public UnityEvent<int, int> OnUnloadChunk;

    [SerializeField] GameEvent OnNewLoadChunk;
    [SerializeField] GameEvent OnLoadChunk;
    [SerializeField] GameEvent OnUnloadChunk;

    public void Awake()
    {
        if (Instance != null) Debug.LogError("Only one GenerateTileMap!");
        else Instance = this;
    }
    private Tilemap NewLayer(GameObject propertys)
    {
        GameObject layer = new(propertys.name, new Type[] { typeof(Tilemap), typeof(TilemapRenderer) });
        if (propertys.TryGetComponent<TilemapCollider2D>(out _))
        {
            var collider = layer.AddComponent<TilemapCollider2D>();
            collider.usedByComposite = true;
            layer.AddComponent<CompositeCollider2D>();
            var rigidbody = layer.GetComponent<Rigidbody2D>();
            rigidbody.bodyType = RigidbodyType2D.Static;
        }
        layer.GetComponent<TilemapRenderer>().sortingOrder = propertys.GetComponent<TilemapRenderer>().sortingOrder;
        layer.transform.parent = transform;
        Tilemap tilemap = layer.GetComponent<Tilemap>();
        tilemap.ClearAllTiles();
        return tilemap;
    }
    private bool HasTiles(Tilemap tilemap, BoundsInt bounds)
    {
        foreach (var tile in tilemap.GetTilesBlock(bounds))
        {
            if (tile != null) return true;
        }
        return false;
    }

    private bool LoadChunk(int PositionX, int PositionY, byte ChunkData)
    {
        int StartX = PositionX * ChunkX;
        int StartY = PositionY * ChunkY;
        foreach (Transform tile in baseTiles[ChunkData].transform)
        {
            Tilemap currTileMap = transform.Find(tile.name)?.GetComponent<Tilemap>() ?? NewLayer(tile.gameObject);
            if (HasTiles(currTileMap, new BoundsInt(new Vector3Int(StartX, StartY), new Vector3Int(ChunkX, ChunkY, 1)))) return false;
            Tilemap currTileMapData = tile.GetComponent<Tilemap>();
            currTileMap.SetTilesBlock(new BoundsInt(new Vector3Int(currTileMapData.cellBounds.position.x + StartX, currTileMapData.cellBounds.position.y + StartY), currTileMapData.cellBounds.size), currTileMapData.GetTilesBlock(currTileMapData.cellBounds));
        }
        Debug.Log($"Chunk {PositionX} : {PositionY} loaded!");
        return true;
    }
    private bool UnloadChunk(int PositionX, int PositionY)
    {

        int StartX = PositionX * ChunkX;
        int StartY = PositionY * ChunkY;
        foreach (Transform tilemapObject in transform)
        {
            Tilemap currTileMap = tilemapObject.GetComponent<Tilemap>();
            if (!HasTiles(currTileMap, new BoundsInt(new Vector3Int(StartX, StartY), new Vector3Int(ChunkX, ChunkY, 1)))) return false;
            for (int x = StartX; x < StartX + ChunkX; x++)
            {
                for (int y = StartY; y < StartY + ChunkY; y++)
                {
                    currTileMap.SetTile(new Vector3Int(x, y), null);
                }
            }
        }
        Debug.Log($"Chunk {PositionX} : {PositionY} unloaded!");
        return true;
    }

    void Start()
    {
        foreach (var grid in baseTiles)
        {
            foreach (Transform tile in grid.transform)
            {
                tile.gameObject.GetComponent<Tilemap>()?.CompressBounds();
            }
        }
    }
    //private void OnDestroy()
    //{
    //    OnLoadChunk.RemoveAllListeners();
    //    OnNewLoadChunk.RemoveAllListeners();
    //    OnUnloadChunk.RemoveAllListeners();
    //}

    private void FixedUpdate()
    {
        if(EntityManager.Player == null) return;
        int x = Mathf.FloorToInt(EntityManager.Player.transform.transform.position.x / ChunkX), y = Mathf.FloorToInt(EntityManager.Player.transform.position.y / ChunkY);
        for (int i = -LoadRange + x; i <= LoadRange + x; i++)
        {
            for (int j = -LoadRange + y; j <= LoadRange + y; j++)
            {
                if (data.TryGetValue((i, j), out byte ChunkData))
                {
                    if (LoadChunk(i, j, ChunkData)) OnLoadChunk.Raise(this, (i, j, ChunkData));
                }
                else
                {

                    byte f = Convert.ToByte(Mathf.PerlinNoise(i * delta, j * delta) * (baseTiles.Length - 1));
                    data.Add((i, j), f);
                    if (LoadChunk(i, j, f))
                    {
                        OnNewLoadChunk.Raise(this, (i, j, f));
                        OnLoadChunk.Raise(this, (i, j, f));
                    }
                }
            }
        }
        for (int i = -(LoadRange + UnloadAfterChunk) + x; i <= LoadRange + UnloadAfterChunk + x; i++)
        {
            for (int j = -(LoadRange + UnloadAfterChunk) + y; j <= LoadRange + UnloadAfterChunk + y; j++)
            {
                if (i >= -LoadRange + x && i <= LoadRange + x && j >= -LoadRange + y && j <= LoadRange + y) continue;
                if (UnloadChunk(i, j)) OnUnloadChunk.Raise(this, (i, j));
            }
        }
    }
}

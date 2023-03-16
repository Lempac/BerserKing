using Mono.Cecil;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class GenerateTileMap : MonoBehaviour
{
    [SerializeField] private Transform player;
    public Tilemap[] baseTiles;
    private Dictionary<(int, int), byte> data = new();
    Tilemap LevelMap;
    public int ChunkX = 16, ChunkY = 16, LoadRange = 1, UnloadAfterChunk = 1;
    [SerializeField] float delta = 0.01f;

    public static GenerateTileMap Instance { get; private set; }

    public delegate void LoadChunkEvent(int x, int y, byte data);
    public delegate void UnloadChunkEvent(int x, int y);

    public static event LoadChunkEvent OnLoadChunk;
    public static event UnloadChunkEvent OnUnloadChunk;
    public void Awake()
    {
        if (Instance != null) Debug.LogError("Only one GenerateTileMap!");
        else Instance = this;
    }

    void LoadChunk(int PositionX, int PositionY, byte ChunkData)
    {
        
        int StartX = PositionX * ChunkX;
        int StartY = PositionY * ChunkY;
        if (LevelMap.HasTile(new Vector3Int(StartX, StartY))) return;
        
        LevelMap.SetTilesBlock(new BoundsInt(new Vector3Int(StartX, StartY), baseTiles[ChunkData].cellBounds.size), baseTiles[ChunkData].GetTilesBlock(baseTiles[ChunkData].cellBounds));
        if (OnLoadChunk != null) OnLoadChunk(PositionX, PositionY, ChunkData);
        Debug.Log($"Chunk {PositionX} : {PositionY} loaded!");
    }
    void UnloadChunk(int PositionX, int PositionY)
    {
        
        int StartX = PositionX * ChunkX;
        int StartY = PositionY * ChunkY;
        if (!LevelMap.HasTile(new Vector3Int(StartX, StartY))) return;
        for (int x = StartX; x < StartX + ChunkX; x++)
        {
            for (int y = StartY; y < StartY + ChunkY; y++)
            {
                LevelMap.SetTile(new Vector3Int(x, y), null);
            }
        }
        if (OnUnloadChunk != null) OnUnloadChunk(PositionX, PositionY);
        Debug.Log($"Chunk {PositionX} : {PositionY} unloaded!");
    }

    void Start()
    {
        LevelMap = gameObject.GetComponent<Tilemap>();
        foreach (var tile in baseTiles)
        {
            tile.CompressBounds();
        }
    }
    private void FixedUpdate()
    {
        int x = Mathf.FloorToInt(player.position.x / ChunkX), y = Mathf.FloorToInt(player.position.y / ChunkY);
        for (int i = -LoadRange + x; i <= LoadRange + x; i++) {
            for (int j = -LoadRange + y; j <= LoadRange + y; j++)
            {
                if (data.TryGetValue((i, j), out byte ChunkData))
                {
                    LoadChunk(i, j, ChunkData);
                } 
                else
                {

                    byte f = Convert.ToByte(Mathf.PerlinNoise(i * delta, j * delta) * (baseTiles.Length - 1));
                    data.Add((i, j), f);
                    LoadChunk(i, j, f);
                }
            }
        }
        for (int i = -(LoadRange + UnloadAfterChunk) + x; i <= LoadRange + UnloadAfterChunk + x; i++)
        {
            for (int j = -(LoadRange + UnloadAfterChunk) + y; j <= LoadRange + UnloadAfterChunk + y; j++)
            {
                if (i >= -LoadRange + x && i <= LoadRange + x && j >= -LoadRange + y && j <= LoadRange + y) continue;
                //Debug.Log(i + " " + j);
                UnloadChunk(i, j);
            }
        }
    }
}

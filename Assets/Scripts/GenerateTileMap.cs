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
    private Dictionary<Tuple<int, int>, byte> data = new Dictionary<Tuple<int, int>, byte>();
    GameObject Level;
    Tilemap LevelMap;
    private int ChunkX = 16, ChunkY = 16, LoadRange = 1, UnloadAfterChunk = 1;
    [SerializeField] float delta = 0.01f;
    void LoadChunk(int PositionX, int PositionY, byte ChunkData)
    {
        int StartX = PositionX * ChunkX;
        int StartY = PositionY * ChunkY;
        if (LevelMap.HasTile(new Vector3Int(StartX, StartY))) return;
        LevelMap.SetTilesBlock(new BoundsInt(new Vector3Int(StartX, StartY), baseTiles[ChunkData].cellBounds.size), baseTiles[ChunkData].GetTilesBlock(baseTiles[ChunkData].cellBounds));
        //Debug.Log($"Chunk {PositionX} : {PositionY} loaded!");
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
        Debug.Log($"Chunk {PositionX} : {PositionY} unloaded!");
    }

    void Start()
    {
        Level = new GameObject("GenLevel", typeof(Tilemap), typeof(TilemapRenderer));
        Level.transform.parent = transform;
        Level.transform.position = Vector3.zero;
        LevelMap = Level.GetComponent<Tilemap>();
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
                if (data.TryGetValue(new Tuple<int, int>(i, j), out byte ChunkData))
                {
                    LoadChunk(i, j, ChunkData);
                } 
                else
                {

                    byte f = Convert.ToByte(Mathf.PerlinNoise(i * delta, j * delta) * (baseTiles.Length - 1));
                    data.Add(new Tuple<int, int>(i, j), f);
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

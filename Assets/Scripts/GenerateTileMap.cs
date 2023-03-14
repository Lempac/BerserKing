using Mono.Cecil;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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
        if (LevelMap.GetTile(new Vector3Int(StartX, StartY)) != null) return;
        LevelMap.SetTilesBlock(new BoundsInt(new Vector3Int(StartX, StartY), baseTiles[ChunkData].cellBounds.size), baseTiles[ChunkData].GetTilesBlock(baseTiles[ChunkData].cellBounds));
        Debug.Log($"Chunk {PositionX} : {PositionY} loaded!");
    }
    void UnloadChunk(int PositionX, int PositionY)
    {
        int StartX = PositionX * ChunkX;
        int StartY = PositionY * ChunkY;
        if (LevelMap.GetTile(new Vector3Int(StartX, StartY)) == null) return;
        LevelMap.SetTilesBlock(new BoundsInt(new Vector3Int(StartX, StartY), new Vector3Int(ChunkX, ChunkY)), null);
        Debug.Log($"Chunk {PositionX} : {PositionY} unloaded!");
    }

    void Start()
    {
        Level = new GameObject("GenLevel", typeof(Tilemap), typeof(TilemapRenderer));
        Level.transform.parent = transform;
        LevelMap = Level.GetComponent<Tilemap>();
        foreach (var tile in baseTiles)
        {
            tile.CompressBounds();
        }
    }
    private void FixedUpdate()
    {
        for (int i = -LoadRange; i <= LoadRange; i++) {
            for (int j = -LoadRange; j <= LoadRange; j++)
            {
                int x = Mathf.FloorToInt(player.position.x / ChunkX)+i, y = Mathf.FloorToInt(player.position.y / ChunkY)+j;
                if (data.TryGetValue(new Tuple<int, int>(x, y), out byte ChunkData))
                {
                    LoadChunk(x, y, ChunkData);
                } 
                else
                {

                    byte f = Convert.ToByte(Mathf.PerlinNoise(x * delta, x * delta) * (baseTiles.Length - 1));
                    data.Add(new Tuple<int, int>(x, y), f);
                    LoadChunk(x, y , f);
                }
            }
        }
        for (int i = -LoadRange - UnloadAfterChunk; i <= LoadRange + UnloadAfterChunk; i++)
        {
            for (int j = -LoadRange - UnloadAfterChunk; j <= LoadRange + UnloadAfterChunk; j++)
            {
                if (i > -LoadRange - UnloadAfterChunk && i < LoadRange + UnloadAfterChunk && j > -LoadRange - UnloadAfterChunk && j < LoadRange + UnloadAfterChunk) continue;
                //Debug.Log(i + " " + j);
                UnloadChunk(i, j);
            }
        }
    }
}

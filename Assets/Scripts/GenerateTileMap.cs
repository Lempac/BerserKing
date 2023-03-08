using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class GenerateTileMap : MonoBehaviour
{
    [SerializeField] private Transform player;
    public TileBase[] baseTiles;
    private Dictionary<Tuple<int,int>, byte> data = new Dictionary<Tuple<int, int>, byte>();
    GameObject Level;
    Tilemap LevelMap;
    private int ChunkX = 16, ChunkY = 16;
    [SerializeField] float delta = 0.01f;
    void LoadChunk(int PositionX, int PositionY)
    {
        int StartX = PositionX * ChunkX;
        int StartY = PositionY * ChunkY;
        for (int x = StartX; x < StartX + ChunkX; x++)
        {
            for (int y = StartY; y < StartY + ChunkY; y++)
            {
                float i = Mathf.PerlinNoise(x * delta, y * delta);
                LevelMap.SetTile(new Vector3Int(x, y), (i < .3f ? baseTiles[0] : (i >= .3f && i <= .7f  ? baseTiles[1] : baseTiles[2])));
            }
        }
        Debug.Log($"Chunk {PositionX} : {PositionY} loaded!");
    }
    void UnloadChunk(int PositionX, int PositionY)
    {
        int StartX = PositionX * ChunkX;
        int StartY = PositionY * ChunkY;
        for (int x = StartX; x < StartX + ChunkX; x++)
        {
            for (int y = StartY; y < StartY + ChunkY; y++)
            {
                LevelMap.SetTile(new Vector3Int(x, y), null);
            }
        }
    }


    void Start()
    {
        Level = new GameObject("GenLevel", typeof(Tilemap), typeof(TilemapRenderer));
        Level.transform.parent = transform;
        LevelMap = Level.GetComponent<Tilemap>();
    }
    private void FixedUpdate()
    {
        int x = Mathf.FloorToInt(player.position.x / ChunkX), y = Mathf.FloorToInt(player.position.y / ChunkY);
        if (!data.TryGetValue(new Tuple<int, int>(x, y), out byte _))
        {
            LoadChunk(x, y);
            data.Add(new Tuple<int, int>(x, y), 0b0);
        }

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class GenerateTileMap : MonoBehaviour
{
    public TileBase[] baseTiles;
    GameObject Level;
    Tilemap LevelMap;
    private int ChunkX = 16, ChunkY = 16;
    [SerializeField] float delta = 0.01f;
    async void Chunk(int PositionX, int PositionY)
    {
        //Tile[] list = new Tile[ChunkX*ChunkY];
        int StartX = PositionX * ChunkX;
        int StartY = PositionY * ChunkY;
        for (int x = StartX; x < StartX + ChunkX; x++)
        {
            for (int y = StartY; y < StartY + ChunkY; y++)
            {
                float i = Mathf.PerlinNoise(x * delta, y * delta);
                LevelMap.SetTile(new Vector3Int(x, y), (i < .3f ? baseTiles[0] : (i >= .3f && i <= .7f  ? baseTiles[1] : baseTiles[2])));
                await Task.Yield();
            }
        }
        Debug.Log($"Chunk {PositionX} : {PositionY} loaded!");
        for (int x = StartX; x < StartX + ChunkX; x++)
        {
            for (int y = StartY; y < StartY + ChunkY; y++)
            {
                TileBase curr = LevelMap.GetTile(new Vector3Int(x, y));
                //Debug.Log(curr.GetTileData);
                //if(curr.name)
                await Task.Yield();
            }
        }
    }
    
    void Start()
    {
        Level = new GameObject("GenLevel", typeof(Tilemap), typeof(TilemapRenderer));
        Level.transform.parent = transform;
        LevelMap = Level.GetComponent<Tilemap>();
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                Chunk(x, y);
            }
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

public class TileMapData : MonoBehaviour
{
    public static List<GameObject> TileMaps;
    public int MaxItemAmount = 0;
    void OnValidate()
    {
        if (TileMaps == null) TileMaps = new List<GameObject>();
        if (!TileMaps.Contains(gameObject)) TileMaps.Add(gameObject);
    }
}

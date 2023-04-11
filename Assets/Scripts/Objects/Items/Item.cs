using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Object/Items/Item", fileName = "New Item")]
public class Item : ScriptableObject
{
    public static List<Item> Items;
    public string ItemName;
    public string ItemDescription;
    public Sprite ItemSprite;
    public AudioClip PlayOnPickUp;
    public float SpawnRate;
    void OnValidate()
    {
        if (Items == null) Items = new List<Item>();
        if (!Items.Contains(this)) Items.Add(this);
    }
}
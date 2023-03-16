using System;
using UnityEngine;

[CreateAssetMenu(menuName = "New Item")]
public class Item : ScriptableObject
{
    public string ItemName;
    public string ItemDescription;
    [Serializable]
    public enum EItemType
    {
        Attribute,
        Weapon,
        Pickup
    }
    public EItemType ItemType;
    public Sprite ItemSprite;
    public AudioClip PlayOnPickUp; 
}
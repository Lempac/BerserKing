using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
}

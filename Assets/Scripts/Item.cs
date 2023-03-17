using System;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Object/Item", fileName ="New Item")]
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
    public float SpawnRate;
}

//[CustomEditor(typeof(Item))]
//public class ItemEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();
//    }
//}
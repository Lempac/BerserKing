using UnityEngine;

[CreateAssetMenu(menuName = "Object/Items/Item", fileName = "New Item")]
public class Item : ScriptableObject
{
    public int ID;
    public string ItemName;
    public string ItemDescription;
    public Sprite ItemSprite;
    public AudioClip PlayOnPickUp;
    public float SpawnRate;
}
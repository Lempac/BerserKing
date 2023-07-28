using UnityEngine;

[CreateAssetMenu(menuName = "Object/Items/Item", fileName = "New Item")]
public class Item : ScriptableObject
{
    public int ID;
    public string Name;
    public string Description;
    public Sprite Sprite;
    public AudioClip PlayOnPickUp;
    public float SpawnRate;
}
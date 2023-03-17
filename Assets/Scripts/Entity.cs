using UnityEngine;

[CreateAssetMenu(menuName = "Object/Entity", fileName = "New Entity")]
public class Entity : ScriptableObject
{
    public string Name;
    public string Description;
    public int Health;
    public int MaxHealth;
    public float Speed;
    public float MaxSpeed;
}

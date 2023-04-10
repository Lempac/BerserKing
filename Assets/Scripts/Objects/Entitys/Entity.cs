using System;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(menuName = "Object/Entity", fileName = "New Entity")]
public class Entity : ScriptableObject
{
    public int ID;
    public string Name;
    public string Description;
    public int Health;
    public int MaxHealth;
    public float Speed;
    public float MaxSpeed;
    public AnimatorController EntityAnimator;
}

using System;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(menuName = "Object/Entity", fileName = "New Entity")]
public class Entity : ScriptableObject
{
    public static List<Entity> Entities;
    public string Name;
    public string Description;
    public int Health;
    public int MaxHealth;
    public float Speed;
    public float MaxSpeed;
    public AnimatorController EntityAnimator;
    void OnValidate()
    {
        if( Entities == null ) Entities = new List<Entity>();
        if (!Entities.Contains(this)) Entities.Add(this);
    }
}

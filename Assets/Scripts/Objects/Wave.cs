using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Object/Wave", fileName = "New Wave")]
public class Wave : ScriptableObject
{
    [Header("Triggers:")]
    [Tooltip("Events on wave will be triggered!")]
    public List<GameEvent> StartOn;
    [Tooltip("Events on wave will spawn entities!")]
    public List<GameEvent> SpawnOn;
    [Tooltip("Events on wave will end!")]
    public List<GameEvent> EndOn;
    [Serializable]
    public struct SpawnEntityInfo
    {
        public Entity entity;
        public int Amount;
    }
    [Header("Entity data:")]
    public List<SpawnEntityInfo> entities;
}

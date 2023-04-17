using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Object/Wave", fileName = "New Wave")]
public class Wave : ScriptableObject
{
    public string StartOn;
    public string EndOn;
    [Serializable]
    public struct SpawnEntityInfo
    {
        public Entity entity;
        public int Amount;
    }
    public SpawnEntityInfo[] entities;
}

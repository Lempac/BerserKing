using UnityEngine;

[CreateAssetMenu(menuName = "Object/Wave", fileName = "New Wave")]
public class Wave : ScriptableObject
{
<<<<<<< HEAD
    public string StartOn;
    public string EndOn;
    [Serializable]
    public struct SpawnEntityInfo
    {
        public Entity entity;
        public int Amount;
    }
    public SpawnEntityInfo[] entities;
=======
    public int MaxEntitys; 
>>>>>>> 0f97433b9b0aae5f27bceb37da231ba96025beac
}

using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Object/Items/Attribute", fileName = "New Attribute")]
public class Attribute : Item
{
    [Serializable]
    public class IState
    {
        public string ComponentName;
        public string StatName;
        public string Value;
        public bool OverWrite;
    }
    [Serializable]
    public class Level
    {
        public string Desicription;
        public IState[] States;
    };
    public Level[] Levels;
    //private void OnValidate()
    //{
    //    foreach (var level in Levels)
    //    {
    //        for (var i = 0; i < level.States.Length; ++i)
    //        {
    //            var type = Type.GetType(level.States[i].ComponentName);
    //            Debug.Log(type);
    //            Type typefield = type.GetField(level.States[i].StatName).FieldType;
    //            level.States[i].Value = new test<typefield>();
    //        }
    //    }
    //}
}

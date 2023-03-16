using System.Collections;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    
    public delegate void EntitySpawn();
    public event EntitySpawn OnEntitySpawn;
    public static EntityManager Instance { get; private set; }
    public void Awake()
    {
        if (Instance != null) Debug.LogError("Only one Entity Manager!");
        else Instance = this;
    }

    private IEnumerator Spawer()
    {
        if(OnEntitySpawn != null) OnEntitySpawn();
        yield return null;
    }

    private void Start()
    {
        StartCoroutine(Spawer());
    }
}

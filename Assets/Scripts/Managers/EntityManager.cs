using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    public delegate void EntitySpawned();
    public event EntitySpawned OnEntitySpawned;
    public delegate void EntityDespawned();
    public event EntityDespawned OnEntityDespawned;
    public delegate void WaveStart();
    public event WaveStart OnWaveStart;
    public delegate void WaveEnd();
    public event WaveEnd OnWaveEnd;

    public int EntityLimit = 200;
    readonly private List<GameObject> Entitys;
    public static EntityManager Instance { get; private set; }
    public void Awake()
    {
        if (Instance != null) Debug.LogError("Only one Entity Manager!");
        else Instance = this;
    }
    
    private void Spawn(Entity data)
    {

        GameObject entity = new(data.name,new System.Type[] { typeof(Animator), });
    }


    private IEnumerator Spawer()
    {
        while (true)
        {
            if(Entitys.Count == EntityLimit) yield return null;
        }
        //OnEntitySpawn?.Invoke();
    }

    private void Start()
    {
        //StartCoroutine(Spawer());
    }
}

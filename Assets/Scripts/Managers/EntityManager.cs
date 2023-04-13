using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    public delegate void EntitySpawned();
    public event EntitySpawned OnEntitySpawned;

    public delegate void EntityDespawned();
    public event EntityDespawned OnEntityDespawned;
<<<<<<< HEAD
    public delegate void WaveStarted();
    public event WaveStarted OnWaveStarted;
    public delegate void WaveEnded();
    public event WaveEnded OnWaveEnded;

    public int EntityLimit = 200;
    public List<Wave> waves;
    readonly private List<GameObject> Entitys;
=======

    public int EntityLimit = 200;
    private List<GameObject> Entitys;
>>>>>>> 0f97433b9b0aae5f27bceb37da231ba96025beac
    public static EntityManager Instance { get; private set; }
    public void Awake()
    {
        if (Instance != null) Debug.LogError("Only one Entity Manager!");
        else Instance = this;
    }
    
    private void Spawn(Entity data)
    {
<<<<<<< HEAD
        GameObject entity = new(data.name,new System.Type[] { });
        entity.AddComponent<SpriteRenderer>();
        entity.AddComponent<Animator>().runtimeAnimatorController = data.EntityAnimator;
=======

        GameObject entity = new GameObject(data.name,new System.Type[] { typeof(Animator), });
>>>>>>> 0f97433b9b0aae5f27bceb37da231ba96025beac
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
        StartCoroutine(Spawer());
        foreach (Wave wave in waves)
        {
            switch (wave.StartOn){
                case "Start":
                    
                    break;
                default:
                    Debug.LogError($"Unknow Event: {wave.StartOn}");
                    break;
            }
        }
    }
}

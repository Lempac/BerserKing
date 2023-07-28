using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class EntityManager : MonoBehaviour
{
    //public delegate void EntitySpawned();
    /// <summary>
    /// Tigger when <c>Entity</c> spawns.
    /// </summary>
    //public event EntitySpawned OnEntitySpawned;
    //public UnityEvent<GameObject> OnEntitySpawned;
    //public delegate void EntityDespawned();
    /// <summary>
    /// Tigger when <c>Entity</c> despawns.
    /// </summary>
    //public event EntityDespawned OnEntityDespawned;
    //public UnityEvent<Entity> OnEntityDespawned;
    //public delegate void EntityKilled(Entity entity, Vector3 position);
    /// <summary>
    /// Tigger when <c>Entity</c> killed.
    /// </summary>
    //public event EntityKilled OnEntityKilled;
    //public UnityEvent<Entity, Vector3> OnEntityKilled;
    //public delegate void EntityHit(GameObject entity, float healthAmount);
    /// <summary>
    /// Tigger when <c>Entity</c> get hit.
    /// </summary>
    //public event EntityHit OnEntityHit;
    //public UnityEvent<GameObject, float> OnEntityHit;
    //public delegate void WaveStarted(Wave data);
    /// <summary>
    /// Tigger when <c>Wave</c> start.
    /// </summary>
    //public event WaveStarted OnWaveStarted;
    //public UnityEvent<Wave> OnWaveStarted;
    //public delegate void WaveEnded(Wave data);
    /// <summary>
    /// Tigger when <c>Wave</c> ends.
    /// </summary>
    //public event WaveEnded OnWaveEnded;
    //public UnityEvent<Wave> OnWaveEnded;

    [SerializeField] GameEvent OnEntitySpawned;
    [SerializeField] GameEvent OnEntityDespawned;
    [SerializeField] GameEvent OnEntityKilled;
    [SerializeField] GameEvent OnEntityHit;
    [SerializeField] GameEvent OnWaveStarted;
    [SerializeField] GameEvent OnWaveEnded;

    [SerializeField] GameEventListener OnGameSecond;
    [SerializeField] GameEventListener OnGameUpdate;

    public static int EntityLimit = 100;
    public List<Wave> waves;
    readonly private List<GameObject> Entities = new();
    public static EntityManager Instance { get; private set; }
    public static GameObject Player { get; private set; }

    private void SpawnHandle(Entity data)
    {
        if (transform.childCount >= EntityLimit)
        {
            bool isAvaiable = false;
            foreach (var entity in Entities)
            {
                if (entity.activeSelf)
                {
                    if (isAvaiable) Destroy(entity);
                    entity.GetComponent<Health>().health = data.Health;
                    entity.GetComponent<Animator>().runtimeAnimatorController = data.EntityAnimator;
                    entity.GetComponent<Data>().data = data;
                    if (Player == null || !Player.activeSelf && data.Playable) CurrentPlayer(entity, data);
                    entity.SetActive(true);
                    isAvaiable = true;
                }
            }
            if (!isAvaiable)
            {
                Spawn(data);
            }
        }
        else
        {
            Spawn(data);
        }
    }

    private Vector2 NewPosition()
    {
        int Radius = 60;
        Vector3 BottomLeftCorner = Camera.main.ScreenToWorldPoint(new Vector2(-Radius, -Radius));
        Vector3 TopRightCorner = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width + Radius, Screen.height + Radius));
        int StartPoint = Random.Range(1, 4);
        if (StartPoint > 2)
        {
            if (StartPoint == 4)
            {
                return new Vector2(TopRightCorner.x-Random.Range(0,Radius), Random.Range(BottomLeftCorner.y, TopRightCorner.y));
            }
            else
            {
                return new Vector2(Random.Range(BottomLeftCorner.x, TopRightCorner.x), TopRightCorner.y-Random.Range(0,Radius));
            }
        }
        else
        {
            if(StartPoint == 1){
                return new Vector2(BottomLeftCorner.x+Random.Range(0,Radius), Random.Range(BottomLeftCorner.y, TopRightCorner.y));
            }
            else
            {
                return new Vector2(Random.Range(BottomLeftCorner.x, TopRightCorner.x), BottomLeftCorner.y+Random.Range(0,Radius));
            }
        }
    }

    private void Spawn(Entity data)
    {
        GameObject entity = new(data.name);
        SpriteRenderer renderer = entity.AddComponent<SpriteRenderer>();
        renderer.sortingOrder = 50;
        entity.AddComponent<Animator>().runtimeAnimatorController = data.EntityAnimator;
        entity.AddComponent<Data>().data = data;
        entity.AddComponent<Health>().health = data.Health;
        entity.AddComponent<CircleCollider2D>();
        Rigidbody2D rigidbody = entity.AddComponent<Rigidbody2D>();
        rigidbody.gravityScale = 0;
        rigidbody.drag = 20;
        rigidbody.freezeRotation = true;
        if ((Player == null && data.Playable) || (Player != null && !Player.activeSelf && data.Playable)) CurrentPlayer(entity, data);
        entity.transform.SetParent(transform, true);
        entity.transform.position = NewPosition();
        Entities.Add(entity);
    }

    private void CurrentPlayer(GameObject entity, Entity data)
    {
        Player?.SetActive(false);
        Destroy(Player?.GetComponent<Camera>());
        Destroy(Player?.GetComponent<Movement>());
        Destroy(Player?.GetComponent<AudioListener>());
        //Destroy(Player?.GetComponent<Rigidbody2D>());
        // Rigidbody2D rigidbody = entity.GetComponent<Rigidbody2D>();
        // rigidbody.gravityScale = 0;
        // rigidbody.drag = 20;
        // rigidbody.freezeRotation = true;
        Camera camera = entity.AddComponent<Camera>();
        camera.transform.SetParent(entity.transform);
        camera.orthographic = true;
        camera.nearClipPlane = -20;
        camera.tag = "MainCamera";
        Movement movement = entity.AddComponent<Movement>();
        movement.Speed = data.Speed;
        movement.an = entity.GetComponent<Animator>();
        movement.rb = entity.GetComponent<Rigidbody2D>();
        movement.tr = entity.AddComponent<TrailRenderer>();
        entity.AddComponent<AudioListener>();
        Player = entity;
    }


    private class EntityTrigger : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            collision.gameObject.GetComponent<Health>().UpdateHealth(gameObject.GetComponent<Data>().data.Damage);
        }
    }

    private class Health : MonoBehaviour
    {
        public float health;
        public void UpdateHealth(float health)
        {
            this.health -= health;
            if (this.health <= 0)
            {
                Instance.OnEntityKilled.Raise(this, (gameObject.GetComponent<Data>().data, transform.position));
                gameObject.SetActive(false);
            }
            else
            {
                Instance.OnEntityHit.Raise(this, (gameObject, health));
            }
        }
    }

    private class Data : MonoBehaviour
    {
        public Entity data;
    }

    IEnumerator MovementLoop()
    {
        while (true)
        {
            if (Player == null) yield return null;
            foreach (var Entity in Entities)
            {
                Entity EntityData = Entity.GetComponent<Data>().data;
                Entity.GetComponent<Rigidbody2D>().AddForce((EntityData.Speed >= EntityData.MaxSpeed ? EntityData.MaxSpeed : EntityData.Speed) * Time.deltaTime * (Player.transform.position - Entity.transform.position).normalized,ForceMode2D.Impulse);
                //Entity.transform.Translate((EntityData.Speed >= EntityData.MaxSpeed ? EntityData.MaxSpeed : EntityData.Speed) * Time.deltaTime * (Player.transform.position - Entity.transform.position).normalized);
            }
            yield return new WaitForSeconds(0.02f);
        }
    }

    private class WaveStatus
    {
        public IEnumerator enumerator;
        public bool hasStarted = false;
        public bool hasEnded = false;
        public bool onEvent = false;
    }

    private void Awake()
    {
        if (Instance != null) Debug.LogError("Only one Entity Manager!");
        else Instance = this;
        foreach (var wave in waves)
        {
            var WaveStatusObject = new WaveStatus();
            IEnumerator Loop()
            {
                List<int> counts = new();
                int MaxEntityCount = wave.entities.Sum(x => x.Amount);
                while (true)
                {
                    int CurrectMaxCount = counts.Sum();
                    for (int i = 0; i < wave.entities.Count; i++)
                    {
                        if (counts.Count - 1 < i) counts.Add(0);
                        if (counts[i] == wave.entities[i].Amount) continue;
                        SpawnHandle(wave.entities[i].entity);
                        counts[i]++;
                    }
                    if (MaxEntityCount == CurrectMaxCount) break;
                    OnGameSecond.responseOnce.AddListener((Component component, object data) =>
                    {
                        WaveStatusObject.onEvent = true;
                    });
                    yield return new WaitUntil(() => { return WaveStatusObject.onEvent; });
                    WaveStatusObject.onEvent = false;
                }
                OnWaveEnded.Raise(this, wave);
            }

            WaveStatusObject.enumerator = Loop();
            foreach (var gameEvent in wave.StartOn)
            {
                var gameEventListener = GetComponents<GameEventListener>()?.FirstOrDefault(x => x.gameEvent == gameEvent);
                if (gameEventListener != null)
                {
                    gameEventListener.responseOnce.AddListener((Component component, object data) =>
                    {
                        if (!WaveStatusObject.hasStarted && !WaveStatusObject.hasEnded)
                        {
                            StartCoroutine(WaveStatusObject.enumerator);
                            WaveStatusObject.hasStarted = true;
                            OnWaveStarted.Raise(this, wave);
                        }
                    });
                    Debug.Log($"Add event {gameEvent.name} and added {wave.name}");
                }
                else
                {
                    gameEventListener = gameObject.AddComponent<GameEventListener>();
                    gameEventListener.gameEvent = gameEvent;
                    gameEventListener.responseOnce.AddListener((Component component, object data) =>
                    {
                        if (!WaveStatusObject.hasStarted && !WaveStatusObject.hasEnded)
                        {
                            StartCoroutine(WaveStatusObject.enumerator);
                            WaveStatusObject.hasStarted = true;
                            OnWaveStarted.Raise(this, wave);
                        }
                    });
                    Debug.Log($"Made event {gameEvent.name} and added {wave.name}");
                }
            }
            foreach (var gameEvent in wave.EndOn)
            {
                var gameEventListener = GetComponents<GameEventListener>()?.FirstOrDefault(x => x.gameEvent == gameEvent);
                if (gameEventListener != null)
                {
                    gameEventListener.responseOnce.AddListener((Component component, object data) =>
                    {
                        if (!WaveStatusObject.hasEnded)
                        {
                            StopCoroutine(WaveStatusObject.enumerator);
                            WaveStatusObject.hasEnded = true;
                            OnWaveEnded.Raise(this, wave);
                        }

                    });
                }
                else
                {
                    gameEventListener = gameObject.AddComponent<GameEventListener>();
                    gameEventListener.gameEvent = gameEvent;
                    gameEventListener.responseOnce.AddListener((Component component, object data) =>
                    {
                        if (!WaveStatusObject.hasEnded)
                        {
                            StopCoroutine(WaveStatusObject.enumerator);
                            WaveStatusObject.hasEnded = true;
                            OnWaveEnded.Raise(this, wave);
                        }
                    });
                }
            }
        }
        StartCoroutine(nameof(MovementLoop));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    public float damage = 10f;
    public ParticleSystem particleSystem;

    List<ParticleCollisionEvent> colEvents = new List<ParticleCollisionEvent>();
    private void OnParticleCollision(GameObject other)
    {
        int events = particleSystem.GetCollisionEvents(other, colEvents);
        
        
        if(other.TryGetComponent(out Enemy en))
        {
            Debug.Log("Hit");
            en.TakeDamage(damage);
        }
    }
    

}

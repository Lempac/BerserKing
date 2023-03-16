using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    public float damage = 10f;
    public ParticleSystem particleSystem;
    int events;

    List<ParticleCollisionEvent> colEvents = new List<ParticleCollisionEvent>();

    private void OnParticleCollision(GameObject other)
    {
        events = particleSystem.GetCollisionEvents(other, colEvents);

        
        if(other.TryGetComponent(out Enemy en))
        {
            
            en.TakeDamage(damage);
        }
    }
    

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ninja_Particle : MonoBehaviour
{
    public float damage = 10f;
    public ParticleSystem particleSystem;
    int events;
    List<ParticleCollisionEvent> colEvents = new List<ParticleCollisionEvent>();

    private void OnParticleCollision(GameObject other)
    {
        events = particleSystem.GetCollisionEvents(other, colEvents);

        //if (other.TryGetComponent(out Enemy en))
        //{
        //    en.TakeDamage(damage);
        //}
    }

    public void FlipYRotation()
    {
        if (particleSystem != null)
        {
            Vector3 rotation = particleSystem.transform.localRotation.eulerAngles;
            rotation.y *= -1f;
            particleSystem.transform.localRotation = Quaternion.Euler(rotation);
        }
    }
}


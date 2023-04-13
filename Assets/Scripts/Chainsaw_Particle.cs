using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chainsaw_Particle : MonoBehaviour
{
    public float damage = 10f;
    public float hitCooldown = 0.5f; // Cooldown time between each hit
    private float timeSinceLastHit; // Time elapsed since last hit

    private void Update()
    {
        // Update time elapsed since last hit
        timeSinceLastHit += Time.deltaTime;
    }

    private void OnParticleCollision(GameObject other)
    {
        // Check if hit cooldown has passed
        if (timeSinceLastHit >= hitCooldown)
        {
            // Deal damage to enemies
            if (other.TryGetComponent(out Enemy en))
            {
                en.TakeDamage(damage);
            }

            // Reset time elapsed since last hit
            timeSinceLastHit = 0f;
        }
    }

    
}
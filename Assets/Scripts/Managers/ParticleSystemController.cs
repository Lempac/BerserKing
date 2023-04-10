using UnityEngine;

public class ParticleSystemController : MonoBehaviour
{
    public GameObject followerObject; // The object that the particle system will follow

    private ParticleSystem particleSystem; // The Unity ParticleSystem component
    private ParticleSystem.Particle[] particles; // Array to hold particles

    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        particles = new ParticleSystem.Particle[1];

        // Emit one particle at the initial position of the followerObject
        ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();
        emitParams.position = followerObject.transform.position;
        emitParams.velocity = Vector3.zero; // Set velocity to zero to prevent movement
        particleSystem.Emit(emitParams, 1);

        // Set the particle system to not loop
        particleSystem.loop = false;
    }

    void Update()
    {
        // Set the particle position to the current followerObject position
        int currentParticleCount = particleSystem.GetParticles(particles);
        particles[0].position = followerObject.transform.position;
        particleSystem.SetParticles(particles, currentParticleCount);
    }
}
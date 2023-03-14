using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateParticle : MonoBehaviour
{
    public ParticleSystem particleSystemToTrack;

    void LateUpdate()
    {
        particleSystemToTrack.transform.position = transform.position;
    }
}

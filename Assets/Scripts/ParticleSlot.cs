using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSlot
{
    private Particle _particle;

    public void AddParticle(Particle particle)
    {
        _particle = particle;
    }

    public bool ContainsParticle()
    {
        return _particle != null;
    }
}

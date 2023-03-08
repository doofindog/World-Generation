using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle
{
    private Vector2Int m_position;
    private ParticleData m_particleData;

    public void Init(Vector2Int position)
    {
        m_position = position;
    }

    public void AddParticleData(ParticleData particle)
    {
        m_particleData = particle;
    }

    public void RemoveParticleData()
    {
        m_particleData = null;
    }

    public bool ContainsData()
    {
        return m_particleData != null;
    }

    public ParticleMovement[] GetMovements()
    {
        return m_particleData.movements;
    }

    public Vector2Int GetPosition()
    {
        return m_position;
    }

    public ParticleData GetParticleData()
    {
        return m_particleData;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle
{
    private Vector2Int m_position;
    private ParticleType m_type;
    private bool m_updated;

    public void Init(Vector2Int position,ParticleType type = ParticleType.Air)
    {
        m_position = position;
        m_type = type;
    }

    public void AddParticle(ParticleType type)
    {
        m_type = type;
    }

    public void RemoveParticleData()
    {
        m_type = ParticleType.Air;
    }

    public ParticleMovement[] GetMovements()
    {
        return GetParticleData().movements;
    }

    public Vector2Int GetPosition()
    {
        return m_position;
    }
    

    public ParticleType GetParticleType()
    {
        return m_type;
    }

    public ParticleData GetParticleData()
    {
        if (m_type == ParticleType.Air)
        {
            return null;
        }
        
        return ParticleManager.GetParticleData(m_type);
    }

    public bool HasUpdated()
    {
        return m_updated;
    }

    public void SetUpdated(bool value)
    {
        m_updated = value;
    }

    public WorldChunk GetChunk()
    {
        int chunkPositionX = m_position.x / WorldManager.instance.chunkSize.x;
        int chunkPositionY = m_position.y / WorldManager.instance.chunkSize.y;

        return WorldManager.instance.GetChunk(chunkPositionX, chunkPositionY);
    }
}

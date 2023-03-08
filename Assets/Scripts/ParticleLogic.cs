using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleLogic : MonoBehaviour
{
    private WorldChunk m_worldChunk;
    
    public void Init(WorldChunk chunk)
    {
        m_worldChunk = chunk;
    }

    public void UpdateParticle(Particle particle)
    {
        MoveParticle(particle);
    }

    public void MoveParticle(Particle particle)
    {
        ParticleMovement[] movements = particle.GetMovements();
        bool movedCheck = false;
        foreach (ParticleMovement movement in movements)
        {
            switch (movement.moveDir)
            {
                case ParticleMovement.MoveDirection.Down:
                {
                    movedCheck = TryMove(particle, new Vector2Int(0, -1), movement.distance);
                    break;
                }
                case ParticleMovement.MoveDirection.DownLeft:
                {
                    movedCheck = TryMove(particle, new Vector2Int(-1, -1), movement.distance);
                    break;
                }
                case ParticleMovement.MoveDirection.DownRight:
                {
                    movedCheck = TryMove(particle, new Vector2Int(1, -1), movement.distance);
                    break;
                }
                case ParticleMovement.MoveDirection.RandomDownDiagonal:
                {
                    int horizontalDirection = Random.Range(-1, 2);
                    movedCheck = TryMove(particle, new Vector2Int(horizontalDirection, -1), movement.distance);
                    break;
                }
            }

            if (movedCheck)
            {
                break;
            }
        }
    }

    private bool TryMove(Particle particle, Vector2Int dir, float distance)
    {
        if (CanMoveInDirection(particle, dir) == false)
        {
            return false;
        }

        MoveParticleInDirection(particle, dir, distance);
        return true;
    }
    

    private bool CanMoveInDirection(Particle particle, Vector2Int dir)
    {
        Vector2Int neighbourPos = particle.GetPosition() + dir;
        if (!m_worldChunk.CheckPositionBounds(neighbourPos.x, neighbourPos.y)) return false;
        if (m_worldChunk.ContainsParticle(neighbourPos.x, neighbourPos.y)) return false;

        return true;
    }

    private void MoveParticleInDirection(Particle particle, Vector2Int dir, float distance)
    {
        Vector2Int neighbourPos = particle.GetPosition() + dir;
        Particle neighbourParticle = m_worldChunk.GetParticle(neighbourPos.x, neighbourPos.y);
        neighbourParticle.AddParticleData(particle.GetParticleData());
        particle.RemoveParticleData();

        m_worldChunk.DrawPixel(neighbourPos, neighbourParticle.GetParticleData().colour);
        m_worldChunk.DrawPixel(particle.GetPosition(), Color.gray);
    }
}

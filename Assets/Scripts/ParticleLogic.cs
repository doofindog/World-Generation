using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        if (particle.GetParticleType() == ParticleType.Empty || particle.HasUpdated())
        {
            return;
        }
        
        MoveParticle(particle);
    }

    public void MoveParticle(Particle particle)
    {
        ParticleMovement[] movements = particle.GetMovements();
        if (movements == null)
        {
            return;
        }
        
        bool continueCheck = false;
        foreach (ParticleMovement movement in movements)
        {
            switch (movement.moveDir)
            {
                case ParticleMovement.MoveDirection.Down:
                {
                    continueCheck = TryMove(particle, new Vector2Int(0, -1), movement.distance);
                    break;
                }
                case ParticleMovement.MoveDirection.DownLeft:
                {
                    continueCheck = TryMove(particle, new Vector2Int(-1, -1), movement.distance);
                    break;
                }
                case ParticleMovement.MoveDirection.DownRight:
                {
                    continueCheck = TryMove(particle, new Vector2Int(1, -1), movement.distance);
                    break;
                }
                case ParticleMovement.MoveDirection.RandomDownDiagonal:
                {
                    int horizontalDirection = Random.Range(-1, 2);
                    continueCheck = TryMove(particle, new Vector2Int(horizontalDirection, -1), movement.distance);
                    break;
                }
                case ParticleMovement.MoveDirection.RandomHorizontal:
                {
                    int horizontalDirection = Random.Range(-1, 2);
                    continueCheck = TryMove(particle, new Vector2Int(horizontalDirection, 0), movement.distance);
                    break;
                }
                case ParticleMovement.MoveDirection.RandomUpDiagonal:
                {
                    int horizontalDirection = Random.Range(-1, 2);
                    continueCheck = TryMove(particle, new Vector2Int(horizontalDirection, 1), movement.distance);
                    break;
                }
                case ParticleMovement.MoveDirection.Left:
                {
                    continueCheck = TryMove(particle, new Vector2Int(-1, 0), movement.distance);
                    break;
                }
                case ParticleMovement.MoveDirection.Right:
                {
                    continueCheck = TryMove(particle, new Vector2Int(1, 0), movement.distance);
                    break;
                }
                case ParticleMovement.MoveDirection.Up:
                {
                    continueCheck = TryMove(particle, new Vector2Int(0, 1), movement.distance);
                    break;
                }
            }

            if (continueCheck)
            {
                break;
            }
        }
    }

    private bool TryMove(Particle particle, Vector2Int dir, int distance)
    {
        if (CheckResistanceInDirection(particle, dir))
        {
            SwapParticleInDirection(particle, dir);
            return true;
        }
        
        bool particleMoved = false;
        for (int i = 1; i <= distance; i++)
        {
            if (CheckMoveInDirection(particle, dir))
            {
                particle = MoveParticleInDirection(particle, dir);
                particleMoved = true;
            }
            else
            {
                break;
            }
        }

        if (particleMoved)
        {
            particle.SetUpdated(true);
        }
        return particleMoved;
    }

    #region Movement Logic

    private bool CheckMoveInDirection(Particle particle, Vector2Int dir)
    {
        Vector2Int neighbourPos = particle.GetPosition() + (dir);
        //WorldChunk chunk = WorldManager.instance.GetChunk(neighbourPos.x / 32, neighbourPos.y / 32);
        if (!m_worldChunk.CheckPositionBounds(neighbourPos.x, neighbourPos.y))
        {
            //Check if In neighbours chunk
            return false;
        }

        if (m_worldChunk.ContainsParticle(neighbourPos.x, neighbourPos.y)) return false;

        return true;
    }
    
    private Particle MoveParticleInDirection(Particle particle, Vector2Int dir)
    {
        Vector2Int neighbourPos = particle.GetPosition() + dir;
        Particle neighbourParticle = m_worldChunk.GetParticleAtIndex(neighbourPos.x, neighbourPos.y);
        
        neighbourParticle.AddParticle(particle.GetParticleType());
        particle.RemoveParticleData();

        return neighbourParticle;
    }

    #endregion


    #region Resistance Check

    private bool CheckResistanceInDirection(Particle particle, Vector2Int dir)
    {
        Vector2Int neighbourPos = particle.GetPosition() + (dir) ;
        if (!m_worldChunk.CheckPositionBounds(neighbourPos.x, neighbourPos.y))
        {
            return false;
            
        }

        if (m_worldChunk.ContainsParticle(neighbourPos.x, neighbourPos.y) &&
            m_worldChunk.GetParticleAtIndex(neighbourPos.x, neighbourPos.y).GetParticleType() != particle.GetParticleType() &&
            m_worldChunk.GetParticleAtIndex(neighbourPos.x, neighbourPos.y).GetParticleType() != ParticleType.Empty
            )
        {
            Particle neighbourParticle = m_worldChunk.GetParticleAtIndex(neighbourPos.x, neighbourPos.y);
            if (particle.GetParticleData().resistance > neighbourParticle.GetParticleData().resistance)
            {
                float chance = Random.Range(0.0f, 1.0f);
                return chance > 0.3f;
            }
        }

        return false;
    }

    private void SwapParticleInDirection(Particle particle,Vector2Int dir)
    {
        Vector2Int neighbourPos = particle.GetPosition() + dir;
        Particle neighbourParticle = m_worldChunk.GetParticleAtIndex(neighbourPos.x, neighbourPos.y);
        
        ParticleType particleType = neighbourParticle.GetParticleType();
        neighbourParticle.AddParticle(particle.GetParticleType());
        particle.AddParticle(particleType);
    }

    #endregion
}

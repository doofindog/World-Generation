using System;
using System.Collections;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class ParticleLogic : MonoBehaviour
{
    private ParticleType m_currentType;

    public void Init()
    {
        m_currentType = ParticleType.Sand;
        StartCoroutine(UpdateChunks());
    }

    public void Update()
    {
        KeyCode[] inputKeys =
        {
            KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5,
            KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9
        };

        for (int i = 0; i < inputKeys.Length; i++)
        {
            if (Input.GetKeyDown(inputKeys[i]))
            {
                ParticleData data = ParticleManager.GetParticleAtIndex(i);
                m_currentType = data.particleType;
                Debug.Log(m_currentType);
            }
        }

        if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(1))
        {
            HandleOnMouseDown();
        }
    }

    private IEnumerator UpdateChunks()
    {
        while (true)
        {
            foreach (WorldChunk chunk in WorldManager.instance.chunks)
            {
                for (int i = 0; i < chunk.particles.Length; i++)
                {
                    int x = i % WorldManager.instance.chunkSize.x;
                    int y = i / WorldManager.instance.chunkSize.x;
            
                    if (chunk.particles[x, y].GetParticleType() == ParticleType.Empty)
                    {
                        continue;
                    }
                    
                    UpdateParticle(chunk.particles[x, y]);
                }
            }
            
            foreach (WorldChunk chunk in WorldManager.instance.chunks)
            {
                chunk.UpdateTexture();
            }
            yield return new WaitForSeconds(0.0125f);
        }
    }

    private void HandleOnMouseDown()
    {
        if (Camera.main == null)
        {
            return;
        }
        
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int pixelPos = GetWorldPos(mouseWorldPosition);

        if (!CheckPositionBounds(pixelPos.x, pixelPos.y))
        {
            return;
        }
        
        AddParticle(pixelPos, ParticleType.Sand);
    }

    private void AddParticle(Vector2Int worldPosition, ParticleType type)
    {
        int pixelPositionX = worldPosition.x % WorldManager.instance.chunkSize.x;
        int pixelPositionY = worldPosition.y % WorldManager.instance.chunkSize.y;
        
        WorldChunk chunk = WorldManager.instance.GetChunkFromWorldPosition(worldPosition.x, worldPosition.y);
        chunk.AddParticle(m_currentType, new Vector2Int(pixelPositionX, pixelPositionY));
    }

    private Vector2Int GetWorldPos(Vector2 pos)
    {
        int maxIndexX = (WorldManager.instance.worldSize.x / WorldManager.instance.chunkSize.x) - 1; // minus 1 because the index starts as one
        int maxIndexY = (WorldManager.instance.worldSize.y / WorldManager.instance.chunkSize.y) - 1;
        WorldChunk minChunk = WorldManager.instance.GetChunk(0, 0);
        WorldChunk maxChunk = WorldManager.instance.GetChunk(maxIndexX, maxIndexY);

        Renderer minSprite = minChunk.GetComponent<Renderer>();  // to get the bounds in world space;
        Renderer maxSprite = maxChunk.GetComponent<Renderer>(); // to get the bounds in world space;
        float xMin = minSprite.bounds.min.x;
        float yMin = minSprite.bounds.min.y;
        float xMax = maxSprite.bounds.max.x;
        float yMax = maxSprite.bounds.max.y;
        
        float xOldRange = xMax - xMin;
        float yOldRange = yMax - yMin;
        float xNewRange = WorldManager.instance.worldSize.x;
        float yNewRange = WorldManager.instance.worldSize.y;

        int xPixelPos = (int) ((pos.x - xMin) * xNewRange / xOldRange);
        int yPixelPos = (int) ((pos.y - yMin) * yNewRange / yOldRange);

        return new Vector2Int(xPixelPos, yPixelPos);
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
        if (!CheckPositionBounds(neighbourPos.x, neighbourPos.y))
        {
            return false;
        }
        
        if (ContainsParticle(neighbourPos.x, neighbourPos.y))
        {
            return false;
        }

        return true;
    }
    
    private Particle MoveParticleInDirection(Particle particle, Vector2Int dir)
    {
        Vector2Int neighbourPos = particle.GetPosition() + dir;
        WorldChunk chunk = WorldManager.instance.GetChunkFromWorldPosition(neighbourPos.x, neighbourPos.y);
        
        int pixelPositionX = neighbourPos.x % WorldManager.instance.chunkSize.x;
        int pixelPositionY = neighbourPos.y % WorldManager.instance.chunkSize.y;
        Particle neighbourParticle = chunk.GetParticleAtIndex(pixelPositionX, pixelPositionY);
        
        neighbourParticle.AddParticle(particle.GetParticleType());
        particle.RemoveParticleData();

        return neighbourParticle;
    }

    #endregion


    #region Resistance Check

    private bool ContainsParticle(int x, int y)
    {
        try
        {
            int chunkPositionX = x / WorldManager.instance.chunkSize.x;
            int chunkPositionY = y / WorldManager.instance.chunkSize.y;
            WorldChunk chunk = WorldManager.instance.GetChunk(chunkPositionX, chunkPositionY);

            int pixelPositionX = x % WorldManager.instance.chunkSize.x;
            int pixelPositionY = y % WorldManager.instance.chunkSize.y;
            return chunk.ContainsParticle(pixelPositionX, pixelPositionY);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

        return false;
    }
    

    private bool CheckResistanceInDirection(Particle particle, Vector2Int dir)
    {
        // Vector2Int neighbourPos = particle.GetPosition() + (dir) ;
        // if (!m_worldChunk.CheckPositionBounds(neighbourPos.x, neighbourPos.y))
        // {
        //     return false;
        //     
        // }
        //
        // if (m_worldChunk.ContainsParticle(neighbourPos.x, neighbourPos.y) &&
        //     m_worldChunk.GetParticleAtIndex(neighbourPos.x, neighbourPos.y).GetParticleType() != particle.GetParticleType() &&
        //     m_worldChunk.GetParticleAtIndex(neighbourPos.x, neighbourPos.y).GetParticleType() != ParticleType.Empty
        //     )
        // {
        //     Particle neighbourParticle = m_worldChunk.GetParticleAtIndex(neighbourPos.x, neighbourPos.y);
        //     if (particle.GetParticleData().resistance > neighbourParticle.GetParticleData().resistance)
        //     {
        //         float chance = Random.Range(0.0f, 1.0f);
        //         return chance > 0.3f;
        //     }
        // }

        return false;
    }

    private void SwapParticleInDirection(Particle particle,Vector2Int dir)
    {
        // Vector2Int neighbourPos = particle.GetPosition() + dir;
        // //Particle neighbourParticle = m_worldChunk.GetParticleAtIndex(neighbourPos.x, neighbourPos.y);
        //
        // ParticleType particleType = neighbourParticle.GetParticleType();
        // neighbourParticle.AddParticle(particle.GetParticleType());
        // particle.AddParticle(particleType);
    }

    private bool CheckPositionBounds(int x, int y)
    {
        if (x < 0 || x >= WorldManager.instance.worldSize.x || y < 0 || y >= WorldManager.instance.worldSize.y)
        {
            return false;
        }

        return true;
    }

    #endregion
}

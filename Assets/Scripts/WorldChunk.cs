using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WorldChunk : MonoBehaviour
{
    private Vector2Int m_chunkPosition;
    private Color m_defaultColor;
    private Texture2D m_worldTexture;
    public Particle[,] particles;
    private Color[] m_chuckColour;
    private ParticleLogic m_logic;
    private WorldChunk[] m_neighbourChunks;
    private ParticleType m_currentType;
    
    public Sprite sprite;
    
    public void Init(Vector2Int position,Vector2Int worldSize)
    {
        sprite = GetComponent<SpriteRenderer>().sprite;

        m_chunkPosition = position;
        m_worldTexture = sprite.texture;
        particles = new Particle[worldSize.x, worldSize.y];

        for(int y = 0; y < worldSize.y; y++)
        {
            for (int x = 0; x < worldSize.x; x++)
            {
                particles[x, y] = new Particle();
                particles[x,y].Init(new Vector2Int(x * m_chunkPosition.x, y * m_chunkPosition.y));
                DrawPixel(new Vector2Int(x,y), Color.gray);
            }
        }

        m_chuckColour = new Color[particles.Length];
        //StartCoroutine(UpdateLogic());
    }

    public Particle GetParticleAtIndex(int x, int y)
    {
        return particles[x, y];
    }

    public bool ContainsParticle(int x, int y)
    {
        Particle particle = GetParticleAtIndex(x, y);
        return particle != null && particle.GetParticleType() != ParticleType.Empty;
    }

    public Particle AddParticle(ParticleType type, Vector2Int particlePos)
    {
        if (ContainsParticle(particlePos.x, particlePos.y))
        {
            return null;
        }
        
        particles[particlePos.x, particlePos.y].AddParticle(type);
        
        return particles[particlePos.x, particlePos.y];
    }

    public void DrawPixel(Vector2Int pixelPosition, Color color)
    {
        m_worldTexture.SetPixel(pixelPosition.x, pixelPosition.y, color);
        m_worldTexture.Apply();
    }

    public void UpdateParticle(ParticleLogic logic)
    {
        if (m_logic == null)
        {
            m_logic = logic;
        }


        // ReSharper disable once IteratorNeverReturns
    }

    public void UpdateTexture()
    {
        for (int i = 0; i < particles.Length; i++)
        {
            int x = i % WorldManager.instance.chunkSize.x;
            int y = i / WorldManager.instance.chunkSize.y;

            if (particles[x, y].GetParticleType() != ParticleType.Empty)
            {
                m_chuckColour[i] = particles[x, y].GetParticleData().colour.Evaluate(Random.Range(0f, 1.0f));
            }
            else
            {
                m_chuckColour[i]= Color.gray;
            }

            if (particles[x, y].HasUpdated())
            {
                particles[x,y].SetUpdated(false);
            }
        }
        m_worldTexture.SetPixels(m_chuckColour);
        m_worldTexture.Apply();
    }


    public bool CheckPositionBounds(int x, int y)
    {
        if (x < 0 || x >= particles.GetLength(0) ||
            y < 0 || y >= particles.GetLength(1))
        {
            return false;
        }

        return true;
    }

    public WorldChunk GetNeighbour(int x, int y)
    {
        int chunkPosX = m_chunkPosition.x;
        int chunkPosY = m_chunkPosition.y;
        
        if (x > WorldManager.instance.chunkSize.x) {
            chunkPosX += 1;
        }
        else if (x < WorldManager.instance.chunkSize.x) {
            chunkPosX += -1;
        }
        
        if (y > WorldManager.instance.chunkSize.y) {
            chunkPosY += 1;
        }
        else if (y < WorldManager.instance.chunkSize.y) {
            chunkPosY += -1;
        }

        if (m_chunkPosition.x != chunkPosX && m_chunkPosition.y != chunkPosY)
        {
            return WorldManager.instance.GetChunk(chunkPosX, chunkPosY);
        }

        return null;
    }

    public Vector2Int GetPosition()
    {
        return m_chunkPosition;
    }
}

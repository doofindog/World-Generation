using UnityEngine;

public class WorldChunk : MonoBehaviour
{
    //Private Variables
    private Vector2Int m_chunkPosition;
    private Color m_defaultColor;
    private Texture2D m_worldTexture;
    private Particle[,] m_particles;
    private Color[] m_chuckColour;         
    private WorldChunk[] m_neighbourChunks;

    //Public Variables
    public Sprite sprite;
    public bool chunkActive;
    public bool isActiveNextStep;
    
    public void Init(Vector2Int chunkPosition,Vector2Int chunkSize)
    {
        sprite = GetComponent<SpriteRenderer>().sprite;

        m_chunkPosition = chunkPosition;
        m_worldTexture = sprite.texture;
        m_particles = new Particle[chunkSize.x, chunkSize.y];
        
        for(int y = 0; y < chunkSize.y; y++)
        {
            int yIndex = y + m_chunkPosition.y * chunkSize.y;
            for (int x = 0; x < chunkSize.y; x++)
            {
                int xIndex = x + m_chunkPosition.x * chunkSize.x;
                m_particles[x, y] = new Particle();
                m_particles[x,y].Init(new Vector2Int(xIndex, yIndex));
                DrawPixel(new Vector2Int(x,y), Color.white);
            } 
        }

        m_chuckColour = new Color[m_particles.Length];
    }

    public Particle GetParticleAtIndex(int x, int y)
    {
        return m_particles[x, y];
    }

    public bool ContainsParticle(int x, int y)
    {
        Particle particle = GetParticleAtIndex(x, y);
        return particle != null && particle.GetParticleType() != ParticleType.Air;
    }

    public Particle AddParticle(ParticleType type, Vector2Int particlePos)
    {
        if (ContainsParticle(particlePos.x, particlePos.y))
        {
            return null;
        }
        
        m_particles[particlePos.x, particlePos.y].AddParticle(type);
        chunkActive = true;
        return m_particles[particlePos.x, particlePos.y];
    }

    public void DrawPixel(Vector2Int pixelPosition, Color color)
    {
        m_worldTexture.SetPixel(pixelPosition.x, pixelPosition.y, color);
        m_worldTexture.Apply();
    }

    public void UpdateTexture()
    {
        for (int i = 0; i < m_particles.Length; i++)
        {
            int x = i % WorldManager.instance.chunkSize.x;
            int y = i / WorldManager.instance.chunkSize.y;

            if (m_particles[x, y].GetParticleType() != ParticleType.Air)
            {
                m_chuckColour[i] = m_particles[x, y].GetParticleData().colour;
            }
            else
            {
                m_chuckColour[i]= Color.white;
            }

            if (m_particles[x, y].HasUpdated())
            {
                m_particles[x,y].SetUpdated(false);
            }
        }
        m_worldTexture.SetPixels(m_chuckColour);
        m_worldTexture.Apply();
    }

    public Particle[,] GetParticles()
    {
        return m_particles;
    }
}

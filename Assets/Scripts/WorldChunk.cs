using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WorldChunk : MonoBehaviour
{
    private Vector2Int m_chunkPosition;
    private Sprite m_sprite;
    private Color m_defaultColor;
    private Texture2D m_worldTexture;
    private Particle[,] m_particles;
    private Color[] m_chuckColour;
    private ParticleLogic m_logic;
    private WorldChunk[] m_neighbourChunks;
    private ParticleType m_currentType;
    
    public void Init(Vector2Int position,Vector2Int worldSize)
    {
        m_sprite = GetComponent<SpriteRenderer>().sprite;
        m_logic = GetComponent<ParticleLogic>();
        
        m_chunkPosition = position;
        m_worldTexture = m_sprite.texture;
        m_particles = new Particle[worldSize.x, worldSize.y];
        m_currentType = ParticleType.Sand;
        
        for(int y = 0; y < worldSize.y; y++)
        {
            for (int x = 0; x < worldSize.x; x++)
            {
                m_particles[x, y] = new Particle();
                m_particles[x,y].Init(new Vector2Int(x * m_chunkPosition.x, y * m_chunkPosition.y));
                DrawPixel(new Vector2Int(x,y), Color.gray);
            }
        }

        m_chuckColour = new Color[m_particles.Length];
        StartCoroutine(UpdateLogic());
    }

    private void HandleOnMouseDown()
    {
        if (Camera.main == null) { return; }
        
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int pixelPos = GetPixelPos(mouseWorldPosition);
        if (!CheckPositionBounds(pixelPos.x, pixelPos.y))
        {
            return;
        }
        
        Particle slot = GetParticleAtIndex(pixelPos.x, pixelPos.y);
        if (slot.GetParticleType() != ParticleType.Empty)
        {
            return;
        }
        
        Particle particle = AddParticle(type: m_currentType, particlePos: pixelPos, particle: slot);
        DrawPixel(pixelPos, particle.GetParticleData().colour.Evaluate(Random.Range(0f,1f)));
    }

    private Vector2Int GetPixelPos(Vector2 pos)
    {
        Vector2 currentTexPos = transform.position;
        Vector2 currentTexScale = transform.localScale;
        
        float xMin = currentTexPos.x + (m_sprite.bounds.min.x * currentTexScale.x);
        float yMin = currentTexPos.y + (m_sprite.bounds.min.y * currentTexScale.y);
        float xMax = currentTexPos.x + (m_sprite.bounds.max.x * currentTexScale.x);
        float yMax = currentTexPos.y + (m_sprite.bounds.max.y * currentTexScale.y);
        
        float xOldRange = xMax - xMin;
        float yOldRange = yMax - yMin;
        float xNewRange = WorldManager.instance.chunkSize.x;
        float yNewRange = WorldManager.instance.chunkSize.y;

        int xPixelPos = (int) ((pos.x - xMin) * xNewRange / xOldRange);
        int yPixelPos = (int) ((pos.y - yMin) * yNewRange / yOldRange);

        return new Vector2Int(xPixelPos, yPixelPos);
    }

    public Particle GetParticleAtIndex(int x, int y)
    {
        return !CheckPositionBounds(x,y) ? null : m_particles[x, y];
    }

    public bool ContainsParticle(int x, int y)
    {
        Particle particle = GetParticleAtIndex(x, y);
        return particle != null && particle.GetParticleType() != ParticleType.Empty;
    }

    private Particle AddParticle(ParticleType type, Vector2Int particlePos, Particle particle)
    {
        if (ContainsParticle(particlePos.x, particlePos.y))
        {
            return null;
        }
        
        particle.AddParticle(type);
        
        return particle;
    }

    public void DrawPixel(Vector2Int pixelPosition, Color color)
    {
        m_worldTexture.SetPixel(pixelPosition.x, pixelPosition.y, color);
    }

    private IEnumerator UpdateLogic()
    {
        while (true)
        {
            for (int i = 0; i < m_particles.Length; i++)
            {
                int x = i % WorldManager.instance.chunkSize.x;
                int y = i / WorldManager.instance.chunkSize.x;
                if (m_particles[x, y].GetParticleType() == ParticleType.Empty)
                {
                    continue;
                }
                    
                m_logic.UpdateParticle(m_particles[x, y]);
            }
            
            for (int i = 0; i < m_particles.Length; i++)
            {
                int x = i % WorldManager.instance.chunkSize.x;
                int y = i / WorldManager.instance.chunkSize.y;

                if (m_particles[x, y].GetParticleType() != ParticleType.Empty)
                {
                    m_chuckColour[i] = m_particles[x, y].GetParticleData().colour.Evaluate(Random.Range(0f, 1.0f));
                }
                else
                {
                    m_chuckColour[i]= Color.gray;
                }

                if (m_particles[x, y].HasUpdated())
                {
                    m_particles[x,y].SetUpdated(false);
                }
            }
            m_worldTexture.SetPixels(m_chuckColour);
            m_worldTexture.Apply();
            yield return new WaitForSeconds(0.0125f);
        }
        // ReSharper disable once IteratorNeverReturns
    }

    private void Update()
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

    public bool CheckPositionBounds(int x, int y)
    {
        if (x < 0 || x >= m_particles.GetLength(0) ||
            y < 0 || y >= m_particles.GetLength(1))
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

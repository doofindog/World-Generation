using System;
using System.Collections;
using UnityEngine;

public class WorldChunk : MonoBehaviour
{
    private Sprite m_sprite;
    private Color m_defaultColor;
    private Texture2D m_worldTexture;
    private Particle[,] m_particles;
    private ParticleLogic m_logic;

    private ParticleType m_currentType;
    
    public void Init(Vector2Int worldSize)
    {
        m_sprite = GetComponent<SpriteRenderer>().sprite;
        m_logic = GetComponent<ParticleLogic>();
        m_worldTexture = m_sprite.texture;
        m_currentType = ParticleType.Sand;

        m_particles = new Particle[worldSize.x, worldSize.y];
        for(int y = 0; y < worldSize.y; y++)
        {
            for (int x = 0; x < worldSize.x; x++)
            {
                m_particles[x, y] = new Particle();
                m_particles[x,y].Init(new Vector2Int(x, y));
                DrawPixel(new Vector2Int(x,y), Color.gray);
            }
        }

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
        
        Particle slot = GetParticle(pixelPos.x, pixelPos.y);
        if (slot.ContainsData())
        {
            Debug.Log("Can't paint on slot. Contain particle");
            return;
        }
        
        Particle particle = AddParticle(type: m_currentType, particlePos: pixelPos, particle: slot);
        DrawPixel(pixelPos, particle.GetParticleData().colour);
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
        float xNewRange = WorldManager.instance.m_worldSize.x;
        float yNewRange = WorldManager.instance.m_worldSize.y;

        int xPixelPos = (int) ((pos.x - xMin) * xNewRange / xOldRange);
        int yPixelPos = (int) ((pos.y - yMin) * yNewRange / yOldRange);

        return new Vector2Int(xPixelPos, yPixelPos);
    }

    public Particle GetParticle(int x, int y)
    {
        return !CheckPositionBounds(x,y) ? null : m_particles[x, y];
    }

    public bool ContainsParticle(int x, int y)
    {
        Particle particle = GetParticle(x, y);
        return particle != null && particle.ContainsData();
    }

    private Particle AddParticle(ParticleType type, Vector2Int particlePos, Particle particle)
    {
        if (particle.ContainsData())
        {
            return null;
        }

        ParticleData particleData = ParticleManager.GetParticleData(type);
        particle.AddParticleData(particleData);

        return particle;
    }

    public void DrawPixel(Vector2Int pixelPosition, Color color)
    {
        m_worldTexture.SetPixel(pixelPosition.x, pixelPosition.y, color);
        m_worldTexture.Apply();
    }

    private IEnumerator UpdateLogic()
    {
        while (true)
        {
            for (int y = 0; y < m_particles.GetLength(1); y++)
            {
                for (int x = 0; x < m_particles.GetLength(0); x++)
                {
                    if (!m_particles[x, y].ContainsData())
                    {
                        continue;
                    }
                    
                    m_logic.UpdateParticle(m_particles[x, y]);
                }
            }
            
            
            yield return new WaitForSeconds(0.0125f);
        }
        
        // ReSharper disable once IteratorNeverReturns
    }

    private void Update()
    {
        KeyCode[] inputKeys = new KeyCode[]
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
    
    
}

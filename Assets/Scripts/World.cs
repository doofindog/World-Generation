using System.Collections;
using UnityEngine;

public class World : MonoBehaviour
{
    private Sprite m_sprite;
    private Texture2D m_worldTexture;
    private ParticleSlot[,] _particleSlots;
    
    public void Init(Vector2Int worldSize)
    {
        _particleSlots = new ParticleSlot[worldSize.x, worldSize.y];
        
        for(int y = 0; y < worldSize.y; y++)
        {
            for (int x = 0; x < worldSize.x; x++)
            {
                _particleSlots[x, y] = new ParticleSlot();
            }
        }
        
        m_sprite = GetComponent<SpriteRenderer>().sprite;
        m_worldTexture = m_sprite.texture;

        StartCoroutine(UpdateLogic());
    }

    private void HandleOnMouseDown()
    {
        if (Camera.main == null)
        {
           return;
        }
        
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int pixelPos = GetPixelPos(mouseWorldPosition);
        AddParticle(type: ParticleType.Sand, particlePos: pixelPos,slot:);
        DrawPixel(pixelPos);
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

    private void AddParticle(ParticleType type, Vector2Int particlePos, ParticleSlot slot)
    {
        if (slot.ContainsParticle())
        {
            return;
        }

        Particle particle = ParticleManager.CreateParticle(ParticleType.Sand);
        slot.AddParticle(particle);
    }

    private void DrawPixel(Vector2Int pixelPosition)
    {
        m_worldTexture.SetPixel(pixelPosition.x, pixelPosition.y, Color.black);
        m_worldTexture.Apply();
    }

    private IEnumerator UpdateLogic()
    {
        yield return new WaitForSeconds(0.1f);
    }
}

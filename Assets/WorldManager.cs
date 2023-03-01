using System;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    [SerializeField] private Vector2Int m_worldSize;
    [SerializeField] private int m_pixelPerUnit;

    private void Start()
    {
        GenerateEmptyWorld();
    }

    private void GenerateEmptyWorld()
    {
        GameObject worldObj = new GameObject("World");
        
        Texture2D worldTexture = new Texture2D(m_worldSize.x,m_worldSize.y)
        {
            filterMode = FilterMode.Point
        };

        SpriteRenderer spriteRenderer = worldObj.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = Sprite.Create(
            worldTexture,
            new Rect(0, 0, m_worldSize.x, m_worldSize.y),
            Vector2.one * 0.5f,
            m_pixelPerUnit);
    }
}

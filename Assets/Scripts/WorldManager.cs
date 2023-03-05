using System;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public int m_pixelPerUnit;
    public Vector2Int m_worldSize;
    
    public static WorldManager instance;

    private void Awake()
    {
        instance = this;
    }

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

        World world = worldObj.AddComponent<World>();
        world.Init(m_worldSize);
    }
}

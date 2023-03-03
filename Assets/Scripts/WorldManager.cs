using System;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    [SerializeField] public Vector2Int m_worldSize;
    [SerializeField] public int m_pixelPerUnit;

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

        worldObj.AddComponent<World>();
    }
}

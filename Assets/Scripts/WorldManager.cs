using System;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public int pixelPerUnit;
    public Vector2Int worldSize;
    public Vector2Int chunkSize;
    
    public static WorldManager instance;
    
    private WorldChunk[,] chunks;

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
        int xChunks = worldSize.x / chunkSize.x;
        int yChunks = worldSize.y / chunkSize.y;

        chunks = new WorldChunk[xChunks, yChunks];

        for (int y = 0; y < chunks.GetLength(1); y++)
        {
            for (int x = 0; x < chunks.GetLength(0); x++)
            {
                GameObject worldObj = new GameObject("World");
                Texture2D worldTexture = new Texture2D(chunkSize.x,chunkSize.y)
                {
                    filterMode = FilterMode.Point
                };

                SpriteRenderer spriteRenderer = worldObj.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = Sprite.Create(
                    worldTexture,
                    new Rect(0, 0, chunkSize.x, chunkSize.y),
                    Vector2.one * 0.5f,
                    pixelPerUnit);

                WorldChunk worldChunk = worldObj.AddComponent<WorldChunk>();
                ParticleLogic particleLogic = worldObj.AddComponent<ParticleLogic>();
                
                int positionOffsetX = (chunkSize.x  / pixelPerUnit) * x;
                int positionOffsetY = (chunkSize.y  / pixelPerUnit) * y;
                worldObj.transform.position += new Vector3(positionOffsetX, positionOffsetY);

                worldChunk.Init(new Vector2Int(x + 1, y + 1), chunkSize);
                particleLogic.Init(worldChunk);
            }
        }
    }

    public WorldChunk GetChunk(int x, int y)
    {
        if((x > 0 && x < chunks.GetLength(0)) && (y > 0 && y < chunks.GetLength(1)))
        {
            return chunks[x, y];
        }

        return null;
    }

    public void Update()
    {
        
    }
}

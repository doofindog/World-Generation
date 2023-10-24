using UnityEngine;


/// <summary>
/// Manager class used to generate worlds
/// </summary>
public class WorldManager : MonoBehaviour
{
    public static WorldManager instance;
    
    [Tooltip("Pixel size of a particle")]
    public int pixelPerUnit;
    [Tooltip("Size of the World. should be divisible by pixel per unit")]
    public Vector2Int worldSize;
    [Tooltip("A small section of the  world. should be divisible by pixel per unit")]
    public Vector2Int chunkSize;
    
    
    private WorldChunk[,] m_chunks;

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
        int screenHeight, screenWidth = 0;
        
        if (Camera.main == null)
        {
            Debug.LogError("No Main Camera Found, Please set main Camera");
            return;
        }

        float orthographicSize = Camera.main.orthographicSize;
        screenHeight = Mathf.RoundToInt(orthographicSize * 2);
        screenWidth =  Mathf.RoundToInt((orthographicSize * 2) * Screen.width / Screen.height);
        
        Debug.Log($"{screenHeight},{screenWidth}");
        Vector3 startPosition = -(new Vector3(screenWidth, screenHeight, 0) * 0.5f) + (new Vector3(1,1,0) * 0.5f);

        int xChunks = screenWidth / 2 + 1; //we add one to 
        int yChunks = screenHeight / 2 + 1;
        
        m_chunks = new WorldChunk[xChunks, yChunks];
        GameObject world = new GameObject("World")
        {
            transform = { position = Vector3.zero }
        };
        
        for (int y = 0; y < m_chunks.GetLength(1); y++)
        {
            for (int x = 0; x < m_chunks.GetLength(0); x++)
            {
                GameObject worldObj = new GameObject($"WorldChunk({x},{y})")
                {
                    transform =
                    {
                        position = startPosition
                    }
                };
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
                
                int positionOffsetX = (chunkSize.x  / pixelPerUnit) * x;
                int positionOffsetY = (chunkSize.y  / pixelPerUnit) * y;
                worldObj.transform.position += new Vector3(positionOffsetX, positionOffsetY);
                
                worldChunk.Init(new Vector2Int(x, y), chunkSize);
                worldChunk.transform.SetParent(world.transform);
                m_chunks[x, y] = worldChunk;
            }
        }

        ParticleLogic logic = world.AddComponent<ParticleLogic>();
        logic.Init(this);
    }

    public WorldChunk GetChunk(int x, int y)
    {
        if((x >= 0 && x < m_chunks.GetLength(0)) && (y >= 0 && y < m_chunks.GetLength(1)))
        {
            return m_chunks[x, y];
        }

        return null;
    }

    public WorldChunk[,] GetAllChunks()
    {
        return m_chunks;
    }

    public WorldChunk GetChunkFromParticlePosition(int x, int y)
    {
        int chunkPositionX = x / WorldManager.instance.chunkSize.x;
        int chunkPositionY = y / WorldManager.instance.chunkSize.y;

        return GetChunk(chunkPositionX, chunkPositionY);
    }

    public Particle GetParticle(int x, int y)
    {
        WorldChunk chunk = WorldManager.instance.GetChunkFromParticlePosition(x, y);
        
        int pixelPositionX = x % WorldManager.instance.chunkSize.x;
        int pixelPositionY = y % WorldManager.instance.chunkSize.y;
        return chunk.GetParticleAtIndex(pixelPositionX, pixelPositionY);
    }

    public bool ContainsParticle(int x, int y)
    {
        WorldChunk chunk = WorldManager.instance.GetChunkFromParticlePosition(x, y);
        
        int pixelPositionX = x % WorldManager.instance.chunkSize.x;
        int pixelPositionY = y % WorldManager.instance.chunkSize.y;
            
        return chunk.ContainsParticle(pixelPositionX, pixelPositionY);
    }
}

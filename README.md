# World-Generation

A celliula Automata enviroment where you can play around with different particles. Particles Added as of now include
- Sand
- water
- gas
- wood


## Particle Data

This script defines a `ParticleData` class that is a subclass of `ScriptableObject` and represents data for a particle in a particle simulation.

-   `ParticleType`: This is an enum that defines different types of particles that can exist in the simulation.
-   `Color`: Color of the Particle
-   `movements`: This is an array of `ParticleMovement` objects that define the movement behaviors that are applied to this particle.
-   `resistance`: This is a float value that represents the amount of resistance that this particle experiences when it moves through other particles.
-   `dispersalRate`: This is an integer value that determines how many neighboring cells this particle will spread to when it moves.

To use this script, you would create a new `ParticleData` asset in your Unity project by right-clicking in the Project window and selecting "World Generation/Create Particle". You can then set the various properties of the particle, including its type, color, movement behaviors, and resistance/dispersal properties. This asset can then be used by other scripts to generate and manipulate particles in the simulation.

---
## Particle Movement

This script defines a `ParticleMovement` class that represents a single movement behavior that can be applied to a particle in a particle simulation.

### variables

-   `enum MoveDirection`: This is an enum that defines the different directions that a particle can move in.
-   `moveDir`: This is a `MoveDirection` value that specifies the direction in which the particle should move.
-   `distance`: This is an integer value that represents the maximum distance that the particle should move in this direction.
-   `chance`: This is a float value between 0 and 1 that represents the probability that the particle will move in this direction.

To use this script, you would create a new `ParticleMovement` object and set its `moveDir`, `distance`, and `chance` properties to define a specific movement behavior. This object can then be added to the `movements` array of a `ParticleData` object to specify the set of possible movements for that particle. During the particle simulation, the simulation logic would Iterate and select one movement from the `movements` array and apply it to each particle to determine its next position.

---

## Particle Logic

This is a Unity C# script that handles particle logic for a 2D world. The script uses coroutine to update chunks, handles input, and updates particle positions based on predefined movement patterns. Here is a brief summary of the script:

### Functions

-   `Init()` sets the selected particle type to sand and starts the UpdateChunks coroutine.
-  `Update()` :  handles input from the user, such as selecting a different particle type or adding particles to the world.
- `UpdateChunks()` coroutine updates the position of particles in each chunk in the world, and then updates the chunk's texture. The coroutine runs indefinitely with a 0.02 second delay between updates.
-   `HandleOnMouseDown()` :  handles mouse input to add particles to the world.
-   `AddParticle()` :  adds a particle to a specific position in the world and marks the chunk as active.
-   `GetWorldPos()` :  converts a screen position to a world position in the 2D world.
-    `UpdateParticle()` :  updates the position of a single particle based on its movement pattern.
-   `MoveParticle()` :  handles movement for a single particle, based on its movement pattern.

---
## World Chunk

WorldChunk class represents a chunk of the world that contains particles. It holds the particles and updates the associated texture.


### Variables
- `Vector2Int m_chunkPosition` : the position of this chunk in the world
- `Color m_defaultColor` : the default color of the chunk
- `Texture2D m_worldTexture` : the texture that represents the particles in this chunk
- `Particle[,] m_particles` : a 2D array that holds the particles in this chunk
- `Color[] m_chuckColour` : an array of colors for each pixel in the chunk
- `WorldChunk[] m_neighbourChunks` :  an array of neighboring chunks
- `Sprite sprite` : the sprite of this chunk
- `bool chunkActive` : whether there are active particles in this chunk
- `bool isActiveNextStep` : whether this chunk will be active in the next step

### Functions
- `Init(Vector2Int chunkPosition, Vector2Int chunkSize)` :  Initializes this chunk with the given position and size.
- `GetParticleAtIndex(int x, int y)` : Returns the particle at the given position in this chunk.
-  `ContainsParticle(int x, int y)` : Returns true if this chunk contains a particle at the given position.
- `AddParticle(ParticleType type, Vector2Int particlePos)` :  Adds a particle of the given type at the given position in this chunk .Returns the added particle or null if the position already contains a particle.
- `DrawPixel(Vector2Int pixelPosition, Color color)` : Draws a pixel of the given color at the given position in the chunk's texture.
- `UpdateTexture()` : Updates the texture of this chunk with the current state of its particles.
- `GetParticles()` : Returns the 2D array of particles in this chunk.
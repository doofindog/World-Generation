using UnityEngine;

public class ParticleData : ScriptableObject
{
    public ParticleType particleType;
    public Color32 colour;
    public ParticleMoveChecks[] moveChecks;
}

public class SolidData : ParticleData
{
    
}

public class EmptyData : ParticleData
{
    
}

[CreateAssetMenu(fileName = "Sand", menuName="World Generation/Particle/Sand")]
public class SandData : SolidData
{
    
}

public enum ParticleType
{
    Empty,
    Water,
    Sand
}


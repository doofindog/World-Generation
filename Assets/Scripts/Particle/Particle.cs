using UnityEngine;

public abstract class Particle : ScriptableObject
{
    public ParticleType particleType;
    public Color32 colour;
}

public abstract class Solid : Particle
{
    
}

public interface IMovableSolid
{
    
}

public interface IImmovableSolid
{
    
}

[CreateAssetMenu(fileName = "Sand", menuName="World Generation/Particle/Sand")]
public class Sand : Solid, IMovableSolid
{
    
}

public enum ParticleType
{
    Air,
    Water,
    Sand
}


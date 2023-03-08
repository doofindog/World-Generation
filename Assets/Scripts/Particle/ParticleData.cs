using UnityEngine;

[System.Serializable]
public class ParticleData : ScriptableObject
{
    public ParticleType particleType;
    public Color32 colour;
    [SerializeField] public ParticleMovement[] moveChecks;
}

public class SolidData : ParticleData
{
    
}

public class EmptyData : ParticleData
{
    
}

public enum ParticleType
{
    Empty,
    Water,
    Sand
}

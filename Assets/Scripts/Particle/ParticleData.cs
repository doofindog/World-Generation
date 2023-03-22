using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "particle", menuName = "World Generation/Create Particle")]
[System.Serializable]
public class ParticleData : ScriptableObject
{
    [Tooltip("Type of Particle")]
    public ParticleType particleType;
    public Color colour;
    
    [Header("Movement")]
    [FormerlySerializedAs("moveChecks")] [SerializeField] public ParticleMovement[] movements;

    [Header("Passthrough")]
    public float resistance;
    public int dispersalRate;
}

public enum ParticleType
{
    Empty,
    Water,
    Sand,
    Wood,
    Gas
}


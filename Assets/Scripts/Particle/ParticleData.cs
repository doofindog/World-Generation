using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "particle", menuName = "World Generation/Create Particle")]
[System.Serializable]
public class ParticleData : ScriptableObject
{
    public ParticleType particleType;
    public Color32 colour;
    [FormerlySerializedAs("moveChecks")] [SerializeField] public ParticleMovement[] movements;
}

public enum ParticleType
{
    Empty,
    Water,
    Sand,
    Stone
}


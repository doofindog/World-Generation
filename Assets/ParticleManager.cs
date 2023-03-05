using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField] private ParticleData[] _particleDatas;
    private Dictionary<ParticleType, ParticleData> _particleDataDict;

    private static ParticleManager instance;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        instance = this;
        
        foreach (ParticleData data in instance._particleDatas)
        {
            if (instance._particleDataDict.ContainsKey(data.particleType))
            {
                continue;
            }
            
            instance._particleDataDict.Add(data.particleType, data);
        }
    }

    public static Particle CreateParticle(ParticleType type)
    {
        ParticleData data = GetParticleData(type);
        switch (type)
        {
            case ParticleType.Empty:
                return new Empty(data);
            case ParticleType.Sand:
                return new Sand(data);
            default:
                return null;
        }
    }

    private static ParticleData GetParticleData(ParticleType type)
    {
        if (instance._particleDataDict.ContainsKey(type))
        {
            return instance._particleDataDict[type];
        }
        
        return null;
    }
}

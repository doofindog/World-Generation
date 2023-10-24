using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ParticleManager : MonoBehaviour
{
    [FormerlySerializedAs("_particleDatas")] [SerializeField] private ParticleData[] m_particleDatas;
    private Dictionary<ParticleType, ParticleData> _particleDataDict;

    private static ParticleManager instance;

    public void Awake()
    {
        Init();
    }

    private void Init()
    {
        instance = this;

        _particleDataDict = new Dictionary<ParticleType, ParticleData>();
        foreach (ParticleData data in instance.m_particleDatas)
        {
            if (instance._particleDataDict.ContainsKey(data.particleType))
            {
                continue;
            }
            
            instance._particleDataDict.Add(data.particleType, data);
        }
    }

    public static ParticleData GetParticleData(ParticleType type)
    {
        if (instance._particleDataDict.ContainsKey(type))
        {
            return instance._particleDataDict[type];
        }
        
        return null;
    }

    public static ParticleData GetParticleAtIndex(int index)
    {
        return instance.m_particleDatas[index];
    }
}

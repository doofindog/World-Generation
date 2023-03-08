using System;
using UnityEngine;

[System.Serializable]
public class ParticleMovement
{
    public enum MoveDirection
    {
        Up,
        UpRight,
        Right,
        DownRight,
        Down,
        DownLeft,
        Left,
        UpLeft,
    }

    public MoveDirection moveDir;
    public float distance;
    [Range(0,1)] public float chance;

}

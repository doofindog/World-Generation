using UnityEngine;

public abstract class Particle
{
    protected ParticleData data;

    protected Particle(ParticleData data)
    {
        this.data = data;
    }

    protected ParticleMovement[] GetMovements()
    {
        return null;
    }

    protected void GetDensity()
    {
        return;
    }
}

public abstract class Solid : Particle
{
    protected Solid(ParticleData data) : base(data)
    {
    }
}

public abstract class MoveAbleSolid : Solid
{
    protected MoveAbleSolid(ParticleData data) : base(data)
    {
    }
}

public class Sand : Solid 
{
    public Sand(ParticleData data) : base(data)
    {
    }
}

public class Empty : Particle
{
    public Empty(ParticleData data) : base(data)
    {
    }
}
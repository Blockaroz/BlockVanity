using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace BlockVanity.Common.Graphics;

public interface IParticleData
{
    public abstract void OnSpawn(Particle particle);

    public abstract void Update(Particle particle);

    public abstract void Draw(Particle particle, SpriteBatch spriteBatch, Vector2 anchorPosition);
}

public sealed class Particle
{
    public bool active;
    public Vector2 position;
    public Vector2 velocity;
    public float scale;
    public float rotation;
    public IParticleData data;

    public void Draw(SpriteBatch spritebatch, Vector2 anchorPosition) => data?.Draw(this, spritebatch, anchorPosition);

    public void Update() => data?.Update(this);
}

public class ParticleSystem<T> where T : IParticleData
{
    public List<Particle> Particles { get; private set; }

    private int _poolSize;

    public ParticleSystem(int poolSize = 500)
    {
        _poolSize = poolSize;
        Init();
    }

    public void NewParticle(T data, Vector2 position, Vector2 velocity, float rotation, float scale)
    {
        Particle particle = RequestParticle();   
        particle.active = true;
        particle.data = data;
        particle.position = position;
        particle.velocity = velocity;
        particle.rotation = rotation;
        particle.scale = scale;
        particle.data.OnSpawn(particle);

        if (Particles.Count(n => n.active) > _poolSize)
            Particles.Add(particle);
    }

    public Particle RequestParticle() => Particles.FirstOrDefault(n => !n.active, new Particle());

    public void Init()
    {
        Particles = new List<Particle>(_poolSize);
        for (int i = 0; i < _poolSize; i++)
            Particles.Add(new Particle());
    }

    public void Update()
    {
        foreach (Particle particle in Particles.ToList())
        {
            if (particle.active)
                particle.Update();
            else if (Particles.Count > _poolSize)
                Particles.RemoveAt(0);
        }
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 anchorPosition, BlendState blendState, Matrix transform, Effect effect = null)
    {
        List<Particle> activeParticles = new List<Particle>();

        foreach (Particle particle in Particles)
        {
            if (particle.active)
                activeParticles.Add(particle);
        }

        spriteBatch.Begin(SpriteSortMode.Deferred, blendState, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, effect, transform);

        if (activeParticles.Count > 0)
        {
            foreach (Particle particle in activeParticles)
                particle.Draw(spriteBatch, anchorPosition);
        }

        spriteBatch.End();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace BlockVanity.Common.Graphics.ParticleRendering;

public class ParticleSystem
{
    public List<Particle> Particles;

    private int _poolSize;
    private bool _allowShaders;

    public ParticleSystem(int poolSize = 3000, bool allowShaders = true)
    {
        _poolSize = poolSize;
        _allowShaders = allowShaders;
        Init();
    }

    public void NewParticle<T>(T data, Vector2 position, Vector2 velocity, float rotation, float scale) where T : IParticleData
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
            {
                particle.position += particle.velocity;
                particle.Update();
            }
            else if (Particles.Count > _poolSize)
                Particles.RemoveAt(0);
        }
    }

    public void Draw(SpriteBatch spriteBatch, bool ui)
    {
        Matrix transform = ui ? Main.UIScaleMatrix : Main.Transform;
        List<Particle> normalParticles = new List<Particle>();
        List<Particle> shaderParticles = new List<Particle>();

        foreach (Particle particle in Particles)
        {
            if (particle.active)
            {
                if (particle.data is IShaderParticleData shaderParticleData && shaderParticleData.ShaderEnabled && _allowShaders)
                    shaderParticles.Add(particle);
                else
                    normalParticles.Add(particle);
            }
        }

        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, transform);

        if (normalParticles.Count > 0)
        {
            foreach (Particle particle in normalParticles)
                particle.Draw(spriteBatch);
        }

        spriteBatch.End();

        if (_allowShaders)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, transform);

            if (shaderParticles.Count > 0)
            {
                foreach (Particle particle in shaderParticles)
                    particle.Draw(spriteBatch);
            }

            spriteBatch.End();
        }
    }
}

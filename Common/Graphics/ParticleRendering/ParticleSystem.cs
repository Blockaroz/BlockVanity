using System;
using System.Collections.Generic;
using System.Linq;
using BlockVanity.Content.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace BlockVanity.Common.Graphics.ParticleRendering;

public class ParticleSystem
{
    public List<Particle> Particles;
    private int _poolSize;

    public ParticleSystem(int poolSize = 6000)
    {
        _poolSize = poolSize;
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
                Particles.Remove(particle);
        }
    }

    public void Draw(SpriteBatch spriteBatch, bool ui)
    {
        List<Particle> normalParticles = new List<Particle>();
        List<Particle> shaderParticles = new List<Particle>();

        foreach (Particle particle in Particles)
        {
            if (particle.active)
            {
                if (particle.data is IShaderParticleData shaderParticleData && shaderParticleData.ShaderEnabled)
                    shaderParticles.Add(particle);
                else
                    normalParticles.Add(particle);
            }
        }

        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, ui ? Main.UIScaleMatrix : Main.Transform);

        if (normalParticles.Count > 0)
        {
            foreach (Particle particle in normalParticles)
                particle.Draw(spriteBatch);
        }

        spriteBatch.End();

        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, ui ? Main.UIScaleMatrix : Main.Transform);

        if (shaderParticles.Count > 0)
        {
            foreach (Particle particle in shaderParticles)
                particle.Draw(spriteBatch);
        }

        spriteBatch.End();
    }
}

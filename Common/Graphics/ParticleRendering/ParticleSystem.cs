using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using ReLogic.Threading;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Renderers;
using Terraria.Graphics.Shaders;

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
        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, ui ? Main.UIScaleMatrix : Main.Transform);

        List<Particle> normalParticles = Particles.Where(n => n.active && n.data is not IShaderParticleData).ToList();
        if (normalParticles.Count > 0)
        {
            foreach (Particle particle in normalParticles)
                particle.Draw(spriteBatch);
        }

        spriteBatch.End();

        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, ui ? Main.UIScaleMatrix : Main.Transform);

        List<Particle> shaderParticles = Particles.ToList().Where(n => n.active && n.data is IShaderParticleData).ToList();
        if (shaderParticles.Count > 0)
        {
            foreach (Particle particle in shaderParticles)
                particle.Draw(spriteBatch);
        }

        spriteBatch.End();
    }
}

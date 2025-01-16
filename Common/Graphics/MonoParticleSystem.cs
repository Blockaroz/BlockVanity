using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Renderers;

namespace BlockVanity.Common.Graphics;

public class MonoParticleSystem<T> where T : IPooledParticle, new()
{
    private ParticlePool<T> Pool;
    public List<T> Particles;

    public ParticleRendererSettings RenderSettings;

    private T Instantiator() => new T();

    public MonoParticleSystem(int poolSize)
    {
        Pool = new ParticlePool<T>(poolSize, Instantiator);
        Particles = new List<T>(poolSize);
        RenderSettings = new ParticleRendererSettings();
    }

    public T RequestParticle() => Pool.RequestParticle();

    public void Update()
    {
        if (Main.dedServ)
        {
            return;
        }

        for (int i = 0; i < Particles.Count; i++)
        {
            Particles[i].Update(ref RenderSettings);

            if (Particles[i].ShouldBeRemovedFromRenderer)
            {
                Particles[i].RestInPool();
                Particles.RemoveAt(i);
                i--;
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch, BlendState blendState, Matrix transform, Effect effect)
    {
        spriteBatch.Begin(SpriteSortMode.Deferred, blendState, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, effect, transform);
        Draw(spriteBatch);
        spriteBatch.End();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        for (int i = 0; i < Particles.Count; i++)
        {
            if (!Particles[i].ShouldBeRemovedFromRenderer)
            {
                Particles[i].Draw(ref RenderSettings, spriteBatch);
            }
        }
    }
}

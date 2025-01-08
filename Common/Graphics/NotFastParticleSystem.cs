using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;
using ReLogic.Threading;
using Terraria;
using Terraria.DataStructures;

namespace BlockVanity.Common.Graphics;

public interface IParticleData
{
    public abstract void OnSpawn(Particle particle);

    public abstract void Update(Particle particle);

    public abstract void Draw(Particle particle, SpriteBatch spriteBatch, Vector2 anchorPosition);
}

public class Particle
{
    public bool active;
    internal int age;
    public int index;
    public Vector2 position;
    public Vector2 velocity;
    public Vector2 scale;
    public float rotation;
    public Color color;
    public IParticleData data;
}

public class NotFastParticleSystem
{
    public Particle[] Particles;

    public int PoolSize => Particles.Length;

    public NotFastParticleSystem(int poolSize)
    {
        Particles = new Particle[poolSize];
        for (int i = 0; i < poolSize; i++)
            Particles[i] = new Particle();

        if (Main.dedServ)
            return;
    }

    public void NewParticle(IParticleData data, Vector2 position, Vector2 velocity, float rotation, float scale)
    {
        if (Main.dedServ)
            return;

        Particle particle = RequestParticle();   
        particle.age = 0;
        particle.data = data;
        particle.active = true;
        particle.position = position;
        particle.velocity = velocity;
        particle.rotation = rotation;
        particle.scale = new Vector2(scale);
        particle.data.OnSpawn(particle);
    }

    public Particle RequestParticle()
    {
        int distanceToDeath = Particles[0].age;
        int oldest = 0;
        for (int i = 0; i < PoolSize; i++)
        {
            if (distanceToDeath > Particles[i].age)
            {
                distanceToDeath = Particles[i].age;
                oldest = i;
            }

            if (!Particles[i].active)
            {
                oldest = i;
                break;
            }
        }

        return Particles[oldest];
    }

    public void Update()
    {
        if (Main.dedServ)
            return;

        for (int i = 0; i < PoolSize; i++)
        {
            if (Particles[i].active)
            {
                Particles[i].age++;
                Particles[i].data.Update(Particles[i]);
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 anchorPosition, BlendState blendState, Matrix transform, Effect effect)
    {
        spriteBatch.Begin(SpriteSortMode.Deferred, blendState, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, effect, transform);
        Draw(spriteBatch, anchorPosition);
        spriteBatch.End();
    }    
    
    public void Draw(SpriteBatch spriteBatch, Vector2 anchorPosition)
    {
        for (int i = 0; i < PoolSize; i++)
        {
            if (Particles[i].active)
                Particles[i].data.Draw(Particles[i], spriteBatch, anchorPosition);
        }
    }
}

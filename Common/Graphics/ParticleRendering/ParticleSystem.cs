using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Renderers;
using Terraria.Graphics.Shaders;

namespace BlockVanity.Common.Graphics.ParticleRendering;

public class ParticleSystem
{
    public List<Particle> Particles;

    public ParticleSystem(int poolSize = 10000)
    {
        Particles = new List<Particle>(poolSize);
    }

    public void NewParticle(IParticleData data, Vector2 position, Vector2 velocity, float rotation, float scale)
    {
        Particle particle = RequestParticle();
        particle.active = true;
        particle.data = data;
        particle.position = position;
        particle.velocity = velocity;
        particle.rotation = rotation;
        particle.scale = scale;
        particle.data.OnSpawn(particle);
        Particles.Add(particle);
    }

    public Particle RequestParticle() => Particles.FirstOrDefault(n => !n.active, new Particle());

    public void Clear() => Particles.Clear();

    public void Update()
    {
        foreach (Particle particle in Particles.ToList())
        {
            if (particle.active)
            {
                particle.position += particle.velocity;
                particle.Update();
            }
            else
                Particles.Remove(particle);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
        IEnumerable<Particle> regularParticles = Particles.ToList().Where(n => n.active && n.data is not IShaderParticleData);

        foreach (Particle particle in regularParticles)
            particle.Draw(spriteBatch);

        spriteBatch.End();
    }
}

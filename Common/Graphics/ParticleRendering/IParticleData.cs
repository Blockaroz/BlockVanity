using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace BlockVanity.Common.Graphics.ParticleRendering;

public interface IParticleData
{
    public abstract void OnSpawn(Particle particle);

    public abstract void Update(Particle particle);

    public abstract void Draw(Particle particle, SpriteBatch spriteBatch);
}

public interface IShaderParticleData : IParticleData
{
}

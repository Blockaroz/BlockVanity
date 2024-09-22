using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BlockVanity.Common.Graphics.ParticleRendering;

public interface IParticleData
{
    public abstract void OnSpawn(Particle particle);

    public abstract void Update(Particle particle);

    public abstract void Draw(Particle particle, SpriteBatch spriteBatch, Vector2 anchorPosition);
}

public interface IShaderParticleData : IParticleData
{
    public virtual bool ShaderEnabled => true;
}

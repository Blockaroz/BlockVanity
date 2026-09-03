using Microsoft.Xna.Framework.Graphics;
using System;
using System.Reflection;
using Terraria.Graphics.Renderers;

namespace BlockVanity.Common.Particles;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class PoolSettingsAttribute(int capacity) : Attribute
{
    /// <summary>
    /// The initial capacity of a particle pool. Defaults to 200.
    /// </summary>
    public int Capacity { get; } = capacity;

    // public RequestBehavior NewParticleBehavior { get; }

    // TODO: Impl via GetNewParticle delegate
    public enum RequestBehavior
    {
        CreateNew,
        ReplaceFirst,
        Ignore,
    }
}

public abstract class BaseParticle<T> : IPooledParticle where T : BaseParticle<T>, new()
{
	public const int DEFAULT_POOL_CAPACITY = 200;

    private static Lazy<ParticlePool<T>> poolInstantiator = new(() =>
    {
        var attribute = typeof(T).GetCustomAttribute<PoolSettingsAttribute>(inherit: false);
        return new(attribute?.Capacity ?? DEFAULT_POOL_CAPACITY, GetNewParticle);
    });

    protected static T GetNewParticle() => new();

    public static ParticlePool<T> Pool => poolInstantiator.Value;

	public bool IsRestingInPool { get; private set; }

    public bool ShouldBeRemovedFromRenderer { get; protected set; }

	public virtual void FetchFromPool()
    {
        IsRestingInPool = false;
        ShouldBeRemovedFromRenderer = false;
    }

    public virtual void RestInPool()
    {
        IsRestingInPool = true;
    }

    public virtual void Draw(ref ParticleRendererSettings settings, SpriteBatch spritebatch) { }

    public virtual void Update(ref ParticleRendererSettings settings) { }
}

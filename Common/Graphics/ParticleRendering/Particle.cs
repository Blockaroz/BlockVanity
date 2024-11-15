﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BlockVanity.Common.Graphics.ParticleRendering;

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

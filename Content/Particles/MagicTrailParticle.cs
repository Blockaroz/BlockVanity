using System;
using System.Linq;
using BlockVanity.Common.Graphics.ParticleRendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;

namespace BlockVanity.Content.Particles;

public struct MagicTrailParticle : IParticleData
{
    private Color color;
    private int trailLength;
    private float lifeTime;
    private bool emitLight;

    private Vector2[] oldPos;
    private float[] oldRot;

    public MagicTrailParticle(Color color, bool emitLight = false, int trailLength = 24)
    {
        this.color = color;
        this.trailLength = trailLength;
        this.emitLight = emitLight;
        oldPos = new Vector2[trailLength];
        oldRot = new float[trailLength];
    }

    public void OnSpawn(Particle particle)
    {
        lifeTime = 3f + Main.rand.Next(100) * particle.scale;
        oldPos = Enumerable.Repeat(particle.position, trailLength).ToArray();
        oldRot = Enumerable.Repeat(particle.rotation, trailLength).ToArray();
    }

    public void Update(Particle particle)
    {
        particle.rotation = particle.velocity.ToRotation();

        particle.velocity *= 0.97f;
        particle.velocity = Vector2.Lerp(particle.velocity, particle.velocity.RotatedByRandom(3f) * 4f - Vector2.UnitY * 0.2f, 0.03f);

        for (int i = oldPos.Length - 1; i > 0; i--)
        {
            oldPos[i] = oldPos[i - 1];
            oldRot[i] = oldRot[i - 1];
        }
        oldPos[0] = particle.position;
        oldRot[0] = particle.rotation;

        lifeTime *= 0.87f;

        if (lifeTime < 1)
            particle.scale *= 0.9f;

        if (emitLight)
            Lighting.AddLight(particle.position, color.ToVector3() * 0.3f * Utils.GetLerpValue(0f, 1f, particle.scale, true));

        if (lifeTime < 0.1f)
            particle.active = false;
    }

    public void Draw(Particle particle, SpriteBatch spriteBatch)
    {
        Texture2D texture = AllAssets.Textures.Glow[2].Value;

        float drawScale = particle.scale * (1f + 0.5f * Utils.GetLerpValue(8f, 6f, lifeTime, true));
        Color drawColor = color * Utils.GetLerpValue(0f, 0.5f, lifeTime, true);
        Rectangle slice = texture.Frame(5, 1, 2);

        for (int i = 0; i < oldPos.Length - 1; i++)
        {
            Color trailColor = drawColor * (float)Math.Pow(Utils.GetLerpValue(oldPos.Length, 0, i, true), 2f);
            trailColor.A /= 3;
            Vector2 trailStretch = new Vector2(oldPos[i].Distance(oldPos[i + 1]) / 1.99f, drawScale / ((float)i / oldPos.Length + 1));
            spriteBatch.Draw(texture, oldPos[i] - Main.screenPosition, slice, trailColor, oldRot[i], slice.Size() * 0.5f + Vector2.UnitX, trailStretch, 0, 0);
        }

        spriteBatch.Draw(texture, particle.position - Main.screenPosition, texture.Frame(), drawColor * 2f, particle.rotation, texture.Size() * 0.5f, drawScale * 0.66f, 0, 0);
    }
}

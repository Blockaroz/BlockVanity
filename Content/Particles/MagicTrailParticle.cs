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
    public Color color;
    public int trailLength;
    public float lifeTime;
    public bool emitLight;

    public Vector2[] oldPos;
    public float[] oldRot;

    public MagicTrailParticle(Color color, bool emitLight = false, int trailLength = 10)
    {
        this.color = color;
        this.trailLength = trailLength;
        this.emitLight = emitLight;
        oldPos = new Vector2[trailLength];
        oldRot = new float[trailLength];
    }

    public void OnSpawn(Particle particle)
    {
        lifeTime = 5f + Main.rand.Next(100) * particle.scale + trailLength;
        oldPos = Enumerable.Repeat(particle.position, trailLength).ToArray();
        oldRot = Enumerable.Repeat(particle.rotation, trailLength).ToArray();
    }

    public void Update(Particle particle)
    {
        particle.rotation = particle.velocity.ToRotation();

        particle.velocity *= 0.97f;
        particle.velocity = Vector2.Lerp(particle.velocity, particle.velocity.RotatedByRandom(3f) * 3f - Vector2.UnitY * 0.4f, 0.04f);

        if (trailLength > 0)
        {
            for (int i = oldPos.Length - 1; i > 0; i--)
            {
                oldPos[i] = oldPos[i - 1];
                oldRot[i] = oldRot[i - 1];
            }
            oldPos[0] = particle.position;
            oldRot[0] = particle.rotation;
        }

        if (lifeTime > 25)
            lifeTime -= Main.rand.Next(1, 15);
        else
            lifeTime *= 0.89f;

        if (lifeTime < 1)
            particle.scale *= 0.9f;

        if (emitLight)
            Lighting.AddLight(particle.position, color.ToVector3() * 0.3f * Utils.GetLerpValue(0f, 1f, particle.scale, true));

        if (lifeTime < 0.1f)
            particle.active = false;
    }

    public void Draw(Particle particle, SpriteBatch spriteBatch, Vector2 anchorPosition)
    {
        Texture2D texture = AllAssets.Textures.Particle[0].Value;

        Color drawColor = color * Utils.GetLerpValue(0f, 0.7f, lifeTime, true);

        for (int i = 0; i < oldPos.Length - 1; i++)
        {
            Color trailColor = drawColor * 0.8f * (float)Math.Pow(Utils.GetLerpValue(oldPos.Length, 0, i, true), 2f);
            trailColor.A /= 3;
            Vector2 trailStretch = new Vector2(oldPos[i].Distance(oldPos[i + 1]) / 1.99f, particle.scale * 0.66f * (1f - (i / (oldPos.Length - 1f))));
            spriteBatch.Draw(texture, oldPos[i] - Main.screenPosition, texture.Frame(), trailColor, oldRot[i], texture.Size() * 0.5f + Vector2.UnitX, trailStretch, 0, 0);
        }

        spriteBatch.Draw(texture, particle.position - Main.screenPosition, texture.Frame(), drawColor, particle.rotation, texture.Size() * 0.5f, particle.scale * 0.66f, 0, 0);
    }
}

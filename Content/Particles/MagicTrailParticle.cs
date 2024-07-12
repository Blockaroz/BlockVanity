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
        lifeTime = Main.rand.NextFloat(8, 16) * particle.scale;
        oldPos = Enumerable.Repeat(particle.position, trailLength).ToArray();
        oldRot = Enumerable.Repeat(particle.rotation, trailLength).ToArray();
    }

    public void Update(Particle particle)
    {
        particle.rotation = particle.velocity.ToRotation();

        particle.velocity *= 0.95f;
        particle.velocity = Vector2.Lerp(particle.velocity, particle.velocity + Main.rand.NextVector2Circular(10, 10) - Vector2.UnitY * 0.5f, 0.05f);

        for (int i = oldPos.Length - 1; i > 0; i--)
        {
            oldPos[i] = oldPos[i - 1];
            oldRot[i] = oldRot[i - 1];
        }
        oldPos[0] = particle.position;
        oldRot[0] = particle.rotation;

        lifeTime *= 0.9f;

        if (lifeTime < 1f)
            particle.scale *= 0.95f;

        if (emitLight)
            Lighting.AddLight(particle.position, color.ToVector3() * 0.3f * Utils.GetLerpValue(0f, 1f, particle.scale, true));

        if (particle.scale < 0.1f)
            particle.active = false;
    }

    public void Draw(Particle particle, SpriteBatch spriteBatch)
    {
        Texture2D texture = AllAssets.Textures.Glow[2].Value;

        float drawScale = particle.scale * (1f + 0.5f * Utils.GetLerpValue(8f, 6f, lifeTime, true));
        Color drawColor = color * Utils.GetLerpValue(0f, 0.5f, lifeTime, true);

        for (int i = 1; i < oldPos.Length; i++)
        {
            Color trailColor = drawColor * (float)Math.Pow(Utils.GetLerpValue(oldPos.Length, 0, i, true), 2f) * 0.5f;
            trailColor.A /= 3;
            Vector2 trailStretch = new Vector2(oldPos[i].Distance(oldPos[i - 1]) / 2f, drawScale);
            spriteBatch.Draw(texture, oldPos[i] - Main.screenPosition, texture.Frame(), trailColor, oldRot[i], texture.Size() * 0.5f - Vector2.UnitX, trailStretch, 0, 0);
        }

        spriteBatch.Draw(texture, particle.position - Main.screenPosition, texture.Frame(), drawColor, particle.rotation, texture.Size() * 0.5f, drawScale, 0, 0);
        spriteBatch.Draw(texture, particle.position - Main.screenPosition, texture.Frame(), drawColor * 2f, particle.rotation, texture.Size() * 0.5f, drawScale * 0.5f, 0, 0);
    }
}

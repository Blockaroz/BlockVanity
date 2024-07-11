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

    public MagicTrailParticle(Color color, bool emitLight = false, int trailLength = 16)
    {
        this.color = color;
        this.trailLength = trailLength;
        this.emitLight = emitLight;
        oldPos = new Vector2[trailLength];
        oldRot = new float[trailLength];
    }

    public void OnSpawn(Particle particle)
    {
        lifeTime = Main.rand.NextFloat (8, 16) * particle.scale;
        oldPos = Enumerable.Repeat(particle.position, 16).ToArray();
        oldRot = Enumerable.Repeat(particle.rotation, 16).ToArray();
    }

    public void Update(Particle particle)
    {
        particle.velocity *= 0.91f;
        particle.rotation = particle.velocity.ToRotation();

        particle.velocity = Vector2.Lerp(particle.velocity, particle.velocity * 1.1f + Main.rand.NextVector2Circular(5, 5) - Vector2.UnitY * 0.5f, 0.07f) * Utils.GetLerpValue(0, 0.2f, lifeTime, true);

        for (int i = oldPos.Length - 1; i > 0; i--)
        {
            oldPos[i] = oldPos[i - 1];
            oldRot[i] = oldRot[i - 1];
        }
        oldPos[0] = particle.position;
        oldRot[0] = particle.rotation;

        lifeTime *= 0.9f;

        if (lifeTime < 0.9f)
            particle.scale *= 0.95f;

        if (emitLight)
            Lighting.AddLight(particle.position, color.ToVector3() * 0.4f * Utils.GetLerpValue(0f, 2f, lifeTime, true));

        if (lifeTime < 0.03f || particle.scale < 0.2f)
            particle.active = false;
    }

    public void Draw(Particle particle, SpriteBatch spriteBatch)
    {
        Texture2D texture = AllAssets.Textures.Glow[2].Value;

        Color drawColor = color * Utils.GetLerpValue(0f, 0.3f, lifeTime, true);

        for (int i = 1; i < oldPos.Length; i++)
        {
            Color trailColor = drawColor * (float)Math.Pow(Utils.GetLerpValue(oldPos.Length, 0, i, true), 2f) * 0.75f;
            trailColor.A /= 3;
            Vector2 trailStretch = new Vector2(oldPos[i].Distance(oldPos[i - 1]) / 2f, particle.scale);
            spriteBatch.Draw(texture, oldPos[i] - Main.screenPosition, texture.Frame(), trailColor, oldRot[i], texture.Size() * 0.5f - Vector2.UnitX, trailStretch, 0, 0);
        }

        spriteBatch.Draw(texture, particle.position - Main.screenPosition, texture.Frame(), drawColor, particle.rotation, texture.Size() * 0.5f, particle.scale, 0, 0);
    }
}

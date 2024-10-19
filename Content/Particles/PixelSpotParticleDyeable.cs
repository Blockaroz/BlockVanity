using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlockVanity.Common.Graphics.ParticleRendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;

namespace BlockVanity.Content.Particles;

public struct PixelSpotParticleDyeable : IShaderParticleData
{
    public Color color;
    public float lifeTime;
    public bool emitLight;
    public int shader;

    public PixelSpotParticleDyeable(Color color, int lifeTime, int shader)
    {
        this.color = color;
        this.emitLight = shader > 0;
        this.shader = shader;
        this.lifeTime = Math.Max(35, lifeTime) + Main.rand.Next(10);
    }

    public void OnSpawn(Particle particle)
    {
        color *= Main.rand.NextFloat(0.7f, 1.1f);
    }

    public void Update(Particle particle)
    {
        particle.velocity *= 0.97f;
        particle.velocity.Y -= Main.rand.NextFloat(-0.5f, 2f) * 0.03f;
        particle.rotation = MathF.Round(particle.velocity.ToRotation() / MathHelper.TwoPi * 8f) / 8f * MathHelper.TwoPi;

        if (lifeTime > 25)
            lifeTime -= 5f / particle.scale;
        else
            lifeTime *= 0.89f;

        if (emitLight)
            Lighting.AddLight(particle.position, color.ToVector3() * 0.1f * Utils.GetLerpValue(0f, 1f, lifeTime, true));

        if (lifeTime < 0.1f)
            particle.active = false;
    }

    public void Draw(Particle particle, SpriteBatch spriteBatch, Vector2 anchorPosition)
    {
        Texture2D texture = AllAssets.Textures.Pixel.Value;
        Texture2D glow = AllAssets.Textures.Glow[2].Value;

        Color drawColor = Color.Lerp(Color.White with { A = 0 }, color, Utils.GetLerpValue(45f, 0f, lifeTime, true)) * Utils.GetLerpValue(0f, 7f, lifeTime, true);
        Color glowColor = color with { A = 0 } * Utils.GetLerpValue(2f, 8f, lifeTime, true);

        Vector2 particlePos = new Vector2((int)Math.Round(particle.position.X / 2f) * 2f, (int)Math.Round(particle.position.Y / 2f) * 2f) - Main.screenPosition;
        Vector2 unRotatedVel = particle.velocity.RotatedBy(-particle.rotation);
        float xS = Math.Abs(unRotatedVel.X) * 3f + particle.scale;
        float yS = Math.Abs(unRotatedVel.Y) * 3f + particle.scale;
        Vector2 stretch = new Vector2(Math.Max(xS - yS * 0.5f, 2f), Math.Max(yS - xS * 0.1f, 2f)) * 0.5f;
        DrawData textureData = new DrawData(texture, particlePos, texture.Frame(), drawColor, particle.rotation, texture.Size() * 0.5f, stretch, 0, 0);
        DrawData glowData = new DrawData(glow, particlePos, glow.Frame(), glowColor with { A = 0 }, particle.rotation, glow.Size() * 0.5f, stretch + Vector2.One * 0.1f, 0, 0);
        textureData.shader = shader;
        glowData.shader = shader;

        Main.EntitySpriteDraw(textureData);
        Main.EntitySpriteDraw(glowData);
    }
}

using System;
using BlockVanity.Common.Graphics.ParticleRendering;
using BlockVanity.Common.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace BlockVanity.Content.Projectiles.Weapons.Magic;

public struct ScholarStaffExplosionParticle : IParticleData
{
    private Color color;
    private int trailLength;
    private float lifeTime;
    private bool emitLight;

    public ScholarStaffExplosionParticle(Color color, bool emitLight = false)
    {
        this.color = color;
        this.emitLight = emitLight;
    }

    public void OnSpawn(Particle particle)
    {
        lifeTime = 10;
        particle.rotation = Main.rand.NextFloat(-5f, 5f);
        particle.scale *= 0.7f;
    }

    public void Update(Particle particle)
    {
        particle.rotation += lifeTime / 200f;

        if (lifeTime-- <= 0)
            particle.active = false;
    }

    public void Draw(Particle particle, SpriteBatch spriteBatch)
    {
        Texture2D texture = TextureAssets.Projectile[ModContent.ProjectileType<ScholarStaffBolt>()].Value;
        Texture2D glow = AllAssets.Textures.Glow[1].Value;

        float growScale = particle.scale * (0.2f + MathF.Sqrt(Utils.GetLerpValue(10, 1, lifeTime, true)) * 2f);

        spriteBatch.Draw(glow, particle.position - Main.screenPosition, glow.Frame(), Color.Black * Utils.GetLerpValue(2, 5, lifeTime, true), particle.rotation * -0.5f, glow.Size() * 0.5f, growScale * 0.2f, 0, 0);
        spriteBatch.Draw(texture, particle.position - Main.screenPosition, texture.Frame(), Color.White with { A = 0 } * 2f, 0f, texture.Size() * 0.5f, growScale * MathF.Pow(Utils.GetLerpValue(0, 5, lifeTime, true), 2f), 0, 0);
        spriteBatch.Draw(texture, particle.position - Main.screenPosition, texture.Frame(), color with { A = 0 }, 0f, texture.Size() * 0.5f, growScale * MathF.Pow(Utils.GetLerpValue(0, 5, lifeTime, true), 2f) * 1.1f, 0, 0);
        spriteBatch.Draw(glow, particle.position - Main.screenPosition, glow.Frame(), color with { A = 0 } * Utils.GetLerpValue(0, 4, lifeTime, true), particle.rotation, glow.Size() * 0.5f, growScale * 0.35f, 0, 0);
        spriteBatch.Draw(glow, particle.position - Main.screenPosition, glow.Frame(), color with { A = 0 } * Utils.GetLerpValue(0, 4, lifeTime, true), particle.rotation * -0.7f, glow.Size() * 0.5f, growScale * 0.2f, 0, 0);
    }
}

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
        lifeTime = 16;
    }

    public void Update(Particle particle)
    {
        if (lifeTime-- <= 0)
            particle.active = false;
    }

    public void Draw(Particle particle, SpriteBatch spriteBatch)
    {
        Main.instance.LoadProjectile(ModContent.ProjectileType<ScholarStaffBolt>());
        Texture2D texture = TextureAssets.Projectile[ModContent.ProjectileType<ScholarStaffBolt>()].Value;
        Texture2D glow = AllAssets.Textures.Glow[0].Value;

        float fadeOut = Utils.GetLerpValue(1, 15, lifeTime, true);
        float growScale = particle.scale * (1f + Utils.GetLerpValue(15, 1, lifeTime, true));

        Color centerColor = Color.Lerp(Color.LightGray, color, Utils.GetLerpValue(11, 1, lifeTime, true)) * Utils.GetLerpValue(0, 3, lifeTime, true);
        spriteBatch.Draw(texture, particle.position - Main.screenPosition, texture.Frame(), centerColor with { A = 0 } * Utils.GetLerpValue(0, 15, lifeTime, true), 0f, texture.Size() * 0.5f, growScale, 0, 0);

        spriteBatch.Draw(glow, particle.position - Main.screenPosition, glow.Frame(), color with { A = 0 } * fadeOut, 0f, glow.Size() * 0.5f, growScale * 0.4f, 0, 0);
        spriteBatch.Draw(glow, particle.position - Main.screenPosition, glow.Frame(), color with { A = 0 } * fadeOut, 0f, glow.Size() * 0.5f, growScale * 0.5f, 0, 0);
    }
}

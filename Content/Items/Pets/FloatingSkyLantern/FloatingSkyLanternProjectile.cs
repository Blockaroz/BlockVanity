using System;
using BlockVanity.Common.Players;
using BlockVanity.Common.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Pets.FloatingSkyLantern;

public class FloatingSkyLanternProjectile : ModProjectile
{
    public override void SetStaticDefaults()
    {
        Main.projFrames[Type] = 1;
        Main.projPet[Type] = true;
        ProjectileID.Sets.TrailingMode[Type] = 2;
        ProjectileID.Sets.TrailCacheLength[Type] = 8;
        ProjectileID.Sets.LightPet[Type] = true;
    }

    public override void SetDefaults()
    {
        Projectile.width = 24;
        Projectile.height = 24;
        Projectile.penetrate = -1;
        Projectile.netImportant = true;
        Projectile.friendly = true;
        Projectile.ignoreWater = true;
        Projectile.tileCollide = false;
        Projectile.manualDirectionChange = true;
    }

    private Player Player => Main.player[Projectile.owner];

    public override void AI()
    {
        const float rotateFactor = 11f;

        float cos = (float)Math.Cos(Projectile.localAI[0] * 0.2f);
        float sin = (float)Math.Sin(Projectile.localAI[0] * 0.2f);
        Vector2 calcPos = new Vector2(Player.direction * 45, -30);
        Vector2 vector = Player.MountedCenter + calcPos - Projectile.Center;

        float distance = Vector2.Distance(Projectile.Center, Player.MountedCenter + calcPos);

        if (!(!Player.active || Player.dead || !Player.GetModPlayer<MiscEffectPlayer>().floatingSkyLanternPet))
            Projectile.timeLeft = 2;

        Projectile.spriteDirection = Player.direction;

        if (distance > 2000)
            Projectile.Center = Player.MountedCenter + calcPos;

        if (distance < 5f)
            Projectile.velocity *= 0.25f;

        if (vector != Vector2.Zero)
        {
            if (vector.Length() < 0.004f)
                Projectile.velocity = vector;

            else
                Projectile.velocity = vector * 0.1f + new Vector2(Player.velocity.X * 0.6f, Player.velocity.Y * 0.4f);
        }

        Projectile.position += new Vector2(sin * Projectile.spriteDirection * 0.3f, cos * 0.5f);

        // Rotation adapted from flying pets

        if (Projectile.velocity.Length() > 1f)
        {
            float value = Projectile.velocity.X * 0.02f +
                          Projectile.velocity.Y * Projectile.spriteDirection * 0.02f;

            if (Math.Abs(Projectile.rotation - value) >= MathHelper.Pi)
            {
                if (value < Projectile.rotation)
                    Projectile.rotation -= MathHelper.TwoPi;
                else
                    Projectile.rotation += MathHelper.TwoPi;
            }

            Projectile.rotation = (Projectile.rotation * (rotateFactor - 1f) + value) / rotateFactor;
        }
        else
        {
            if (Projectile.rotation > MathHelper.Pi)
                Projectile.rotation -= MathHelper.TwoPi;

            if (Math.Abs(Projectile.rotation) < 0.004f)
                Projectile.rotation = 0;

            else
                Projectile.rotation *= 0.95f;
        }

        float sine = ((float)Math.Sin(Projectile.localAI[0] * 0.3f) + 1f) * 0.2f;
        Lighting.AddLight(Projectile.Center, Color.DarkOrange.ToVector3() * (sine + 1f));

        if (Player.miscCounter % 5 == 0 && Main.rand.NextBool(10))
        {
            float rand = 1f + Main.rand.NextFloat() * 0.5f;

            if (Main.rand.NextBool(2))
                rand *= -1f;

            rand *= -Projectile.spriteDirection;

            Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Firefly, 0, 0, 100);
            dust.rotation = Main.rand.NextFloat() * MathHelper.TwoPi;
            dust.velocity.X = rand * 0.2f;
            dust.noGravity = true;
            dust.customData = Projectile;
            dust.shader = GameShaders.Armor.GetSecondaryShader(Player.cLight, Player);
        }

        Projectile.localAI[0] += 0.1f;
    }

    public static Asset<Texture2D> tasselTexture;

    public override void Load()
    {
        tasselTexture = ModContent.Request<Texture2D>(Texture + "_Tassel");
    }

    public override bool PreDraw(ref Color lightColor)
    {
        Texture2D texture = TextureAssets.Projectile[Type].Value;
        SpriteEffects dir = Projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

        ArmorShaderData shaderData = GameShaders.Armor.GetSecondaryShader(Player.cLight, Player);
        if (shaderData != null)
        {
            shaderData.Apply(null);
        }

        Vector2 tailPos = Projectile.Center + new Vector2(0, 10).RotatedBy(Projectile.rotation);
        float tailWind = (float)Math.Sin(Projectile.localAI[0] * 1.5f) * 0.15f;
        float airVelocity = Projectile.velocity.X + Math.Max(0, Projectile.velocity.Y) * Projectile.spriteDirection;
        Main.EntitySpriteDraw(tasselTexture.Value, tailPos - Main.screenPosition, null, lightColor, Projectile.rotation + airVelocity * 0.05f + tailWind, tasselTexture.Value.Size() * new Vector2(0.5f, 0f), Projectile.scale, dir, 0);

        Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, texture.Size() * 0.5f, Projectile.scale, dir, 0);

        float sine = ((float)Math.Sin(Projectile.localAI[0] * 0.3f) + 1f);
        int shades = 4;
        Color shadeColor = lightColor * 0.1f * (0.5f + sine * 0.5f);
        shadeColor.A /= 2;
        for (int i = 0; i < shades; i++)
        {
            Vector2 offset = new Vector2(sine * 2f, 0).RotatedBy(MathHelper.TwoPi / shades * i);
            Main.EntitySpriteDraw(texture, Projectile.Center + offset - Main.screenPosition, null, shadeColor, Projectile.rotation, texture.Size() * 0.5f, Projectile.scale, dir, 0);
        }

        Main.pixelShader.CurrentTechnique.Passes[0].Apply();

        return false;
    }
}

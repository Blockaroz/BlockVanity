using System;
using BlockVanity.Content.Dusts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Pets.HauntedCandelabraPet;

public class HauntedCandelabra : ModProjectile
{
    public override void SetStaticDefaults()
    {
        Main.projFrames[Type] = 12;
        Main.projPet[Type] = true;
        ProjectileID.Sets.LightPet[Type] = true;
        ProjectileID.Sets.TrailCacheLength[Type] = 4;
        ProjectileID.Sets.TrailingMode[Type] = 0;
    }

    public override void SetDefaults()
    {
        Projectile.width = 30;
        Projectile.height = 30;
        Projectile.penetrate = -1;
        Projectile.netImportant = true;
        Projectile.friendly = true;
        Projectile.ignoreWater = true;
        Projectile.tileCollide = false;
        Projectile.manualDirectionChange = true;
    }

    public override void AI()
    {
        Player player = Main.player[Projectile.owner];

        if (!player.active)
        {
            Projectile.active = false;
            return;
        }

        if (!player.dead && player.GetPets().hauntedCandelabra)
        {
            Projectile.timeLeft = 2;
        }

        Vector2 homePos = player.Center + new Vector2(50 * player.direction, -30 + MathF.Sin(Projectile.ai[0] / 240f * MathHelper.TwoPi) * 10f);
        if (player.controlDown)
        {
            homePos = player.Center + new Vector2(0, 80);
        }
        else if (player.controlUp)
        {
            homePos = player.Center + new Vector2(0, -80);
        }

        if (Projectile.Distance(homePos) > 3000)
        {
            Projectile.Center = homePos;
        }

        Projectile.velocity += (homePos - Projectile.Center) * 0.07f;
        Projectile.velocity += new Vector2(player.velocity.X * 0.5f, 0);
        Projectile.velocity *= 0.6f;

        float rotationTarget = 0.001f;
        if (Math.Abs(Projectile.velocity.X) > 6f)
        {
            rotationTarget = 0.04f;
            Projectile.direction = Math.Sign(Projectile.velocity.X);
        }
        else
        {
            Projectile.ai[1]++;
            if (Projectile.ai[1] > 240)
            {
                Projectile.ai[1] = 0;
            }

            if (Projectile.ai[1] == 30 || Projectile.ai[1] == 70)
            {
                Projectile.frame++;
            }

            Projectile.direction = player.direction;
        }

        Projectile.rotation = Utils.AngleLerp(Projectile.rotation, Math.Clamp(Projectile.velocity.X, -24, 24) * rotationTarget, 0.2f);
        Projectile.spriteDirection = Projectile.direction;

        UpdateFrame();
        DoDust();

        if (Collision.SolidTiles(Projectile.position, Projectile.width, Projectile.height))
        {
            Lighting.AddLight(Projectile.Center, Color.DodgerBlue.ToVector3() * 0.5f);
        }
        else
        {
            Lighting.AddLight(Projectile.Center, Color.DodgerBlue.ToVector3() * 1.2f);
        }

        Projectile.ai[0]++;
    }

    public void UpdateFrame()
    {
        if (Math.Abs(Projectile.velocity.X) > 6f || !(Projectile.frame == 0 || Projectile.frame == 6))
        {
            Projectile.frameCounter += (int)(Math.Abs(Projectile.velocity.X) * 0.2f) + 2;
        }

        if (Projectile.frameCounter > 8)
        {
            Projectile.frameCounter = 0;
            Projectile.frame++;
        }

        if (Projectile.frame >= 12)
        {
            Projectile.frame = 0;
        }
    }

    public void DoDust()
    {
        ArmorShaderData shader = GameShaders.Armor.GetSecondaryShader(Main.player[Projectile.owner].cLight, Main.player[Projectile.owner]);
        Color flameColor = Color.White with { A = 30 };
        int dustType = ModContent.DustType<HauntedFlameDust>();
        float directionAmt = Projectile.direction * MathF.Cos(Projectile.frame / 12f * MathHelper.TwoPi);
        if (Main.rand.NextBool(3))
        {
            Dust flame = Dust.NewDustPerfect(Projectile.Center + new Vector2(0, -36) + Main.rand.NextVector2Circular(2, 2), dustType, Main.rand.NextVector2Circular(1, 2) - Vector2.UnitY * 2, 0, flameColor, 0.5f + Main.rand.NextFloat());
            flame.noGravity = true;
            flame.shader = shader;
        }
        if (Main.rand.NextBool(3))
        {
            Dust flame = Dust.NewDustPerfect(Projectile.Center + new Vector2(-10 * directionAmt, -30) + Main.rand.NextVector2Circular(2, 2), dustType, Main.rand.NextVector2Circular(1, 2) - Vector2.UnitY * 2, 0, flameColor, 0.5f + Main.rand.NextFloat());
            flame.noGravity = true;
            flame.shader = shader;
        }
        if (Main.rand.NextBool(3))
        {
            Dust flame = Dust.NewDustPerfect(Projectile.Center + new Vector2(10 * directionAmt, -26) + Main.rand.NextVector2Circular(2, 2), dustType, Main.rand.NextVector2Circular(1, 2) - Vector2.UnitY * 2, 0, flameColor, 0.5f + Main.rand.NextFloat());
            flame.noGravity = true;
            flame.shader = shader;
        }
    }

    public static Asset<Texture2D> highlightTexture;
    public static Asset<Texture2D> flameTexture;

    public override void Load()
    {
        highlightTexture = ModContent.Request<Texture2D>(Texture + "_Highlight");
        flameTexture = ModContent.Request<Texture2D>(Texture + "_Flame");
    }

    public override bool PreDraw(ref Color lightColor)
    {
        Texture2D texture = TextureAssets.Projectile[Type].Value;
        Rectangle frame = texture.Frame(12, 1, Projectile.frame, 0);
        SpriteEffects spriteEffects = Projectile.direction < 0 ? SpriteEffects.FlipHorizontally : 0;
        lightColor = Color.Lerp(lightColor, Color.White, 0.4f);
        Vector2 origin = new Vector2(frame.Width / 2f, frame.Height / 2f + 16);

        Vector2 baseOffset = new Vector2(0, 4).RotatedBy(Projectile.rotation) * Projectile.scale;
        float baseRoll = Projectile.velocity.X * 0.04f;
        Color glowColor = Color.DimGray with { A = 40 };
        Color flameColor = Color.White with { A = 80 };
        int max = ProjectileID.Sets.TrailCacheLength[Type] - 1;
        for (int i = 0; i < max; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                float p = 1f - (i + j / 2f - 1) / (max - 1f);
                if (i > 0 || j > 0)
                {
                    p *= 0.5f;
                }

                Vector2 oldCenter = Vector2.Lerp(Projectile.oldPos[i], Projectile.oldPos[i + 1], j / 2f) + Projectile.Size / 2;
                Main.EntitySpriteDraw(highlightTexture.Value, oldCenter - Main.screenPosition, frame, glowColor * p, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0);
            }
        }

        Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, frame, lightColor, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0);

        Main.EntitySpriteDraw(flameTexture.Value, Projectile.Center + Main.rand.NextVector2Circular(2, 1) - Main.screenPosition, frame, flameColor, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0);
        Main.EntitySpriteDraw(flameTexture.Value, Projectile.Center - Main.screenPosition, frame, flameColor, Projectile.rotation, origin, Projectile.scale * new Vector2(1f, Main.rand.NextFloat(0.9f, 1.1f)), spriteEffects, 0);

        return false;
    }
}

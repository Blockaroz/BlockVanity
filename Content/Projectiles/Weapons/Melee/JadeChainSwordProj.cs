using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using ReLogic.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using BlockVanity.Common.Graphics;
using BlockVanity.Content.Particles;
using Terraria.GameContent;
using Terraria.Audio;
using BlockVanity.Core.Camera;
using BlockVanity.Core;

namespace BlockVanity.Content.Projectiles.Weapons.Melee;

public class JadeChainSwordProj : ModProjectile
{
    public override void SetStaticDefaults()
    {
        ProjectileID.Sets.TrailingMode[Type] = 4;
        ProjectileID.Sets.TrailCacheLength[Type] = 5;
    }

    public override void SetDefaults()
    {
        Projectile.width = 24;
        Projectile.height = 24;
        Projectile.friendly = true;
        Projectile.hostile = false;
        Projectile.tileCollide = true;
        Projectile.ignoreWater = true;

        Projectile.DamageType = DamageClass.Melee;
        Projectile.penetrate = -1;
        Projectile.manualDirectionChange = true;
        Projectile.ownerHitCheck = true;
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = 20;
        Projectile.extraUpdates = 1;
    }

    public ref float Time => ref Projectile.ai[0];
    public ref float Mode => ref Projectile.ai[1];
    public ref float Curvature => ref Projectile.ai[2];

    public ref float AngularVelocity => ref Projectile.localAI[0];

    public ref Player Player => ref Main.player[Projectile.owner];

    private float targetDistance;
    private Vector2 targetPosition;
    private Vector2 targetDirection;

    private Rope chainRope;

    public override void AI()
    {
        const float maxDist = 600;
        float distanceToPlayer = Projectile.Distance(Player.MountedCenter);

        Projectile.rotation += AngularVelocity;
        AngularVelocity *= 0.96f;
        Vector2 chainGravity = Vector2.Zero;

        float swingSpeed = Player.GetAttackSpeed(DamageClass.Melee);
        switch (Mode)
        {
            default:
            case 0:

                const int WaitTime = 24;

                if (Main.myPlayer == Projectile.owner)
                {
                    if (Time == 0)
                    {
                        Vector2 mousePos = Main.MouseWorld;
                        Player.LimitPointToPlayerReachableArea(ref mousePos);
                        targetDistance = Player.Distance(mousePos);
                        targetPosition = mousePos - Player.MountedCenter;
                        Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero) * 20f * (swingSpeed * 0.1f + 0.9f);
                        Projectile.rotation = Projectile.velocity.ToRotation();
                        Projectile.direction = Projectile.velocity.X > 0 ? 1 : -1;
                        Projectile.netUpdate = true;
                    }
                }

                if (Time == 0)
                    SoundEngine.PlaySound(SoundID.DD2_SonicBoomBladeSlash, Projectile.Center);

                if (Time < WaitTime && Projectile.tileCollide)
                {
                    Projectile.rotation = Projectile.velocity.ToRotation();
                    float retargetCurve = Curvature * 0.5f / (targetDistance / maxDist * 3f + 1f);
                    Projectile.velocity = Projectile.velocity.RotatedBy(-retargetCurve * Utils.GetLerpValue(0, WaitTime / 2, Time, true) * Projectile.spriteDirection);
                }
                else
                {
                    Time = 0;
                    Mode = -1; 
                    Projectile.velocity *= 0.6f;
                    return;
                }

                break;

            case 1:

                Projectile.extraUpdates = 2;

                if (Main.myPlayer == Projectile.owner)
                {
                    if (Time == 0)
                    {
                        Vector2 mousePos = Main.MouseWorld;
                        Player.LimitPointToPlayerReachableArea(ref mousePos);
                        targetDistance = Math.Clamp(Player.Distance(mousePos) + Main.rand.Next(50), 30, maxDist) + 50;
                        targetPosition = mousePos - Player.MountedCenter;
                        targetDirection = Projectile.velocity.SafeNormalize(Vector2.Zero);
                        Projectile.netUpdate = true;
                    }
                }

                int SlashTime = (int)(80 / (swingSpeed * 0.3f + 0.7f) / (Utils.GetLerpValue(maxDist / 2f, 50, targetDistance, true) + 1f));

                if (Time == 10)
                    SoundEngine.PlaySound(SoundID.DD2_SonicBoomBladeSlash, Projectile.Center);

                float EasingCurve(float x) => Math.Clamp(MathF.Sin(x * MathHelper.Pi), 0f, 1f);
                Vector2 slashDirection = targetDirection.RotatedBy(MathHelper.Lerp(-Curvature, Curvature, MathF.Pow(Time / SlashTime, 2)) * Projectile.direction);
                Vector2 targetPos = Player.MountedCenter - targetDirection * targetDistance * 0.45f + slashDirection * EasingCurve(MathF.Pow(Time / SlashTime, 2)) * targetDistance * 1.33f;
                Projectile.velocity = (targetPos - Projectile.Center) * Utils.GetLerpValue(0, SlashTime / 3f, Time, true) * 0.5f;
                 
                AngularVelocity = Projectile.direction * 0.1f;
                Projectile.rotation = (-targetDirection).ToRotation() + MathF.Pow(Time / SlashTime, 2.5f) * MathHelper.TwoPi * Projectile.direction;
                
                chainGravity = slashDirection.RotatedBy(MathHelper.PiOver4 * Projectile.direction) * 4f;

                if (Time > SlashTime)
                {
                    Time = 0;
                    Mode = -1;
                    Projectile.velocity *= 0.2f;
                    return;
                }

                break;

            case -1:

                Projectile.extraUpdates = 1;

                const int PullBackTime = 15;

                Projectile.Center += Player.velocity * 0.2f;

                if (Time > PullBackTime)
                {
                    if (distanceToPlayer < 20)
                    {
                        Projectile.Kill();
                        return;
                    }

                    Projectile.tileCollide = false;
                    Projectile.velocity += new Vector2(0, 0.4f * Projectile.direction).RotatedBy(Projectile.AngleFrom(Player.Center));
                    Projectile.rotation = Utils.AngleLerp(Projectile.rotation, Projectile.AngleFrom(Player.MountedCenter), 0.2f);
                }
                else
                    Projectile.velocity *= 0.96f;

                bool secondOut = Player.ownedProjectileCounts[Type] > 1;

                if (distanceToPlayer < maxDist / 2)
                {
                    for (int i = 0; i < chainRope.segments.Length - 1; i++)
                    {
                        float ropeProgress = Utils.GetLerpValue(maxDist / 2f, 0, distanceToPlayer, true);
                        Vector2 finalPos = Vector2.Lerp(chainRope.segments[i + 1].position, Player.MountedCenter, ropeProgress);
                        chainRope.segments[i].position = Vector2.Lerp(chainRope.segments[i].position, finalPos, 0.05f + swingSpeed * 0.05f);
                    }
                }

                chainGravity = Player.DirectionFrom(Projectile.Center).RotatedBy(-0.5f * Projectile.direction);

                float pullSpeed = (secondOut ? 1f : 0.5f) * Math.Clamp(1f / (distanceToPlayer * 0.2f + 0.1f), 30f, 70f);
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(Player.MountedCenter) * pullSpeed, Utils.GetLerpValue(PullBackTime, PullBackTime + 50, Time, true) * 0.4f);

                break;
        } 

        if (distanceToPlayer > maxDist)
            Projectile.Center = Vector2.Lerp(Projectile.Center, Player.MountedCenter + Projectile.DirectionFrom(Player.MountedCenter) * maxDist, 0.2f);

        int chainLength = Mode switch
        {
            1 => (int)(targetDistance / 15),
            _ => 48
        };
        Vector2 chainStart = Projectile.Center + Projectile.velocity * 0.5f + new Vector2(-30, 0).RotatedBy(Projectile.rotation) * Projectile.scale;
        
        chainRope ??= new Rope(chainStart, Player.MountedCenter, chainLength, 11, chainGravity, 20);
        chainRope.damping = 0.1f;
        chainRope.segments[0].position = chainStart;
        chainRope.segments[^1].position = Player.MountedCenter;
        chainRope.gravity = (Projectile.rotation - MathHelper.PiOver2 * Projectile.direction).ToRotationVector2() * 0.1f + chainGravity;
        Rope.Update(chainRope);

        float lightPower = 0.2f;
        if (!Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
            lightPower = 0.5f;

        Lighting.AddLight(Projectile.Center, Color.Teal.ToVector3() * lightPower);

        Time++;
        hitGlowTime *= 0.9f;
    }

    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        switch (Mode)
        {
            default:
            case 0:

                if (Math.Abs(Projectile.velocity.X - oldVelocity.X) > 0)
                {
                    Projectile.velocity.X = -oldVelocity.X * 0.2f;
                    Projectile.velocity.Y *= 0.5f;
                }
                if (Math.Abs(Projectile.velocity.Y - oldVelocity.Y) > 0)
                {
                    Projectile.velocity.Y = -oldVelocity.Y * 0.2f;
                    Projectile.velocity.X *= 0.5f;
                }

                Projectile.position += oldVelocity;
                Projectile.tileCollide = false;

                OnHitEffects(true);

                break;

            case -1:
                break;
        }

        return false;
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        OnHitEffects(false);
        if (Projectile.tileCollide)
        {
            Projectile.velocity *= -0.2f;
            Projectile.tileCollide = false;
        }
    }

    public override void OnHitPlayer(Player target, Player.HurtInfo info)
    {
        OnHitEffects(false);
        if (Projectile.tileCollide)
        {
            Projectile.velocity *= -0.2f;
            Projectile.tileCollide = false;
        }
    }

    private float hitGlowTime;

    private void OnHitEffects(bool tile = false)
    {
        if (Main.myPlayer == Projectile.owner)
        {
            Main.instance.CameraModifiers.Add(new ContinuousShakeModifier(Projectile.Center, Player.DirectionTo(Projectile.Center), 3f, 10, 1));
            AngularVelocity = Projectile.direction * Main.rand.NextFloat(-0.2f, 0.2f);
            Projectile.netUpdate = true;
        }

        if (tile)
        {
            hitGlowTime = 2f;
            SoundEngine.PlaySound(SoundID.DD2_SonicBoomBladeSlash with { Pitch = 1f }, Projectile.Center);
        }
        else
        {
            hitGlowTime = 4f;
            SoundEngine.PlaySound(SoundID.Item71 with { Pitch = 1f }, Projectile.Center);
        }


        for (int i = 0; i < (tile ? 10 : Main.rand.Next(1, 4)); i++)
        {
            PhysicalSparkParticle particle = PhysicalSparkParticle.pool.RequestParticle();
            particle.Prepare(Projectile.Center + Projectile.velocity * 0.4f, (Projectile.velocity + Main.rand.NextVector2Circular(12, 12)) * 0.4f, Vector2.UnitY * 0.5f, Color.White with { A = 10 }, (Color.Green * 0.5f) with { A = 50 }, Main.rand.NextFloat(0.5f, 1.2f), true);
            ParticleEngine.Particles.Add(particle);
        }
    }

    public override void ModifyDamageHitbox(ref Rectangle hitbox)
    {
        hitbox.Inflate(32, 32);
    }

    public static Asset<Texture2D> glowTexture;
    public static Asset<Texture2D> chainTexture;

    public override void Load()
    {
        glowTexture = ModContent.Request<Texture2D>(Texture + "_Glow");
        chainTexture = ModContent.Request<Texture2D>(Texture + "_Chain");
    }

    public override bool PreDraw(ref Color lightColor)
    {
        Texture2D texture = TextureAssets.Projectile[Type].Value;

        SpriteEffects flipEffect = Projectile.direction < 0 ? SpriteEffects.FlipHorizontally : 0;

        lightColor = Color.Lerp(lightColor, Color.White, 0.3f);
        Color jadeColor = new Color(20, 255, 60, 20);
        Vector2 origin = texture.Size() * 0.5f;

        float trailLength = ProjectileID.Sets.TrailCacheLength[Type];
        for (int i = 0; i < trailLength; i++)
            Main.EntitySpriteDraw(glowTexture.Value, Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition, glowTexture.Frame(), jadeColor * 0.1f * (1f - i / trailLength), Projectile.rotation + MathHelper.PiOver2, origin, Projectile.scale + (1f - i / trailLength) * 0.3f, flipEffect, 0);

        if (chainRope != null)
        {
            Rope.RopeSegment[] chain = chainRope.segments;
            for (int i = 0; i < chain.Length - 1; i++)
            {
                float chainColorLerp = MathF.Pow(i / (float)chain.Length, 3f);
                Color chainColor = Color.Lerp(Lighting.GetColor(chain[i].position.ToTileCoordinates()) * 1.2f, Color.White, 0.1f);
                Rectangle chainFrame = chainTexture.Frame(1, 3, 0, i % 2);
                float chainRotation = chain[i].position.AngleTo(chain[i + 1].position) - MathHelper.PiOver2;
                float chainScale = MathHelper.Lerp(Projectile.scale, 1f, MathF.Sqrt((float)i / chain.Length));
                Main.EntitySpriteDraw(chainTexture.Value, chain[i].position - Main.screenPosition, chainFrame, chainColor, chainRotation, new Vector2(chainFrame.Width / 2, 6), chainScale, flipEffect, 0);
                if (i > chain.Length / 2)
                {
                   Color glowColor = Color.MediumSeaGreen with { A = 0 } * chainColorLerp;
                    Main .EntitySpriteDraw(chainTexture.Value, chain[i].position - Main.screenPosition, chainFrame, glowColor, chainRotation, new Vector2(chainFrame.Width / 2, 6), chainScale, flipEffect, 0);
                }
            }
        }

        Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, texture.Frame(), lightColor, Projectile.rotation + MathHelper.PiOver2, origin, Projectile.scale, flipEffect, 0);
        Main.EntitySpriteDraw(glowTexture.Value, Projectile.Center - Main.screenPosition, glowTexture.Frame(), jadeColor * (0.15f + hitGlowTime / 3f), Projectile.rotation + MathHelper.PiOver2, origin, Projectile.scale, flipEffect, 0);

        return false;
    }
}

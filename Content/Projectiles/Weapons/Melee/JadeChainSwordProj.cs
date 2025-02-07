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
        Main.projFrames[Type] = 1;
        ProjectileID.Sets.TrailingMode[Type] = 4;
        ProjectileID.Sets.TrailCacheLength[Type] = 5;
    }

    public override void SetDefaults()
    {
        Projectile.width = 20;
        Projectile.height = 20;
        Projectile.friendly = true;
        Projectile.hostile = false;
        Projectile.tileCollide = true;
        Projectile.ignoreWater = true;

        Projectile.DamageType = DamageClass.Melee;
        Projectile.penetrate = -1;
        Projectile.manualDirectionChange = true;
        Projectile.scale *= 1.2f;
        Projectile.extraUpdates = 1;
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = 30;
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

        switch (Mode)
        {
            default:
            case 0:

                const int WaitTime = 20;

                if (Main.myPlayer == Projectile.owner)
                {
                    if (Time == 0)
                    {
                        Vector2 mousePos = Main.MouseWorld;
                        Player.LimitPointToPlayerReachableArea(ref mousePos);
                        targetDistance = Player.Distance(mousePos);
                        targetPosition = mousePos - Player.MountedCenter;
                        Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero) * 24f;
                        Projectile.rotation = Projectile.velocity.ToRotation();
                        Projectile.direction = Projectile.velocity.X > 0 ? 1 : -1;
                        Projectile.netUpdate = true;
                    }
                }

                if (Time == 0)
                    SoundEngine.PlaySound(SoundID.DD2_JavelinThrowersAttack with { Pitch = 0.5f }, Projectile.Center);

                if (Time < WaitTime)
                {
                    Projectile.rotation = Projectile.velocity.ToRotation();
                    float retargetCurve = Curvature * 0.6f / (targetDistance / maxDist * 3f + 1f);
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
                const int SlashTime = 60;
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

                Vector2 slashDirection = targetDirection.RotatedBy(MathHelper.Lerp(-Curvature * 0.9f, Curvature, Time / SlashTime) * Projectile.direction);
                Vector2 targetPos = Player.MountedCenter - targetDirection * targetDistance * 0.3f + slashDirection * (Utils.PingPongFrom01To010(Time / SlashTime * 0.95f) * 0.8f + 0.2f) * targetDistance * 1.2f;
                Projectile.velocity = (targetPos - Projectile.Center) * Utils.GetLerpValue(0, SlashTime / 10f, Time, true) * 0.4f;
                 
                AngularVelocity = Projectile.direction * 0.5f;
                Projectile.rotation = (-targetDirection).ToRotation() + Time / SlashTime * MathHelper.TwoPi * Projectile.direction;
                
                chainGravity = -targetDirection + Projectile.velocity * 0.1f;

                if (Time == 0)
                    SoundEngine.PlaySound(SoundID.DD2_JavelinThrowersAttack with { Pitch = 0.5f }, Projectile.Center);

                if (Time > SlashTime)
                {
                    Time = 0;
                    Mode = -1;
                    Projectile.velocity *= 0.7f;
                    return;
                }

                break;

            case -1:

                const int PullBackTime = 20;

                Projectile.Center += Player.velocity * 0.2f;

                if (Time > PullBackTime)
                {
                    if (distanceToPlayer < 10)
                        Projectile.Kill();

                    Projectile.velocity += new Vector2(0, 0.4f * Projectile.direction).RotatedBy(Projectile.AngleFrom(Player.Center));
                    Projectile.rotation = Utils.AngleLerp(Projectile.rotation, Projectile.AngleFrom(Player.MountedCenter), 0.05f);
                }
                else
                    Projectile.velocity *= 0.96f;

                chainGravity = Player.DirectionFrom(Projectile.Center).RotatedBy(-0.5f * Projectile.direction) * 0.4f;

                float pullSpeed = Player.ownedProjectileCounts[Type] > 1 ? 1f : 0.3f;
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(Player.MountedCenter) * 30f * pullSpeed, Utils.GetLerpValue(PullBackTime, PullBackTime + 100, Time, true) * 0.8f * pullSpeed);

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
        chainRope ??= new Rope(chainStart, Player.MountedCenter, chainLength, 12, -Projectile.velocity, 20);
        chainRope.damping = 0.1f;
        chainRope.segments[0].position = chainStart;
        chainRope.segments[^1].position = Player.MountedCenter;
        chainRope.gravity = (Projectile.rotation - MathHelper.PiOver2 * Projectile.direction).ToRotationVector2() * 0.1f + chainGravity;
        chainRope.Update();

        if (!Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
            Lighting.AddLight(Projectile.Center, Color.Teal.ToVector3());

        Time++;
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
        }

        return false;
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        OnHitEffects(false);
        if (Projectile.tileCollide)
        {
            Projectile.velocity *= -0.1f;
            Projectile.tileCollide = false;
        }
    }

    public override void OnHitPlayer(Player target, Player.HurtInfo info)
    {
        OnHitEffects(false);
        if (Projectile.tileCollide)
        {
            Projectile.velocity *= -0.1f;
            Projectile.tileCollide = false;
        }
    }

    private void OnHitEffects(bool tile = false)
    {
        AngularVelocity = Projectile.direction * Main.rand.NextFloat(-0.1f, 0.2f);
        Main.instance.CameraModifiers.Add(new ContinuousShakeModifier(Vector2.Zero, 5f, 16));

        if (tile)
            SoundEngine.PlaySound(SoundID.DD2_MonkStaffGroundImpact with { Pitch = 1f, Volume = 0.5f }, Projectile.Center);
        else
        {
            SoundEngine.PlaySound(SoundID.Item71 with { Pitch = 1f, PitchVariance = 0.3f, Volume = 0.7f }, Projectile.Center);
            SoundEngine.PlaySound(SoundID.DD2_CrystalCartImpact with { Pitch = 0.5f }, Projectile.Center);
        }

        for (int i = 0; i < Main.rand.Next(3, 12); i++)
        {
            PhysicalSparkParticle particle = PhysicalSparkParticle.pool.RequestParticle();
            particle.Prepare(Projectile.Center, (Projectile.velocity + Main.rand.NextVector2Circular(12, 12)) * 0.5f, Vector2.UnitY * 0.33f, Color.White with { A = 10 }, (Color.Green * 0.5f) with { A = 50 }, Main.rand.NextFloat(0.5f, 1.2f), true);
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

        Color jadeColor = new Color(20, 255, 50, 20);

        Vector2 origin = texture.Size() * 0.5f;

        float trailLength = ProjectileID.Sets.TrailCacheLength[Type];
        for (int i = 0; i < trailLength; i++)
            Main.EntitySpriteDraw(glowTexture.Value, Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition, glowTexture.Frame(), jadeColor * 0.1f * (1f - i / trailLength), Projectile.rotation + MathHelper.PiOver2, origin, Projectile.scale + (1f - i / trailLength) * 0.3f, flipEffect, 0);

        if (chainRope != null)
        {
            Vector2[] chainPoints = chainRope.GetPoints();
            for (int i = 0; i < chainPoints.Length - 1; i++)
            {
                Color chainColor = Lighting.GetColor(chainPoints[i].ToTileCoordinates()) * 1.2f;
                Rectangle chainFrame = chainTexture.Frame(1, 3, 0, i % 2);
                float chainRotation = chainPoints[i].AngleTo(chainPoints[i + 1]) - MathHelper.PiOver2;

                Main.EntitySpriteDraw(chainTexture.Value, chainPoints[i] - Main.screenPosition, chainFrame, chainColor, chainRotation, new Vector2(chainFrame.Width / 2, 6), 1f, flipEffect, 0);
                if (i < chainPoints.Length / 2)
                {
                    Color glowColor = Color.MediumSeaGreen with { A = 0 } * MathF.Pow(1f - i / (float)chainPoints.Length, 4f) * 1.5f;
                    Main.EntitySpriteDraw(chainTexture.Value, chainPoints[i] - Main.screenPosition, chainFrame, glowColor, chainRotation, new Vector2(chainFrame.Width / 2, 6), 1.1f, flipEffect, 0);
                }
            }
        }

        Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, texture.Frame(), lightColor * 1.2f, Projectile.rotation + MathHelper.PiOver2, origin, Projectile.scale, flipEffect, 0);
        Main.EntitySpriteDraw(glowTexture.Value, Projectile.Center - Main.screenPosition, glowTexture.Frame(), jadeColor * 0.4f, Projectile.rotation + MathHelper.PiOver2, origin, Projectile.scale, flipEffect, 0);

        return false;
    }
}

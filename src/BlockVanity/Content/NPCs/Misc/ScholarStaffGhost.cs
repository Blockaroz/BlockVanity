using BlockVanity.Common.Graphics;
using BlockVanity.Content.Items.Weapons.Magic;
using BlockVanity.Content.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.NPCs.Misc;

public class ScholarStaffGhost : ModNPC
{
    public override void SetStaticDefaults()
    {
        NPCID.Sets.CountsAsCritter[Type] = true;
        NPCID.Sets.CannotDropSouls[Type] = true;
        NPCID.Sets.CantTakeLunchMoney[Type] = true;
        NPCID.Sets.TeleportationImmune[Type] = true;
        NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, new NPCID.Sets.NPCBestiaryDrawModifiers() { Hide = true });
    }

    public override void SetDefaults()
    {
        NPC.width = 36;
        NPC.height = 48;
        NPC.lifeMax = 5;
        NPC.rarity = 3;
        NPC.dontTakeDamage = true;
        NPC.noGravity = true;
        NPC.aiStyle = -1;
        // NPC.catchItem = ModContent.ItemType<ScholarStaff>();
        NPC.GravityIgnoresLiquid = true;
        NPC.timeLeft = 60;
        NPC.ShowNameOnHover = true;
    }

    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
        if (Main.hardMode && spawnInfo.Player.ZoneBeach && spawnInfo.Water && !Main.dayTime)
            return 0.001f;

        return 0f;
    }

    public override void ModifyNPCLoot(NPCLoot npcLoot)
    {
        npcLoot.Add(new CommonDrop(ModContent.ItemType<ScholarStaff>(), 1));
    }

    public override void AI()
    {
        if (Main.netMode != NetmodeID.MultiplayerClient)
        {
            if (NPC.ai[0] == 0)
            {
                NPC.TargetClosest();
                NPC.netUpdate = true;
            }
        }

        NPC.velocity.X = 0.1f * NPC.direction;

        if (!Collision.WetCollision(NPC.position, NPC.width, NPC.height * 2))
            NPC.velocity.Y += 0.01f;
        else
            NPC.velocity.Y -= 0.01f;

        int waterLine = -1;
        for (int i = -10; i < 10; i++)
        {
            int yLocation = (int)((NPC.Center.Y / 16f) + i);
            int xLocation = (int)(NPC.Center.X / 16f);
            waterLine = yLocation * 16;

            Tile tile = Framing.GetTileSafely(xLocation, yLocation);
            if (tile.LiquidAmount > 2 || WorldGen.SolidTile(tile))
            {
                waterLine += tile.LiquidAmount / 255;
                break;
            }
        }

        NPC.velocity.Y = MathHelper.Lerp(NPC.velocity.Y, (waterLine - NPC.position.Y - NPC.height * 2) * 0.1f, 0.1f);

        Lighting.AddLight(NPC.Center, Color.Turquoise.ToVector3() * Utils.GetLerpValue(0, 100, NPC.ai[0], true) * 0.5f * (0.8f + MathF.Sin(NPC.ai[1] * 0.04f) * 0.2f));

        if (Main.rand.NextBool(50))
        {
            PixelEmber particle = PixelEmber.RequestNew(NPC.Center + Main.rand.NextVector2Circular(20, 30), Main.rand.NextVector2Circular(2, 3), Main.rand.Next(300, 450), 150, Color.Cyan with { A = 0 }, ScholarStaffBolt.EnergyColor with { A = 0 }, 0.5f + Main.rand.NextFloat());
            ParticleEngine.Particles.Add(particle);
        }

        if (Main.dayTime)
        {
            NPC.ai[0]--;

            if (NPC.ai[0] <= 0)
                NPC.active = false;
        }
        else
            NPC.ai[0]++;

        NPC.ai[0] = MathHelper.Clamp(NPC.ai[0], 0, 150);
        NPC.ai[1]++;
    }

    private bool IsHovered;

    public override bool PreHoverInteract(bool mouseIntersects)
    {
        if (NPC.ai[0] < 120)
            return false;

        Player player = Main.LocalPlayer;
        bool inRange = player.InInteractionRange((int)(NPC.Center.X / 16), (int)(NPC.Center.Y / 16), TileReachCheckSettings.Simple);
        if (!player.dead && mouseIntersects && inRange)
        {
            player.noThrow = 2;
            PlayerInput.SetZoom_MouseInWorld();
            IsHovered = true;

            if (Main.mouseRight && Main.npcChatRelease)
            {
                Main.npcChatRelease = false;

                if (PlayerInput.UsingGamepad)
                    player.releaseInventory = false;

                if (player.talkNPC != NPC.whoAmI && !player.tileInteractionHappened)
                {
                    NPC.life = 0;
                    NPC.checkDead();
                    NPC.netUpdate = true;
                }
            }
        }

        return false;
    }

    public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        Texture2D texture = TextureAssets.Npc[Type].Value;
        Rectangle solidFrame = texture.Frame(3, 1, 0, 0);
        Rectangle ghostFrame = texture.Frame(3, 1, 1, 0);
        Rectangle highlightFrame = texture.Frame(3, 1, 2, 0);

        if (NPC.IsABestiaryIconDummy)
            NPC.ai[0] = 150;

        float fadeIn = Utils.GetLerpValue(40, 150, NPC.ai[0], true) * Utils.GetLerpValue(0, 60, NPC.timeLeft, true);
        float fadeInGlow = Utils.GetLerpValue(0, 150, NPC.ai[0], true) * Utils.GetLerpValue(0, 60, NPC.timeLeft, true);
        spriteBatch.Draw(texture, NPC.Center - screenPos, solidFrame, Color.DarkTurquoise * 0.2f * fadeIn, NPC.rotation, solidFrame.Size() * 0.5f, NPC.scale, 0, 0);
        spriteBatch.Draw(texture, NPC.Center - screenPos, ghostFrame, Color.White with { A = 0 } * 0.8f * fadeInGlow, NPC.rotation, ghostFrame.Size() * 0.5f, NPC.scale, 0, 0);

        Texture2D flare = TextureAssets.Extra[ExtrasID.SharpTears].Value;
        Vector2 flarePosition = NPC.Center - new Vector2(0, 18).RotatedBy(NPC.rotation) * NPC.scale;
        float flareScale = 1f + 0.05f * MathF.Sin(Main.GlobalTimeWrappedHourly * 5f);
        spriteBatch.Draw(flare, flarePosition - screenPos, flare.Frame(), Color.DarkTurquoise with { A = 0 } * 0.2f * fadeIn, MathHelper.PiOver2, flare.Size() / 2, new Vector2(0.2f, 1.5f * flareScale) * NPC.scale, 0, 0);

        if (IsHovered)
        {
            IsHovered = false;
            spriteBatch.Draw(texture, NPC.Center - screenPos, highlightFrame, Main.OurFavoriteColor, NPC.rotation, highlightFrame.Size() * 0.5f, NPC.scale, 0, 0);
        }

        return false;
    }
}

public class ScholarStaffBolt : ModProjectile
{
    public override void SetStaticDefaults()
    {
        Main.projFrames[Type] = 8;
        ProjectileID.Sets.TrailingMode[Type] = 2;
        ProjectileID.Sets.TrailCacheLength[Type] = 10;
    }

    public override void SetDefaults()
    {
        Projectile.width = 24;
        Projectile.height = 24;
        Projectile.friendly = true;
        Projectile.hostile = false;
        Projectile.tileCollide = true;
        Projectile.ignoreWater = true;
        Projectile.DamageType = DamageClass.Magic;
        Projectile.timeLeft = 300;
        Projectile.scale = 1.125f;
    }

    public ref float Speed => ref Projectile.ai[0];

    public override void OnSpawn(IEntitySource source)
    {
        Projectile.spriteDirection = Utils.ToDirectionInt(Main.rand.NextBool());
        Projectile.frame = Main.rand.Next(8);
    }

    public override void AI()
    {
        Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero) * Speed;

        Projectile.localAI[0]++;
        if (Projectile.frameCounter++ > 2)
        {
            Projectile.frameCounter = 0;
            if (++Projectile.frame >= 8)
                Projectile.frame = 0;
        }
        if (Projectile.localAI[0] % 3 == 0)
        {
            Projectile.localAI[1]++;
            if (Projectile.localAI[1] >= 12)
            {
                Projectile.localAI[1] = 0;
                sparkRotation = Main.rand.Next(-4, 4);
            }
        }

        if (Projectile.localAI[0] % 8 == 0 || Main.rand.NextBool(8))
        {
            MagicSmokeParticle darkParticle = MagicSmokeParticle.RequestNew(
                Projectile.Center + Main.rand.NextVector2Circular(8, 8),
                Projectile.velocity + Main.rand.NextVector2Circular(3, 3),
                Projectile.velocity.ToRotation() + Main.rand.NextFloat(-1f, 1f),
                Main.rand.Next(30, 40),
                (Color.Beige * 0.3f) with { A = 120 },
                Color.Beige with { A = 20 } * 0.1f,
                0.4f + Main.rand.NextFloat(0.4f));
            darkParticle.LightAffected = true;
            ParticleEngine.Particles.Add(darkParticle);
        }

        if (Projectile.localAI[0] % 10 == 0 || Main.rand.NextBool(8))
        {
            MagicSmokeParticle particle = MagicSmokeParticle.RequestNew(
                Projectile.Center + Projectile.velocity,
                Projectile.velocity + Main.rand.NextVector2Circular(3, 3),
                Projectile.velocity.ToRotation() + Main.rand.NextFloat(-1f, 1f),
                Main.rand.Next(20, 30),
                Color.White with { A = 50 },
                EnergyColor with { A = 0 },
                0.4f + Main.rand.NextFloat(0.3f));
            ParticleEngine.Particles.Add(particle);
        }

        if (Main.rand.NextBool(20) || Projectile.localAI[0] % 20 == 0)
        {
            PixelEmber particle = PixelEmber.RequestNew(Projectile.Center + Main.rand.NextVector2Circular(15, 15), Projectile.velocity.RotatedByRandom(0.2f) * Main.rand.NextFloat(0.5f), 60, 0, Color.White with { A = 0 }, EnergyColor with { A = 60 }, 1.5f + Main.rand.NextFloat());
            ParticleEngine.Particles.Add(particle);
        }

        Projectile.rotation = Projectile.velocity.X * 0.005f;
        Lighting.AddLight(Projectile.Center, EnergyColor.ToVector3() * 0.33f);
    }

    public override bool OnTileCollide(Vector2 oldVelocity) => true;

    public override void OnKill(int timeLeft)
    {
        SoundStyle hitSound = SoundID.Item101.WithPitchOffset(0.5f).WithVolumeScale(0.6f);
        hitSound.MaxInstances = 0;
        hitSound.PitchVariance = 0.3f;
        SoundEngine.PlaySound(hitSound, Projectile.Center);

        for (int i = 0; i < 10; i++)
        {
            Vector2 offset = Main.rand.NextVector2Circular(8, 8);
            PixelEmber particle = PixelEmber.RequestNew(Projectile.Center + offset, Projectile.velocity * Main.rand.NextFloat(0.4f) + offset * 0.2f, Main.rand.Next(40, 80), 0, Color.White with { A = 0 }, EnergyColor with { A = 60 }, Main.rand.NextFloat(1.5f, 3f));
            ParticleEngine.Particles.Add(particle);
        }

        for (int i = 0; i < 3; i++)
        {
            Vector2 particleVelocity = Projectile.velocity * 0.5f + Main.rand.NextVector2Circular(2, 2);
            MagicSmokeParticle burstParticle = MagicSmokeParticle.RequestNew(Projectile.Center, particleVelocity, particleVelocity.ToRotation(), Main.rand.Next(25, 30), Color.White with { A = 50 }, EnergyColor with { A = 0 }, 0.6f + i / 3f);
            ParticleEngine.Particles.Add(burstParticle);
        }
    }

    public static readonly Color EnergyColor = new Color(22, 224, 214);

    public static Asset<Texture2D> sparksTexture;

    public override void Load()
    {
        sparksTexture = ModContent.Request<Texture2D>(Texture + "Sparks");
    }

    private int sparkRotation;

    public override bool PreDraw(ref Color lightColor)
    {
        Texture2D texture = TextureAssets.Projectile[Type].Value;
        Texture2D glow = Assets.Textures.Glow[0].Value;
        Rectangle frame = texture.Frame(1, 8, 0, Projectile.frame);

        lightColor = EnergyColor * 0.5f;
        SpriteEffects effects = Projectile.spriteDirection > 0 ? SpriteEffects.FlipHorizontally : 0;

        Main.EntitySpriteDraw(glow, Projectile.Center - Main.screenPosition, glow.Frame(), lightColor with { A = 50 }, -Projectile.localAI[0] * 0.07f, glow.Size() * 0.5f, 0.4f * Projectile.scale, effects, 0);

        Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, frame, Color.White, Projectile.rotation, frame.Size() * 0.5f, Projectile.scale, effects, 0);

        Rectangle sparksFrame = sparksTexture.Frame(1, 12, 0, (int)Math.Floor(Projectile.localAI[1]));
        SpriteEffects sparksEffect = sparkRotation < 0 ? SpriteEffects.FlipHorizontally : 0;

        Main.EntitySpriteDraw(sparksTexture.Value, Projectile.Center - Main.screenPosition, sparksFrame, Color.LightCyan with { A = 0 }, Projectile.rotation * 0.5f + sparkRotation * MathHelper.PiOver2, sparksFrame.Size() * 0.5f, Projectile.scale * 0.9f, sparksEffect, 0);
        Main.EntitySpriteDraw(glow, Projectile.Center - Main.screenPosition, glow.Frame(), lightColor with { A = 0 } * 0.3f, Projectile.localAI[0] * 0.03f, glow.Size() * 0.5f, 0.7f * Projectile.scale, effects, 0);

        return false;
    }
}
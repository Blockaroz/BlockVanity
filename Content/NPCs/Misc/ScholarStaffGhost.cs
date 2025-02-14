using BlockVanity.Common.Graphics;
using BlockVanity.Content.Items.Weapons.Magic;
using BlockVanity.Content.Particles;
using BlockVanity.Content.Projectiles.Weapons.Magic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.NPCs.Misc;

public class ScholarStaffGhost : ModNPC
{
    public override void SetStaticDefaults()
    {
        NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, new NPCID.Sets.NPCBestiaryDrawModifiers() { Hide = true });
        NPCID.Sets.CountsAsCritter[Type] = true;
        NPCID.Sets.CannotDropSouls[Type] = true;
        NPCID.Sets.CantTakeLunchMoney[Type] = true;
        NPCID.Sets.TeleportationImmune[Type] = true;
    }

    public override void SetDefaults()
    {
        NPC.width = 40;
        NPC.height = 40;
        NPC.lifeMax = 5;
        NPC.rarity = 3;
        NPC.dontTakeDamage = true;
        NPC.noGravity = true;
        NPC.aiStyle = -1;
        NPC.catchItem = ModContent.ItemType<ScholarStaff>();
        NPC.GravityIgnoresLiquid = true;
        NPC.timeLeft = 60;
    }

    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
        if (spawnInfo.Player.ZoneBeach && spawnInfo.Water && !Main.dayTime)
            return 0.001f;

        return 0f;
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

        Lighting.AddLight(NPC.Center, Color.DarkTurquoise.ToVector3() * Utils.GetLerpValue(0, 100, NPC.ai[0], true) * 0.5f * (0.8f + MathF.Sin(NPC.ai[1] * 0.04f) * 0.2f));

        if (Main.rand.NextBool(30))
        {
            PixelSpotParticle particle = PixelSpotParticle.pool.RequestParticle();
            particle.Prepare(NPC.Center + Main.rand.NextVector2Circular(20, 30), Main.rand.NextVector2Circular(4, 3), Main.rand.Next(400, 750), 150, Color.Cyan with { A = 0 }, ScholarStaffBolt.EnergyColor with { A = 0 }, 0.5f + Main.rand.NextFloat());
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

    public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        Texture2D texture = TextureAssets.Npc[Type].Value;
        Rectangle solidFrame = texture.Frame(2, 1, 0, 0);
        Rectangle ghostFrame = texture.Frame(2, 1, 1, 0);

        float fadeIn = Utils.GetLerpValue(40, 150, NPC.ai[0], true) * Utils.GetLerpValue(0, 60, NPC.timeLeft, true);
        float fadeInGlow = Utils.GetLerpValue(0, 150, NPC.ai[0], true) * Utils.GetLerpValue(0, 60, NPC.timeLeft, true);
        spriteBatch.Draw(texture, NPC.Center - screenPos, solidFrame, Color.DarkTurquoise with { A = 250 } * 0.2f * fadeIn, NPC.rotation, solidFrame.Size() * 0.5f, NPC.scale, 0, 0);
        spriteBatch.Draw(texture, NPC.Center - screenPos, ghostFrame, Color.DarkGray with { A = 0 } * fadeInGlow, NPC.rotation, ghostFrame.Size() * 0.5f, NPC.scale, 0, 0);

        return false;
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlockVanity.Content.Items.Weapons.Magic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        NPC.width = 20;
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

    public override void AI()
    {
        if (NPC.ai[0] == 0)
        {
            NPC.TargetClosest();
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

            int liquidAmount = Framing.GetTileSafely(xLocation, yLocation).LiquidAmount;
            if (liquidAmount > 2)
            {
                waterLine += liquidAmount / 255;
                break;
            }
        }

        NPC.velocity.Y = MathHelper.Lerp(NPC.velocity.Y, (waterLine - NPC.position.Y - NPC.height * 2) * 0.1f, 0.1f);

        Lighting.AddLight(NPC.Center, Color.DarkTurquoise.ToVector3() * Utils.GetLerpValue(0, 100, NPC.ai[0], true));

        if (Main.dayTime)
        {
            NPC.ai[0]--;
            if (NPC.ai[0] <= 0)
                NPC.active = false;
        }
        else
            NPC.ai[0]++;

        NPC.ai[0] = MathHelper.Clamp(NPC.ai[0], 0, 150);
    }

    public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        Texture2D texture = TextureAssets.Npc[Type].Value;
        Rectangle solidFrame = texture.Frame(2, 1, 0, 0);
        Rectangle ghostFrame = texture.Frame(2, 1, 1, 0);

        float fadeIn = Utils.GetLerpValue(40, 150, NPC.ai[0], true) * Utils.GetLerpValue(0, 60, NPC.timeLeft, true);
        float fadeInGlow = Utils.GetLerpValue(0, 150, NPC.ai[0], true) * Utils.GetLerpValue(0, 60, NPC.timeLeft, true);
        spriteBatch.Draw(texture, NPC.Center - screenPos, solidFrame, Color.Turquoise with { A = 250 } * 0.5f * fadeIn, NPC.rotation, solidFrame.Size() * 0.5f, NPC.scale, 0, 0);
        spriteBatch.Draw(texture, NPC.Center - screenPos, ghostFrame, Color.Silver with { A = 10 } * fadeInGlow, NPC.rotation, ghostFrame.Size() * 0.5f, NPC.scale, 0, 0);

        return false;
    }
}

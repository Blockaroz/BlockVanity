using BlockVanity.Content.Items.Dyes;
using BlockVanity.Content.Items.Vanity.Myrtle;
using BlockVanity.Core;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace BlockVanity.Common.Players;

public class MiscEffectPlayer : ModPlayer
{
    public bool disableBootsEffect;

    public bool accBlackLens;

    public override void Load()
    {
        On_Player.SpawnFastRunParticles += DisableFastRunParticles;
        On_Player.PlayerFrame += SlowLegs;
        On_PlayerDrawLayers.DrawPlayer_21_Head += HideHeadLayer;
        On_PlayerDrawLayers.DrawPlayer_13_Leggings += HideLegsLayer;
    }

    public static List<int> hideHead = new List<int>();
    public static List<int> hideLegs = new List<int>();

    private void HideHeadLayer(On_PlayerDrawLayers.orig_DrawPlayer_21_Head orig, ref PlayerDrawSet drawinfo)
    {
        if (!hideHead.Contains(drawinfo.drawPlayer.head))
        {
            orig(ref drawinfo);
        }
    }

    private void HideLegsLayer(On_PlayerDrawLayers.orig_DrawPlayer_13_Leggings orig, ref PlayerDrawSet drawinfo)
    {
        if (!hideLegs.Contains(drawinfo.drawPlayer.legs))
        {
            orig(ref drawinfo);
        }
    }

    private void DisableFastRunParticles(On_Player.orig_SpawnFastRunParticles orig, Player self)
    {
        if (!self.GetModPlayer<MiscEffectPlayer>().disableBootsEffect)
        {
            orig(self);
        }
    }

    public void SetWalkSpeed(float speed = 0.275f)
    {
        UseCustomWalkSpeed = true;
        walkSpeed = speed;
    }

    internal bool UseCustomWalkSpeed { get; private set; }
    internal float walkSpeed;
    internal float walkCounter;
    internal int walkFrame;

    private void SlowLegs(On_Player.orig_PlayerFrame orig, Player self)
    {
        orig(self);

        MiscEffectPlayer miscPlayer = self.GetModPlayer<MiscEffectPlayer>();

        if (miscPlayer.UseCustomWalkSpeed && self.velocity.Y == 0)
        {
            if (!Main.gameInactive)
                miscPlayer.walkCounter += Math.Abs(self.velocity.X * miscPlayer.walkSpeed);

            while (miscPlayer.walkCounter > 8)
            {
                miscPlayer.walkCounter -= 8;
                miscPlayer.walkFrame += self.legFrame.Height;
            }

            if (miscPlayer.walkFrame < self.legFrame.Height * 7)
                miscPlayer.walkFrame = self.legFrame.Height * 19;
            else if (miscPlayer.walkFrame > self.legFrame.Height * 19)
                miscPlayer.walkFrame = self.legFrame.Height * 7;

            if (self.velocity.X == 0)
            {
                miscPlayer.walkFrame = 0;
                miscPlayer.walkCounter = 0;
            }

            self.bodyFrameCounter = 0.0;
            self.legFrameCounter = 0.0;
            self.bodyFrame.Y = miscPlayer.walkFrame;
            self.legFrame.Y = miscPlayer.walkFrame;
        }
    }

    public override void UpdateEquips()
    {
        for (int i = 10; i < 13; i++)
        {
            if (Player.armor[i].ModItem is IUpdateArmorInVanity)
            {
                ItemLoader.UpdateEquip(Player.armor[i], Player);
            }
        }
    }

    public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
    {
        if (accBlackLens)
        {
            drawInfo.colorEyeWhites = new Color(20, 20, 20);
        }
    }

    public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
    {
        if (!mediumCoreDeath)
        {
            if (Player.name.Equals("Myrtle", StringComparison.CurrentCultureIgnoreCase))
            {
                return [
                    new Item(ModContent.ItemType<FishFood>()),
                    new Item(ModContent.ItemType<SeasideHairDye>()),
                ];
            }
        }

        return Enumerable.Empty<Item>();
    }

    public override void GetDyeTraderReward(List<int> rewardPool)
    {
        if (Main.hardMode)
        {
            if (NPC.downedPirates)
                rewardPool.Add(ModContent.ItemType<PhantomDye>());

            if (NPC.downedGolemBoss)
                rewardPool.Add(ModContent.ItemType<RadiationDye>());
        }
    }

    public override void ResetEffects()
    {
        UseCustomWalkSpeed = false;
        disableBootsEffect = false;
        accBlackLens = false;
    }
}
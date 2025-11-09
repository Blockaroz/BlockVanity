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

    private void HideHeadLayer(On_PlayerDrawLayers.orig_DrawPlayer_21_Head orig, ref PlayerDrawSet drawinfo)
    {
        if (drawinfo.drawPlayer.head < 0 || !BlockVanity.Sets.HideHead[drawinfo.drawPlayer.head])
            orig(ref drawinfo);
    }

    private void HideLegsLayer(On_PlayerDrawLayers.orig_DrawPlayer_13_Leggings orig, ref PlayerDrawSet drawinfo)
    {
        if (drawinfo.drawPlayer.legs < 0 || !BlockVanity.Sets.HideLegs[drawinfo.drawPlayer.legs])
            orig(ref drawinfo);
    }

    private void DisableFastRunParticles(On_Player.orig_SpawnFastRunParticles orig, Player self)
    {
        if (!self.GetModPlayer<MiscEffectPlayer>().disableBootsEffect)
            orig(self);
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

    // This is really jank
    private void SlowLegs(On_Player.orig_PlayerFrame orig, Player self)
    {
        MiscEffectPlayer miscPlayer = self.GetModPlayer<MiscEffectPlayer>();

        if (!Main.gameInactive && miscPlayer.UseCustomWalkSpeed && self.velocity.X != 0 && self.velocity.Y == 0)
        {
            self.legFrameCounter -= (double)Math.Abs(self.velocity.X) * 1.3;
            self.legFrameCounter += Math.Abs(self.velocity.X * miscPlayer.walkSpeed) * 0.9;
        }

        orig(self);
    }

    public override void UpdateEquips()
    {
        for (int i = 10; i < 13; i++)
        {
            if (Player.armor[i].ModItem is IUpdateArmorInVanity)
                ItemLoader.UpdateEquip(Player.armor[i], Player);
        }
    }

    public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
    {
        if (accBlackLens)
            drawInfo.colorEyeWhites = new Color(20, 20, 20);
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
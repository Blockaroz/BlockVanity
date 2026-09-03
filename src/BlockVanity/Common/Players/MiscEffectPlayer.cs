using BlockVanity.Common;
using BlockVanity.Content.Dyes;
using BlockVanity.Content.Vanity.Myrtle;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Common.Players;

public class MiscEffectPlayer : ModPlayer
{
    public bool disableBootsEffect;
    internal bool UseCustomWalkSpeed { get; private set; }
    internal float walkSpeed;
    internal float walkCounter;
    internal int walkFrame;

    public override void Load()
    {
        On_Player.SpawnFastRunParticles += DisableFastRunParticles;
        On_Player.PlayerFrame += SlowLegs;
        On_PlayerDrawLayers.DrawPlayer_13_Leggings += HideLegsLayer;
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

    private void SlowLegs(On_Player.orig_PlayerFrame orig, Player self)
    {
        MiscEffectPlayer miscPlayer = self.GetModPlayer<MiscEffectPlayer>();

        if (FocusHelper.GameplayActive && miscPlayer.UseCustomWalkSpeed && self.velocity.X != 0 && self.velocity.Y == 0)
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

    public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
    {
        if (!mediumCoreDeath)
        {
            if (Player.name.Equals("Myrtle", StringComparison.CurrentCultureIgnoreCase))
            {
                return [
                    new Item(ModContent.ItemType<FishFoodItem>()),
                    new Item(ModContent.ItemType<SeasideHairDye>()),
                ];
            }
        }

        return [];
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
    }
}
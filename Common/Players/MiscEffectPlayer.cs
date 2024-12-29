using System.Collections.Generic;
using System.Linq;
using BlockVanity.Content.Items.Dyes;
using BlockVanity.Content.Items.Vanity.Myrtle;
using BlockVanity.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace BlockVanity.Common.Players;

public class MiscEffectPlayer : ModPlayer
{
    public bool disableBootsEffect;

    public bool accBlackEye;
    public bool blockheadSkin;

    public override void Load()
    {
        On_Player.SpawnFastRunParticles += DisableFastRunParticles;
    }

    private void DisableFastRunParticles(On_Player.orig_SpawnFastRunParticles orig, Player self)
    {
        if (!self.GetModPlayer<MiscEffectPlayer>().disableBootsEffect)
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
        if (blockheadSkin)
        {
            Color lightColor = drawInfo.drawPlayer.GetImmuneAlpha(Lighting.GetColorClamped((int)((double)drawInfo.Position.X + (double)drawInfo.drawPlayer.width * 0.5) / 16, (int)(((double)drawInfo.Position.Y + (double)drawInfo.drawPlayer.height * 0.5) / 16.0), drawInfo.drawPlayer.skinColor.ToGrayscale()), drawInfo.shadow);
            drawInfo.colorBodySkin = lightColor;
            drawInfo.colorHead = drawInfo.colorBodySkin;
        }

        if (accBlackEye)
            drawInfo.colorEyeWhites = new Color(20, 20, 20);
    }

    public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
    {
        if (!mediumCoreDeath)
        {
            if (Player.name.Equals("Myrtle", System.StringComparison.CurrentCultureIgnoreCase))
                return [
                    new Item(ModContent.ItemType<FishFood>()),
                    new Item(ModContent.ItemType<SeasideHairDye>()),
                ];
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
        disableBootsEffect = false;
        accBlackEye = false;
        blockheadSkin = false;
    }
}

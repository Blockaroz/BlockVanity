using BlockVanity.Common.Players;
using BlockVanity.Core;
using Terraria;
using Terraria.ID;

namespace BlockVanity.Content.Vanity.Myrtle;

public class FishFood : VanityItem
{
    public FishFood() : base(ItemRarityID.Green, 20, 28, accessory: true) { }

    public override void UpdateVanity(Player player)
    {
        //HitEffectPlayer.SetSkinHitSound(player, AllAssets.Sounds.FishyHit);
        player.GetModPlayer<BlueFishSkinPlayer>().enabled = true;
        player.GetModPlayer<ReskinPlayer>().enabled = true;
    }
}
using BlockVanity.Common.Players;
using BlockVanity.Common.Players.Skins;
using BlockVanity.Core;
using Terraria;
using Terraria.ID;

namespace BlockVanity.Content.Items.Vanity.Myrtle;

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
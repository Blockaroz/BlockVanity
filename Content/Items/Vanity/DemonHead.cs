using BlockVanity.Common.Players;
using BlockVanity.Core;
using Terraria;
using Terraria.ID;

namespace BlockVanity.Content.Items.Vanity;

public class DemonHead : VanityItem
{
    public DemonHead() : base(ItemRarityID.Green, 34, 48, Item.buyPrice(0, 0, 60, 0), true) { }

    public override void UpdateVanity(Player player) => HitEffectPlayer.SetSkinHitSound(player, AllAssets.Sounds.DemonHit);
}
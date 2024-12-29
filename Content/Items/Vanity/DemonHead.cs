using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlockVanity.Common.Players;
using BlockVanity.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Vanity;

public class DemonHead : VanityItem
{
    public DemonHead() : base(ItemRarityID.Green, 34, 48, Item.buyPrice(0, 0, 60, 0), true) { }

    public override void UpdateVanity(Player player) => HitEffectPlayer.SetSkinHitSound(player, AllAssets.Sounds.DemonHit);
}

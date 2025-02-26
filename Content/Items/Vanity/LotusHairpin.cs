﻿using BlockVanity.Core;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Vanity;

[AutoloadEquip(EquipType.Head)]
public class LotusHairpin : VanityItem
{
    public LotusHairpin() : base(ItemRarityID.Green) { }

    public override void SetStaticDefaults()
    {
        ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;
    }
}

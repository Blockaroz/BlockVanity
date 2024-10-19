﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlockVanity.Common.Utilities;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Vanity.CountChaos;

[AutoloadEquip(EquipType.Head)]
public class CountChaosHornedHead : VanityItem
{
    public CountChaosHornedHead() : base(ItemRarityID.Cyan) { }

    public override void SetStaticDefaults()
    {
        ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using BlockVanity.Common.Utilities;

namespace BlockVanity.Content.Items.Vanity.Myrtle;

public class FishFood : VanityItem
{
    public FishFood() : base(ItemRarityID.Green, 20, 28, accessory: true) { }
}

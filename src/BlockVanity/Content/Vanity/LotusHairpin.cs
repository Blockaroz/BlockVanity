using BlockVanity.Common;
using BlockVanity.Core;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Vanity;

[AutoloadEquip(EquipType.Head)]
public class LotusHairpin : VanityItem
{
    public override int Rarity => ItemRarityID.Green;

    public override void SetStaticDefaults()
    {
        ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;
    }
}

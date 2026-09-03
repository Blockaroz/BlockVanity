using BlockVanity.Common;
using BlockVanity.Core;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Vanity.Misc;

[AutoloadEquip(EquipType.Head)]
public class LotusHairpinItem : VanityItem
{
    public override string Texture => Assets.Textures.Content.Vanity.Misc.LotusHairpin_Item.KEY;

    public override int Rarity => ItemRarityID.Green;

    public override void SetStaticDefaults()
    {
        ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;
    }
}

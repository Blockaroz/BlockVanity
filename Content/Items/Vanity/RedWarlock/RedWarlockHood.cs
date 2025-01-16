using BlockVanity.Core;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Vanity.RedWarlock;

[AutoloadEquip(EquipType.Head)]
public class RedWarlockHood : VanityItem
{
    public RedWarlockHood() : base(ItemRarityID.Orange) { }

    public override void SetStaticDefaults()
    {
        ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;
    }
}

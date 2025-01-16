using BlockVanity.Core;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Vanity.Midra;

[AutoloadEquip(EquipType.Legs)]
public class MidraLegs : VanityItem
{
    public MidraLegs() : base(ItemRarityID.Cyan) { }

    public override void SetStaticDefaults()
    {
        ArmorIDs.Legs.Sets.HidesBottomSkin[Item.legSlot] = true;
        ArmorIDs.Legs.Sets.OverridesLegs[Item.legSlot] = true;
        ArmorIDs.Legs.Sets.IncompatibleWithFrogLeg[Item.legSlot] = true;
    }
}

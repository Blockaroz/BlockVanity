using BlockVanity.Common.Utilities;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Vanity.Myrtle;

[AutoloadEquip(EquipType.Head)]
public class PlumeriaPin : VanityItem
{
    public PlumeriaPin() : base(ItemRarityID.Green) { }

    public override void SetStaticDefaults()
    {
        ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;
    }
}

[AutoloadEquip(EquipType.Body)]
public class MyrtleDress : VanityItem
{
    public MyrtleDress() : base(ItemRarityID.Green) { }
}

[AutoloadEquip(EquipType.Legs)]
public class MyrtleSandals : VanityItem
{
    public MyrtleSandals() : base(ItemRarityID.Green) { }
}

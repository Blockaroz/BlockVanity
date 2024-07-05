using BlockVanity.Common;
using BlockVanity.Common.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Vanity.Myrtle;

[AutoloadEquip(EquipType.Head)]
public class PlumeriaHairpin : VanityItem
{
    public PlumeriaHairpin() : base(ItemRarityID.Green) { }

    public override void SetStaticDefaults()
    {
        ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;
    }

    public override bool IsArmorSet(Item head, Item body, Item legs) => true;
    public override bool IsVanitySet(int head, int body, int legs) => true;

    public override void UpdateArmorSet(Player player)
    {
        if (player.builderAccStatus[ModContent.GetInstance<AreaEffectsToggle>().Type] == 0)
            Lighting.AddLight(player.MountedCenter, Colors.CoinPlatinum.ToVector3() * 0.2f);
    }

    public override void UpdateVanitySet(Player player) => UpdateArmorSet(player);
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

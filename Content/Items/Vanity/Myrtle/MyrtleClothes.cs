using BlockVanity.Common.UI;
using BlockVanity.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Vanity.Myrtle;

[AutoloadEquip(EquipType.Head)]
public class PlumeriaHairpin : VanityItem
{
    public PlumeriaHairpin() : base(ItemRarityID.Green, value: Item.buyPrice(0, 10, 0, 0)) { }

    public override void SetStaticDefaults()
    {
        ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;
    }

    public override bool IsArmorSet(Item head, Item body, Item legs) => true;
    public override bool IsVanitySet(int head, int body, int legs) => true;

    public override void UpdateArmorSet(Player player)
    {
        if (AreaEffectsToggle.IsActive(player))
            Lighting.AddLight(player.MountedCenter, Color.SlateGray.ToVector3() * 0.33f);
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

    public override void Load()
    {
        if (Main.netMode == NetmodeID.Server)
            return;

        EquipLoader.AddEquipTexture(Mod, $"{Texture}_{EquipType.Legs}Female", EquipType.Legs,  name: this.Name + "Female");
    }

    public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
    {
        if (!male)
            equipSlot = EquipLoader.GetEquipSlot(Mod, Name + "Female", EquipType.Legs);
    }
}

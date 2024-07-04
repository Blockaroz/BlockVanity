using BlockVanity.Common.Players;
using BlockVanity.Common.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Vanity.MushroomCow;

[AutoloadEquip(EquipType.Head)]
public class RedMushroomCowHead : VanityItem
{
    public RedMushroomCowHead() : base(ItemRarityID.Blue) { }

    public override void SetStaticDefaults() => ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;

    public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ModContent.ItemType<RedMushroomCowHide>() && legs.type == ModContent.ItemType<RedMushroomCowTrotters>();

    public override void PreUpdateVanitySet(Player player) => player.GetModPlayer<MushroomCowVisuals>().red = true;

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.Mushroom, 15)
            .AddIngredient(ItemID.Bone, 5)
            .AddIngredient(ItemID.Leather, 5)
            .AddTile(TileID.Loom)
            .Register();
    }

    public override bool CanRightClick() => true;

    public override void ModifyItemLoot(ItemLoot itemLoot) => itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoolRedMushroomCowHead>(), 1, 1));
}

[AutoloadEquip(EquipType.Head)]
public class CoolRedMushroomCowHead : VanityItem
{
    public CoolRedMushroomCowHead() : base(ItemRarityID.Green) { }

    public override void SetStaticDefaults() => ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;

    public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ModContent.ItemType<BrownMushroomCowHide>() && legs.type == ModContent.ItemType<BrownMushroomCowTrotters>();

    public override void PreUpdateVanitySet(Player player) => player.GetModPlayer<MushroomCowVisuals>().brown = true;

    public override bool CanRightClick() => true;

    public override void ModifyItemLoot(ItemLoot itemLoot) => itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<RedMushroomCowHead>(), 1, 1));
}

[AutoloadEquip(EquipType.Body)]
public class RedMushroomCowHide : VanityItem
{
    public RedMushroomCowHide() : base(ItemRarityID.Blue) { }

    public override bool IsArmorSet(Item head, Item body, Item legs) => head.type == ModContent.ItemType<RedMushroomCowHead>() && legs.type == ModContent.ItemType<RedMushroomCowTrotters>();

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.Mushroom, 30)
            .AddIngredient(ItemID.Silk, 3)
            .AddIngredient(ItemID.Leather, 10)
            .AddTile(TileID.Loom)
            .Register();
    }
}

[AutoloadEquip(EquipType.Legs)]
public class RedMushroomCowTrotters : VanityItem
{
    public RedMushroomCowTrotters() : base(ItemRarityID.Blue) { }

    public override bool IsArmorSet(Item head, Item body, Item legs) => head.type == ModContent.ItemType<RedMushroomCowHead>() && body.type == ModContent.ItemType<RedMushroomCowHide>();

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.Mushroom, 20)
            .AddIngredient(ItemID.Silk, 3)
            .AddIngredient(ItemID.Leather, 8)
            .AddTile(TileID.Loom)
            .Register();
    }
}

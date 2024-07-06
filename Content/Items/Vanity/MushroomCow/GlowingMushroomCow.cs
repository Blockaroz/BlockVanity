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
public class GlowingMushroomCowHead : VanityItem
{
    public GlowingMushroomCowHead() : base(ItemRarityID.Green) { }

    public override void SetStaticDefaults() => ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;

    public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ModContent.ItemType<GlowingMushroomCowHide>() && legs.type == ModContent.ItemType<GlowingMushroomCowTrotters>();

    public override void PreUpdateVanitySet(Player player) => player.GetModPlayer<MushroomCowVisualPlayer>().glowing = true;

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.GlowingMushroom, 50)
            .AddIngredient<RedMushroomCowHead>()
            .AddTile(TileID.Loom)
            .Register();
    }

    public override bool CanRightClick() => true;

    public override void ModifyItemLoot(ItemLoot itemLoot) => itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoolGlowingMushroomCowHead>(), 1, 1));
}

[AutoloadEquip(EquipType.Head)]
public class CoolGlowingMushroomCowHead : VanityItem
{
    public CoolGlowingMushroomCowHead() : base(ItemRarityID.Green) { }

    public override void SetStaticDefaults() => ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;

    public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ModContent.ItemType<BrownMushroomCowHide>() && legs.type == ModContent.ItemType<BrownMushroomCowTrotters>();

    public override void PreUpdateVanitySet(Player player) => player.GetModPlayer<MushroomCowVisualPlayer>().brown = true;

    public override bool CanRightClick() => true;

    public override void ModifyItemLoot(ItemLoot itemLoot) => itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<GlowingMushroomCowHead>(), 1, 1));
}

[AutoloadEquip(EquipType.Body)]
public class GlowingMushroomCowHide : VanityItem
{
    public GlowingMushroomCowHide() : base(ItemRarityID.Green) { }

    public override bool IsArmorSet(Item head, Item body, Item legs) => (head.type == ModContent.ItemType<GlowingMushroomCowHead>() || head.type == ModContent.ItemType<CoolGlowingMushroomCowHead>()) && legs.type == ModContent.ItemType<GlowingMushroomCowTrotters>();

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.GlowingMushroom, 90)
            .AddIngredient<RedMushroomCowHide>()
            .AddTile(TileID.Loom)
            .Register();
    }
}

[AutoloadEquip(EquipType.Legs)]
public class GlowingMushroomCowTrotters : VanityItem
{
    public GlowingMushroomCowTrotters() : base(ItemRarityID.Green) { }

    public override bool IsArmorSet(Item head, Item body, Item legs) => (head.type == ModContent.ItemType<GlowingMushroomCowHead>() || head.type == ModContent.ItemType<CoolGlowingMushroomCowHead>()) && body.type == ModContent.ItemType<GlowingMushroomCowHide>();

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.GlowingMushroom, 70)
            .AddIngredient<RedMushroomCowTrotters>()
            .AddTile(TileID.Loom)
            .Register();
    }
}

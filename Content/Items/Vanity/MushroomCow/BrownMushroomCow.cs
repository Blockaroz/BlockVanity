using BlockVanity.Common.Players;
using BlockVanity.Common.Utilities;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Vanity.MushroomCow;

[AutoloadEquip(EquipType.Head)]
public class BrownMushroomCowHead : VanityItem
{
    public BrownMushroomCowHead() : base(ItemRarityID.Green) { }

    public override void SetStaticDefaults() => ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;

    public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ModContent.ItemType<BrownMushroomCowHide>() && legs.type == ModContent.ItemType<BrownMushroomCowTrotters>();

    public override void PreUpdateVanitySet(Player player) => player.GetModPlayer<MushroomCowVisuals>().brown = true;

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<RedMushroomCowHead>()
            .AddTile(TileID.DemonAltar)
            .Register();
    }

    public override bool CanRightClick() => true;

    public override void ModifyItemLoot(ItemLoot itemLoot) => itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<CoolBrownMushroomCowHead>(), 1, 1));
}

[AutoloadEquip(EquipType.Head)]
public class CoolBrownMushroomCowHead : VanityItem
{
    public CoolBrownMushroomCowHead() : base(ItemRarityID.Green) { }

    public override void SetStaticDefaults() => ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;

    public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ModContent.ItemType<BrownMushroomCowHide>() && legs.type == ModContent.ItemType<BrownMushroomCowTrotters>();

    public override void PreUpdateVanitySet(Player player) => player.GetModPlayer<MushroomCowVisuals>().brown = true;

    public override bool CanRightClick() => true;

    public override void ModifyItemLoot(ItemLoot itemLoot) => itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<BrownMushroomCowHead>(), 1, 1));
}

[AutoloadEquip(EquipType.Body)]
public class BrownMushroomCowHide : VanityItem
{
    public BrownMushroomCowHide() : base(ItemRarityID.Green) { }

    public override bool IsArmorSet(Item head, Item body, Item legs) => head.type == ModContent.ItemType<BrownMushroomCowHead>() && legs.type == ModContent.ItemType<BrownMushroomCowTrotters>();

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<RedMushroomCowHide>()
            .AddTile(TileID.DemonAltar)
            .Register();
    }
}

[AutoloadEquip(EquipType.Legs)]
public class BrownMushroomCowTrotters : VanityItem
{
    public BrownMushroomCowTrotters() : base(ItemRarityID.Green) { }

    public override bool IsArmorSet(Item head, Item body, Item legs) => head.type == ModContent.ItemType<BrownMushroomCowHead>() && body.type == ModContent.ItemType<BrownMushroomCowHide>();

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<RedMushroomCowTrotters>()
            .AddTile(TileID.DemonAltar)
            .Register();
    }
}

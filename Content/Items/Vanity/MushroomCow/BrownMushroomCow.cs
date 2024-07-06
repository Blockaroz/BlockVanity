using BlockVanity.Common.Players;
using BlockVanity.Common.Utilities;
using Terraria;
using Terraria.Audio;
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

    public override void PreUpdateVanitySet(Player player) => player.GetModPlayer<MushroomCowVisualPlayer>().brown = true;

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<RedMushroomCowHead>()
            .AddTile(TileID.DemonAltar)
            .Register();
    }

    public override bool CanRightClick() => true;

    public override void RightClick(Player player)
    {
        Item.type = ModContent.ItemType<CoolBrownMushroomCowHead>();
        Item.SetDefaults(Type);
        SoundEngine.PlaySound(SoundID.Grab);
    }
}

[AutoloadEquip(EquipType.Head)]
public class CoolBrownMushroomCowHead : VanityItem
{
    public CoolBrownMushroomCowHead() : base(ItemRarityID.Green) { }

    public override void SetStaticDefaults() => ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;

    public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ModContent.ItemType<BrownMushroomCowHide>() && legs.type == ModContent.ItemType<BrownMushroomCowTrotters>();

    public override void PreUpdateVanitySet(Player player) => player.GetModPlayer<MushroomCowVisualPlayer>().brown = true;

    public override bool CanRightClick() => true;

    public override void RightClick(Player player)
    {
        Item.type = ModContent.ItemType<BrownMushroomCowHead>();
        Item.SetDefaults(Type);
        SoundEngine.PlaySound(SoundID.Grab);
    }
}

[AutoloadEquip(EquipType.Body)]
public class BrownMushroomCowHide : VanityItem
{
    public BrownMushroomCowHide() : base(ItemRarityID.Green) { }

    public override bool IsArmorSet(Item head, Item body, Item legs) => (head.type == ModContent.ItemType<BrownMushroomCowHead>() || head.type == ModContent.ItemType<CoolBrownMushroomCowHead>()) && legs.type == ModContent.ItemType<BrownMushroomCowTrotters>();

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

    public override bool IsArmorSet(Item head, Item body, Item legs) => (head.type == ModContent.ItemType<BrownMushroomCowHead>() || head.type == ModContent.ItemType<CoolBrownMushroomCowHead>()) && body.type == ModContent.ItemType<BrownMushroomCowHide>();

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<RedMushroomCowTrotters>()
            .AddTile(TileID.DemonAltar)
            .Register();
    }
}

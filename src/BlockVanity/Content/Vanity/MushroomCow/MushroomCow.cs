using BlockVanity.Common;
using BlockVanity.Common.Players;
using BlockVanity.Content.Rarities;
using BlockVanity.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Vanity.MushroomCow;

[AutoloadEquip(EquipType.Head)]
public class MushroomCowHead : VanityItem
{
    public MushroomCowHead() : base(ItemRarityID.Blue) { }

    public override void SetStaticDefaults()
    {
        ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;
        NeckPlayerLayer.AddNeck(this);
        BlockVanity.AddStyles(Type, ModContent.ItemType<MushroomCowHead>());
    }

    public override bool IsVanitySet(int head, int body, int legs) =>
        body == EquipLoader.GetEquipSlot(Mod, nameof(MushroomCowHide), EquipType.Body) &&
        legs == EquipLoader.GetEquipSlot(Mod, nameof(MushroomCowTrotters), EquipType.Legs);

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.Mushroom, 15)
            .AddIngredient(ItemID.WormTooth, 2)
            .AddIngredient(ItemID.FlinxFur, 3)
            .AddIngredient(ItemID.Leather, 5)
            .AddTile(TileID.Loom)
            .Register();

        CreateRecipe()
            .AddIngredient(ItemID.Mushroom, 15)
            .AddIngredient(ItemID.Vertebrae, 2)
            .AddIngredient(ItemID.FlinxFur, 3)
            .AddIngredient(ItemID.Leather, 5)
            .AddTile(TileID.Loom)
            .Register();
    }
}

[AutoloadEquip(EquipType.Head)]
public class GamingMushroomCowHead : VanityItem
{
    public GamingMushroomCowHead() : base(ModContent.RarityType<RarityCommonVanity>()) { }

    public override void SetStaticDefaults()
    {
        ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;
        NeckPlayerLayer.AddNeck(this);
    }

    public override bool IsVanitySet(int head, int body, int legs) =>
        body == EquipLoader.GetEquipSlot(Mod, nameof(MushroomCowHide), EquipType.Body) &&
        legs == EquipLoader.GetEquipSlot(Mod, nameof(MushroomCowTrotters), EquipType.Legs);
}

[AutoloadEquip(EquipType.Body)]
public class MushroomCowHide : VanityItem
{
    public MushroomCowHide() : base(ItemRarityID.Blue) { }

    public override bool IsVanitySet(int head, int body, int legs) =>
        (head == EquipLoader.GetEquipSlot(Mod, nameof(MushroomCowHead), EquipType.Head) || head == EquipLoader.GetEquipSlot(Mod, nameof(GamingMushroomCowHead), EquipType.Head)) &&
        legs == EquipLoader.GetEquipSlot(Mod, nameof(MushroomCowTrotters), EquipType.Legs);

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.Mushroom, 30)
            .AddIngredient(ItemID.FlinxFur, 5)
            .AddIngredient(ItemID.Leather, 10)
            .AddTile(TileID.Loom)
            .Register();
    }
}

[AutoloadEquip(EquipType.Legs)]
public class MushroomCowTrotters : VanityItem
{
    public MushroomCowTrotters() : base(ItemRarityID.Blue) { }

    public override bool IsVanitySet(int head, int body, int legs) =>
        (head == EquipLoader.GetEquipSlot(Mod, nameof(MushroomCowHead), EquipType.Head) || head == EquipLoader.GetEquipSlot(Mod, nameof(GamingMushroomCowHead), EquipType.Head)) &&
        body == EquipLoader.GetEquipSlot(Mod, nameof(MushroomCowHide), EquipType.Body);

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.Mushroom, 20)
            .AddIngredient(ItemID.FlinxFur, 4)
            .AddIngredient(ItemID.Leather, 8)
            .AddTile(TileID.Loom)
            .Register();
    }
}
using BlockVanity.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Vanity;

[AutoloadEquip(EquipType.Head)]
public class CardboardBox : VanityItem
{
    public CardboardBox() : base(30, 28, Item.buyPrice(0, 0, 0, 5)) { }

    public override int Rarity => ItemRarityID.White;

    public override void SetStaticDefaults()
    {
        ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddRecipeGroup(RecipeGroupID.Wood, 10)
            .AddIngredient(ItemID.Gel, 5)
            .AddTile(TileID.Sawmill)
            .Register();
    }
}
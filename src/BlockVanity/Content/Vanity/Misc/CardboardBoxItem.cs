using BlockVanity.Common;
using BlockVanity.Core;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Vanity.Misc;

[AutoloadEquip(EquipType.Head)]
public class CardboardBoxItem() : VanityItem(30, 28, Item.buyPrice(0, 0, 0, 5))
{
    public override string Texture => Assets.Textures.Content.Vanity.Misc.CardboardBox_Item.KEY;

    public override int Rarity => ItemRarityID.White;

    public override void SetStaticDefaults()
    {
        ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddRecipeGroup(RecipeGroups.Wood, 10)
            .AddIngredient(ItemID.Gel, 5)
            .AddTile(TileID.Sawmill)
            .Register();
    }
}
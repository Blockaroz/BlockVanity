using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items
{
    public class Cardboard : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 50;
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 24;
            Item.material = true;
            Item.maxStack = 9999;
        }

        public override void AddRecipes()
        {
            CreateRecipe(4)
                .AddIngredient(ItemID.Wood, 2)
                .AddIngredient(ItemID.Gel)
                .AddTile(TileID.Sawmill)
                .Register();
        }
    }
}

using BlockVanity.Common;
using BlockVanity.Common.Players;
using Terraria;
using Terraria.ID;

namespace BlockVanity.Content.Vanity;

public class BlackEyeLens() : VanityItem(accessory: true)
{
    public override int Rarity => ItemRarityID.Blue;

    public override void UpdateAccessory(Player player, bool hideVisual) => player.GetModPlayer<MiscEffectPlayer>().accBlackLens = !hideVisual;

    public override void UpdateVanity(Player player) => UpdateAccessory(player, false);

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.BlackLens, 2)
            .AddTile(TileID.WorkBenches)
            .Register();
    }
}
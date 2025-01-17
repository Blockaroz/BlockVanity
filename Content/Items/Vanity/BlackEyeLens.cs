using BlockVanity.Common.Players;
using BlockVanity.Core;
using Terraria;
using Terraria.ID;

namespace BlockVanity.Content.Items.Vanity;

public class BlackEyeLens : VanityItem
{
    public BlackEyeLens() : base(ItemRarityID.Blue, accessory: true) { }

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
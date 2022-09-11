using BlockVanity.Common.Players;
using BlockVanity.Core;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Vanity.RoyalNinja
{
    [AutoloadEquip(EquipType.Head)]
    public class RoyalNinjaHood : VanityItem
    {
        public RoyalNinjaHood() : base("Royal Ninja Hood", ItemRarityID.Blue) { }

        public override void PostStaticDefaults()
        {
            ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = false;
            ArmorIDs.Head.Sets.DrawBackHair[Item.headSlot] = false;
            ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = false;
            ArmorIDs.Head.Sets.PreventBeardDraw[Item.headSlot] = true;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ModContent.ItemType<RoyalNinjaRobe>() && legs.type == ModContent.ItemType<RoyalNinjaPants>();

        public override void PreUpdateVanitySet(Player player) => player.GetModPlayer<RoyalNinjaVisuals>().active = true;

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Silk, 20)
                .AddTile(TileID.Loom)
                .Register();
        }
    }

    [AutoloadEquip(EquipType.Body)]
    public class RoyalNinjaRobe : VanityItem
    {
        public RoyalNinjaRobe() : base("Royal Ninja Robe", ItemRarityID.Blue) { }

        public override bool IsArmorSet(Item head, Item body, Item legs) => head.type == ModContent.ItemType<RoyalNinjaHood>() && legs.type == ModContent.ItemType<RoyalNinjaPants>();

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Silk, 20)
                .AddTile(TileID.Loom)
                .Register();
        }
    }

    [AutoloadEquip(EquipType.Legs)]
    public class RoyalNinjaPants : VanityItem
    {
        public RoyalNinjaPants() : base("Royal Ninja Pants", ItemRarityID.Blue) { }

        public override bool IsArmorSet(Item head, Item body, Item legs) => head.type == ModContent.ItemType<RoyalNinjaHood>() && body.type == ModContent.ItemType<RoyalNinjaRobe>();

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Silk, 20)
                .AddTile(TileID.Loom)
                .Register();
        }
    }
}

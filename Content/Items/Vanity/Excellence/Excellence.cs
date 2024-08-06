using BlockVanity.Common.Utilities;
using BlockVanity.Content.Rarities;
using Microsoft.Xna.Framework;
using ReLogic.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Vanity.Excellence;

public class Excellence : VanityItem
{
    public Excellence() : base(ModContent.RarityType<PerfectRarity>(), 34, 30, Item.buyPrice(gold: 15), true) { }

    public override void Load()
    {
        if (Main.netMode == NetmodeID.Server)
            return;

        EquipLoader.AddEquipTexture(Mod, $"{Texture}_{EquipType.Head}", EquipType.Head, this);
        EquipLoader.AddEquipTexture(Mod, $"{Texture}_{EquipType.Body}", EquipType.Body, this);
        EquipLoader.AddEquipTexture(Mod, $"{Texture}_{EquipType.Legs}", EquipType.Legs, this);
    }

    public override void SetStaticDefaults()
    {
        if (Main.netMode == NetmodeID.Server)
            return;

        int head = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
        int body = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Body);
        int legs = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Legs);

        ArmorIDs.Head.Sets.DrawHead[head] = false;

        ArmorIDs.Body.Sets.HidesTopSkin[body] = true;
        ArmorIDs.Body.Sets.DisableBeltAccDraw[body] = true;
        ArmorIDs.Body.Sets.DisableHandOnAndOffAccDraw[body] = true;

        ArmorIDs.Legs.Sets.HidesTopSkin[legs] = true;
        ArmorIDs.Legs.Sets.OverridesLegs[legs] = true;
    }
}

using System;
using BlockVanity.Common.Players;
using BlockVanity.Common.Utilities;
using BlockVanity.Content.Rarities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Vanity.Excellence;

public class PhantomRuby : VanityItem
{
    public override string Texture => AllAssets.Textures.Placeholder;

    public PhantomRuby() : base(ModContent.RarityType<VanityQuestRarity>(), 24, 24, Item.buyPrice(gold: 8), true) { }

    public override void Load()
    {
        if (Main.netMode == NetmodeID.Server)
            return;

        string assetPath = $"{nameof(BlockVanity)}/Assets/Textures/Items/Vanity/SonicTheHedgehog/Sonic";
        EquipLoader.AddEquipTexture(Mod, $"{assetPath}_{EquipType.Head}", EquipType.Head, this);
        EquipLoader.AddEquipTexture(Mod, $"{assetPath}_{EquipType.Body}", EquipType.Body, this);
        EquipLoader.AddEquipTexture(Mod, $"{assetPath}_{EquipType.Legs}", EquipType.Legs, this);
    }

    public override void SetStaticDefaults()
    {
        if (Main.netMode == NetmodeID.Server)
            return;

        Item.headSlot = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
        Item.bodySlot = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Body);
        Item.legSlot = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Legs);

        ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;

        ArmorIDs.Body.Sets.HidesTopSkin[Item.bodySlot] = true;
        ArmorIDs.Body.Sets.DisableBeltAccDraw[Item.bodySlot] = true;
        ArmorIDs.Body.Sets.DisableHandOnAndOffAccDraw[Item.bodySlot] = true;

        ArmorIDs.Legs.Sets.HidesTopSkin[Item.legSlot] = true;
        ArmorIDs.Legs.Sets.OverridesLegs[Item.legSlot] = true;
    }

    public override void UpdateVanity(Player player)
    {
        player.GetModPlayer<SonicTheHedgehogPlayer>().enabled = true;
        player.GetModPlayer<MiscEffectPlayer>().disableBootsEffect = true;
        HitEffectPlayer.SetEquipHitSound(player, SoundID.NPCHit1);
    }
}


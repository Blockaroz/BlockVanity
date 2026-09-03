using BlockVanity.Common;
using BlockVanity.Common.Players;
using BlockVanity.Content.Rarities;
using BlockVanity.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Vanity.Excellence;

public class ExcellenceItem() : VanityItem(34, 30, Item.buyPrice(gold: 15), true)
{
    public override string Texture =>  Assets.Textures.Content.Vanity.Excellence.Excellence_Item.KEY;

    public override int Rarity => ModContent.RarityType<PerfectRarity>();

    public override void Load()
    {
        if (Main.dedServ)
            return;

        EquipLoader.AddEquipTexture(Mod, $"{Texture}_{EquipType.Head}", EquipType.Head, this);
        EquipLoader.AddEquipTexture(Mod, $"{Texture}_{EquipType.Body}", EquipType.Body, this);
        EquipLoader.AddEquipTexture(Mod, $"{Texture}_{EquipType.Legs}", EquipType.Legs, this);
    }

    public override void SetStaticDefaults()
    {
        ItemID.Sets.ItemNoGravity[Type] = true;

        if (Main.dedServ)
            return;

        Item.headSlot = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
        Item.bodySlot = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Body);
        Item.legSlot = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Legs);

        ArmorIDs.Head.Sets.HidesHead[Item.headSlot] = true;

        ArmorIDs.Body.Sets.HidesTopSkin[Item.bodySlot] = true;
        ArmorIDs.Body.Sets.DisableBeltAccDraw[Item.bodySlot] = true;
        ArmorIDs.Body.Sets.DisableHandOnAndOffAccDraw[Item.bodySlot] = true;

        ArmorIDs.Legs.Sets.HidesTopSkin[Item.legSlot] = true;
        ArmorIDs.Legs.Sets.HidesBottomSkin[Item.legSlot] = true;
        ArmorIDs.Legs.Sets.OverridesLegs[Item.legSlot] = true;
        ArmorIDs.Legs.Sets.IncompatibleWithFrogLeg[Item.legSlot] = true;
        BlockVanity.Sets.HideLegs[Item.legSlot] = true;
    }

    public override void UpdateVanity(Player player)
    {
        player.GetModPlayer<MiscEffectPlayer>().disableBootsEffect = true;
        player.GetModPlayer<MiscEffectPlayer>().SetWalkSpeed(0.6f);
    }

    public override void ArmorSetShadows(Player player)
    {
    }

    public override void Update(WorldItem item, ref float gravity, ref float maxFallSpeed)
    {
        item.position.Y += MathF.Sin(item.timeSinceItemSpawned * 0.05f) * 0.3f;
        Lighting.AddLight(item.Center, Color.DarkRed.ToVector3());
    }

    public override void UpdateVisibleAccessory(Player player, bool hideVisual)
    {
        if (!hideVisual)
        {
            player.GetModPlayer<ExcellencePlayer>().Enabled = true;
            player.head = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
            player.body = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Body);
            player.legs = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Legs);
        }
    }

    public override Color? GetAlpha(Color lightColor) => lightColor;

    public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
    {
        var glowTexture = Assets.Textures.Content.Vanity.Excellence.Excellence_Item_Glow.Asset;
        spriteBatch.Draw(glowTexture.Value, position, null, Color.White with { A = 10 }, 0, glowTexture.Size() * 0.5f, scale, 0, 0);
    }

    public override void PostDrawInWorld(WorldItem item, SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
    {
        var glowTexture = Assets.Textures.Content.Vanity.Excellence.Excellence_Item_Glow.Asset;
        spriteBatch.Draw(glowTexture.Value, item.Center - Main.screenPosition, null, Color.White with { A = 10 }, rotation, glowTexture.Size() * 0.5f, scale, 0, 0);
    }
}

public class ExcellencePlayer : ModPlayer
{
    public bool Enabled { get; set; }

    public override void ResetEffects()
    {
        Enabled = false;
    }

    public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
    {
        if (Enabled && drawInfo.shadow > 0f)
        {
            a = 0.33f;
            r = 1.1f;
            b = 0.3f;
            g = 0.3f;
        }
    }
}
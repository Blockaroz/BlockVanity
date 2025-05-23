﻿using BlockVanity.Common.Players;
using BlockVanity.Content.Rarities;
using BlockVanity.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Vanity.MetalSonic;

public class PhantomRuby : VanityItem
{
    public PhantomRuby() : base(ModContent.RarityType<VanityRareCommon>(), 32, 36, Item.buyPrice(gold: 8), true) { }

    public override void Load()
    {
        if (Main.netMode == NetmodeID.Server)
            return;

        string assetPath = $"{nameof(BlockVanity)}/Assets/Textures/Items/Vanity/MetalSonic/MetalSonic";
        EquipLoader.AddEquipTexture(Mod, $"{assetPath}_{EquipType.Head}", EquipType.Head, this);
        EquipLoader.AddEquipTexture(Mod, $"{assetPath}_{EquipType.Body}", EquipType.Body, this);
        EquipLoader.AddEquipTexture(Mod, $"{assetPath}_{EquipType.Legs}", EquipType.Legs, this);

        glowTexture = ModContent.Request<Texture2D>(Texture + "_Clean");
    }

    public override void SetStaticDefaults()
    {
        ItemID.Sets.AnimatesAsSoul[Type] = true;

        if (Main.netMode == NetmodeID.Server)
            return;

        Main.RegisterItemAnimation(Type, new DrawAnimationVertical(10, 5));

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
        player.GetModPlayer<MetalSonicPlayer>().enabled = true;
        player.GetModPlayer<MiscEffectPlayer>().disableBootsEffect = true;
        player.GetModPlayer<MiscEffectPlayer>().SetWalkSpeed(1.1f);
        HitEffectPlayer.SetEquipHitSound(player, SoundID.NPCHit1);
    }

    public override Color? GetAlpha(Color lightColor) => Color.Lerp(lightColor, Color.White, 0.7f) with { A = 180 };

    public static Asset<Texture2D> glowTexture;

    public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
    {
        for (int i = 0; i < 2; i++)
        {
            float progress = (Main.GlobalTimeWrappedHourly * 0.333f + i * 0.5f) % 1f;
            float glowScale = scale + (progress - 0.5f) * 0.7f;
            Color glowColor = (i == 0 ? Color.Red : Color.Cyan) with { A = 0 } * 0.5f * MathF.Sin(progress * MathHelper.Pi);
            spriteBatch.Draw(glowTexture.Value, Item.Center - Main.screenPosition, glowTexture.Frame(), glowColor, rotation, glowTexture.Size() * 0.5f, glowScale, 0, 0);
        }
    }

    public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
    {
        for (int i = 0; i < 2; i++)
        {
            float progress = (Main.GlobalTimeWrappedHourly * 0.333f + i * 0.5f) % 1f;
            float glowScale = scale + (progress - 0.5f) * 0.6f;
            Color glowColor = (i == 0 ? Color.Red : Color.Cyan) with { A = 0 } * 0.5f * MathF.Sin(progress * MathHelper.Pi);
            spriteBatch.Draw(glowTexture.Value, position, glowTexture.Frame(), glowColor, 0f, origin, glowScale, 0, 0);
        }
    }
}
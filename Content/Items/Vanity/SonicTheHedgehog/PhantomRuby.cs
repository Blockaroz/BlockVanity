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

namespace BlockVanity.Content.Items.Vanity.SonicTheHedgehog;

public class PhantomRuby : VanityItem
{
    public PhantomRuby() : base(ModContent.RarityType<CommonVanityRarity>(), 32, 36, Item.buyPrice(gold: 8), true) { }

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

        ItemID.Sets.AnimatesAsSoul[Type] = true;
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
        player.GetModPlayer<SonicTheHedgehogPlayer>().enabled = true;
        player.GetModPlayer<MiscEffectPlayer>().disableBootsEffect = true;
        HitEffectPlayer.SetEquipHitSound(player, SoundID.NPCHit1);
    }

    public override Color? GetAlpha(Color lightColor) => Color.Lerp(lightColor, Color.White, 0.7f) with { A = 180 };

    public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
    {
        Texture2D texture = TextureAssets.Item[Type].Value;
        Rectangle frame = texture.Frame(1, 5, 0, Main.itemFrame[Item.whoAmI]);
        for (int i = 0; i < 2; i++)
        {
            float progress = (Main.GlobalTimeWrappedHourly * 0.333f + i * 0.5f) % 1f;
            float glowScale = scale + (progress - 0.5f) * 0.6f;
            Color glowColor = (i == 0 ? Color.Red : Color.Cyan) with { A = 0 } * 0.5f * MathF.Sin(progress * MathHelper.Pi);
            spriteBatch.Draw(texture, Item.Center - Main.screenPosition, frame, glowColor, rotation, frame.Size() * 0.5f, glowScale, 0, 0);
        }
    }

    public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
    {
        Texture2D texture = TextureAssets.Item[Type].Value;
        for (int i = 0; i < 2; i++)
        {
            float progress = (Main.GlobalTimeWrappedHourly * 0.333f + i * 0.5f) % 1f;
            float glowScale = scale + (progress - 0.5f) * 0.6f;
            Color glowColor = (i == 0 ? Color.Red : Color.Cyan) with { A = 0 } * 0.5f * MathF.Sin(progress * MathHelper.Pi);
            spriteBatch.Draw(texture, position, frame, glowColor, 0f, origin, glowScale, 0, 0);
        }
    }
}


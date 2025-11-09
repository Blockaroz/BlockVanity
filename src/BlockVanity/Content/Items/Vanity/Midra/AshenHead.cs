using BlockVanity.Common.Players;
using BlockVanity.Common.UI;
using BlockVanity.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Vanity.Midra;

[AutoloadEquip(EquipType.Head)]
public class AshenHead : VanityItem, IUpdateArmorInVanity
{
    public AshenHead() : base(ItemRarityID.Cyan, 28, 30) { }

    public override void SetStaticDefaults()
    {
        MiscEffectPlayer.hideHead.Add(Item.headSlot);
        ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;
    }

    public override void UpdateEquip(Player player)
    {
        player.GetModPlayer<MiscEffectPlayer>().disableBootsEffect = true;
        player.GetModPlayer<MiscEffectPlayer>().SetWalkSpeed(0.5f);
    }

    public static Asset<Texture2D> glowTexture;

    public override void Load()
    {
        glowTexture = ModContent.Request<Texture2D>(Texture + "_Glow");
    }

    public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
    {
        Color darkColor = Color.DarkOrchid with { A = 100 } * 0.5f;

        for (int i = 0; i < 4; i++)
        {
            Vector2 offset = new Vector2(1f + Math.Abs(MathF.Sin(Main.GlobalTimeWrappedHourly * MathHelper.Pi)), 0).RotatedBy(i / 4f * MathHelper.TwoPi);
            spriteBatch.Draw(glowTexture.Value, position, glowTexture.Frame(), darkColor, 0, glowTexture.Size() * 0.5f, scale, 0, 0);
        }
        spriteBatch.Draw(glowTexture.Value, position, glowTexture.Frame(), Color.White with { A = 170 }, 0, glowTexture.Size() * 0.5f, scale, 0, 0);
    }

    public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
    {
        Color darkColor = Color.DarkOrchid.MultiplyRGB(lightColor) with { A = 100 } * 0.5f;

        for (int i = 0; i < 4; i++)
        {
            Vector2 offset = new Vector2(1f + Math.Abs(MathF.Sin(Main.GlobalTimeWrappedHourly * MathHelper.Pi)), 0).RotatedBy(i / 4f * MathHelper.TwoPi);
            spriteBatch.Draw(glowTexture.Value, Item.Center + offset - Main.screenPosition, glowTexture.Frame(), darkColor, rotation, glowTexture.Size() * 0.5f, scale, 0, 0);
        }
        spriteBatch.Draw(glowTexture.Value, Item.Center - Main.screenPosition, glowTexture.Frame(), Color.White with { A = 170 }, rotation, glowTexture.Size() * 0.5f, scale, 0, 0);

    }
}
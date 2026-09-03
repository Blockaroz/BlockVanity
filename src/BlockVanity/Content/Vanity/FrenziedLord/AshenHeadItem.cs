using BlockVanity.Common;
using BlockVanity.Common.Players;
using BlockVanity.Common.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Vanity.FrenziedLord;

[AutoloadEquip(EquipType.Head)]
public class AshenHeadItem() : VanityItem(width: 28, height: 30), IUpdateArmorInVanity
{
    public override string Texture => Assets.Textures.Content.Vanity.FrenziedLord.AshenHead_Item.KEY;

    public override int Rarity => ItemRarityID.Cyan;

    public override void SetStaticDefaults()
    {
        ArmorIDs.Head.Sets.HidesHead[Item.headSlot] = true;
    }

    public override void UpdateEquip(Player player)
    {
        player.GetModPlayer<MiscEffectPlayer>().disableBootsEffect = true;
        player.GetModPlayer<MiscEffectPlayer>().SetWalkSpeed(0.5f);
    }

    public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
    {
        var glowTexture = Assets.Textures.Content.Vanity.FrenziedLord.AshenHead_Item_Glow.Asset;
        Color darkColor = Color.DarkOrchid with { A = 100 } * 0.5f;

        for (int i = 0; i < 4; i++)
        {
            Vector2 offset = new Vector2(1f + Math.Abs(MathF.Sin(Main.GlobalTimeWrappedHourly * MathHelper.Pi)), 0).RotatedBy(i / 4f * MathHelper.TwoPi);
            spriteBatch.Draw(glowTexture.Value, position, null, darkColor, 0, glowTexture.Size() * 0.5f, scale, 0, 0);
        }
        spriteBatch.Draw(glowTexture.Value, position, null, Color.White with { A = 170 }, 0, glowTexture.Size() * 0.5f, scale, 0, 0);
    }

    public override void PostDrawInWorld(WorldItem item, SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
    {
        var glowTexture = Assets.Textures.Content.Vanity.FrenziedLord.AshenHead_Item_Glow.Asset;
        Color darkColor = Color.DarkOrchid.MultiplyRGB(lightColor) with { A = 100 } * 0.5f;

        for (int i = 0; i < 4; i++)
        {
            Vector2 offset = new Vector2(1f + Math.Abs(MathF.Sin(Main.GlobalTimeWrappedHourly * MathHelper.Pi)), 0).RotatedBy(i / 4f * MathHelper.TwoPi);
            spriteBatch.Draw(glowTexture.Value, item.VisualPosition + offset - Main.screenPosition, null, darkColor, rotation, glowTexture.Size() * 0.5f, scale, 0, 0);
        }
        spriteBatch.Draw(glowTexture.Value, item.VisualPosition - Main.screenPosition, null, Color.White with { A = 170 }, rotation, glowTexture.Size() * 0.5f, scale, 0, 0);
    }
}
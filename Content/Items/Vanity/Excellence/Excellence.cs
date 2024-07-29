using BlockVanity.Common.Utilities;
using Microsoft.Xna.Framework;
using ReLogic.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Vanity.Excellence;

public class Excellence : VanityItem
{
    public Excellence() : base(ItemRarityID.Gray, 34, 30, Item.buyPrice(gold: 15), true) { }

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

    public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
    {
        if (line.Name == "ItemName")
        {
            for (int i = 0; i < 8; i++)
            {
                Vector2 off = new Vector2(3).RotatedBy(i / 8f * MathHelper.TwoPi + Main.GlobalTimeWrappedHourly);
                Main.spriteBatch.DrawString(FontAssets.MouseText.Value, line.Text, new Vector2(line.X, line.Y + yOffset) + off, Color.Black * 0.4f, line.Rotation, line.Origin, line.BaseScale, 0, 0);
            }            
            for (int i = 0; i < 4; i++)
            {
                Vector2 off = new Vector2(0, 2).RotatedBy(i / 4f * MathHelper.TwoPi);
                Main.spriteBatch.DrawString(FontAssets.MouseText.Value, line.Text, new Vector2(line.X, line.Y + yOffset) + off, Color.Red, line.Rotation, line.Origin, line.BaseScale, 0, 0);
            }
            Main.spriteBatch.DrawString(FontAssets.MouseText.Value, line.Text, new Vector2(line.X, line.Y + yOffset), Color.Lerp(Color.LightCoral, Color.White, Utils.GetLerpValue(150, 255, Main.mouseTextColor, true)), line.Rotation, line.Origin, line.BaseScale, 0, 0);
            return false;
        }

        return true;
    }
}

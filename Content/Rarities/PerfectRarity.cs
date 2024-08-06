using Microsoft.Xna.Framework;
using ReLogic.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace BlockVanity.Content.Rarities;

public class PerfectRarity : ModRarity
{
    public override Color RarityColor => new Color(255, 0, 0, Main.mouseTextColor);
}

public class PerfectRarityText : GlobalItem
{
    public override bool PreDrawTooltipLine(Item item, DrawableTooltipLine line, ref int yOffset)
    {
        if (item.rare == ModContent.RarityType<PerfectRarity>() && line.Name == "ItemName")
        {
            for (int i = 0; i < 8; i++)
            {
                Vector2 off = new Vector2(3).RotatedBy(i / 8f * MathHelper.TwoPi + Main.GlobalTimeWrappedHourly);
                Main.spriteBatch.DrawString(FontAssets.MouseText.Value, line.Text, new Vector2(line.X, line.Y + yOffset) + off, Color.Black * 0.4f, line.Rotation, line.Origin, line.BaseScale, 0, 0);
            }
            for (int i = 0; i < 4; i++)
            {
                Vector2 off = Vector2.Zero;
                if (i % 2 == 0)
                    off = Vector2.UnitY * (i / 2 % 2 - 0.5f) * 3.33f;
                if (i % 2 == 1)
                    off = Vector2.UnitX * (i / 2 % 2 - 0.5f) * 3.33f;

                Main.spriteBatch.DrawString(FontAssets.MouseText.Value, line.Text, new Vector2(line.X, line.Y + yOffset) + off, Color.Red, line.Rotation, line.Origin, line.BaseScale, 0, 0);
            }

            Color whiteColor = Color.Lerp(Color.LightCoral, Color.White, MathHelper.SmoothStep(0.4f, 1f, Utils.GetLerpValue(190, 255, Main.mouseTextColor, true)));
            Main.spriteBatch.DrawString(FontAssets.MouseText.Value, line.Text, new Vector2(line.X, line.Y + yOffset), whiteColor, line.Rotation, line.Origin, line.BaseScale, 0, 0);
            return false;
        }

        return base.PreDrawTooltipLine(item, line, ref yOffset);
    }
}

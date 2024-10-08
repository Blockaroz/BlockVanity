using BlockVanity.Content.Rarities.GlowingRarities;
using Microsoft.Xna.Framework;
using ReLogic.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace BlockVanity.Content.Rarities;

public class PerfectRarity : SpecialRarity
{
    public override Color RarityColor => new Color(255, 0, 0, Main.mouseTextColor);

    public override void DrawRareLine(string text, Vector2 position, float rotation, Vector2 origin, Vector2 scale)
    {
        for (int i = 0; i < 8; i++)
        {
            Vector2 off = new Vector2(3).RotatedBy(i / 8f * MathHelper.TwoPi + Main.GlobalTimeWrappedHourly);
            Main.spriteBatch.DrawString(FontAssets.MouseText.Value, text, position + off, Color.Black * 0.4f, rotation, origin, scale, 0, 0);
        }
        for (int i = 0; i < 4; i++)
        {
            Vector2 off;
            if (i % 2 == 0)
                off = Vector2.UnitY * (i / 2 % 2 - 0.5f) * 3.33f;
            else
                off = Vector2.UnitX * (i / 2 % 2 - 0.5f) * 3.33f;

            Main.spriteBatch.DrawString(FontAssets.MouseText.Value, text, position + off, Color.Red, rotation, origin, scale, 0, 0);
        }

        Color whiteColor = Color.Lerp(Color.LightCoral, Color.White, MathHelper.SmoothStep(0.4f, 1f, Utils.GetLerpValue(190, 255, Main.mouseTextColor, true)));
        Main.spriteBatch.DrawString(FontAssets.MouseText.Value, text, position, whiteColor, rotation, origin, scale, 0, 0);

    }
}

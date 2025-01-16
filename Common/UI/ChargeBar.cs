using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI;

namespace BlockVanity.Common.UI;

public class ChargeBar : ModSystem
{
    private static int displayTime;
    private static Color baseColor;
    private static Color fillColor;
    private static float fillPercent;
    private static int barStyle;
    private static int shineTime;
    private static bool shine;

    public static void UseColors(Color first, Color fill)
    {
        baseColor = first;
        fillColor = fill;
    }

    public static void UseShineWhenFull(bool use = true)
    {
        shine = use;
    }

    public static void Display(float percent, int style = 0, int withTime = 2)
    {
        fillPercent = percent;
        barStyle = style;
        displayTime = withTime;
    }

    public override void UpdateUI(GameTime gameTime)
    {
        if (Main.gamePaused)
        {
            return;
        }

        if (displayTime > 0)
        {
            displayTime--;
        }

        if (displayTime <= 0)
        {
            shineTime = 0;
            shine = false;
            barStyle = 0;
            baseColor = Color.DimGray;
            fillColor = Color.White;
        }
    }

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        int layerIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Entity Health Bars"));
        if (layerIndex != -1)
        {
            layers.Insert(layerIndex, new LegacyGameInterfaceLayer(
                "BlockVanity: Player Overhead Bars",
                delegate
                {
                    if (displayTime > 0)
                    {
                        Texture2D bar = AllAssets.Textures.Bar[barStyle].Value;
                        Rectangle baseFrame = new Rectangle(0, 0, bar.Width, 12);

                        int fillAmount = (fillPercent > 0.99f) ? bar.Width : (int)(bar.Width * fillPercent);
                        Rectangle fillFrame = new Rectangle(0, 14, fillAmount, 12);
                        Vector2 position = (Main.LocalPlayer.Center - Main.screenPosition) / Main.UIScale - new Vector2(bar.Width / 2f, -36f / Main.UIScale);

                        Color postFillColor = fillColor;
                        switch (barStyle)
                        {
                            case 1:
                                postFillColor = Color.White;
                                fillColor = Color.CornflowerBlue;
                                break;
                        }

                        Main.spriteBatch.Draw(bar, position, baseFrame, baseColor, 0, Vector2.Zero, 1f, 0, 0);
                        Main.spriteBatch.Draw(bar, position, fillFrame, postFillColor, 0, Vector2.Zero, 1f, 0, 0);

                        if (shine && fillPercent > 0.95f)
                        {
                            if (shineTime < 12)
                            {
                                shineTime++;
                            }

                            Texture2D shine = TextureAssets.Extra[178].Value;
                            float shineProgress = shineTime / 12f;

                            Main.spriteBatch.Draw(shine, position + new Vector2(fillFrame.Width / 2, fillFrame.Height / 2), shine.Frame(), fillColor with { A = 0 } * 1.5f * (1f - shineProgress), MathHelper.Pi, shine.Size() * 0.5f, new Vector2((fillFrame.Width - 4f) / shine.Width, (fillFrame.Height - 4f) / (shine.Height)), 0, 0);
                            Main.spriteBatch.Draw(shine, position + new Vector2(fillFrame.Width / 2, fillFrame.Height / 2), shine.Frame(), fillColor with { A = 0 } * Utils.GetLerpValue(0.7f, 0f, shineProgress), 0, shine.Size() * 0.5f, new Vector2((fillFrame.Width - 4f) / shine.Width, (fillFrame.Height - 4f) / (shine.Height)), 0, 0);
                        }
                    }

                    return true;
                },
                InterfaceScaleType.UI));
        }
    }
}

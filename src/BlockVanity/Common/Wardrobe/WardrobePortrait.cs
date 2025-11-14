using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.UI;

namespace BlockVanity.Common.Wardrobe;

public readonly record struct WardrobePortraitInfo(
    QuestEntry Entry,
    Vector2 Center,
    bool Hovering,
    float Scale,
    bool SmallPortrait);

public interface IWardrobeIcon
{
    void UpdatePortrait(WardrobePortraitInfo info);

    void DrawPortrait(SpriteBatch spriteBatch, WardrobePortraitInfo info);
}

public sealed class WardrobeItemIcon(int ItemType) : IWardrobeIcon
{
    private float hoverFade;

    public void UpdatePortrait(WardrobePortraitInfo info)
    {
        if (info.Hovering)
            hoverFade = MathHelper.Lerp(hoverFade, 1f, 0.1f);
        else
            hoverFade -= 0.1f;

        hoverFade = Math.Clamp(hoverFade, 0, 1);
    }

    public void DrawPortrait(SpriteBatch spriteBatch, WardrobePortraitInfo info)
    {
        Texture2D shadow = Assets.Textures.Glow[0].Value;
        spriteBatch.Draw(shadow, info.Center, shadow.Frame(), Color.Black * 0.3f * (1f - hoverFade), 0, shadow.Size() / 2, 0.66f, 0, 0);
        
        Texture2D glow = Assets.Textures.Glow[1].Value;
        spriteBatch.Draw(glow, info.Center + new Vector2(0, 35), glow.Frame(), Main.OurFavoriteColor * 0.5f * hoverFade, Main.GlobalTimeWrappedHourly * 0.2f, glow.Size() / 2, 1f, 0, 0);

        Item item = ContentSamples.ItemsByType[ItemType];
        ItemSlot.DrawItemIcon(item, 31, spriteBatch, info.Center, info.Scale, 100f, !info.Entry.IsLocked ? Color.White : Color.Black);
    }
}

public sealed class WardrobePlayerPortrait : IWardrobeIcon
{
    public Player DisplayPlayer { get; }

    public WardrobePlayerPortrait(Player player)
    {
        DisplayPlayer = player;
        DisplayPlayer.isDisplayDollOrInanimate = true;
    }

    public void UpdatePortrait(WardrobePortraitInfo info)
    {
    }

    public void DrawPortrait(SpriteBatch spriteBatch, WardrobePortraitInfo info)
    {
        Texture2D shadow = Assets.Textures.Glow[0].Value;
        spriteBatch.Draw(shadow, info.Center, shadow.Frame(), Color.Black * 0.3f, 0, shadow.Size() / 2, 0.66f, 0, 0);

        Texture2D glow = Assets.Textures.Glow[1].Value;
        float glowPower = Utils.GetLerpValue(1.05f, 1.3f, info.Scale, true) * 0.5f;
        spriteBatch.Draw(glow, info.Center + new Vector2(0, 35), glow.Frame(), Main.OurFavoriteColor * glowPower, Main.GlobalTimeWrappedHourly * 0.2f, glow.Size() / 2, 1f, 0, 0);

        Main.PlayerRenderer.DrawPlayer(Main.Camera, DisplayPlayer, info.Center, 0, Vector2.Zero, scale: info.Scale);
    }
}
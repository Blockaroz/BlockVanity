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
    bool SmallPortrait);

public interface IWardrobePortrait
{
    void UpdatePortrait(WardrobePortraitInfo info);

    void DrawPortrait(SpriteBatch spriteBatch, WardrobePortraitInfo info);
}

public sealed class WardrobeItemPortrait(int ItemType) : IWardrobePortrait
{
    private float hoverScale = 1f;

    public void UpdatePortrait(WardrobePortraitInfo info)
    {
        if (info.Hovering)
            hoverScale = MathHelper.Lerp(hoverScale, 1.33f, 0.3f);
        else
            hoverScale = MathHelper.Lerp(hoverScale, 1f, 0.3f);
    }

    public void DrawPortrait(SpriteBatch spriteBatch, WardrobePortraitInfo info)
    {
        Texture2D shadow = Assets.Textures.Glow[0].Value;
        spriteBatch.Draw(shadow, info.Center, shadow.Frame(), Color.Black * 0.3f, 0, shadow.Size() / 2, 0.66f, 0, 0);
        
        Texture2D glow = Assets.Textures.Glow[1].Value;
        float glowPower = Utils.GetLerpValue(1.05f, 1.3f, hoverScale, true) * 0.5f;
        spriteBatch.Draw(glow, info.Center + new Vector2(0, 35), glow.Frame(), Main.OurFavoriteColor * glowPower, Main.GlobalTimeWrappedHourly * 0.2f, glow.Size() / 2, 1f, 0, 0);

        Item item = ContentSamples.ItemsByType[ItemType];
        ItemSlot.DrawItemIcon(item, 31, spriteBatch, info.Center, hoverScale, 100f, !info.Entry.IsLocked ? Color.White : Color.Black);
    }
}

public sealed class WardrobeNPCPortrait : IWardrobePortrait
{
    public NPC DisplayNPC { get; }

    public WardrobeNPCPortrait(int NPCType)
    {
        DisplayNPC = new NPC();
        DisplayNPC.SetDefaults(NPCType);
        DisplayNPC.IsABestiaryIconDummy = true;
    }

    private float hoverScale = 1f;

    public void UpdatePortrait(WardrobePortraitInfo info)
    {
        if (info.Hovering)
            hoverScale = MathHelper.Lerp(hoverScale, 1.33f, 0.3f);
        else
            hoverScale = MathHelper.Lerp(hoverScale, 1f, 0.3f);
    }

    public void DrawPortrait(SpriteBatch spriteBatch, WardrobePortraitInfo info)
    {
        Texture2D shadow = Assets.Textures.Glow[0].Value;
        spriteBatch.Draw(shadow, info.Center, shadow.Frame(), Color.Black * 0.3f, 0, shadow.Size() / 2, 0.66f, 0, 0);

        Texture2D glow = Assets.Textures.Glow[1].Value;
        float glowPower = Utils.GetLerpValue(1.05f, 1.3f, hoverScale, true) * 0.5f;
        spriteBatch.Draw(glow, info.Center + new Vector2(0, 35), glow.Frame(), Main.OurFavoriteColor * glowPower, Main.GlobalTimeWrappedHourly * 0.2f, glow.Size() / 2, 1f, 0, 0);

        DisplayNPC.Center = info.Center;
        DisplayNPC.scale = hoverScale;
        Main.instance.DrawNPCDirect(spriteBatch, DisplayNPC, false, Vector2.Zero);
    }
}

public sealed class WardrobePlayerPortrait : IWardrobePortrait
{
    public Player DisplayPlayer { get; }

    public WardrobePlayerPortrait(Player player)
    {
        DisplayPlayer = player;
        DisplayPlayer.isDisplayDollOrInanimate = true;
    }

    private float hoverScale = 1f;

    public void UpdatePortrait(WardrobePortraitInfo info)
    {
        if (info.Hovering)
            hoverScale = MathHelper.Lerp(hoverScale, 1.33f, 0.3f);
        else
            hoverScale = MathHelper.Lerp(hoverScale, 1f, 0.3f);
    }

    public void DrawPortrait(SpriteBatch spriteBatch, WardrobePortraitInfo info)
    {
        Texture2D shadow = Assets.Textures.Glow[0].Value;
        spriteBatch.Draw(shadow, info.Center, shadow.Frame(), Color.Black * 0.3f, 0, shadow.Size() / 2, 0.66f, 0, 0);

        Texture2D glow = Assets.Textures.Glow[1].Value;
        float glowPower = Utils.GetLerpValue(1.05f, 1.3f, hoverScale, true) * 0.5f;
        spriteBatch.Draw(glow, info.Center + new Vector2(0, 35), glow.Frame(), Main.OurFavoriteColor * glowPower, Main.GlobalTimeWrappedHourly * 0.2f, glow.Size() / 2, 1f, 0, 0);

        Main.PlayerRenderer.DrawPlayer(Main.Camera, DisplayPlayer, info.Center, 0, Vector2.Zero, scale: hoverScale);
    }
}
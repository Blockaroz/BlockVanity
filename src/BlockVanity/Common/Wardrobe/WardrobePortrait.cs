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

public interface IWardrobePortrait
{
    public abstract void DrawPortrait(SpriteBatch spriteBatch, QuestEntry entry, Vector2 center, bool isHovered);
}

public sealed class WardrobeItemPortrait(int ItemType) : IWardrobePortrait
{
    public void DrawPortrait(SpriteBatch spriteBatch, QuestEntry entry, Vector2 center, bool isHovered)
    {
        Item item = ContentSamples.ItemsByType[ItemType];

        ItemSlot.DrawItemIcon(item, 31, spriteBatch, center, 1f, 100f, !entry.Locked ? Color.White : Color.Black);
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

    public void DrawPortrait(SpriteBatch spriteBatch, QuestEntry entry, Vector2 center, bool isHovered)
    {
        Main.PlayerRenderer.DrawPlayer(Main.Camera, DisplayPlayer, center, 0, Vector2.Zero);
    }
}
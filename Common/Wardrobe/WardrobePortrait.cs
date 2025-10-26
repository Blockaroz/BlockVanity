using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace BlockVanity.Common.Wardrobe;

public interface IWardrobePortrait
{
    public abstract void DrawPortrait(SpriteBatch spriteBatch, Vector2 center, bool isHovered);
}

public sealed class PlayerPortrait : IWardrobePortrait
{
    public Player DisplayPlayer { get; }

    public PlayerPortrait(Player player)
    {
        DisplayPlayer = player;
        DisplayPlayer.isDisplayDollOrInanimate = true;
    }

    public void DrawPortrait(SpriteBatch spriteBatch, Vector2 center, bool isHovered)
    {
        Main.PlayerRenderer.DrawPlayer(Main.Camera, DisplayPlayer, center, 0, Vector2.Zero);
    }
}
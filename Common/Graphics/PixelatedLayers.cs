using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace BlockVanity.Common.Graphics;

public class PixelatedLayers : ILoadable
{
    public void Load(Mod mod)
    {
        On_Main.CheckMonoliths += DrawToTargets;
        Main.OnRenderTargetsInitialized += InitTargets;
        Main.OnRenderTargetsReleased += ClearTargets;
        Main.targetSet = false;

        On_Main.DrawPlayers_AfterProjectiles += DrawOverPlayers;
    }

    private void DrawOverPlayers(On_Main.orig_DrawPlayers_AfterProjectiles orig, Main self)
    {
        orig(self);

        Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.Transform);
        OverPlayers.Draw();
        Main.spriteBatch.End();
    }

    private void InitTargets(int width, int height)
    {
        OverPlayers.Init(width, height);
    }

    private void ClearTargets()
    {
        OverPlayers.Clear();
    }

    public static PixelationLayer OverPlayers = new PixelationLayer();

    private void DrawToTargets(On_Main.orig_CheckMonoliths orig)
    {
        OverPlayers.DrawToTarget(Main.spriteBatch);

        Main.instance.GraphicsDevice.SetRenderTarget(null);
        Main.instance.GraphicsDevice.Clear(Color.Transparent);

        orig();
    }

    public void Unload()
    {
    }
}

public interface IDrawPixelated
{
    public void DrawPixelated(SpriteBatch spriteBatch);
}

public class PixelationLayer
{
    private List<IDrawPixelated> draws = new List<IDrawPixelated>();

    private RenderTarget2D target;

    public void Add(IDrawPixelated draw) => draws.Add(draw);

    public void Init(int width, int height) => target = new RenderTarget2D(Main.instance.GraphicsDevice, width, height);

    public void Clear()
    {
        try
        {
            target?.Dispose();
        }
        catch (Exception ex)
        {
            Utils.LogAndChatAndConsoleInfoMessage("Pixelation layer failed to dispose.\n\n" + ex.Message);
        }

        target = null;
    }

    public void DrawToTarget(SpriteBatch spriteBatch)
    {
        Main.instance.GraphicsDevice.SetRenderTarget(target);
        Main.instance.GraphicsDevice.Clear(Color.Transparent);

        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null);

        foreach (IDrawPixelated item in draws)
            item.DrawPixelated(spriteBatch);

        spriteBatch.End();

        draws.Clear();
    }

    public void Draw()
    {
        if (target != null)
            Main.spriteBatch.Draw(target, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 3f, 0, 0);
    }
}
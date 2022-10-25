using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace BlockVanity.Common.Metaballs
{
    public class MetaballLoader : ILoadable
    {
        public void Load(Mod mod)
        {
            On.Terraria.Main.CheckMonoliths += DrawToTargets;
            metaball = new List<MetaballSystem>();
        }

        public static List<MetaballSystem> metaball;
        private Point oldScreenSize;

        private void DrawToTargets(On.Terraria.Main.orig_CheckMonoliths orig)
        {
            if (Main.gameMenu || Main.dedServ)
            {
                orig();
                return;
            }

            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);

            metaball.ForEach(n =>
            {
                if (oldScreenSize != Main.ScreenSize || n.renderTarget == null)
                {
                    if (n.renderTarget != null)
                    {
                        if (!n.renderTarget.IsDisposed)
                            n.renderTarget.Dispose();
                    }

                    n.renderTarget = new RenderTarget2D(Main.graphics.GraphicsDevice, Main.screenWidth, Main.screenHeight);
                }

                if (n.renderTarget != null)
                {
                    Main.graphics.GraphicsDevice.SetRenderTarget(n.renderTarget);
                    Main.graphics.GraphicsDevice.Clear(Color.Transparent);

                    n.DrawToTarget(Main.spriteBatch);
                    
                    Main.graphics.GraphicsDevice.SetRenderTarget(null);
                    Main.graphics.GraphicsDevice.Clear(Color.Transparent);

                }
            });

            oldScreenSize = Main.ScreenSize;

            Main.spriteBatch.End();

            orig();
        }

        public void Unload()
        {
            Main.QueueMainThreadAction(() => metaball.ForEach(m => m.renderTarget.Dispose()));
            metaball.Clear();
        }
    }
}

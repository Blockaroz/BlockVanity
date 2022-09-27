using BlockVanity.Common.Players;
using BlockVanity.Content.Particles;
using BlockVanity.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleEngine;
using System;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.TrailItems
{
    public class PrismaticRockCrystal : VanityItem, ITrailItem
    {
        public PrismaticRockCrystal() : base("Prismatic Rock Crystal", ItemRarityID.Expert, "A streak of color follows behind you", true) { }

        public override Color? GetAlpha(Color lightColor) => new Color(255, 255, 255, 200);

        public void UpdateParticleEffect(Player player, int slot, ArmorShaderData dye)
        {
            if (player.velocity.Length() > 1.5f)
            {
                Color starColor = Main.hslToRgb(Main.GlobalTimeWrappedHourly * 2 % 1f, 1f, 0.6f) * 0.5f;
                if (dye != null)
                    starColor = Main.hslToRgb(Main.GlobalTimeWrappedHourly * 2 % 1f, 0.3f, 0.6f) * 0.5f;
                //starColor.A /= 2;
                Lighting.AddLight(player.MountedCenter, starColor.ToVector3() * 0.5f);
                if (Main.rand.NextBool(5))
                {
                    Particle star = Particle.NewParticle(Particle.ParticleType<PrettySparkle>(), player.Center + Main.rand.NextVector2Circular(17, 17), player.velocity * 0.1f, starColor, Main.rand.NextFloat());
                    star.shader = dye;
                    star.emit = true;
                }
            }
        }

        public override void Load() => On.Terraria.Graphics.Renderers.LegacyPlayerRenderer.DrawPlayerFull += DrawRainbowTrail;

        private void DrawRainbowTrail(On.Terraria.Graphics.Renderers.LegacyPlayerRenderer.orig_DrawPlayerFull orig, Terraria.Graphics.Renderers.LegacyPlayerRenderer self, Camera camera, Player drawPlayer)
        {
            int prismIndex = drawPlayer.armor.ToList().FindIndex(n => n.type == ModContent.ItemType<PrismaticRockCrystal>());
            if (drawPlayer.armor.Any(n => n.type == ModContent.ItemType<PrismaticRockCrystal>()) && !drawPlayer.ShouldNotDraw && !Main.gameMenu)
            {
                OldPosPlayer oldPlayer = drawPlayer.GetModPlayer<OldPosPlayer>();

                VertexStrip strip = new VertexStrip();

                int length = 5 + (int)(prismIndex % 10f / 10f * 25f);

                Vector2[] oldPos = new Vector2[length];
                for (int i = 0; i < length; i++)
                    oldPos[i] = oldPlayer.oldPos[i];

                oldPos[0] = drawPlayer.MountedCenter + drawPlayer.velocity * 0.06f;

                Effect shader = Mod.Assets.Request<Effect>("Assets/Effects/PrismaticRockTrail", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
                shader.Parameters["transformMatrix"].SetValue(Main.GameViewMatrix.NormalizedTransformationmatrix);
                shader.Parameters["uStreak0"].SetValue(TextureAssets.Extra[189].Value);
                shader.Parameters["uStreak1"].SetValue(TextureAssets.Extra[193].Value);
                shader.Parameters["uTime"].SetValue(Main.GlobalTimeWrappedHourly * 0.75f % 1f);
                shader.Parameters["uGlow"].SetValue(0.5f);
                shader.Parameters["uOpacity"].SetValue(0.8f);

                shader.CurrentTechnique.Passes[0].Apply();
                 
                ArmorShaderData dye = GameShaders.Armor.GetShaderFromItemId(drawPlayer.dye[prismIndex % 10].type);
                //if (dye != null)
                //    dye.Apply();

                float stripVelocity = Utils.GetLerpValue(-4, 8, drawPlayer.velocity.Length(), true);
                VertexStrip.StripHalfWidthFunction widthFunction = (float progress) => 33 * stripVelocity * (float)Math.Sqrt(Utils.GetLerpValue(-0.06f, 0.04f, progress, true));

                strip.PrepareStrip(oldPos, oldPlayer.oldRot, (float progress) => Main.hslToRgb((Main.GlobalTimeWrappedHourly - progress * 0.66f) % 1f, 1f, 0.8f), widthFunction, -Main.screenPosition, length, true);
                strip.DrawTrail();

                Main.pixelShader.CurrentTechnique.Passes[0].Apply();

            }

            orig(self, camera, drawPlayer);
        }
    }
}

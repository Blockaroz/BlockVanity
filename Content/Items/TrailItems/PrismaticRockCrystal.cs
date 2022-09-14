using BlockVanity.Common.Players;
using BlockVanity.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using static tModPorter.ProgressUpdate;

namespace BlockVanity.Content.Items.TrailItems
{
    public class PrismaticRockCrystal : VanityItem, ITrailItem
    {
        public PrismaticRockCrystal() : base("Prismatic Rock Crystal", ItemRarityID.Expert, "A streak of color follows behind you", true) { }

        public override Color? GetAlpha(Color lightColor) => new Color(255, 255, 255, 200);

        public void UpdateParticleEffect(Player player, int slot, ArmorShaderData dye)
        {
            if (player.velocity.Length() > 1.5f && Main.rand.NextBool(7))
            {
                Color starColor = Main.hslToRgb(Main.GlobalTimeWrappedHourly % 1f, 1f, 0.7f);
                starColor.A = 0;
                //Particle star = Particle.NewParticle(Particle.ParticleType<DreamEssence>(), player.Center + Main.rand.NextVector2Circular(20, 20), -player.velocity * 0.1f, starColor, Main.rand.NextFloat(1.5f));
                //star.shader = dye;
                //star.emit = true;
            }
        }

        public override void Load()
        {
            On.Terraria.Graphics.Renderers.LegacyPlayerRenderer.DrawPlayerFull += LegacyPlayerRenderer_DrawPlayerFull;
        }

        private void LegacyPlayerRenderer_DrawPlayerFull(On.Terraria.Graphics.Renderers.LegacyPlayerRenderer.orig_DrawPlayerFull orig, Terraria.Graphics.Renderers.LegacyPlayerRenderer self, Camera camera, Player drawPlayer)
        {
            int prismIndex = drawPlayer.armor.ToList().FindIndex(n => n.type == ModContent.ItemType<PrismaticRockCrystal>());
            if (drawPlayer.armor.Any(n => n.type == ModContent.ItemType<PrismaticRockCrystal>()) && !Main.gameMenu)
            {
                OldPosPlayer oldPlayer = drawPlayer.GetModPlayer<OldPosPlayer>();

                VertexStrip strip = new VertexStrip();

                float stripVelocity = Utils.GetLerpValue(-8, 8, drawPlayer.velocity.Length(), true);
                VertexStrip.StripHalfWidthFunction widthFunction = (float progress) => 30 * stripVelocity * Utils.GetLerpValue(1f, 0.1f, progress, true) * (float)Math.Sqrt(Utils.GetLerpValue(-0.2f, 0.2f, progress, true));


                Effect shader = Mod.Assets.Request<Effect>("Assets/Effects/PrismaticRockTrail", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
                shader.Parameters["transformMatrix"].SetValue(Main.GameViewMatrix.NormalizedTransformationmatrix);
                shader.Parameters["uStreak0"].SetValue(TextureAssets.Extra[197].Value);
                shader.Parameters["uStreak1"].SetValue(TextureAssets.Extra[189].Value);
                shader.Parameters["uTime"].SetValue(Main.GlobalTimeWrappedHourly % 1f);

                shader.CurrentTechnique.Passes[0].Apply();

                int length = (int)MathHelper.Lerp(0, 32, prismIndex % 10f / 10f);

                strip.PrepareStrip(oldPlayer.oldPos, oldPlayer.oldRot, (float progress) => Main.hslToRgb(progress + Main.GlobalTimeWrappedHourly % 1f, 1f, 0.75f, 0) * (1f - progress), widthFunction, -Main.screenPosition, length, true);
                //strip.DrawTrail();

                Main.pixelShader.CurrentTechnique.Passes[0].Apply();
            }

            orig(self, camera, drawPlayer);
        }
    }

    public class PrismaticRockCrystalTrail : PlayerDrawLayer
    {
        public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.Wings);

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            if (drawInfo.shadow != 0f || drawInfo.drawPlayer.velocity.Length() < 0.1f)
                return;

            int prismIndex = drawInfo.drawPlayer.armor.ToList().FindIndex(n => n.type == ModContent.ItemType<PrismaticRockCrystal>());
            int length = (int)MathHelper.Lerp(0, 32, prismIndex % 10f / 10f);
            int shadowCount = Math.Min(drawInfo.drawPlayer.availableAdvancedShadowsCount - 1, length);

            for (int i = shadowCount; i > 0; i--)
            {
                EntityShadowInfo shadowInfo = drawInfo.drawPlayer.GetAdvancedShadow(i);
                EntityShadowInfo shadowInfoNext = drawInfo.drawPlayer.GetAdvancedShadow(i - 1);

                Color drawColor = Main.hslToRgb((i / (float)shadowCount) + Main.GlobalTimeWrappedHourly % 1f, 1f, 0.75f, 0) * (0.9f - (i / (float)shadowCount));
                Vector2 drawPos = shadowInfo.Position + drawInfo.drawPlayer.Size / 2f;
                DrawData trailData = new DrawData(TextureAssets.MagicPixel.Value, drawPos - Main.screenPosition, new Rectangle(0, 0, drawInfo.drawPlayer.width, drawInfo.drawPlayer.height - 6), drawColor * 0.6f, drawInfo.rotation, new Vector2(drawInfo.drawPlayer.width / 2f, drawInfo.drawPlayer.height / 2f - 5), 1f, drawInfo.playerEffect, 0);
                trailData.shader = GameShaders.Armor.GetShaderIdFromItemId(drawInfo.drawPlayer.dye[prismIndex % 10].type);
                drawInfo.DrawDataCache.Add(trailData);
            }
        }
    }
}

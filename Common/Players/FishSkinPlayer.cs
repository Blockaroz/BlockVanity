using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlockVanity.Common.Systems.Players;
using BlockVanity.Common.Utilities;
using BlockVanity.Content.Items.Vanity.Excellence;
using BlockVanity.Content.Items.Vanity.Myrtle;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Common.Players;

public class FishSkinPlayer : ModPlayer
{
    public bool enabled;
    public int fishSkinStyle;
    public Vector2 headFinVector;

    private SlowAsset<Texture2D>[] FishSkinTextures => fishSkinStyle switch
    {
        _ => AllAssets.Textures.BlueFishSkin
    };

    public static class FishSkinID
    {
        public static readonly int 
            Head = 0,
            EarsHigh = 1,
            EarsLow = 2,
            Eyes = 3,
            Body = 4,
            Legs = 5;
    }

    public override void Load()
    {
        On_PlayerDrawLayers.DrawPlayer_12_Skin += DrawFishSkin_BodyLegs;
        On_PlayerDrawLayers.DrawPlayer_12_Skin_Composite += DrawFishSkin_Composite;
        On_PlayerDrawLayers.DrawPlayer_21_Head_TheFace += DrawFishSkin_Face;
        IL_PlayerDrawLayers.DrawPlayer_21_Head += DrawFishSkin_Ears;
        On_Player.UpdateVisibleAccessory += EnableFishSkin;
    }

    private void DrawFishSkin_Ears(ILContext il)
    {
        try
        {
            ILCursor c = new ILCursor(il);

            c.TryGotoNext(i => i.MatchLdcI4(270));
            c.TryGotoPrev(i => i.MatchLdarg0());
            c.Index++;

            c.EmitDelegate((ref PlayerDrawSet drawinfo) =>
            {
                if (!drawinfo.drawPlayer.invis && drawinfo.drawPlayer.GetModPlayer<FishSkinPlayer>().enabled)
                {
                    Vector2 headPos = drawinfo.HeadPosition() - Vector2.UnitY * 2;
                    headPos.ApplyVerticalOffset(drawinfo);

                    Vector2 finVector = drawinfo.drawPlayer.GetModPlayer<FishSkinPlayer>().headFinVector;
                    float earsRotation = finVector.Y * 0.04f * drawinfo.drawPlayer.direction;
                    earsRotation = Math.Clamp(earsRotation, -0.2f, 0.2f);
                    float upperEarWobble = MathF.Sin(drawinfo.drawPlayer.miscCounter * 0.2f % MathHelper.Pi) * 0.1f * Math.Clamp(Math.Abs(finVector.X) * 0.5f + Math.Abs(finVector.Y) * 0.1f, 0, 1f);
                    float lowerEarWobble = MathF.Sin((drawinfo.drawPlayer.miscCounter * 0.2f - 0.8f) % MathHelper.Pi) * 0.1f * Math.Clamp(Math.Abs(finVector.X) * 0.5f + Math.Abs(finVector.Y) * 0.1f, 0, 1f);

                    if (drawinfo.hatHair || drawinfo.fullHair || drawinfo.drawPlayer.head < 0)
                    {
                        Texture2D earsLowTexture = AllAssets.Textures.BlueFishSkin[FishSkinID.EarsLow].Value;
                        DrawData earData = new DrawData(earsLowTexture, headPos, earsLowTexture.Frame(), drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation + earsRotation + lowerEarWobble, drawinfo.headVect, 1f, drawinfo.playerEffect, 0);
                        earData.shader = drawinfo.skinDyePacked;
                        drawinfo.DrawDataCache.Add(earData);

                        if (drawinfo.drawPlayer.head > 0)
                            return;

                        Texture2D earsHighTexture = AllAssets.Textures.BlueFishSkin[FishSkinID.EarsHigh].Value;
                        earData = new DrawData(earsHighTexture, headPos, earsHighTexture.Frame(), drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation + earsRotation + upperEarWobble, drawinfo.headVect, 1f, drawinfo.playerEffect, 0);
                        earData.shader = drawinfo.skinDyePacked;
                        drawinfo.DrawDataCache.Add(earData);
                    }
                }
            });
            c.EmitLdarg0();

            c.TryGotoNext(i => i.MatchLdfld<Player>("beard"));
            c.TryGotoPrev(i => i.MatchLdarg0());
            c.Index++;

            c.EmitDelegate((ref PlayerDrawSet drawinfo) =>
            {
                if (!drawinfo.drawPlayer.invis && drawinfo.drawPlayer.GetModPlayer<FishSkinPlayer>().enabled)
                {
                    Vector2 headPos = drawinfo.HeadPosition() - Vector2.UnitY * 2;
                    headPos.ApplyVerticalOffset(drawinfo);

                    Vector2 finVector = drawinfo.drawPlayer.GetModPlayer<FishSkinPlayer>().headFinVector;
                    float earsRotation = finVector.Y * 0.04f * drawinfo.drawPlayer.direction;
                    earsRotation = Math.Clamp(earsRotation, -0.2f, 0.2f);
                    float upperEarWobble = MathF.Sin(drawinfo.drawPlayer.miscCounter * 0.2f % MathHelper.Pi) * 0.1f * Math.Clamp(Math.Abs(finVector.X) * 0.5f + Math.Abs(finVector.Y) * 0.1f, 0, 1f);
                    float lowerEarWobble = MathF.Sin((drawinfo.drawPlayer.miscCounter * 0.2f - 0.8f) % MathHelper.Pi) * 0.1f * Math.Clamp(Math.Abs(finVector.X) * 0.5f + Math.Abs(finVector.Y) * 0.1f, 0, 1f);

                    if (drawinfo.hatHair || drawinfo.fullHair || drawinfo.drawPlayer.head < 0)
                    {
                        Texture2D earsLowTexture = AllAssets.Textures.BlueFishSkin[FishSkinID.EarsLow].Value;
                        DrawData earData = new DrawData(earsLowTexture, headPos, earsLowTexture.Frame(), drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation + earsRotation + lowerEarWobble, drawinfo.headVect, 1f, drawinfo.playerEffect, 0);
                        earData.shader = drawinfo.skinDyePacked;
                        drawinfo.DrawDataCache.Add(earData);

                        Texture2D earsHighTexture = AllAssets.Textures.BlueFishSkin[FishSkinID.EarsHigh].Value;
                        earData = new DrawData(earsHighTexture, headPos, earsHighTexture.Frame(), drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation + earsRotation + upperEarWobble, drawinfo.headVect, 1f, drawinfo.playerEffect, 0);
                        earData.shader = drawinfo.skinDyePacked;
                        drawinfo.DrawDataCache.Add(earData);
                    }
                }
            });
            c.EmitLdarg0();
        }
        catch
        {
            MonoModHooks.DumpIL(Mod, il);
        }
    }

    private void DrawFishSkin_Face(On_PlayerDrawLayers.orig_DrawPlayer_21_Head_TheFace orig, ref PlayerDrawSet drawinfo)
    {
        if (drawinfo.drawPlayer.GetModPlayer<FishSkinPlayer>().enabled)
        {
            bool flag = drawinfo.drawPlayer.head > 0 && !ArmorIDs.Head.Sets.DrawHead[drawinfo.drawPlayer.head];

            if (!flag && drawinfo.drawPlayer.faceHead > 0)
            {
                Vector2 faceHeadOffsetFromHelmet = drawinfo.drawPlayer.GetFaceHeadOffsetFromHelmet();
                DrawData item = new DrawData(TextureAssets.AccFace[drawinfo.drawPlayer.faceHead].Value, new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect + faceHeadOffsetFromHelmet, drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
                item.shader = drawinfo.cFaceHead;
                drawinfo.DrawDataCache.Add(item);
                if (drawinfo.drawPlayer.face <= 0 || !ArmorIDs.Face.Sets.DrawInFaceUnderHairLayer[drawinfo.drawPlayer.face])
                    return;

                float num = 0f;
                if (drawinfo.drawPlayer.face == 5)
                {
                    int faceHead = drawinfo.drawPlayer.faceHead;
                    if ((uint)(faceHead - 10) <= 3u)
                        num = 2 * drawinfo.drawPlayer.direction;
                }

                item = new DrawData(TextureAssets.AccFace[drawinfo.drawPlayer.face].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)) + num, (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
                item.shader = drawinfo.cFace;
                drawinfo.DrawDataCache.Add(item);
            }
            else if (!drawinfo.drawPlayer.invis && !flag)
            {
                Color headColor = drawinfo.colorArmorHead;

                DrawData headData = new DrawData(FishSkinTextures[FishSkinID.Head].Value, new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.drawPlayer.bodyFrame, headColor, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
                headData.shader = drawinfo.skinDyePacked;
                DrawData newData = headData;

                Color eyeColor = new Color(
                    Math.Clamp(drawinfo.colorArmorHead.R / 255f * 3f, 0, 1f), 
                    Math.Clamp(drawinfo.colorArmorHead.G / 255f * 3f, 0, 1f), 
                    Math.Clamp(drawinfo.colorArmorHead.B / 255f * 3f, 0, 1f), 
                    drawinfo.colorArmorHead.A / 255f);

                drawinfo.DrawDataCache.Add(newData);
                newData = new DrawData(AllAssets.Textures.FishEyes.Value, new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.drawPlayer.bodyFrame, eyeColor, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
                drawinfo.DrawDataCache.Add(newData);
                
                Texture2D eyelids = FishSkinTextures[FishSkinID.Eyes].Value;
                Vector2 headOffset = Main.OffsetsPlayerHeadgear[drawinfo.drawPlayer.bodyFrame.Y / drawinfo.drawPlayer.bodyFrame.Height];
                headOffset.Y -= 2f;
                headOffset *= (float)(-drawinfo.playerEffect.HasFlag(SpriteEffects.FlipVertically).ToDirectionInt());
                Rectangle value = eyelids.Frame(1, 3, 0, (int)drawinfo.drawPlayer.eyeHelper.CurrentEyeFrame);
                headData = new DrawData(eyelids, new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect + headOffset, value, headColor, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
                headData.shader = drawinfo.skinDyePacked;
                newData = headData;
                drawinfo.DrawDataCache.Add(newData);

                if (drawinfo.drawPlayer.yoraiz0rDarkness)
                {
                    headData = new DrawData(TextureAssets.Extra[67].Value, new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
                    headData.shader = drawinfo.skinDyePacked;
                    newData = headData;
                    drawinfo.DrawDataCache.Add(newData);
                }

                if (drawinfo.drawPlayer.face > 0 && ArmorIDs.Face.Sets.DrawInFaceUnderHairLayer[drawinfo.drawPlayer.face])
                {
                    newData = new DrawData(TextureAssets.AccFace[drawinfo.drawPlayer.face].Value, new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
                    newData.shader = drawinfo.cFace;
                    drawinfo.DrawDataCache.Add(newData);
                }
            }
        }
        else
            orig(ref drawinfo);
    }

    private void DrawFishSkin_Composite(On_PlayerDrawLayers.orig_DrawPlayer_12_Skin_Composite orig, ref PlayerDrawSet drawinfo)
    {
        if (drawinfo.drawPlayer.GetModPlayer<FishSkinPlayer>().enabled)
        {
        }
        else
            orig(ref drawinfo);
    }

    private void DrawFishSkin_BodyLegs(On_PlayerDrawLayers.orig_DrawPlayer_12_Skin orig, ref PlayerDrawSet drawinfo)
    {
        if (drawinfo.drawPlayer.GetModPlayer<FishSkinPlayer>().enabled)
        {
            if (drawinfo.usesCompositeTorso)
            {
                PlayerDrawLayers.DrawPlayer_12_Skin_Composite(ref drawinfo);
                return;
            }

            if (drawinfo.isSitting)
                drawinfo.hidesBottomSkin = true;

            if (!drawinfo.hidesTopSkin)
            {
                drawinfo.Position.Y += drawinfo.torsoOffset;
                DrawData drawData = new DrawData(TextureAssets.Players[drawinfo.skinVar, 3].Value, new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2(drawinfo.drawPlayer.bodyFrame.Width / 2, drawinfo.drawPlayer.bodyFrame.Height / 2), drawinfo.drawPlayer.bodyFrame, drawinfo.colorBodySkin, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
                drawData.shader = drawinfo.skinDyePacked;
                DrawData item = drawData;
                drawinfo.DrawDataCache.Add(item);
                drawinfo.Position.Y -= drawinfo.torsoOffset;
            }

            if (!drawinfo.hidesBottomSkin && !drawinfo.isBottomOverriden)
            {
                if (drawinfo.isSitting)
                {
                    VanityUtils.DrawSittingLegs(ref drawinfo, TextureAssets.Players[drawinfo.skinVar, 10].Value, drawinfo.colorLegs);
                    return;
                }

                DrawData item = new DrawData(TextureAssets.Players[drawinfo.skinVar, 10].Value, new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2(drawinfo.drawPlayer.bodyFrame.Width / 2, drawinfo.drawPlayer.bodyFrame.Height / 2), drawinfo.drawPlayer.legFrame, drawinfo.colorLegs, drawinfo.drawPlayer.legRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
                drawinfo.DrawDataCache.Add(item);
            }
        }
        else
            orig(ref drawinfo);
    }

    private void EnableFishSkin(On_Player.orig_UpdateVisibleAccessory orig, Player self, int itemSlot, Item item, bool modded)
    {
        orig(self, itemSlot, item, modded);

        if (item.ModItem is FishFood fishFood)
            self.GetModPlayer<FishSkinPlayer>().enabled = true;
    }

    public override void PostUpdateMiscEffects()
    {
        headFinVector.X = MathHelper.Lerp(headFinVector.X, Player.velocity.X, 0.07f);
        headFinVector.Y = MathHelper.Lerp(headFinVector.Y, Player.velocity.Y, 0.3f);

        headFinVector.X += Utils.GetLerpValue(0.5f, 0.96f, MathF.Sin(Player.miscCounterNormalized * 10), true) * Player.direction * 0.1f;
    }

    public override void ResetEffects()
    {
        fishSkinStyle = 0;
        enabled = false;
    }
}

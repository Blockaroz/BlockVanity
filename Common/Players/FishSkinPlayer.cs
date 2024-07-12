using System;
using BlockVanity.Content.Items.Vanity.Myrtle;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
    public int skinStyle;
    public Vector2 headFinVector;
    public float tailCounter;
    public float[] tailRotations;
    private bool Male;

    public Asset<Texture2D>[] SkinTextures => skinStyle switch
    {
        (int)SkinStyle.BlueFish => AllAssets.Textures.BlueFishSkin,
        _ => AllAssets.Textures.BlueFishSkin
    };

    public enum SkinStyle
    {
        BlueFish
    }

    public override void Load()
    {
        On_PlayerDrawLayers.DrawPlayer_21_Head_TheFace += DrawFishSkin_SpecialFace;
        IL_PlayerDrawLayers.DrawPlayer_21_Head += DrawFishSkin_Ears;
        On_Player.UpdateVisibleAccessory += EnableSkins;

        ReskinPlayer.OnPreSetSkin += SetSkin;
        ReskinPlayer.OnPreResetSkins += ResetSkin;
    }

    private void SetSkin(ref PlayerDrawSet drawInfo)
    {
        FishSkinPlayer fishPlayer = drawInfo.drawPlayer.GetModPlayer<FishSkinPlayer>();

        if (fishPlayer.enabled)
        {
            switch (fishPlayer.skinStyle)
            {
                default:
                case (int)SkinStyle.BlueFish:
                    fishPlayer.Male = drawInfo.drawPlayer.Male;
                    drawInfo.drawPlayer.Male = false;

                    drawInfo.colorHead = drawInfo.colorArmorHead;
                    drawInfo.colorBodySkin = drawInfo.colorArmorBody;
                    drawInfo.colorLegs = drawInfo.colorArmorLegs;
                    break;
            }

            drawInfo.drawPlayer.GetSkinPlayer().SetSkin(SkinTextures);
        }
    }

    private void ResetSkin(ref PlayerDrawSet drawInfo)
    {
        if (drawInfo.drawPlayer.GetModPlayer<FishSkinPlayer>().enabled)
            drawInfo.drawPlayer.Male = drawInfo.drawPlayer.GetModPlayer<FishSkinPlayer>().Male;
    }

    private void DrawFishSkin_Ears(ILContext il)
    {
        try
        {
            ILCursor c = new ILCursor(il);

            c.TryGotoNext(i => i.MatchLdcI4(270));
            c.TryGotoPrev(i => i.MatchLdarg0());
            c.Index++;

            c.EmitDelegate((ref PlayerDrawSet drawInfo) => DrawEars(ref drawInfo, false));
            c.EmitLdarg0();

            c.TryGotoNext(i => i.MatchLdfld<Player>("beard"));
            c.TryGotoPrev(i => i.MatchLdarg0());
            c.Index++;

            c.EmitDelegate((ref PlayerDrawSet drawInfo) => DrawEars(ref drawInfo, true));
            c.EmitLdarg0();
        }
        catch
        {
            MonoModHooks.DumpIL(Mod, il);
        }
    }

    private void DrawEars(ref PlayerDrawSet drawinfo, bool upperEars = true)
    {
        if (!drawinfo.drawPlayer.invis && drawinfo.drawPlayer.GetModPlayer<FishSkinPlayer>().enabled)
        {
            Vector2 headPos = drawinfo.HeadPosition() - Vector2.UnitY * 2;
            headPos.ApplyVerticalOffset(drawinfo);

            Vector2 finVector = drawinfo.drawPlayer.GetModPlayer<FishSkinPlayer>().headFinVector;
            float upperEarWobble = drawinfo.drawPlayer.velocity.Y * drawinfo.drawPlayer.direction * 0.02f + MathF.Sin(drawinfo.drawPlayer.miscCounter * 0.25f % MathHelper.Pi) * 0.1f * Math.Clamp(Math.Abs(finVector.Y) * 0.1f, 0, 1f) * drawinfo.drawPlayer.direction + finVector.X * 0.15f;
            float lowerEarWobble = drawinfo.drawPlayer.velocity.Y * drawinfo.drawPlayer.direction * 0.01f + MathF.Sin((drawinfo.drawPlayer.miscCounter * 0.25f - 0.6f) % MathHelper.Pi) * 0.1f * Math.Clamp(Math.Abs(finVector.Y) * 0.1f, 0, 1f) * drawinfo.drawPlayer.direction + finVector.X * 0.1f;
            upperEarWobble = Math.Clamp(upperEarWobble, -0.2f, 0.2f);
            lowerEarWobble = Math.Clamp(lowerEarWobble, -0.2f, 0.2f);

            if (drawinfo.hatHair || drawinfo.fullHair || drawinfo.drawPlayer.head < 0)
            {
                Texture2D earsLowTexture = drawinfo.drawPlayer.GetModPlayer<FishSkinPlayer>().SkinTextures[(int)ReskinPlayer.SkinID.EarsLow].Value;
                DrawData earData = new DrawData(earsLowTexture, headPos, earsLowTexture.Frame(), drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation + lowerEarWobble, drawinfo.headVect, 1f, drawinfo.playerEffect, 0);
                earData.shader = drawinfo.skinDyePacked;
                drawinfo.DrawDataCache.Add(earData);

                if ((upperEars && drawinfo.drawPlayer.head > 0) || drawinfo.hatHair)
                    return;

                Texture2D earsHighTexture = drawinfo.drawPlayer.GetModPlayer<FishSkinPlayer>().SkinTextures[(int)ReskinPlayer.SkinID.EarsHigh].Value;
                earData = new DrawData(earsHighTexture, headPos, earsHighTexture.Frame(), drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation + upperEarWobble, drawinfo.headVect, 1f, drawinfo.playerEffect, 0);
                earData.shader = drawinfo.skinDyePacked;
                drawinfo.DrawDataCache.Add(earData);
            }
        }
    }

    private void DrawFishSkin_SpecialFace(On_PlayerDrawLayers.orig_DrawPlayer_21_Head_TheFace orig, ref PlayerDrawSet drawinfo)
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

                DrawData headData = new DrawData(TextureAssets.Players[drawinfo.skinVar, 0].Value, new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.drawPlayer.bodyFrame, headColor, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
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
                
                Texture2D eyelids = drawinfo.drawPlayer.GetModPlayer<FishSkinPlayer>().SkinTextures[(int)ReskinPlayer.SkinID.Eyes].Value;
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

    private void EnableSkins(On_Player.orig_UpdateVisibleAccessory orig, Player self, int itemSlot, Item item, bool modded)
    {
        orig(self, itemSlot, item, modded);

        if (item.ModItem is FishFood fishFood)
        {
            self.GetSkinPlayer().enabled = true;

            FishSkinPlayer fishPlayer = self.GetModPlayer<FishSkinPlayer>();
            fishPlayer.enabled = true;
            fishPlayer.skinStyle = (int)SkinStyle.BlueFish;

            fishPlayer.headFinVector.Y -= Utils.GetLerpValue(0.3f, 0.96f, MathF.Sin(self.miscCounterNormalized * 10), true);
            float bounce = Main.OffsetsPlayerHeadgear[self.bodyFrame.Y / self.bodyFrame.Height].Y;
            if (bounce < 1)
                fishPlayer.headFinVector.X += self.direction * 0.5f;


            fishPlayer.tailCounter += Math.Abs(self.velocity.X) / (Main.OffsetsPlayerHeadgear.Length - 1f);
            fishPlayer.tailCounter %= MathHelper.TwoPi;
            fishPlayer.tailRotations ??= new float[3];

            if (Math.Abs(self.velocity.X) < 0.5f || Math.Abs(self.velocity.Y) > 0f)
            {
                float[] wobble = [
                    MathF.Sin(self.miscCounter / 150f * MathHelper.TwoPi * 2f),
                    MathF.Sin(self.miscCounter / 150f * MathHelper.TwoPi * 2f - 2f),
                    MathF.Sin(self.miscCounter / 150f * MathHelper.TwoPi * 2f - 3f)
                    ];
                fishPlayer.tailRotations[0] = MathHelper.Lerp(fishPlayer.tailRotations[0], (wobble[0] * 0.05f + self.velocity.Y * 0.04f) * self.direction, 0.1f);
                fishPlayer.tailRotations[1] = MathHelper.Lerp(fishPlayer.tailRotations[1], (wobble[1] * 0.057f + self.velocity.Y * 0.04f) * self.direction, 0.1f);
                fishPlayer.tailRotations[2] = MathHelper.Lerp(fishPlayer.tailRotations[2], (wobble[2] * 0.1f + self.velocity.Y * 0.04f) * self.direction, 0.1f);
                fishPlayer.tailCounter = MathHelper.Lerp(fishPlayer.tailCounter, 0f, 0.05f);
            }
            else
            {
                fishPlayer.tailRotations[0] = MathHelper.Lerp(fishPlayer.tailRotations[0], MathF.Sin(fishPlayer.tailCounter) * 0.1f * self.direction, 0.3f);
                fishPlayer.tailRotations[1] = MathHelper.Lerp(fishPlayer.tailRotations[1], MathF.Sin(fishPlayer.tailCounter - 1.5f) * 0.2f * self.direction, 0.3f);
                fishPlayer.tailRotations[2] = MathHelper.Lerp(fishPlayer.tailRotations[2], MathF.Sin(fishPlayer.tailCounter - 2.5f) * 0.3f * self.direction, 0.3f);
            }

            fishPlayer.headFinVector.X = MathHelper.Lerp(fishPlayer.headFinVector.X, 0f, 0.4f);
            fishPlayer.headFinVector.Y = MathHelper.Lerp(fishPlayer.headFinVector.Y, self.velocity.Y, 0.3f);
        }
    }

    public override void ResetEffects()
    {
        if (headFinVector.HasNaNs())
            headFinVector = Vector2.Zero;

        enabled = false;
        skinStyle = 0;
    }
}

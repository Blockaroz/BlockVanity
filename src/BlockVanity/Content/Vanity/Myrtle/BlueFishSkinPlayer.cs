using BlockVanity.Common.Players;
using BlockVanity.Common.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Vanity.Myrtle;

public class BlueFishSkinPlayer : ModPlayer
{
    public bool enabled;
    public Vector2 headFinVector;
    public float tailCounter;
    public float[] tailRotations;

    public static Asset<Texture2D>[] SkinTextures;
    public static Asset<Texture2D>[] EarTextures;
    public static Asset<Texture2D> TailTexture;
    public static Asset<Texture2D> GlowyEyesTexture;

    public override void Load()
    {
        On_PlayerDrawLayers.DrawPlayer_21_Head_TheFace += DrawFishSkin_SpecialFace;
        IL_PlayerDrawLayers.DrawPlayer_21_Head += DrawFishSkin_Ears;

        ReskinPlayer.OnSetNewSkin += SetSkin;

        const string assetsPath = $"{nameof(BlockVanity)}/Assets/Textures/Skins/BlueFishSkin/BlueFishSkin_";
        SkinTextures = VanityUtils.GetSkinTextures(assetsPath);
        EarTextures = [
            ModContent.Request<Texture2D>(assetsPath + "Ears_High"),
            ModContent.Request<Texture2D>(assetsPath + "Ears_Low"),
            ];
        TailTexture = ModContent.Request<Texture2D>(assetsPath + "Tail");
        GlowyEyesTexture = ModContent.Request<Texture2D>(assetsPath + "GlowyEyes");
    }

    private void SetSkin(ref PlayerDrawSet drawInfo)
    {
        BlueFishSkinPlayer fishPlayer = drawInfo.drawPlayer.GetModPlayer<BlueFishSkinPlayer>();

        if (fishPlayer.enabled)
        {
            drawInfo.colorHead = drawInfo.colorArmorHead;
            drawInfo.colorBodySkin = drawInfo.colorArmorBody;
            drawInfo.colorLegs = drawInfo.colorArmorLegs;
            drawInfo.drawPlayer.GetModPlayer<ReskinPlayer>().SetSkin(SkinTextures);
        }
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

    private void DrawEars(ref PlayerDrawSet drawinfo, bool upperEars)
    {
        if (!drawinfo.drawPlayer.invis && drawinfo.drawPlayer.GetModPlayer<BlueFishSkinPlayer>().enabled)
        {
            Vector2 headPos = drawinfo.HeadPosition() - Vector2.UnitY * 2;
            headPos.ApplyVerticalOffset(drawinfo);

            Vector2 finVector = drawinfo.drawPlayer.GetModPlayer<BlueFishSkinPlayer>().headFinVector;
            float upperEarWobble = drawinfo.drawPlayer.velocity.Y * drawinfo.drawPlayer.direction * 0.02f + MathF.Sin(drawinfo.drawPlayer.miscCounter * 0.25f % MathHelper.Pi) * 0.1f * Math.Clamp(Math.Abs(finVector.Y) * 0.1f, 0, 1f) * drawinfo.drawPlayer.direction + finVector.X * 0.15f;
            float lowerEarWobble = drawinfo.drawPlayer.velocity.Y * drawinfo.drawPlayer.direction * 0.01f + MathF.Sin((drawinfo.drawPlayer.miscCounter * 0.25f - 0.6f) % MathHelper.Pi) * 0.1f * Math.Clamp(Math.Abs(finVector.Y) * 0.1f, 0, 1f) * drawinfo.drawPlayer.direction + finVector.X * 0.1f;
            upperEarWobble = Math.Clamp(upperEarWobble, -0.2f, 0.2f);
            lowerEarWobble = Math.Clamp(lowerEarWobble, -0.2f, 0.2f);

            if (drawinfo.hatHair || drawinfo.fullHair || drawinfo.drawPlayer.head < 0)
            {
                Texture2D earsLowTexture = EarTextures[1].Value;
                DrawData earData = new DrawData(earsLowTexture, headPos, earsLowTexture.Frame(), drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation + lowerEarWobble, drawinfo.headVect, 1f, drawinfo.playerEffect, 0);
                earData.shader = drawinfo.skinDyePacked;
                drawinfo.DrawDataCache.Add(earData);

                if (upperEars && drawinfo.drawPlayer.head > 0 || drawinfo.hatHair)
                    return;

                Texture2D earsHighTexture = EarTextures[0].Value;
                earData = new DrawData(earsHighTexture, headPos, earsHighTexture.Frame(), drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation + upperEarWobble, drawinfo.headVect, 1f, drawinfo.playerEffect, 0);
                earData.shader = drawinfo.skinDyePacked;
                drawinfo.DrawDataCache.Add(earData);
            }
        }
    }

    private void DrawFishSkin_SpecialFace(On_PlayerDrawLayers.orig_DrawPlayer_21_Head_TheFace orig, ref PlayerDrawSet drawinfo)
    {
        if (drawinfo.drawPlayer.GetModPlayer<BlueFishSkinPlayer>().enabled)
        {
            bool flag = drawinfo.drawPlayer.head > 0 && !ArmorIDs.Head.Sets.DrawHead[drawinfo.drawPlayer.head];

            if (!flag && drawinfo.drawPlayer.faceHead > 0)
                orig(ref drawinfo);

            else if (!drawinfo.drawPlayer.invis && !flag)
            {
                Color headColor = drawinfo.colorArmorHead;

                DrawData headData = new DrawData(TextureAssets.Players[drawinfo.skinVar, 0].Value, new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - drawinfo.drawPlayer.bodyFrame.Width / 2 + drawinfo.drawPlayer.width / 2), (int)(drawinfo.Position.Y - Main.screenPosition.Y + drawinfo.drawPlayer.height - drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.drawPlayer.bodyFrame, headColor, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
                headData.shader = drawinfo.skinDyePacked;
                DrawData newData = headData;

                Color eyeColor = new Color(
                    Math.Clamp(drawinfo.colorArmorHead.R / 255f * 3f, 0, 1f),
                    Math.Clamp(drawinfo.colorArmorHead.G / 255f * 3f, 0, 1f),
                    Math.Clamp(drawinfo.colorArmorHead.B / 255f * 3f, 0, 1f),
                    drawinfo.colorArmorHead.A / 255f);

                drawinfo.DrawDataCache.Add(newData);
                newData = new DrawData(GlowyEyesTexture.Value, new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - drawinfo.drawPlayer.bodyFrame.Width / 2 + drawinfo.drawPlayer.width / 2), (int)(drawinfo.Position.Y - Main.screenPosition.Y + drawinfo.drawPlayer.height - drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.drawPlayer.bodyFrame, eyeColor, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
                drawinfo.DrawDataCache.Add(newData);

                Texture2D eyelids = TextureAssets.Players[drawinfo.skinVar, 15].Value;
                Vector2 headOffset = Main.OffsetsPlayerHeadgear[drawinfo.drawPlayer.bodyFrame.Y / drawinfo.drawPlayer.bodyFrame.Height];
                headOffset.Y -= 2f;
                headOffset *= -drawinfo.playerEffect.HasFlag(SpriteEffects.FlipVertically).ToDirectionInt();
                Rectangle value = eyelids.Frame(1, 3, 0, (int)drawinfo.drawPlayer.eyeHelper.CurrentEyeFrame);
                headData = new DrawData(eyelids, new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - drawinfo.drawPlayer.bodyFrame.Width / 2 + drawinfo.drawPlayer.width / 2), (int)(drawinfo.Position.Y - Main.screenPosition.Y + drawinfo.drawPlayer.height - drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect + headOffset, value, headColor, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
                headData.shader = drawinfo.skinDyePacked;
                newData = headData;
                drawinfo.DrawDataCache.Add(newData);

                if (drawinfo.drawPlayer.yoraiz0rDarkness)
                {
                    headData = new DrawData(TextureAssets.Extra[ExtrasID.Yoraiz0rDarkness].Value, new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - drawinfo.drawPlayer.bodyFrame.Width / 2 + drawinfo.drawPlayer.width / 2), (int)(drawinfo.Position.Y - Main.screenPosition.Y + drawinfo.drawPlayer.height - drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
                    headData.shader = drawinfo.skinDyePacked;
                    newData = headData;
                    drawinfo.DrawDataCache.Add(newData);
                }

                if (drawinfo.drawPlayer.face > 0 && ArmorIDs.Face.Sets.DrawInFaceUnderHairLayer[drawinfo.drawPlayer.face])
                {
                    newData = new DrawData(TextureAssets.AccFace[drawinfo.drawPlayer.face].Value, new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - drawinfo.drawPlayer.bodyFrame.Width / 2 + drawinfo.drawPlayer.width / 2), (int)(drawinfo.Position.Y - Main.screenPosition.Y + drawinfo.drawPlayer.height - drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
                    newData.shader = drawinfo.cFace;
                    drawinfo.DrawDataCache.Add(newData);
                }
            }
        }
        else
            orig(ref drawinfo);
    }

    public override void FrameEffects()
    {
        if (enabled)
        {
            if (headFinVector.HasNaNs())
                headFinVector = Vector2.Zero;

            headFinVector.Y -= Utils.GetLerpValue(0.3f, 0.96f, MathF.Sin(Player.miscCounterNormalized * 10), true);
            float bounce = Main.OffsetsPlayerHeadgear[Player.bodyFrame.Y / Player.bodyFrame.Height].Y;
            if (bounce < 2)
                headFinVector.X += Player.direction * 0.5f;

            tailCounter += Math.Abs(Player.velocity.X) / (Main.OffsetsPlayerHeadgear.Length - 1f);
            tailCounter %= MathHelper.TwoPi;
            tailRotations ??= new float[3];

            if (Math.Abs(Player.velocity.X) < 0.5f || Math.Abs(Player.velocity.Y) > 0f)
            {
                float[] wobble = [
                    MathF.Sin(Player.miscCounter / 150f * MathHelper.TwoPi * 2f),
                    MathF.Sin(Player.miscCounter / 150f * MathHelper.TwoPi * 2f - 2f),
                    MathF.Sin(Player.miscCounter / 150f * MathHelper.TwoPi * 2f - 3f)
                    ];
                tailRotations[0] = MathHelper.Lerp(tailRotations[0], (wobble[0] * 0.05f + Player.velocity.Y * 0.04f) * Player.direction, 0.1f);
                tailRotations[1] = MathHelper.Lerp(tailRotations[1], (wobble[1] * 0.057f + Player.velocity.Y * 0.04f) * Player.direction, 0.1f);
                tailRotations[2] = MathHelper.Lerp(tailRotations[2], (wobble[2] * 0.1f + Player.velocity.Y * 0.04f) * Player.direction, 0.1f);
                tailCounter = MathHelper.Lerp(tailCounter, 0f, 0.05f);
            }
            else
            {
                tailRotations[0] = MathHelper.Lerp(tailRotations[0], MathF.Sin(tailCounter) * 0.1f * Player.direction, 0.3f);
                tailRotations[1] = MathHelper.Lerp(tailRotations[1], MathF.Sin(tailCounter - 1.5f) * 0.2f * Player.direction, 0.3f);
                tailRotations[2] = MathHelper.Lerp(tailRotations[2], MathF.Sin(tailCounter - 2.5f) * 0.3f * Player.direction, 0.3f);
            }

            headFinVector.X = MathHelper.Lerp(headFinVector.X, 0f, 0.4f);
            headFinVector.Y = MathHelper.Lerp(headFinVector.Y, Player.velocity.Y, 0.3f);

        }
    }

    public override void ResetEffects()
    {
        enabled = false;
    }
}
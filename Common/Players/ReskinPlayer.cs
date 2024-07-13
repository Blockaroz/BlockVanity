using System;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Common.Players;

public class ReskinPlayer : ModPlayer
{
    private static Asset<Texture2D>[] OriginalSkinTextures;
    private Asset<Texture2D>[] QueuedSkinTextures;
    public bool enabled;

    public delegate void ReskinDelegate(ref PlayerDrawSet drawInfo);

    public static event ReskinDelegate OnSetSkin;
    public static event ReskinDelegate OnResetSkins;

    public void SetSkin(Asset<Texture2D>[] textures)
    {
        QueuedSkinTextures = textures;
    }    
  
    public enum SkinID
    {
        Head,
        EarsHigh,
        EarsLow,
        Eyes,
        Body,
        Arms,
        Hands,
        HandsBack,
        Legs,
        Tail,
        Length
    }

    private static int MaleSkinVar(PlayerDrawSet drawInfo)
    {
        if (drawInfo.drawPlayer.Male)
            return drawInfo.skinVar;
        else
            return PlayerVariantID.Sets.AltGenderReference[drawInfo.skinVar];
    }
    
    private static int FemaleSkinVar(PlayerDrawSet drawInfo)
    {
        if (drawInfo.drawPlayer.Male)
            return PlayerVariantID.Sets.AltGenderReference[drawInfo.skinVar];
        else
            return drawInfo.skinVar;
    }

    public override void Load()
    {
        On_PlayerDrawLayers.DrawPlayer_RenderAllLayers += SetBackToNormal;
    }

    private void SetBackToNormal(On_PlayerDrawLayers.orig_DrawPlayer_RenderAllLayers orig, ref PlayerDrawSet drawinfo)
    {
        if (drawinfo.drawPlayer.Male)
            Main.NewText("male");

        orig(ref drawinfo);

        OnResetSkins?.Invoke(ref drawinfo);

        SetOriginalSkinTextures(drawinfo);

        int maleVar = MaleSkinVar(drawinfo);
        int femVar = FemaleSkinVar(drawinfo);
        TextureAssets.Players[maleVar, 0] = OriginalSkinTextures[0];
        TextureAssets.Players[maleVar, 3] = OriginalSkinTextures[1];
        TextureAssets.Players[maleVar, 5] = OriginalSkinTextures[2];
        TextureAssets.Players[maleVar, 7] = OriginalSkinTextures[3];
        TextureAssets.Players[maleVar, 9] = OriginalSkinTextures[4];
        TextureAssets.Players[maleVar, 10] = OriginalSkinTextures[5];
        TextureAssets.Players[femVar, 0] = OriginalSkinTextures[6];
        TextureAssets.Players[femVar, 3] = OriginalSkinTextures[7];
        TextureAssets.Players[femVar, 5] = OriginalSkinTextures[8];
        TextureAssets.Players[femVar, 7] = OriginalSkinTextures[9];
        TextureAssets.Players[femVar, 9] = OriginalSkinTextures[10];
        TextureAssets.Players[femVar, 10] = OriginalSkinTextures[11];
    }

    public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
    {
        OnSetSkin?.Invoke(ref drawInfo);

        SetOriginalSkinTextures(drawInfo, true);

        if (enabled)
        {
            TextureAssets.Players[drawInfo.skinVar, 0] = QueuedSkinTextures[(int)SkinID.Head];
            TextureAssets.Players[drawInfo.skinVar, 3] = QueuedSkinTextures[(int)SkinID.Body];
            TextureAssets.Players[drawInfo.skinVar, 5] = QueuedSkinTextures[(int)SkinID.HandsBack];
            TextureAssets.Players[drawInfo.skinVar, 7] = QueuedSkinTextures[(int)SkinID.Arms];
            TextureAssets.Players[drawInfo.skinVar, 9] = QueuedSkinTextures[(int)SkinID.Hands];
            TextureAssets.Players[drawInfo.skinVar, 10] = QueuedSkinTextures[(int)SkinID.Legs];
        }
    }

    private static void SetOriginalSkinTextures(PlayerDrawSet drawInfo, bool force = false)
    {
        if (OriginalSkinTextures == null || force)
        {
            int maleVar = MaleSkinVar(drawInfo);
            int femVar = FemaleSkinVar(drawInfo);

            OriginalSkinTextures = [
                TextureAssets.Players[maleVar, 0],
                TextureAssets.Players[maleVar, 3],
                TextureAssets.Players[maleVar, 5],
                TextureAssets.Players[maleVar, 7],
                TextureAssets.Players[maleVar, 9],
                TextureAssets.Players[maleVar, 10],
                TextureAssets.Players[femVar, 0],
                TextureAssets.Players[femVar, 3],
                TextureAssets.Players[femVar, 5],
                TextureAssets.Players[femVar, 7],
                TextureAssets.Players[femVar, 9],
                TextureAssets.Players[femVar, 10],
                ];
        }
    }

    public override void ResetEffects()
    {
        enabled = false;
    }
}

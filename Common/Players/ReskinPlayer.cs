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

    public static event ReskinDelegate OnPreSetSkin;
    public static event ReskinDelegate OnPreResetSkins;

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

    public override void Load()
    {
        On_PlayerDrawLayers.DrawPlayer_RenderAllLayers += SetBackToNormal;
    }

    private void SetBackToNormal(On_PlayerDrawLayers.orig_DrawPlayer_RenderAllLayers orig, ref PlayerDrawSet drawinfo)
    {
        orig(ref drawinfo);

        OnPreResetSkins?.Invoke(ref drawinfo);

        SetOriginalSkinTextures(drawinfo);

        TextureAssets.Players[drawinfo.skinVar, 0] = OriginalSkinTextures[0];
        TextureAssets.Players[drawinfo.skinVar, 3] = OriginalSkinTextures[1];
        TextureAssets.Players[drawinfo.skinVar, 5] = OriginalSkinTextures[2];
        TextureAssets.Players[drawinfo.skinVar, 7] = OriginalSkinTextures[3];
        TextureAssets.Players[drawinfo.skinVar, 9] = OriginalSkinTextures[4];
        TextureAssets.Players[drawinfo.skinVar, 10] = OriginalSkinTextures[5];
        TextureAssets.Players[PlayerVariantID.Sets.AltGenderReference[drawinfo.skinVar], 0] = OriginalSkinTextures[6];
        TextureAssets.Players[PlayerVariantID.Sets.AltGenderReference[drawinfo.skinVar], 3] = OriginalSkinTextures[7];
        TextureAssets.Players[PlayerVariantID.Sets.AltGenderReference[drawinfo.skinVar], 5] = OriginalSkinTextures[8];
        TextureAssets.Players[PlayerVariantID.Sets.AltGenderReference[drawinfo.skinVar], 7] = OriginalSkinTextures[9];
        TextureAssets.Players[PlayerVariantID.Sets.AltGenderReference[drawinfo.skinVar], 9] = OriginalSkinTextures[10];
        TextureAssets.Players[PlayerVariantID.Sets.AltGenderReference[drawinfo.skinVar], 10] = OriginalSkinTextures[11];
    }

    public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
    {
        OnPreSetSkin?.Invoke(ref drawInfo);

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
            OriginalSkinTextures = [
                TextureAssets.Players[drawInfo.skinVar, 0],
                TextureAssets.Players[drawInfo.skinVar, 3],
                TextureAssets.Players[drawInfo.skinVar, 5],
                TextureAssets.Players[drawInfo.skinVar, 7],
                TextureAssets.Players[drawInfo.skinVar, 9],
                TextureAssets.Players[drawInfo.skinVar, 10],
                TextureAssets.Players[PlayerVariantID.Sets.AltGenderReference[drawInfo.skinVar], 0],
                TextureAssets.Players[PlayerVariantID.Sets.AltGenderReference[drawInfo.skinVar], 3],
                TextureAssets.Players[PlayerVariantID.Sets.AltGenderReference[drawInfo.skinVar], 5],
                TextureAssets.Players[PlayerVariantID.Sets.AltGenderReference[drawInfo.skinVar], 7],
                TextureAssets.Players[PlayerVariantID.Sets.AltGenderReference[drawInfo.skinVar], 9],
                TextureAssets.Players[PlayerVariantID.Sets.AltGenderReference[drawInfo.skinVar], 10],
                ];
        }
    }

    public override void ResetEffects()
    {
        enabled = false;
    }
}

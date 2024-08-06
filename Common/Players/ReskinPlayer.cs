using System;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Renderers;
using Terraria.ID;
using Terraria.Initializers;
using Terraria.ModLoader;

namespace BlockVanity.Common.Players;

public class ReskinPlayer : ModPlayer
{
    private static Asset<Texture2D>[] OriginalSkinTextures;
    private Asset<Texture2D>[] QueuedSkinTextures;
    public bool enabled;

    public delegate void ReskinDelegate(ref PlayerDrawSet drawInfo);

    public static event ReskinDelegate OnPreCopyVariables;
    public static event ReskinDelegate OnSetNewSkin;
    public static event ReskinDelegate OnSetNormalSkin;

    public void SetSkin(Asset<Texture2D>[] textures)
    {
        QueuedSkinTextures = textures;
    }    
  
    public enum SkinPieceID
    {
        Head,
        Eyes,
        Body,
        Arms,
        Hands,
        HandsBack,
        Legs,
        LegsSlim,
        Misc1,
        Misc2,
        Misc3,
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
        On_PlayerDrawSet.CopyBasicPlayerFields += SetSkin;
        On_PlayerDrawLayers.DrawPlayer_RenderAllLayers += SetBackToNormal;
    }

    private void SetSkin(On_PlayerDrawSet.orig_CopyBasicPlayerFields orig, ref PlayerDrawSet self)
    {
        OnPreCopyVariables?.Invoke(ref self);

        orig(ref self);
    }

    public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
    {
        SetOriginalSkinTextures(drawInfo, true);

        OnSetNewSkin?.Invoke(ref drawInfo);

        if (enabled && QueuedSkinTextures != null)
        {
            TextureAssets.Players[drawInfo.skinVar, 0] = QueuedSkinTextures[(int)SkinPieceID.Head];
            TextureAssets.Players[drawInfo.skinVar, 3] = QueuedSkinTextures[(int)SkinPieceID.Body];
            TextureAssets.Players[drawInfo.skinVar, 5] = QueuedSkinTextures[(int)SkinPieceID.HandsBack];
            TextureAssets.Players[drawInfo.skinVar, 7] = QueuedSkinTextures[(int)SkinPieceID.Arms];
            TextureAssets.Players[drawInfo.skinVar, 9] = QueuedSkinTextures[(int)SkinPieceID.Hands];

            if (drawInfo.drawPlayer.Male)
                TextureAssets.Players[drawInfo.skinVar, 10] = QueuedSkinTextures[(int)SkinPieceID.Legs];
            else
                TextureAssets.Players[drawInfo.skinVar, 10] = QueuedSkinTextures[(int)SkinPieceID.LegsSlim];

            TextureAssets.Players[drawInfo.skinVar, 15] = QueuedSkinTextures[(int)SkinPieceID.Eyes];
        }
    }

    private void SetBackToNormal(On_PlayerDrawLayers.orig_DrawPlayer_RenderAllLayers orig, ref PlayerDrawSet drawinfo)
    {
        orig(ref drawinfo);

        OnSetNormalSkin?.Invoke(ref drawinfo);

        SetOriginalSkinTextures(drawinfo);

        int maleVar = MaleSkinVar(drawinfo);
        int femVar = FemaleSkinVar(drawinfo);
        int[] ids = [0, 3, 5, 7, 9, 10, 15];
        for (int i = 0; i < ids.Length; i++)
        {
            TextureAssets.Players[maleVar, ids[i]] = OriginalSkinTextures[i];
            TextureAssets.Players[femVar, ids[i]] = OriginalSkinTextures[i + ids.Length];
        }
    }

    private static void SetOriginalSkinTextures(PlayerDrawSet drawInfo, bool force = false)
    {
        if (OriginalSkinTextures == null || force)
        {
            int maleVar = MaleSkinVar(drawInfo);
            int femVar = FemaleSkinVar(drawInfo);

            LoadOriginalSkins(drawInfo, force);

            OriginalSkinTextures = [
                TextureAssets.Players[0, 0],
                TextureAssets.Players[0, 3],
                TextureAssets.Players[0, 5],
                TextureAssets.Players[0, 7],
                TextureAssets.Players[maleVar, 9],
                TextureAssets.Players[maleVar, 10],
                TextureAssets.Players[maleVar, 15],
                TextureAssets.Players[0, 0],
                TextureAssets.Players[femVar, 3],
                TextureAssets.Players[femVar, 5],
                TextureAssets.Players[femVar, 7],
                TextureAssets.Players[femVar, 9],
                TextureAssets.Players[femVar, 10],
                TextureAssets.Players[femVar, 15],
                ];
        }
    }

    private static void LoadOriginalSkins(PlayerDrawSet drawInfo, bool force = false)
    {
        PlayerDataInitializer.Load();
    }

    public override void ResetEffects()
    {
        enabled = false;
    }
}

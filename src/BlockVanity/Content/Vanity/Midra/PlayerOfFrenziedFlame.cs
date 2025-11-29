using BlockVanity.Common.Graphics;
using BlockVanity.Common.UI;
using BlockVanity.Common.Utilities;
using BlockVanity.Content.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Vanity.Midra;

public class PlayerOfFrenziedFlame : ModPlayer
{
    public DrawData GetFrenzyTarget() => new DrawData(
        TextureAssets.Extra[ExtrasID.SharpTears].Value, 
        Vector2.Zero,
        TextureAssets.Extra[ExtrasID.SharpTears].Frame(), 
        Color.Gold with { A = 10 }, 
        -Player.fullRotation, 
        TextureAssets.Extra[ExtrasID.SharpTears].Size() * 0.5f, 
        1f,
        0);

    public static Vector2 GetOffsetAnchor(Player player) => player.Center / 3f;

    public bool needsTarget;

    public bool forceFlameBack;

    public float miscTimer;

    public override void FrameEffects()
    {
        if (Main.gameInactive)
            return;

        if (Player.head == EquipLoader.GetEquipSlot(Mod, nameof(AshenHead), EquipType.Head) || forceFlameBack)
        {
            if (AreaEffectsToggle.IsActive(Player))
                Lighting.AddLight(Player.MountedCenter, Color.Orange.ToVector3() * 0.5f);
        }

        if (++miscTimer > 1200)
            miscTimer = 0;
    }

    public override void ResetEffects()
    {
        needsTarget = false;
        forceFlameBack = false;
    }
}
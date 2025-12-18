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
using Terraria.Graphics.Renderers;
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


    public ParticleRenderer particles;

    private static RenderTarget2D FrenzyPreTarget;
    private static RenderTarget2D FrenzyTarget;

    private const int targetSize = 800;

    public override void Load()
    {
        Main.QueueMainThreadAction(() =>
        {
            FrenzyPreTarget = new RenderTarget2D(Main.graphics.GraphicsDevice, targetSize / 2, targetSize / 2);
            FrenzyTarget = new RenderTarget2D(Main.graphics.GraphicsDevice, targetSize, targetSize);
        });
    }

    public static Vector2 GetOffsetAnchor(Player player) => player.Center / 2f;

    public float miscTimer;

    public override void UpdateVisibleVanityAccessories()
    {
        if (Main.gameInactive)
            return;

        if (Player.head == EquipLoader.GetEquipSlot(Mod, nameof(AshenHead), EquipType.Head))
        {
            if (AreaEffectsToggle.IsActive(Player))
                Lighting.AddLight(Player.MountedCenter, Color.Orange.ToVector3() * 0.5f);
        }

        if (++miscTimer > 1200)
            miscTimer = 0;
    }
}
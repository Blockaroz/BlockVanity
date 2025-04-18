﻿using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace BlockVanity.Common.UI;

public class HitSoundToggle : BuilderToggle
{
    public static LocalizedText OnText;
    public static LocalizedText OffText;

    public override Position OrderPosition => new After(ModContent.GetInstance<AreaEffectsToggle>());

    public override bool Active() => true;

    public override void SetStaticDefaults()
    {
        OnText = Mod.GetLocalization("HitSoundToggle.OnText");
        OffText = Mod.GetLocalization("HitSoundToggle.OffText");
    }

    public override string DisplayValue() => CurrentState == 0 ? OnText.Value : OffText.Value;

    public override bool Draw(SpriteBatch spriteBatch, ref BuilderToggleDrawParams drawParams)
    {
        drawParams.Frame = drawParams.Texture.Frame(3, 1, CurrentState);
        return true;
    }

    public override bool DrawHover(SpriteBatch spriteBatch, ref BuilderToggleDrawParams drawParams)
    {
        drawParams.Texture = ModContent.Request<Texture2D>(Texture).Value;
        drawParams.Frame = drawParams.Texture.Frame(3, 1, 2);
        drawParams.Color = Colors.FancyUIFatButtonMouseOver;
        drawParams.Scale = 0.933f;
        return true;
    }

    public static bool IsActive(Player player) => player.builderAccStatus[ModContent.GetInstance<HitSoundToggle>().Type] == 0;
}
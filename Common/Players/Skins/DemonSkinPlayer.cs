using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace BlockVanity.Common.Players.Skins;

public class DemonSkinPlayer : ModPlayer
{
    public static Asset<Texture2D>[] SkinTextures;

    public bool enabled;

    public override void Load()
    {
        SkinTextures = VanityUtils.GetSkinTextures($"{nameof(BlockVanity)}/Assets/Textures/Skins/DemonSkin/DemonSkin_");
        ReskinPlayer.OnSetNewSkin += SetSkin;
    }

    private void SetSkin(ref PlayerDrawSet drawInfo)
    {
        DemonSkinPlayer demonPlayer = drawInfo.drawPlayer.GetModPlayer<DemonSkinPlayer>();

        if (demonPlayer.enabled)
        {
            drawInfo.colorHead = drawInfo.colorArmorHead;
            drawInfo.colorBodySkin = drawInfo.colorArmorBody;
            drawInfo.colorLegs = drawInfo.colorArmorLegs;
            drawInfo.drawPlayer.GetModPlayer<ReskinPlayer>().SetSkin(SkinTextures);
        }
    }

    public override void ResetEffects()
    {
        enabled = false;
    }
}

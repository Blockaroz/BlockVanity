using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace BlockVanity.Content.Vanity.Excellence;

public class ExcellencePlayer : ModPlayer
{
    public bool enabled;

    public override void Load()
    {
        On_Player.SetArmorEffectVisuals += ExcellenceShadows;
    }

    private void ExcellenceShadows(On_Player.orig_SetArmorEffectVisuals orig, Player self, Player drawPlayer)
    {
        if (self.GetModPlayer<ExcellencePlayer>().enabled)
            self.armorEffectDrawOutlinesForbidden = true;
        else
            orig(self, drawPlayer);
    }

    public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
    {
        if (enabled && drawInfo.shadow > 0f)
        {
            a = 0.33f;
            r = 1.1f;
            b = 0.3f;
            g = 0.3f;
        }
    }

    public override void ResetEffects()
    {
        enabled = false;
    }
}
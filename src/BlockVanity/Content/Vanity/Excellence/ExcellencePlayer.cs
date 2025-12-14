using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics;
using Terraria.ModLoader;

namespace BlockVanity.Content.Vanity.Excellence;

public class ExcellencePlayer : ModPlayer
{
    public bool Enabled { get; set; }

    public override void ResetEffects()
    {
        Enabled = false;
    }

    public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
    {
        if (Enabled && drawInfo.shadow > 0f)
        {
            a = 0.33f;
            r = 1.1f;
            b = 0.3f;
            g = 0.3f;
        }
    }

    public override void DrawPlayer(Camera camera)
    {
        
    }
}
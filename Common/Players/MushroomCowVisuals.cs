using System;
using Terraria.ModLoader;
using Terraria.ID;

namespace BlockVanity.Common.Players
{
    public class MushroomCowVisuals : ModPlayer
    {
        public bool redMushroomCow;
        public bool brownMushroomCow;
        public bool glowingMushroomCow;

        public override void FrameEffects()
        {
            if (!Player.hideMisc[0])
            {
                if (redMushroomCow)
                {

                }
                if (brownMushroomCow)
                {

                }                
                if (glowingMushroomCow)
                {

                }
            }
        }

        public override void ResetEffects()
        {
            redMushroomCow = false;
            brownMushroomCow = false;
            glowingMushroomCow = false;
        }
    }
}

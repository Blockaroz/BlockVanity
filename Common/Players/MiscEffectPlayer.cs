using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Common.Players
{
    public class MiscEffectPlayer : ModPlayer
    {
        public bool accBlackEye;

        //pets
        public bool floatingSkyLanternPet;

        public bool impishEyePet;

        public override void ResetEffects()
        {
            accBlackEye = false;

            floatingSkyLanternPet = false;
            impishEyePet = false;
        }
    }
}

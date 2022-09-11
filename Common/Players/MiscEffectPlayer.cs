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

        public bool boneSound;

        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource, ref int cooldownCounter)
        {
            bool customSound = boneSound;
            if (customSound)
            {
                playSound = false;
                if (boneSound)
                    SoundEngine.PlaySound(SoundID.NPCHit2.WithPitchOffset(Main.rand.NextFloat(-0.1f, 0.1f)), Player.Center);

            }

            return true;
        }

        public override void ResetEffects()
        {
            accBlackEye = false;

            boneSound = false;
        }
    }
}

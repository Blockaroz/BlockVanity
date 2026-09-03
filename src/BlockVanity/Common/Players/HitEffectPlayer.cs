using BlockVanity.Common.UI;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace BlockVanity.Common.Players;

public class HitEffectPlayer : ModPlayer
{
    public bool Enabled { get; set; }
    public SoundStyle HitSound { get; set; }
    public bool UsePitch { get; set; }

    public static void SetEquipHitSound(Player player, SoundStyle hitSound, bool usePitch = false)
    {
        player.GetModPlayer<HitEffectPlayer>().Enabled = true;
        player.GetModPlayer<HitEffectPlayer>().HitSound = hitSound;
        player.GetModPlayer<HitEffectPlayer>().UsePitch = usePitch;
    }

    public override void Load()
    {
        On_Player.PlayHurtSound += PlayMyOwnSound;
    }

    private void PlayMyOwnSound(On_Player.orig_PlayHurtSound orig, Player self)
    {
        var hitEffect = self.GetModPlayer<HitEffectPlayer>();
        if (hitEffect.Enabled)
        {
            Vector2 position = self.position;
            if (self.whoAmI == Main.myPlayer)
                position = new Vector2(-1f, -1f);

            float pitchOffset = hitEffect.UsePitch ? Utils.Remap(self.voicePitchOffset, -1f, 1f, -0.7f, 0.7f) : 0f;
            SoundEngine.PlaySound(hitEffect.HitSound, position, pitchOffset, 1f);
            return;
        }

        orig(self);
    }
    public override void ResetEffects()
    {
        Enabled = false;
        UsePitch = false;
    }
}
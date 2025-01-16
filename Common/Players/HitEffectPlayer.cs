using BlockVanity.Common.UI;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace BlockVanity.Common.Players;

public class HitEffectPlayer : ModPlayer
{
    public static void SetEquipHitSound(Player player, SoundStyle hitSound)
    {
        player.GetModPlayer<HitEffectPlayer>().enabled = true;
        player.GetModPlayer<HitEffectPlayer>().hitSound = hitSound;
    }

    public static void SetSkinHitSound(Player player, SoundStyle hitSound)
    {
        player.GetModPlayer<HitEffectPlayer>().skin = true;
        player.GetModPlayer<HitEffectPlayer>().hitSoundSkin = hitSound;
    }

    private bool enabled;
    private SoundStyle hitSound;

    private bool skin;
    private SoundStyle hitSoundSkin;

    public override void ModifyHurt(ref Player.HurtModifiers modifiers)
    {
        if (HitSoundToggle.IsActive(Player))
        {
            if (enabled || skin)
            {
                modifiers.DisableSound();
            }
        }
    }

    public override void OnHurt(Player.HurtInfo info)
    {
        if (HitSoundToggle.IsActive(Player))
        {
            if (enabled)
            {
                SoundEngine.PlaySound(hitSound, Player.position);
            }
            else if (skin)
            {
                SoundEngine.PlaySound(hitSoundSkin, Player.position);
            }
        }
    }

    public override void ResetEffects()
    {
        enabled = false;
        skin = false;
    }
}

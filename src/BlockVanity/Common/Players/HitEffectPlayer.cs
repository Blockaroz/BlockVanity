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

    private bool enabled;
    private SoundStyle hitSound;

    public override void ModifyHurt(ref Player.HurtModifiers modifiers)
    {
        if (enabled)
        {
            modifiers.DisableSound();

            if (!Main.dedServ)
                SoundEngine.PlaySound(hitSound, Player.position);
        }
    }

    public override void ResetEffects()
    {
        enabled = false;
    }
}
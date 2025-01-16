using Terraria;
using Terraria.ModLoader;

namespace BlockVanity.Content.Pets.HauntedCandelabraPet;

public class HauntedCandelabraBuff : ModBuff
{
    public override void SetStaticDefaults()
    {
        Main.buffNoTimeDisplay[Type] = true;
        Main.lightPet[Type] = true;
    }

    public override void Update(Player player, ref int buffIndex)
    {
        player.BuffHandle_SpawnPetIfNeededAndSetTime(buffIndex, ref player.GetPets().hauntedCandelabra, ModContent.ProjectileType<HauntedCandelabra>());
    }
}

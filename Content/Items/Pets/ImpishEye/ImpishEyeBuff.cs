using BlockVanity.Common.Players;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Pets.ImpishEye
{
    public class ImpishEyeBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Impish Eye");
            Description.SetDefault("Something about this one feels different");

            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<MiscEffectPlayer>().impishEyePet = true;
            player.buffTime[buffIndex] = 18000;

            int projType = ModContent.ProjectileType<ImpishEyeProj>();
            Vector2 pos = new Vector2(player.position.X + player.width / 2f, player.position.Y + player.height / 2f);
            if (player.ownedProjectileCounts[projType] <= 0 && player.whoAmI == Main.myPlayer)
                Projectile.NewProjectileDirect(player.GetSource_Buff(buffIndex), pos, Vector2.Zero, projType, 0, 0f, player.whoAmI);
        }
    }
}

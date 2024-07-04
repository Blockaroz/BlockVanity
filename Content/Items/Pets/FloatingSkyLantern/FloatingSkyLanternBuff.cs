using BlockVanity.Common.Players;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Pets.FloatingSkyLantern
{
    public class FloatingSkyLanternBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Floating Sky Lantern");
            // Description.SetDefault("A lantern is floating behind you");

            Main.buffNoTimeDisplay[Type] = true;
            Main.lightPet[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<MiscEffectPlayer>().floatingSkyLanternPet = true;
            player.buffTime[buffIndex] = 18000;

            int projType = ModContent.ProjectileType<FloatingSkyLanternProj>();
            Vector2 pos = new Vector2(player.position.X + player.width / 2f, player.position.Y + player.height / 2f);
            if (player.ownedProjectileCounts[projType] <= 0 && player.whoAmI == Main.myPlayer)
            {
                Projectile.NewProjectileDirect(player.GetSource_Buff(buffIndex), pos, Vector2.Zero, projType, 0, 0f, player.whoAmI);
            }
        }
    }
}

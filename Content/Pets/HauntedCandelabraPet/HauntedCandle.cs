using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Pets.HauntedCandelabraPet;

public class HauntedCandle : ModItem
{
    public override void SetDefaults()
    {
        Item.buffType = ModContent.BuffType<HauntedCandelabraBuff>();
        Item.DefaultToVanitypet(ModContent.ProjectileType<HauntedCandelabra>(), Item.buffType);
    }

    public static Asset<Texture2D> flameTexture;

    public override void Load()
    {
        flameTexture = ModContent.Request<Texture2D>(Texture + "_Flame");
    }

    public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
    {
    }

    public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
    {
    }
}

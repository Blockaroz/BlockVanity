using BlockVanity.Content.Rarities;
using BlockVanity.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Vanity.CountChaos;

[AutoloadEquip(EquipType.Body)]
public class CountChaosCuirass : VanityItem
{
    public CountChaosCuirass() : base(ModContent.RarityType<RarityCommonVanity>(), 36, 34) { }

    public override void SetStaticDefaults()
    {
        ArmorIDs.Body.Sets.shouldersAreAlwaysInTheBack[Item.bodySlot] = true;
        ArmorIDs.Body.Sets.HidesArms[Item.bodySlot] = true;
        ArmorIDs.Body.Sets.HidesTopSkin[Item.bodySlot] = true;
        Main.RegisterItemAnimation(Type, new DrawAnimationVertical(7, 9));
        ItemID.Sets.AnimatesAsSoul[Type] = true;

        if (Main.dedServ)
        {
            return;
        }

        glowTexture = ModContent.Request<Texture2D>(Texture + "_Glow");
        glowTextureArmor = ModContent.Request<Texture2D>(Texture + "_Body_Glow");
    }

    public static Asset<Texture2D> glowTexture;
    public static Asset<Texture2D> glowTextureArmor;

    public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
    {
        Color color = Color.Lerp(Color.White, lightColor, 0.5f) with { A = 0 };
        Rectangle frame = glowTexture.Frame();
        frame.Height -= 4;
        spriteBatch.Draw(glowTexture.Value, Item.Center - Main.screenPosition, frame, color * 0.5f, rotation, frame.Size() / 2, scale, 0, 0);
    }

    public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
    {
        spriteBatch.Draw(glowTexture.Value, position, glowTexture.Frame(), drawColor with { A = 0 } * 0.5f, 0, origin, scale, 0, 0);
    }
}
using BlockVanity.Common.Utilities;
using BlockVanity.Content.Items.Vanity.BoneKing.Frostbuilt;
using BlockVanity.Content.Items.Vanity.BoneKing.Hellforged;
using BlockVanity.Content.Items.Vanity.BoneKing.Platinum;
using BlockVanity.Content.Rarities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Vanity.BoneKing.Hellforged;

[AutoloadEquip(EquipType.Body)]
public class HellforgedKingGarb : VanityItem
{
    public override LocalizedText DisplayName => ModContent.GetInstance<BoneKingGarb>().DisplayName;
    public override LocalizedText Tooltip => ModContent.GetInstance<BoneKingGarb>().Tooltip;

    public HellforgedKingGarb() : base(ModContent.RarityType<VanityRareCommon>(), 34, 32) { }

    public override void SetStaticDefaults()
    {
        ArmorIDs.Body.Sets.showsShouldersWhileJumping[Item.bodySlot] = true;

        glowTexture = ModContent.Request<Texture2D>(Texture + "_Glow");
        glowTextureArmor = ModContent.Request<Texture2D>(Texture + "_Body_Glow");
    }

    public static Asset<Texture2D> glowTexture;
    public static Asset<Texture2D> glowTextureArmor;

    public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
    {
        spriteBatch.Draw(glowTexture.Value, Item.Center - Main.screenPosition, glowTexture.Frame(), Color.White with { A = 150 }, rotation, glowTexture.Size() / 2f, scale, 0, 0);
    }

    public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
    {
        spriteBatch.Draw(glowTexture.Value, position, glowTexture.Frame(), Color.White with { A = 150 }, 0, origin, scale, 0, 0);
    }
}

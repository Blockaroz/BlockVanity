using BlockVanity.Common.Utilities;
using BlockVanity.Content.Items.Vanity.BoneKing.Platinum;
using BlockVanity.Content.Items.Vanity.Scholar;
using BlockVanity.Content.Rarities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Vanity.BoneKing.Frostbuilt;

public class FrostbuiltKingSkull : VanityItem
{
    public override void Load()
    {
        if (Main.dedServ)
            return;

        EquipLoader.AddEquipTexture(Mod, $"{Texture}_{EquipType.Head}", EquipType.Head, this, equipTexture: new OversizedHeadEquipTexture(0, -4));
    }

    public override LocalizedText DisplayName => ModContent.GetInstance<BoneKingSkull>().DisplayName;
    public override LocalizedText Tooltip => ModContent.GetInstance<BoneKingSkull>().Tooltip;

    public FrostbuiltKingSkull() : base(ModContent.RarityType<VanityRareCommon>(), 30, 34) { }

    public override void SetStaticDefaults()
    {
        if (Main.dedServ)
            return;

        Item.headSlot = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
        ArmorIDs.Head.Sets.IsTallHat[Item.headSlot] = true;
        ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;

        glowTexture = ModContent.Request<Texture2D>(Texture + "_Glow");
        glowTextureArmor = ModContent.Request<Texture2D>(Texture + "_Head_Glow");
    }

    public static Asset<Texture2D> glowTexture;
    public static Asset<Texture2D> glowTextureArmor;

    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.headSlot = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
    }

    public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
    {
        spriteBatch.Draw(glowTexture.Value, Item.Center - Main.screenPosition, glowTexture.Frame(), Color.Coral with { A = 60 }, rotation, glowTexture.Size() / 2f, scale, 0, 0);
    }

    public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
    {
        spriteBatch.Draw(glowTexture.Value, position, glowTexture.Frame(), Color.Coral with { A = 60 }, 0, origin, scale, 0, 0);
    }
}

using BlockVanity.Content.Rarities;
using BlockVanity.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Vanity.BoneKing;

public class FrozenKingSkull : VanityItem
{
    public override void Load()
    {
        if (Main.dedServ)
            return;

        EquipLoader.AddEquipTexture(Mod, $"{Texture}_{EquipType.Head}", EquipType.Head, this, equipTexture: new OversizedHeadEquipTexture(0, -4));
    }

    public override LocalizedText DisplayName => ModContent.GetInstance<BoneKingSkull>().DisplayName;
    public override LocalizedText Tooltip => ModContent.GetInstance<BoneKingSkull>().Tooltip;

    public FrozenKingSkull() : base(ModContent.RarityType<VanityRareCommon>(), 30, 34) { }

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

[AutoloadEquip(EquipType.Body)]
public class FrozenKingGarb : VanityItem
{
    public override LocalizedText DisplayName => ModContent.GetInstance<BoneKingGarb>().DisplayName;
    public override LocalizedText Tooltip => ModContent.GetInstance<BoneKingGarb>().Tooltip;

    public FrozenKingGarb() : base(ModContent.RarityType<VanityRareCommon>(), 34, 32) { }

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
        spriteBatch.Draw(glowTexture.Value, Item.Center - Main.screenPosition, glowTexture.Frame(), Color.Coral with { A = 60 }, rotation, glowTexture.Size() / 2f, scale, 0, 0);
    }

    public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
    {
        spriteBatch.Draw(glowTexture.Value, position, glowTexture.Frame(), Color.Coral with { A = 60 }, 0, origin, scale, 0, 0);
    }
}

[AutoloadEquip(EquipType.Legs)]
public class FrozenKingPants : VanityItem
{
    public override LocalizedText DisplayName => ModContent.GetInstance<BoneKingPants>().DisplayName;
    public override LocalizedText Tooltip => ModContent.GetInstance<BoneKingPants>().Tooltip;

    public FrozenKingPants() : base(ModContent.RarityType<VanityRareCommon>(), 30, 18) { }

    public override void SetStaticDefaults()
    {
        ArmorIDs.Legs.Sets.OverridesLegs[Item.legSlot] = true;
        ArmorIDs.Legs.Sets.IncompatibleWithFrogLeg[Item.legSlot] = true;
    }
}

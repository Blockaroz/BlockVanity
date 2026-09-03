using BlockVanity.Common;
using BlockVanity.Common.Players;
using BlockVanity.Content.Rarities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Vanity.CountChaos;

[AutoloadEquip(EquipType.Head)]
public class CountChaosHornedHeadItem : VanityItem
{
    public override string Texture => Assets.Textures.Content.Vanity.CountChaos.CountChaosHornedHead_Item.KEY;

    public override int Rarity => ModContent.RarityType<CommonVanityRarity>();

    public override void SetStaticDefaults()
    {
        ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;
    }
}

[AutoloadEquip(EquipType.Body)]
public class CountChaosCuirassItem() : VanityItem(width: 36, height: 34)
{
    public override string Texture => Assets.Textures.Content.Vanity.CountChaos.CountChaosCuirass_Item.KEY;

    public override int Rarity => ModContent.RarityType<CommonVanityRarity>();

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
    }

    public override void PostDrawInWorld(WorldItem item, SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
    {
        var glowTexture = Assets.Textures.Content.Vanity.CountChaos.CountChaosCuirass_Item_Glow.Asset;
        Color color = Color.Lerp(Color.White, lightColor, 0.5f) with { A = 0 };
        Rectangle frame = glowTexture.Frame();
        frame.Height -= 4;
        spriteBatch.Draw(glowTexture.Value, item.Center - Main.screenPosition, frame, color * 0.5f, rotation, frame.Size() / 2, scale, 0, 0);
    }

    public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
    {
        var glowTexture = Assets.Textures.Content.Vanity.CountChaos.CountChaosCuirass_Item_Glow.Asset;
        spriteBatch.Draw(glowTexture.Value, position, null, drawColor with { A = 0 } * 0.5f, 0, origin, scale, 0, 0);
    }
}

[AutoloadEquip(EquipType.Legs)]
public class CountChaosGownItem : VanityItem, IUpdateArmorInVanity
{
    public override string Texture => Assets.Textures.Content.Vanity.CountChaos.CountChaosGown_Item.KEY;

    public override int Rarity => ModContent.RarityType<CommonVanityRarity>();

    public override void SetStaticDefaults()
    {
        ArmorIDs.Legs.Sets.HidesBottomSkin[Item.legSlot] = true;
        ArmorIDs.Legs.Sets.OverridesLegs[Item.legSlot] = true;
        ArmorIDs.Legs.Sets.IncompatibleWithFrogLeg[Item.legSlot] = true;

        BlockVanity.Sets.HideLegs[Item.legSlot] = true;
    }

    public override void UpdateEquip(Player player)
    {
        player.GetModPlayer<MiscEffectPlayer>().disableBootsEffect = true;
        player.GetModPlayer<MiscEffectPlayer>().SetWalkSpeed(0.275f);
    }
}
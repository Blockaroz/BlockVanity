using BlockVanity.Common.Players;
using BlockVanity.Common.UI;
using BlockVanity.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Vanity.Midra;

[AutoloadEquip(EquipType.Head)]
public class EyeOfFrenziedFlame : VanityItem
{
    public EyeOfFrenziedFlame() : base(ItemRarityID.Cyan) { }

    public override void SetStaticDefaults()
    {
        MiscEffectPlayer.hideHead.Add(Item.headSlot);
        ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;
    }

    public override bool IsArmorSet(Item head, Item body, Item legs) => true;
    public override bool IsVanitySet(int head, int body, int legs) => true;

    public override void UpdateArmorSet(Player player)
    {
        player.GetModPlayer<PlayerOfFrenziedFlame>().forceFlameBack = true;

        if (AreaEffectsToggle.IsActive(player))
        {
            Lighting.AddLight(player.MountedCenter, Color.DarkOrange.ToVector3() * 0.7f);
        }
    }

    public override void UpdateVanitySet(Player player) => UpdateArmorSet(player);

    public static Asset<Texture2D> glowTexture;

    public override void Load()
    {
        glowTexture = ModContent.Request<Texture2D>(Texture + "_Glow");
    }

    public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
    {
        spriteBatch.Draw(glowTexture.Value, position, glowTexture.Frame(), Color.White with { A = 200 }, 0, glowTexture.Size() * 0.5f, scale, 0, 0);
    }

    public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
    {
        spriteBatch.Draw(glowTexture.Value, Item.Center - Vector2.UnitY * 2 - Main.screenPosition, glowTexture.Frame(), Color.White with { A = 200 }, rotation, glowTexture.Size() * 0.5f, scale, 0, 0);
    }
}

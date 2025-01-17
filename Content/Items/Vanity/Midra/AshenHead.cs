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
public class AshenHead : VanityItem, IUpdateArmorInVanity
{
    public AshenHead() : base(ItemRarityID.Cyan, 32, 36) { }

    public override void SetStaticDefaults()
    {
        MiscEffectPlayer.hideHead.Add(Item.headSlot);
        ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;
    }

    public override void UpdateEquip(Player player)
    {
        player.GetModPlayer<MiscEffectPlayer>().disableBootsEffect = true;
        player.GetModPlayer<MiscEffectPlayer>().SetWalkSpeed(0.5f);
    }

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
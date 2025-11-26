using BlockVanity.Core;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Vanity.Midra;

[AutoloadEquip(EquipType.Back)]
public class MidraCloak : VanityItem
{
    public MidraCloak() : base(ItemRarityID.Cyan, accessory: true) { }

    public override void SetStaticDefaults()
    {
    }

    public static Asset<Texture2D> cloakBodyTexture;
    public static Asset<Texture2D> cloakCapeTexture;

    public override void Load()
    {
        cloakBodyTexture = ModContent.Request<Texture2D>($"{Texture}_Body");
        cloakCapeTexture = ModContent.Request<Texture2D>($"{Texture}_Cape");
    }
}
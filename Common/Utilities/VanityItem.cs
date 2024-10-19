using Microsoft.Xna.Framework.Graphics;
using ReLogic.Utilities;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Common.Utilities;

public abstract class VanityItem : ModItem
{
    private int _rarity;
    private int _width;
    private int _height;
    private int _value;
    private bool _isAccessory;

    public VanityItem(int rarity = ItemRarityID.White, int width = 24, int height = 24, int value = 0, bool accessory = false)
    {
        _rarity = rarity;
        _width = width;
        _height = height;
        _value = height;
        _isAccessory = accessory;
    }

    public override void SetDefaults()
    {
        Item.width = _width;
        Item.height = _height;
        Item.accessory = _isAccessory;
        Item.value = _value;
        Item.vanity = true;
        Item.hasVanityEffects = true;
        Item.rare = _rarity;
    }
}

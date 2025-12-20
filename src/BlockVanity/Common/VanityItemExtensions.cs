using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Common;

public abstract class VanityItem(int width = 24, int height = 24, int value = 0, bool accessory = false) : ModItem
{
    public virtual int Rarity => ItemRarityID.White;

    public override void SetDefaults()
    {
        Item.width = width;
        Item.height = height;
        Item.accessory = accessory;
        Item.value = value;
        Item.vanity = true;
        Item.hasVanityEffects = true;
        Item.rare = Rarity;
    }  
}
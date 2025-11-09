using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Core;

public abstract class VanityItem : ModItem
{
    public virtual int Rarity { get; }
    private int width;
    private int height;
    private int value;
    private bool isAccessory;

    public VanityItem(int width = 24, int height = 24, int value = 0, bool accessory = false)
    {
        this.width = width;
        this.height = height;
        this.value = height;
        this.isAccessory = accessory;
    }

    public override void SetDefaults()
    {
        Item.width = width;
        Item.height = height;
        Item.accessory = isAccessory;
        Item.value = value;
        Item.vanity = true;
        Item.hasVanityEffects = true;
        Item.rare = Rarity;
    }
}
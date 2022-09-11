using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.Enums;

namespace BlockVanity.Core
{
    public abstract class VanityItem : ModItem
    {
        private string _name;
        private string _desc;
        private int _rarity;
        private bool _isAnAccessory;

        public VanityItem(string name, int rarity, string toolTip = null, bool accessory = false)
        {
            _name = name;
            _desc = toolTip;
            _rarity = rarity;
            _isAnAccessory = accessory;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(_name);
            if (_desc != null)
                Tooltip.SetDefault(_desc);
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            PostStaticDefaults();
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.vanity = true;
            Item.accessory = _isAnAccessory;
            Item.canBePlacedInVanityRegardlessOfConditions = true;
            Item.rare = _rarity;
            PostDefaults();
        }

        public virtual void PostStaticDefaults() { }
        
        public virtual void PostDefaults() { }
    }
}

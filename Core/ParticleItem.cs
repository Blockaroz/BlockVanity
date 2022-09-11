using System;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace BlockVanity.Core
{
    public abstract class ParticleItem : ModItem
    {
        private string _name;
        private string _desc;
        private int _rarity;

        public ParticleItem(string name, int rarity, string toolTip = null)
        {
            _name = name;
            _desc = toolTip;
            _rarity = rarity;
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
            Item.DefaultToAccessory(); 
            Item.vanity = true; 
            Item.rare = _rarity;
            PostDefaults();
        }

        public virtual void PostStaticDefaults() { }

        public virtual void PostDefaults() { }

        public virtual void UpdateParticleEffect(Player player) { }

    }
}

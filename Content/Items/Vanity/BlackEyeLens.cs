using BlockVanity.Common.Players;
using BlockVanity.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Vanity
{
    //[AutoloadEquip(EquipType.Face)]
    public class BlackEyeLens : VanityItem
    {
        public BlackEyeLens() : base("Black Scleral Lens", ItemRarityID.Blue, "Gives your eyes black sclerae", true) { }

        public override void UpdateVanity(Player player)
        {
            player.GetModPlayer<MiscEffectPlayer>().accBlackEye = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.BlackLens, 2)
                .AddTile(TileID.Furnaces)
                .Register();
        }

        public override void Load() => On.Terraria.DataStructures.PlayerDrawLayers.DrawPlayer_21_Head_TheFace += SetEyeBlack;

        private void SetEyeBlack(On.Terraria.DataStructures.PlayerDrawLayers.orig_DrawPlayer_21_Head_TheFace orig, ref PlayerDrawSet drawinfo)
        {
            if (drawinfo.drawPlayer.GetModPlayer<MiscEffectPlayer>().accBlackEye)
                drawinfo.colorEyeWhites = new Color(30, 30, 30);
            orig(ref drawinfo);
        }
    }
}

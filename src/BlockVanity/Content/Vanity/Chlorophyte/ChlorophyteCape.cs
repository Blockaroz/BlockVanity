using BlockVanity.Common.Utilities;
using BlockVanity.Content.Vanity.Midra;
using BlockVanity.Core;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Vanity.Chlorophyte;

//public sealed class ChlorophyteCape() : VanityItem(accessory: true)
//{
//    public override int Rarity => ItemRarityID.Green;
//}

//public sealed class ChlorophyteCapeArmLayer : PlayerDrawLayer
//{
//    public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.ArmOverItem);

//    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.back == EquipLoader.GetEquipSlot(Mod, nameof(MidraCloak), EquipType.Back);

//    protected override void Draw(ref PlayerDrawSet drawInfo)
//    {
//    }
//}
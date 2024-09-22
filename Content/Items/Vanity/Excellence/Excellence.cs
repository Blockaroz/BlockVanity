using System;
using BlockVanity.Common.Players;
using BlockVanity.Common.Utilities;
using BlockVanity.Content.Rarities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Vanity.Excellence;

public class Excellence : VanityItem
{
    public Excellence() : base(ModContent.RarityType<PerfectRarity>(), 34, 30, Item.buyPrice(gold: 15), true) { }

    public override void Load()
    {
        if (Main.netMode == NetmodeID.Server)
            return;

        EquipLoader.AddEquipTexture(Mod, $"{Texture}_{EquipType.Head}", EquipType.Head, this);
        EquipLoader.AddEquipTexture(Mod, $"{Texture}_{EquipType.Body}", EquipType.Body, this);
        EquipLoader.AddEquipTexture(Mod, $"{Texture}_{EquipType.Legs}", EquipType.Legs, this);
    }

    public override void SetStaticDefaults()
    {
        ItemID.Sets.ItemNoGravity[Type] = true;

        if (Main.netMode == NetmodeID.Server)
            return;

        int head = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
        int body = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Body);
        int legs = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Legs);

        ArmorIDs.Head.Sets.DrawHead[head] = false;

        ArmorIDs.Body.Sets.HidesTopSkin[body] = true;
        ArmorIDs.Body.Sets.DisableBeltAccDraw[body] = true;
        ArmorIDs.Body.Sets.DisableHandOnAndOffAccDraw[body] = true;

        ArmorIDs.Legs.Sets.HidesTopSkin[legs] = true;
        ArmorIDs.Legs.Sets.OverridesLegs[legs] = true;
    }

    public override void UpdateVanity(Player player) => player.GetModPlayer<MiscEffectPlayer>().disableBootsEffect = true;

    public override void Update(ref float gravity, ref float maxFallSpeed)
    {
        Item.position.Y += MathF.Sin(Item.timeSinceItemSpawned * 0.05f) * 0.3f;
    }

    public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
    {
        Texture2D glowTexture = AllAssets.Textures.Glow[0].Value;
        Vector2 offset = Vector2.UnitY * 9 * scale;
        spriteBatch.Draw(glowTexture, position - offset, glowTexture.Frame(), Color.Red with { A = 10 }, 0, glowTexture.Size() * 0.5f, scale * 0.3f, 0, 0);    
    }

    public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
    {
        Texture2D glowTexture = AllAssets.Textures.Glow[0].Value;
        Vector2 offset = Vector2.UnitY.RotatedBy(rotation) * 9 * scale;
        spriteBatch.Draw(glowTexture, Item.Center - Main.screenPosition - offset, glowTexture.Frame(), Color.Red with { A = 10 }, rotation, glowTexture.Size() * 0.5f, scale * 0.3f, 0, 0);
    }
}

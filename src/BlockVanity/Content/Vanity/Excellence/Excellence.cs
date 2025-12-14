using BlockVanity.Common.Players;
using BlockVanity.Content.Rarities;
using BlockVanity.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Vanity.Excellence;

public class Excellence : VanityItem
{
    public Excellence() : base(34, 30, Item.buyPrice(gold: 15), true) { }

    public override int Rarity => ModContent.RarityType<PerfectRarity>();

    public override void Load()
    {
        if (Main.dedServ)
            return;

        EquipLoader.AddEquipTexture(Mod, $"{Texture}_{EquipType.Head}", EquipType.Head, this);
        EquipLoader.AddEquipTexture(Mod, $"{Texture}_{EquipType.Body}", EquipType.Body, this);
        EquipLoader.AddEquipTexture(Mod, $"{Texture}_{EquipType.Legs}", EquipType.Legs, this);
    }

    public override void SetStaticDefaults()
    {
        ItemID.Sets.ItemNoGravity[Type] = true;

        if (Main.dedServ)
            return;

        Item.headSlot = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
        Item.bodySlot = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Body);
        Item.legSlot = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Legs);

        BlockVanity.Sets.HideHead[Item.headSlot] = true;
        BlockVanity.Sets.HideLegs[Item.legSlot] = true;

        ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;

        ArmorIDs.Body.Sets.HidesTopSkin[Item.bodySlot] = true;
        ArmorIDs.Body.Sets.DisableBeltAccDraw[Item.bodySlot] = true;
        ArmorIDs.Body.Sets.DisableHandOnAndOffAccDraw[Item.bodySlot] = true;

        ArmorIDs.Legs.Sets.HidesTopSkin[Item.legSlot] = true;
        ArmorIDs.Legs.Sets.HidesBottomSkin[Item.legSlot] = true;
        ArmorIDs.Legs.Sets.OverridesLegs[Item.legSlot] = true;
        ArmorIDs.Legs.Sets.IncompatibleWithFrogLeg[Item.legSlot] = true;

        GlowTexture = ModContent.Request<Texture2D>(Texture + "_Glow");
    }

    public override void UpdateVanity(Player player)
    {
        player.GetModPlayer<MiscEffectPlayer>().disableBootsEffect = true;
        player.GetModPlayer<MiscEffectPlayer>().SetWalkSpeed(0.6f);
    }

    public override void ArmorSetShadows(Player player)
    {
    }

    public override void Update(ref float gravity, ref float maxFallSpeed)
    {
        Item.position.Y += MathF.Sin(Item.timeSinceItemSpawned * 0.05f) * 0.3f;
        Lighting.AddLight(Item.Center, Color.DarkRed.ToVector3());
    }

    public override void UpdateVisibleAccessory(Player player, bool hideVisual)
    {
        if (!hideVisual)
        {
            player.GetModPlayer<ExcellencePlayer>().Enabled = true;
            player.head = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
            player.body = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Body);
            player.legs = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Legs);
        }
    }

    public override Color? GetAlpha(Color lightColor) => lightColor;

    public static Asset<Texture2D> GlowTexture { get; private set; }

    public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
    {
        Texture2D glowTexture = GlowTexture.Value;
        spriteBatch.Draw(glowTexture, position, glowTexture.Frame(), Color.White with { A = 10 }, 0, glowTexture.Size() * 0.5f, scale, 0, 0);
    }

    public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
    {
        Texture2D glowTexture = GlowTexture.Value;
        spriteBatch.Draw(glowTexture, Item.Center - Main.screenPosition, glowTexture.Frame(), Color.White with { A = 10 }, rotation, glowTexture.Size() * 0.5f, scale, 0, 0);
    }
}
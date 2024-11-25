using BlockVanity.Common.Graphics;
using BlockVanity.Common.UI;
using BlockVanity.Common.Utilities;
using BlockVanity.Content.Dusts;
using BlockVanity.Content.Items.Vanity.BoneKing.Platinum;
using BlockVanity.Content.Items.Vanity.Scholar;
using BlockVanity.Content.Particles;
using BlockVanity.Content.Rarities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Vanity.BoneKing.Hellforged;

public class HellforgedKingSkull : VanityItem
{
    public override void Load()
    {
        if (Main.dedServ)
            return;

        EquipLoader.AddEquipTexture(Mod, $"{Texture}_{EquipType.Head}", EquipType.Head, this, equipTexture: new OversizedHeadEquipTexture(0, -4));
    }

    public override LocalizedText DisplayName => ModContent.GetInstance<BoneKingSkull>().DisplayName;
    public override LocalizedText Tooltip => ModContent.GetInstance<BoneKingSkull>().Tooltip;

    public HellforgedKingSkull() : base(ModContent.RarityType<VanityRareCommon>(), 30, 34) { }

    public override void SetStaticDefaults()
    {
        if (Main.dedServ)
            return;

        Item.headSlot = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
        ArmorIDs.Head.Sets.IsTallHat[Item.headSlot] = true;
        ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;

        glowTexture = ModContent.Request<Texture2D>(Texture + "_Glow");
        glowTextureArmor = ModContent.Request<Texture2D>(Texture + "_Head_Glow");
    }

    public static Asset<Texture2D> glowTexture;
    public static Asset<Texture2D> glowTextureArmor;

    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.headSlot = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
    }

    public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ModContent.ItemType<HellforgedKingGarb>() && legs.type == ModContent.ItemType<HellforgedKingPants>();

    public override void UpdateVanitySet(Player player)
    {
        if (AreaEffectsToggle.IsActive(player))
        {
            int randomShader = Main.rand.NextFromList(player.cHead, player.cBody, player.cLegs);
            ArmorShaderData shaderData = GameShaders.Armor.GetSecondaryShader(randomShader, player);

            if (Main.rand.NextBool(15))
            {
                Dust spark = Dust.NewDustDirect(player.MountedCenter - new Vector2(10, 20), 20, 40, DustID.Torch, 0f, -5f);
                spark.velocity.Y -= player.gravDir;
                spark.velocity.X += player.velocity.X * 0.2f;
                spark.noGravity = true;
                spark.noLight = true;
                spark.fadeIn = 1f;
                if (randomShader > 0)
                    spark.shader = shaderData;
            }

            if (Main.rand.NextBool(12))
            {
                Vector2 particlePos = player.MountedCenter + Main.rand.NextVector2Circular(player.width, player.height / 2f);
                Vector2 particleVel = new Vector2(player.velocity.X * 0.1f, -player.gravDir).RotatedByRandom(0.5f);
                Dust fire = Dust.NewDustPerfect(particlePos, ModContent.DustType<HellfireSparkDust>(), particleVel, 0, Color.LightGoldenrodYellow with { A = 0 }, Main.rand.NextFloat(0.5f, 1f));
                fire.fadeIn = Main.rand.Next(100, 120);
                if (randomShader > 0)
                    fire.shader = GameShaders.Armor.GetSecondaryShader(randomShader, player);
            }
        }
    }

    public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
    {
        spriteBatch.Draw(glowTexture.Value, Item.Center - Main.screenPosition, glowTexture.Frame(), Color.White with { A = 150 }, rotation, glowTexture.Size() / 2f, scale, 0, 0);
    }

    public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
    {
        spriteBatch.Draw(glowTexture.Value, position, glowTexture.Frame(), Color.White with { A = 150 }, 0, origin, scale, 0, 0);
    }
}

using BlockVanity.Common;
using BlockVanity.Common.Utilities;
using BlockVanity.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Vanity.FrenziedLord;

[AutoloadEquip(EquipType.Back)]
public class FrenziedLordCloakItem() : VanityItem(accessory: true)
{
    public override string Texture => Assets.Textures.Content.Vanity.FrenziedLord.FrenziedLordCloak_Item.KEY;

    public override int Rarity => ItemRarityID.Cyan;
}

public class FrenziedLordCloakBodyLayer : PlayerDrawLayer
{
    public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.NeckAcc);

    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.back == EquipLoader.GetEquipSlot(Mod, nameof(FrenziedLordCloakItem), EquipType.Back);

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        var texture = Assets.Textures.Content.Vanity.FrenziedLord.FrenziedLordCloak_Item_Body.Asset;
        Vector2 position = drawInfo.BodyPosition();
        position.ApplyVerticalOffset(drawInfo);
        position.Y += drawInfo.torsoOffset;

        DrawData data = new DrawData(texture.Value, position, drawInfo.compTorsoFrame, drawInfo.colorArmorBody, drawInfo.drawPlayer.bodyRotation, drawInfo.bodyVect, 1f, drawInfo.playerEffect, 0);
        data.shader = drawInfo.cBack;
        drawInfo.DrawDataCache.Add(data);
    }
}

public class FrenziedLordCloakArmLayer : PlayerDrawLayer
{
    public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.ArmOverItem);

    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.back == EquipLoader.GetEquipSlot(Mod, nameof(FrenziedLordCloakItem), EquipType.Back);

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        var texture = Assets.Textures.Content.Vanity.FrenziedLord.FrenziedLordCloak_Item_Body.Asset;

        Vector2 compOffset = VanityUtils.GetCompositeOffset_FrontArm(ref drawInfo);
        Vector2 position = drawInfo.BodyPosition();
        position.ApplyVerticalOffset(drawInfo);
        position.Y += drawInfo.torsoOffset;

        if (drawInfo.compFrontArmFrame.X / drawInfo.compFrontArmFrame.Width >= 7)
            position += new Vector2((!drawInfo.playerEffect.HasFlag(SpriteEffects.FlipHorizontally)) ? 1 : (-1), (!drawInfo.playerEffect.HasFlag(SpriteEffects.FlipVertically)) ? 1 : (-1));

        DrawData data = new DrawData(texture.Value, position + compOffset, drawInfo.compFrontArmFrame, drawInfo.colorArmorBody, drawInfo.compositeFrontArmRotation, drawInfo.bodyVect + compOffset, 1f, drawInfo.playerEffect, 0);
        data.shader = drawInfo.drawPlayer.cBack;
        drawInfo.DrawDataCache.Add(data);
    }
}

// TODO
public class FrenziedLordCloakCapeLayer : PlayerDrawLayer
{
    public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.BackAcc);

    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.back == EquipLoader.GetEquipSlot(Mod, nameof(FrenziedLordCloakItem), EquipType.Back);

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        var texture = Assets.Textures.Content.Vanity.FrenziedLord.FrenziedLordCloak_Item_Cape.Asset;
        Vector2 position = drawInfo.BodyPosition();
        position.Y += drawInfo.torsoOffset;

        Vector2 walkOffset = Vector2.Zero;
        walkOffset.ApplyVerticalOffset(drawInfo);
    }
}
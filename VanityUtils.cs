using System;
using BlockVanity.Common.Players;
using BlockVanity.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;

namespace BlockVanity;

public static class VanityUtils
{
    public static Vector2 HeadPosition(this PlayerDrawSet drawInfo) => drawInfo.drawPlayer.GetHelmetDrawOffset() + new Vector2((int)(drawInfo.Position.X - Main.screenPosition.X - drawInfo.drawPlayer.bodyFrame.Width / 2 + drawInfo.drawPlayer.width / 2), (int)(drawInfo.Position.Y - Main.screenPosition.Y + drawInfo.drawPlayer.height - drawInfo.drawPlayer.bodyFrame.Height + 4f)) + drawInfo.drawPlayer.headPosition + drawInfo.headVect + drawInfo.helmetOffset;

    public static Vector2 BodyPosition(this PlayerDrawSet drawInfo) => new Vector2((int)(drawInfo.Position.X - Main.screenPosition.X - drawInfo.drawPlayer.bodyFrame.Width / 2 + drawInfo.drawPlayer.width / 2), (int)(drawInfo.Position.Y - Main.screenPosition.Y + drawInfo.drawPlayer.height - drawInfo.drawPlayer.bodyFrame.Height + 4f)) + drawInfo.drawPlayer.bodyPosition + new Vector2(drawInfo.drawPlayer.bodyFrame.Width / 2, drawInfo.drawPlayer.bodyFrame.Height / 2);

    public static Vector2 LegsPosition(this PlayerDrawSet drawInfo) => new Vector2((int)(drawInfo.Position.X - Main.screenPosition.X - drawInfo.drawPlayer.legFrame.Width / 2 + drawInfo.drawPlayer.width / 2), (int)(drawInfo.Position.Y - Main.screenPosition.Y + drawInfo.drawPlayer.height - drawInfo.drawPlayer.legFrame.Height + 4f)) + drawInfo.drawPlayer.legPosition + drawInfo.legVect;

    public static void ApplyVerticalOffset(ref this Vector2 drawPos, PlayerDrawSet drawInfo)
    {
        Vector2 value = Main.OffsetsPlayerHeadgear[drawInfo.drawPlayer.bodyFrame.Y / drawInfo.drawPlayer.bodyFrame.Height];
        value.Y -= 2f;
        drawPos += value * -drawInfo.playerEffect.HasFlag(SpriteEffects.FlipVertically).ToDirectionInt();
    }

    /// <summary>
    /// Calculates the midpoint limb of a two-limbed IK system via trigonometry.
    /// </summary>
    /// <param name="start">The start of the IK system.</param>
    /// <param name="end">The end effector position of the IK system.</param>
    /// <param name="firstLength">The length of the first limb.</param>
    /// <param name="secondLength">The length of the second limb.</param>
    /// <param name="flip">Whether the angles need to be flipped.</param>
    public static Vector2 FindIKElbow(Vector2 start, Vector2 end, float firstLength, float secondLength, bool flip)
    {
        float c = Vector2.Distance(start, end);
        float angle = MathF.Acos(Math.Clamp((c * c + firstLength * firstLength - secondLength * secondLength) / (c * firstLength * 2f), -1f, 1f)) * flip.ToDirectionInt();
        return start + (angle + start.AngleTo(end)).ToRotationVector2() * firstLength;
    }

    public static Vector3 ToVector3(this Vector2 vector, float depth = 0f) => new Vector3(vector, depth);

    public static Matrix NormalizedEffectMatrix => Matrix.Invert(Main.GameViewMatrix.EffectMatrix) * Matrix.CreateOrthographicOffCenter(0f, Main.instance.GraphicsDevice.Viewport.Width, Main.instance.GraphicsDevice.Viewport.Height, 0f, 0f, 1f);

    public static Color ToGrayscale(this Color color)
    {
        Vector3 colorValues = color.ToVector3();
        float grayValue = colorValues.X * 0.299f + colorValues.Y * 0.587f + colorValues.Z * 0.114f;
        return new Color(grayValue, grayValue, grayValue, color.A / 255f);
    }

    public delegate void DrawSittingLegsDelegate(ref PlayerDrawSet drawinfo, Texture2D textureToDraw, Color matchingColor, int shaderIndex = 0, bool glowmask = false);
    public delegate void DrawSittingCoatsDelegate(ref PlayerDrawSet drawinfo, Texture2D textureToDraw, Color matchingColor, int shaderIndex = 0, bool glowmask = false);

    public static Vector2 GetCompositeOffset_BackArm(ref PlayerDrawSet drawinfo) => new Vector2(6 * ((!drawinfo.playerEffect.HasFlag(SpriteEffects.FlipHorizontally)) ? 1 : (-1)), 2 * ((!drawinfo.playerEffect.HasFlag(SpriteEffects.FlipVertically)) ? 1 : (-1)));
    public static Vector2 GetCompositeOffset_FrontArm(ref PlayerDrawSet drawinfo) => new Vector2(-5 * ((!drawinfo.playerEffect.HasFlag(SpriteEffects.FlipHorizontally)) ? 1 : (-1)), 0f);

    public static int BodyFrameArmFromRotation(Player player, float rotation)
    {
        rotation = Math.Clamp(rotation, 0, MathHelper.Pi);

        if (rotation <= MathHelper.PiOver4 * 0.3f)
            return 1;        
        if (rotation <= MathHelper.PiOver4)
            return 2;
        else if (rotation <= MathHelper.PiOver4 * 2.2f)
            return 3;            
        else if (rotation <= MathHelper.PiOver4 * 3.5f)
            return 4;
        else
            return 0;
    }

    public static void DrawSittingLongCoats(ref PlayerDrawSet drawinfo, Texture2D textureToDraw, Color matchingColor, int shaderIndex = 0, bool glowmask = false)
    {
        Vector2 position = new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.legFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.legFrame.Height + 4f)) + drawinfo.drawPlayer.legPosition + drawinfo.legVect;
        Rectangle legFrame = drawinfo.drawPlayer.legFrame;
        position += drawinfo.legsOffset;
        position.X += 2 * drawinfo.drawPlayer.direction;
        legFrame.Y = legFrame.Height * 5;

        DrawData item = new DrawData(textureToDraw, position, legFrame, matchingColor, drawinfo.drawPlayer.legRotation, drawinfo.legVect, 1f, drawinfo.playerEffect);
        item.shader = shaderIndex;
        drawinfo.DrawDataCache.Add(item);
    }

    public static Player CreatePlayer(int head = -1, int body = -1, int legs = -1, int acc1 = -1, int acc2 = -1)
    {
        Player newPlayer = new Player();
        if (head > -1)
            newPlayer.armor[10] = ContentSamples.ItemsByType[head];
        if (body > -1)
            newPlayer.armor[11] = ContentSamples.ItemsByType[body];
        if (legs > -1)
            newPlayer.armor[12] = ContentSamples.ItemsByType[legs];
        if (acc1 > -1)
            newPlayer.armor[13] = ContentSamples.ItemsByType[acc1];
        if (acc2 > -1)
            newPlayer.armor[14] = ContentSamples.ItemsByType[acc2];

        return newPlayer;
    }

    public static PetPlayer GetPets(this Player player) => player.GetModPlayer<PetPlayer>();
}

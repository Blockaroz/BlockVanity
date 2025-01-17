using BlockVanity.Content.Rarities.GlowingRarities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace BlockVanity.Content.Rarities.SpecialRarities;

public class SpecialRarityItem : GlobalItem
{
    //public override void Load()
    //{
    //    IL_Main.MouseTextInner += DrawHoverName;
    //    IL_Main.DrawItemTextPopups += DrawPopupName;
    //}

    //private void DrawHoverName(ILContext il)
    //{
    //    try
    //    {
    //        ILCursor c = new ILCursor(il);
    //        c.TryGotoNext(n => n.MatchCall(typeof(ChatManager), "DrawColorCodedStringWithShadow"));
    //        c.TryGotoPrev(n => n.MatchLdsfld<Main>("spriteBatch"));
    //        ILLabel label = c.DefineLabel();
    //        c.Emit(OpCodes.Ldloc_1);
    //        c.Emit(OpCodes.Ldloc_0);
    //        c.Emit(OpCodes.Ldloc, 7);
    //        c.Emit(OpCodes.Ldloc, 8);
    //        c.EmitDelegate<Func<int, string, int, int, bool>>((rare, text, x, y) =>
    //        {
    //            if (RarityLoader.GetRarity(rare) is SpecialRarity rarity)
    //            {
    //                rarity.DrawRareLine(text, new Vector2(x, y), 0f, Vector2.Zero, Vector2.One);
    //                return true;
    //            }
    //            return false;
    //        });
    //        c.Emit(OpCodes.Brfalse, label);
    //        c.Emit(OpCodes.Ret);
    //        c.MarkLabel(label);
    //    }
    //    catch
    //    {
    //        MonoModHooks.DumpIL(Mod, il);
    //        Console.Write("Special rarities in-world hover text IL failed");
    //    }
    //}

    //private void DrawPopupName(ILContext il)
    //{

    //}

    public override bool PreDrawTooltipLine(Item item, DrawableTooltipLine line, ref int yOffset)
    {
        if (line.Mod == "Terraria" && line.Name == "ItemName")
        {
            if (RarityLoader.GetRarity(item.rare) is SpecialRarity specialRarity)
            {
                specialRarity.DrawRareLine(line.Text, new Vector2(line.X, line.Y + yOffset), line.Rotation, line.Origin, line.BaseScale);
                return false;
            }
        }

        return base.PreDrawTooltipLine(item, line, ref yOffset);
    }
}
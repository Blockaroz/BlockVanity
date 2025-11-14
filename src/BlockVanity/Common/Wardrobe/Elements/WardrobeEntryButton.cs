using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader.UI;
using Terraria.UI;
using Terraria.UI.Chat;

namespace BlockVanity.Common.Wardrobe;

public sealed class WardrobeEntryButton : UIElement
{
    private sealed class WardrobePortraitElement : UIElement
    {
        public QuestEntry Entry { get; set; }

        public WardrobePortraitElement(QuestEntry entry)
        {
            Entry = entry;
            Width.Set(0f, 1f);
            Height.Set(0f, 1f);

            IgnoresMouseInteraction = true;
        }

        private float HoverScale = 1f;

        public override void Update(GameTime gameTime)
        {
            if (Parent.IsMouseHovering)
                HoverScale = MathHelper.Lerp(HoverScale, 1.33f, 0.3f);
            else
                HoverScale = MathHelper.Lerp(HoverScale, 1f, 0.3f);

            if (!Entry.IsLocked)
                Entry.Icon?.UpdatePortrait(new WardrobePortraitInfo(Entry, GetDimensions().Center(), Parent.IsMouseHovering, HoverScale, true));
        }

        private static LazyAsset<Texture2D> LockedIcon = new LazyAsset<Texture2D>("Terraria/Images/UI/Bestiary/Icon_Locked");

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            var dimensions = GetDimensions();
            Vector2 center = dimensions.Center();

            if (Entry.IsLocked)
            {
                Texture2D shadow = Assets.Textures.Glow[0].Value;
                spriteBatch.Draw(shadow, center, shadow.Frame(), Color.Black * 0.1f, 0, shadow.Size() / 2, 0.66f, 0, 0);

                Texture2D mark = LockedIcon.Value;
                spriteBatch.Draw(mark, center, mark.Frame(), Color.White * 0.2f, 0, mark.Size() / 2, 1f, 0, 0);
            }
            else
                Entry.Icon?.DrawPortrait(spriteBatch, new WardrobePortraitInfo(Entry, center, Parent.IsMouseHovering, HoverScale, true));
        }
    }

    public QuestEntry Entry { get; set; }

    public WardrobeEntryButton(QuestEntry entry)
    {
        Entry = entry;
        Width.Set(70, 0);
        Height.Set(78, 0);

        var container = new UIElement();
        container.Left.Set(2f, 0f);
        container.Top.Set(2f, 0f);
        container.Width.Set(-4f, 1f);
        container.Height.Set(-4f, 1f);
        container.OverflowHidden = true;
        Append(container);

        var element = new WardrobePortraitElement(entry);
        container.Append(element);
        Recalculate();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        Entry.CompletionState = QuestUnlockState.Complete;
        if (IsMouseHovering)
            Main.instance.MouseTextHackZoom(Entry.IsLocked ? "???" : Entry.DisplayName.Value);
    }

    public override void MouseOver(UIMouseEvent evt)
    {
        base.MouseOver(evt);
        SoundEngine.PlaySound(SoundID.MenuTick);
    }

    public static LazyAsset<Texture2D> Border = new LazyAsset<Texture2D>($"{nameof(BlockVanity)}/Assets/Textures/UI/MagicalWardrobe/Entry");
    public static LazyAsset<Texture2D> BorderHiglight = new LazyAsset<Texture2D>($"{nameof(BlockVanity)}/Assets/Textures/UI/MagicalWardrobe/Entry_Highlight");
    public static LazyAsset<Texture2D> BackGradient = new LazyAsset<Texture2D>($"{nameof(BlockVanity)}/Assets/Textures/UI/MagicalWardrobe/Entry_Back");

    public override void Draw(SpriteBatch spriteBatch)
    {
        Vector2 center = GetDimensions().Center();
        Texture2D border = Border.Value;
        Texture2D hightlight = BorderHiglight.Value;
        Texture2D backGradient = BackGradient.Value;

        spriteBatch.Draw(backGradient, center, null, Color.White, 0, backGradient.Size() / 2, 1f, 0, 0);
        
        base.Draw(spriteBatch);

        Vector2 idPos = new Vector2(GetDimensions().X + 6, GetDimensions().Y + 5);
        Utils.DrawBorderString(spriteBatch, Entry.ID.ToString(), idPos, Color.White, 0.85f);

        DrawStarCount(spriteBatch);

        spriteBatch.Draw(border, center, null, Color.White, 0, border.Size() / 2, 1f, 0, 0);

        if (IsMouseHovering)
        {
            spriteBatch.Draw(hightlight, center, null, Color.White, 0, border.Size() / 2, 1f, 0, 0);
        }
    }

    private void DrawStarCount(SpriteBatch spriteBatch)
    {
        string starCount = Entry.IsLocked ? "???" : Entry.Stars.ToString();

        var starCountSize = FontAssets.MouseText.Value.MeasureString(starCount);
        Vector2 textPosition = GetDimensions().Center() + new Vector2(starCountSize.X / 2.33f - 8f, 24);
        Utils.DrawBorderString(spriteBatch, starCount, textPosition, Color.White, 0.85f, 1f, 0.35f);

        Texture2D star = Assets.Textures.VanityStar.Value;
        Rectangle frame = star.Frame(1, 2, 0, 0);
        Rectangle glowFrame = star.Frame(1, 2, 0, 1);
        float hue = (GetDimensions().X - GetDimensions().Y) * 0.002f + Main.GlobalTimeWrappedHourly * 0.3f;
        Color glowColor = Main.hslToRgb(hue % 1f, 1f, 0.66f, a: 100);
        spriteBatch.Draw(star, textPosition + new Vector2(10, 0), frame, Color.White, 0, frame.Size() / 2, 0.5f, 0, 0);
        spriteBatch.Draw(star, textPosition + new Vector2(10, 0), glowFrame, glowColor, 0, glowFrame.Size() / 2, 0.5f, 0, 0);
    }
}
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

namespace BlockVanity.Common.Wardrobe;

public sealed class WardrobeEntryButton : UIElement
{
    private sealed class WardrobePortraitElement : UIElement
    {
        public QuestEntry Entry { get; set; }

        public WardrobePortraitElement(QuestEntry entry)
        {
            Entry = entry;
            Left.Set(2f, 0f);
            Top.Set(2f, 0f);
            Width.Set(-4f, 1f);
            Height.Set(-4f, 1f);

            IgnoresMouseInteraction = true;
        }

        private static LazyAsset<Texture2D> LockedIcon = new LazyAsset<Texture2D>("Terraria/Images/UI/Bestiary/Icon_Locked");

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (Entry.IsLocked)
            {
                Vector2 center = GetDimensions().Center();

                Texture2D shadow = Assets.Textures.Glow[0].Value;
                spriteBatch.Draw(shadow, center, shadow.Frame(), Color.Black * 0.1f, 0, shadow.Size() / 2, 0.66f, 0, 0);

                Texture2D mark = LockedIcon.Value;
                spriteBatch.Draw(mark, center, mark.Frame(), Color.White * 0.2f, 0, mark.Size() / 2, 1f, 0, 0);
            }
            else
                Entry.SmallPortrait.DrawPortrait(spriteBatch, new WardrobePortraitInfo(Entry, GetDimensions().Center(), Parent.IsMouseHovering, true));
        }
    }

    public QuestEntry Entry { get; set; }

    public WardrobeEntryButton(QuestEntry entry)
    {
        Entry = entry;
        Width.Set(70, 0);
        Height.Set(78, 0);
        OverflowHidden = true;

        Append(new WardrobePortraitElement(entry));
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (IsMouseHovering)
            Main.instance.MouseTextHackZoom(Entry.IsLocked ? "???" : Entry.DisplayName.Value);

        if (!Entry.IsLocked)
            Entry.SmallPortrait.UpdatePortrait(new WardrobePortraitInfo(Entry, GetDimensions().Center(), IsMouseHovering, true));
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
        spriteBatch.Draw(border, center, null, Color.White, 0, border.Size() / 2, 1f, 0, 0);

        if (IsMouseHovering)
        {
            spriteBatch.Draw(hightlight, center, null, Color.White, 0, border.Size() / 2, 1f, 0, 0);
        }
    }
}
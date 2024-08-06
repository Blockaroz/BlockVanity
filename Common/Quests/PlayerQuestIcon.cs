using System;
using BlockVanity.Common.UI.QuestUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;

namespace BlockVanity.Common.Quests;

public class PlayerQuestIcon : IQuestEntryIcon
{
    private bool _drawWithoutTarget;
    private Player _player;
    private float _fade;

    private RenderTarget2D _playerTarget;

    public PlayerQuestIcon(Player player)
    {
        _player = player;

        _player.socialIgnoreLight = true;
        _player.PlayerFrame();

        try
        {
            PlayerQuestIconTargetHandler.DrawPlayerToTarget += DrawPlayerToTarget;
        }
        catch
        {
            _drawWithoutTarget = true;
            ModContent.GetInstance<BlockVanity>().Logger.Warn("Quest icon failed to create target");
        }
    }

    public void Update(QuestEntry entry, EntryIconDrawSettings settings)
    {
        _fade = MathHelper.Lerp(_fade, Utils.ToInt(settings.IsHovered), settings.IsHovered ? 0.2f : 0.1f);
        if (_fade < 0.1f)
            _fade = 0f;
        if (_fade > 0.99f)
            _fade = 1f;

        _player.velocity.X = Utils.ToInt(settings.IsHovered) * 1.3f;

        _player.ResetEffects();
        _player.ResetVisibleAccessories();
        _player.UpdateMiscCounter();
        _player.UpdateDyes();
        _player.PlayerFrame();
    }

    public void Draw(QuestEntry entry, SpriteBatch spriteBatch, EntryIconDrawSettings settings)
    {
        Texture2D shadow = AllAssets.Textures.Glow[0].Value;
        spriteBatch.Draw(shadow, settings.iconbox.Center(), shadow.Frame(), Color.Black * 0.2f, 0, shadow.Size() * 0.5f, 1f, 0, 0);

        if (_drawWithoutTarget)
        {
            _player.Center = settings.iconbox.Center();

            if (entry.Completion != QuestCompletionState.Hidden)
                DrawPlayer();
        }
        else if (_playerTarget != null)
        {
            Color drawColor = Color.White;
            if (entry.Completion == QuestCompletionState.Hidden)
                drawColor = Color.Black;

            spriteBatch.Draw(_playerTarget, settings.iconbox.Center(), _playerTarget.Frame(), drawColor, 0, _playerTarget.Size() * 0.5f, 1f, 0, 0);
        }
    }

    private void DrawPlayerToTarget(SpriteBatch spriteBatch)
    {
        if (_playerTarget == null)
            _playerTarget = new RenderTarget2D(Main.graphics.GraphicsDevice, QuestUIEntryGridButton.EntryWidth, QuestUIEntryGridButton.EntryHeight);

        spriteBatch.GraphicsDevice.SetRenderTarget(_playerTarget);
        spriteBatch.GraphicsDevice.Clear(Color.Transparent);

        Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone);

        _player.Center = new Vector2(QuestUIEntryGridButton.EntryWidth, QuestUIEntryGridButton.EntryHeight) / 2f;

        DrawPlayer();

        Main.spriteBatch.End();
    }

    private void DrawPlayer()
    {
        _player.socialIgnoreLight = true;
        float playerScale = 1.5f + _fade * 0.5f;
        Vector2 centeringOffset = new Vector2(-_player.width / 3f, _player.height / 2f * _fade * 0.5f - 8);
        Main.PlayerRenderer.DrawPlayer(Main.Camera, _player, _player.Center + centeringOffset + Main.screenPosition, _player.fullRotation, _player.fullRotationOrigin, scale: playerScale);
    }
}

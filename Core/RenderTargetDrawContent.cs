using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;

namespace BlockVanity.Core;

public class RenderTargetDrawContent : INeedRenderTargetContent
{
    private struct DrawContent
    {
        public bool requested;
        public int identifier;
        public int width;
        public int height;
        public Action<SpriteBatch> draw;
        public RenderTarget2D target;
    }

    private int _capacity;
    private DrawContent[] _draws;

    private bool _ready;
    private bool _requestedAny;

    public bool IsReady => _ready;

    public RenderTargetDrawContent(int capacity)
    {
        _capacity = capacity;
        Reset();
    }

    public void Request(int width, int height, int identifier, Action<SpriteBatch> draw)
    {
        if (identifier > _capacity)
        {
            _capacity = identifier;
            _draws = new DrawContent[_capacity];
        }
        _draws[identifier].requested = true;
        _draws[identifier].width = width;
        _draws[identifier].height = height;
        _draws[identifier].draw = draw;

        _requestedAny = true;
    }

    public bool IsTargetReady(int identifier) => _draws[identifier].target != null && IsReady;

    public Texture2D GetTarget(int identifier) => _draws[identifier].target;

    public void PrepareRenderTarget(GraphicsDevice device, SpriteBatch spriteBatch)
    {
        _ready = false;

        if (_requestedAny)
        {
            for (int i = 0; i < _draws.Length; i++)
            {
                if (_draws[i].requested && _draws[i].width > 0 && _draws[i].height > 0)
                {
                    InitTargetIfNeeded(ref _draws[i].target, device, _draws[i].width, _draws[i].height, RenderTargetUsage.PreserveContents);

                    device.SetRenderTarget(_draws[i].target);
                    device.Clear(Color.Transparent);

                    _draws[i].draw.Invoke(spriteBatch);
                }
            }

            for (int i = 0; i < _draws.Length; i++)
                _draws[i].requested = false;

            device.SetRenderTarget(null);
            device.Clear(Color.Transparent);

            _requestedAny = false;
            _ready = true;
        }
    }

    public void Reset()
    {
        _ready = false;
        _requestedAny = false;
        _draws = new DrawContent[_capacity];
    }

    protected void InitTargetIfNeeded(ref RenderTarget2D target, GraphicsDevice device, int width, int height, RenderTargetUsage usage)
    {
        if (target == null || target.IsDisposed || target.Width != width || target.Height != height)
        {
            if (target != null)
            {
                target.ContentLost -= target_ContentLost;
                target.Disposing -= target_Disposing;
            }

            target = new RenderTarget2D(device, width, height, mipMap: false, device.PresentationParameters.BackBufferFormat, DepthFormat.None, 0, usage);
            target.ContentLost += target_ContentLost;
            target.Disposing += target_Disposing;
        }
    }

    private void target_Disposing(object sender, EventArgs e)
    {
        _ready = false;
    }

    private void target_ContentLost(object sender, EventArgs e)
    {
        _ready = false;
    }
}

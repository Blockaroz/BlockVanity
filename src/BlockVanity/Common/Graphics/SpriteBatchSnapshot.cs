using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockVanity.Common.Graphics;

// Adapted Daybreak Concept
public readonly record struct SpriteBatchSnapshot(
    SpriteSortMode SortMode,
    BlendState BlendState,
    SamplerState SamplerState,
    DepthStencilState DepthStencilState,
    RasterizerState Rasterizer,
    Effect Effect,
    Matrix TransformMatrix
    );

public static class SpritebatchSnapshotUtils
{
    public static void End(this SpriteBatch spriteBatch, out SpriteBatchSnapshot snapshot)
    {
        snapshot = new SpriteBatchSnapshot(
            spriteBatch.sortMode,
            spriteBatch.blendState,
            spriteBatch.samplerState,
            spriteBatch.depthStencilState,
            spriteBatch.rasterizerState,
            spriteBatch.customEffect,
            spriteBatch.transformMatrix);
        spriteBatch.End();
    }

    public static void Begin(this SpriteBatch spriteBatch, SpriteBatchSnapshot snapshot)
    {
        spriteBatch.Begin(
            snapshot.SortMode,
            snapshot.BlendState,
            snapshot.SamplerState,
            snapshot.DepthStencilState,
            snapshot.Rasterizer,
            snapshot.Effect,
            snapshot.TransformMatrix);
    }
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace BlockVanity.Core;

public static class PrimitiveRenderer
{
    public struct VertexRenderData : IVertexType
    {
        public Vector2 Position;
        public Color Color;
        public Vector3 TextureCoordinate;

        public VertexDeclaration VertexDeclaration => Declaration;

        private static readonly VertexDeclaration Declaration = new VertexDeclaration(
            new VertexElement(0, VertexElementFormat.Vector2, VertexElementUsage.Position, 0),
            new VertexElement(8, VertexElementFormat.Color, VertexElementUsage.Color, 0),
            new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.TextureCoordinate, 0)
            );
    }

    private static VertexRenderData[] _vertices;
    private static short[] _indices;

    public delegate Color StripColorFunction(float progressAlongStrip);

    public delegate float StripWidthFunction(float progressAlongStrip);

    public static void DrawStrip(Vector2[] positions, float[] rotations, StripColorFunction color, StripWidthFunction width, Vector2 offset = default)
    {
        if (positions.Length < 2 | positions.Length != rotations.Length)
            return;

        int length = positions.Length;
        _vertices = new VertexRenderData[length * 2];
        _indices = new short[length * 6];

        for (int i = 0; i < length; i++)
            SetVertexPair(positions[i] + offset, rotations[i], color, width, i * 2);

        SetIndices();

        Main.instance.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
        Main.instance.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, _vertices, 0, _vertices.Length, _indices, 0, _indices.Length / 3);
    }

    private static void SetVertexPair(Vector2 position, float rotation, StripColorFunction colorFunction, StripWidthFunction widthFunction, int i)
    {
        float progress = (float)i / (_vertices.Length - 1);
        float width = widthFunction(progress);
        Color color = colorFunction(progress);
        Vector2 unitRotation = MathHelper.WrapAngle(rotation + MathHelper.PiOver2).ToRotationVector2();
        Vector2 offset = unitRotation * width;

        _vertices[i].Position = position - offset;
        _vertices[i].TextureCoordinate = new Vector3(progress, 0.5f - 0.5f * width, width);
        _vertices[i].Color = color;
        _vertices[i + 1].Position = position + offset;
        _vertices[i + 1].TextureCoordinate = new Vector3(progress, 0.5f + 0.5f * width, width);
        _vertices[i + 1].Color = color;
    }

    private static void SetIndices()
    {
        short iI = 0;
        for (short i = 0; i < _vertices.Length / 2 - 1; i++)
        {
            int nextIndex = i * 2;
            _indices[iI++] = (short)(nextIndex);
            _indices[iI++] = (short)(nextIndex + 1);
            _indices[iI++] = (short)(nextIndex + 2);
            _indices[iI++] = (short)(nextIndex + 2);
            _indices[iI++] = (short)(nextIndex + 3);
            _indices[iI++] = (short)(nextIndex + 1);
        }
    }
}

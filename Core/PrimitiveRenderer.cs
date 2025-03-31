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

public class PrimitiveRenderer : ILoadable
{
    private static DynamicVertexBuffer _vertexBuffer;
    private static DynamicIndexBuffer _indexBuffer;

    private static VertexPositionColorTexture[] _vertices;
    private static short[] _indices;

    public void Load(Mod mod)
    {
        if (!Main.dedServ)
        {
            const short vertexCount = 16384; // short.MaxValue / 5;
            const short indexCount = 6144; // short.MaxValue / 2;
            Main.QueueMainThreadAction(() =>
            {
                _vertexBuffer = new DynamicVertexBuffer(Main.instance.GraphicsDevice, typeof(VertexPositionColorTexture), vertexCount, BufferUsage.WriteOnly);
                _indexBuffer = new DynamicIndexBuffer(Main.instance.GraphicsDevice, IndexElementSize.SixteenBits, indexCount, BufferUsage.WriteOnly);
            });
        }
    }

    public void Unload()
    {
        if (!Main.dedServ)
        {
            Main.QueueMainThreadAction(() =>
            {
                _vertexBuffer.Dispose();
                _indexBuffer.Dispose();

                _vertexBuffer = null;
                _indexBuffer = null;

                _vertices = null;
                _indices = null;
            });
        }
    }

    public delegate Color StripColorFunction(float progressAlongStrip);

    public delegate float StripWidthFunction(float progressAlongStrip);

    public static void DrawStrip(Vector2[] positions, float[] rotations, StripColorFunction color, StripWidthFunction width, Vector2 offset = default)
    {
        if (positions.Length < 2 | positions.Length != rotations.Length)
            return;

        int length = positions.Length;
        _vertices = new VertexPositionColorTexture[length * 2];
        _indices = new short[length * 6];

        for (int i = 0; i < length; i++)
            SetVertexPair(positions[i] + offset, rotations[i], color, width, i * 2);

        SetIndices();
        _vertexBuffer.SetData(_vertices, 0, _vertices.Length, SetDataOptions.Discard);
        _indexBuffer.SetData(_indices, 0, _indices.Length, SetDataOptions.Discard);

        Main.instance.GraphicsDevice.SetVertexBuffer(_vertexBuffer);
        Main.instance.GraphicsDevice.Indices = _indexBuffer;
        Main.instance.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, _vertices.Length - 1, 0, _indices.Length / 3);
    }

    private static void SetVertexPair(Vector2 position, float rotation, StripColorFunction colorFunction, StripWidthFunction widthFunction, int i)
    {
        float progress = (float)i / (_vertices.Length - 1);
        Vector2 offset = MathHelper.WrapAngle(rotation - MathHelper.PiOver2).ToRotationVector2() * widthFunction(progress);
        Color color = colorFunction(progress);

        _vertices[i].Position = new Vector3(position + offset, 0);
        _vertices[i].TextureCoordinate = new Vector2(progress, 1f);
        _vertices[i].Color = color;
        _vertices[i + 1].Position = new Vector3(position - offset, 0);
        _vertices[i + 1].TextureCoordinate = new Vector2(progress, 0f);
        _vertices[i + 1].Color = color;
    }

    private static void SetIndices()
    {
        for (short i = 0; i < _vertices.Length / 2 - 1; i++)
        {
            short j = (short)(i * 6);
            int nextIndex = i * 2;
            _indices[j] = (short)nextIndex;
            _indices[j + 1] = (short)(nextIndex + 1);
            _indices[j + 2] = (short)(nextIndex + 2);
            _indices[j + 3] = (short)(nextIndex + 2);
            _indices[j + 4] = (short)(nextIndex + 1);
            _indices[j + 5] = (short)(nextIndex + 3);
        }
    }
}

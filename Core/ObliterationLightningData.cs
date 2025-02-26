using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace BlockVanity.Core;

public struct ObliterationLightningData
{
    public ObliterationLightningData(int pointCount)
    {
        Length = pointCount;
        points = new Vector2[Length];
        offsets = new Vector2[Length];
        offsetTargets = new Vector2[Length];
        controls = new Vector2[3];
    }

    public int Length { get; private set; }
    public float Width { get; private set; }
    public bool End { get; private set; }

    private int time;

    public void SetVariables(int length, float width, bool end)
    {
        Length = length;
        Width = width;
        End = end;
        points = new Vector2[Length];

        Vector2[] oldOffsets = offsets;
        offsets = new Vector2[Length];
        for (int i = 0; i < Math.Min(oldOffsets.Length, Length); i++)
            offsets[i] = oldOffsets[i];


        Vector2[] oldTargets = offsetTargets;
        offsetTargets = new Vector2[Length];
        for (int i = 0; i < Math.Min(oldTargets.Length, Length); i++)
            offsetTargets[i] = oldTargets[i];
    }

    public void SetEnds(Vector2 start, Vector2 middle, Vector2 end)
    {
        controls = [start, middle, end];
    }

    public void Update()
    {
        if (--time <= 0)
        {
            time = Main.rand.Next(3, 6);
            for (int i = 0; i < Length; i++)
                offsetTargets[i] = Main.rand.NextVector2Circular(1, 4);
        }

        float rotation = controls[0].AngleTo(controls[2]);
        for (int i = Length - 1; i > 1; i--)
        {
            offsets[i] += offsetTargets[i];
            offsets[i] *= 0.6f;
            offsetTargets[i] *= 0.7f;
            offsets[i] = Vector2.Lerp(offsets[i], offsets[i - 1], Main.rand.NextFloat(0.4f, 0.7f));
        }

        offsets[0] = Vector2.Lerp(offsets[0], offsetTargets[0], 0.2f);
        offsets[^1] = Vector2.Lerp(offsets[^1], offsetTargets[^1], 0.2f);

        for (int i = 0; i < Length; i++)
        {
            if (i < Length - 1)
                offsets[i] = Vector2.Lerp(offsets[i], offsets[i + 1], Main.rand.NextFloat(0.8f));

            float progress = (float)i / (Length - 1);
            float offsetScalar = progress * Width * (End ? Utils.GetLerpValue(1, 5, Length - i, true) : 1f);
            Vector2 mainLine = Vector2.Lerp(Vector2.Lerp(controls[0], controls[1], progress), Vector2.Lerp(controls[1], controls[2], progress), progress);
            points[i] = mainLine + offsets[i].RotatedBy(rotation) * offsetScalar;
        }
    }

    public Vector2[] GetPoints() => points;

    private Vector2[] controls;
    private Vector2[] points;
    private Vector2[] offsets;
    private Vector2[] offsetTargets;
}

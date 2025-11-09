using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Utilities;

namespace BlockVanity.Core;

public struct ObliterationLightningData
{
    public ObliterationLightningData(int pointCount, int seed)
    {
        Length = pointCount;

        this.seed = seed;
        points = new Vector2[Length];
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
    }

    public void SetEnds(Vector2 start, Vector2 middle, Vector2 end)
    {
        controls = [start, middle, end];
    }

    public void Update()
    {
        time++;
        FastRandom random = new FastRandom(seed);
        for (int i = 0; i < Length; i++)
        {
            float progress = (float)i / (Length - 1);
            float offsetScalar = MathF.Sqrt(progress) * Width * (End ? Utils.GetLerpValue(1, 5, Length - i, true) : 1f);
            Vector2 mainLine = Vector2.Lerp(Vector2.Lerp(controls[0], controls[1], progress), Vector2.Lerp(controls[1], controls[2], progress), progress);
            float mainAngle = Utils.AngleLerp(controls[0].AngleTo(controls[1]), controls[1].AngleTo(controls[2]), progress);
            
            float curve = MathF.Sin(time * 0.1f - (float)i / Length * 9f);
            for (float j = 0; j < 3; j++)
                curve += MathF.Sin(time * (random.NextFloat() * 0.5f * j + 0.1f) - (float)i / Length * (random.NextFloat() - 0.5f) * 15f) / (j + 1f);

            points[i] = mainLine + new Vector2(0, curve * offsetScalar).RotatedBy(mainAngle);
        }
    }

    public Vector2[] Points => points;

    private int seed;
    private Vector2[] controls;
    private Vector2[] points;
}

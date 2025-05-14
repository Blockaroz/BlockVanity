using Microsoft.Xna.Framework;
using System;
using Terraria;
using static BlockVanity.Core.Rope;

namespace BlockVanity.Core;

public class Rope
{
    public struct RopeSegment
    {
        public RopeSegment(Vector2 position)
        {
            this.position = position;
            this.oldPosition = position;
        }

        public Vector2 position;
        public Vector2 oldPosition;
        public bool pinned;
    }

    public Rope(Vector2 startPos, Vector2 endPos, int segmentCount, float segmentLength, Vector2 gravity, int accuracy = 10)
    {
        segments = new RopeSegment[segmentCount];
        for (int i = 0; i < segmentCount; i++)
        {
            Vector2 segmentPos = Vector2.Lerp(startPos, endPos, i / (segmentCount - 1f));
            segments[i] = new RopeSegment(segmentPos);
        }
        segments[0].pinned = true;
        segments[^1].pinned = true;

        this.segmentLength = segmentLength;
        this.gravity = gravity;
        this.accuracy = accuracy;
    }

    public RopeSegment[] segments;
    public float segmentLength;
    public Vector2 gravity;

    public bool tileCollide;
    public Vector2 colliderOrigin;
    public int colliderWidth;
    public int colliderHeight;
    public float damping;

    private int accuracy;

    public static void Settle(Rope rope)
    {
        float oldDamp = rope.damping;
        rope.damping = 0.66667f;
        for (int a = 0; a < rope.segments.Length; a++)
            Update(rope);
        rope.damping = oldDamp;
    }

    public static void Update(Rope rope)
    {
        for (int i = 0; i < rope.segments.Length; i++)
        {
            Vector2 velocity = (rope.segments[i].position - rope.segments[i].oldPosition) * (1f - rope.damping);
            if (velocity.Length() < 0.015f)
                velocity = Vector2.Zero;

            rope.segments[i].oldPosition = rope.segments[i].position;

            if (!rope.segments[i].pinned)
                rope.segments[i].position += TileCollision(rope, rope.segments[i].position, velocity + rope.gravity);
        }

        // Constrain
        for (int a = 0; a < rope.accuracy; a++)
        {
            for (int i = 0; i < rope.segments.Length - 1; i++)
            {
                float dist = rope.segments[i].position.Distance(rope.segments[i + 1].position);
                float error = dist - rope.segmentLength;
                Vector2 correction = rope.segments[i].position.DirectionFrom(rope.segments[i + 1].position) * error;

                bool pinned = rope.segments[i].pinned;
                bool nextPinned = rope.segments[i + 1].pinned;
                float multiplier = pinned || nextPinned ? 1f : 0.5f;

                if (!pinned)
                    rope.segments[i].position -= TileCollision(rope, rope.segments[i].position, correction * multiplier);
                if (!nextPinned)
                    rope.segments[i + 1].position += TileCollision(rope, rope.segments[i + 1].position, correction * multiplier);
            }
        }
    }

    private static Vector2 TileCollision(Rope rope, Vector2 position, Vector2 velocity)
    {
        if (!rope.tileCollide)
            return velocity;

        Vector2 newVelocity = Collision.noSlopeCollision(position + rope.colliderOrigin, velocity, rope.colliderWidth, rope.colliderHeight + 2, true, true);
        newVelocity = Collision.noSlopeCollision(position + rope.colliderOrigin, newVelocity, rope.colliderWidth, rope.colliderHeight, true, true);
        Vector2 result = velocity;
        if (Math.Abs(velocity.X) > Math.Abs(newVelocity.X))
            result.X = 0;
        if (Math.Abs(velocity.Y) > Math.Abs(newVelocity.Y))
            result.Y = 0;

        return result;
    }

    public Rectangle GetCollisionRect(int i) => new Rectangle((int)(segments[i].position.Floor().X + colliderOrigin.X), (int) (segments[i].position.Floor().Y + colliderOrigin.Y), colliderWidth, colliderHeight);
}

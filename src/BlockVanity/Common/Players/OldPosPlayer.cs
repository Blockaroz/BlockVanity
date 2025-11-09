using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace BlockVanity.Common.Players;

public class OldPosPlayer : ModPlayer
{
    public Vector2[] oldPos;
    public Vector2[] oldVel;
    public float[] oldRot;
    public float pad;

    public override void OnEnterWorld()
    {
        if (oldPos == null)
        {
            oldPos = Enumerable.Repeat(Player.MountedCenter, 32).ToArray();
        }

        if (oldVel == null)
        {
            oldVel = Enumerable.Repeat(Player.velocity, 32).ToArray();
        }

        if (oldRot == null)
        {
            oldRot = new float[32];
        }
    }

    public override void PostUpdateRunSpeeds()
    {
        if (oldPos == null)
        {
            oldPos = Enumerable.Repeat(Player.MountedCenter, 32).ToArray();
        }

        if (oldVel == null)
        {
            oldVel = Enumerable.Repeat(Player.velocity, 32).ToArray();
        }

        if (oldRot == null)
        {
            oldRot = new float[32];
        }

        for (int i = 31; i > 0; i--)
        {
            oldPos[i] = Vector2.Lerp(oldPos[i - 1], oldPos[i], Utils.GetLerpValue(0, 40, Player.velocity.Length() * 0.5f, true) * 0.7f + 0.1f);
            oldVel[i] = oldVel[i - 1];
            oldRot[i] = oldPos[i].AngleTo(oldPos[i - 1]);
        }

        oldPos[0] = Player.MountedCenter;
        oldVel[0] = Player.velocity;
        oldRot[0] = Player.velocity.ToRotation();

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlockVanity.Common.Systems.Players;
using BlockVanity.Content.Items.Vanity.Excellence;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace BlockVanity.Common.Players;

public class SonicTheHedgehogPlayer : ModPlayer
{
    public bool enabled;
    public int cSonicShader;
    public bool UseSpinDash => Player.velocity.Y < 0;
    public bool IsRunning => Math.Abs(Player.velocity.X) > ((Player.maxRunSpeed + Player.accRunSpeed) / 1.75f) && Player.wingTime >= Player.wingTimeMax;

    public override void Load()
    {
        On_Player.UpdateVisibleAccessory += EnableSonic;
        On_Player.PlayerFrame += SpecialRunAnimation;
        On_Player.SetArmorEffectVisuals += ArmorSetShadows;

        On_PlayerDrawLayers.DrawPlayer_21_Head += HideHead;
        On_PlayerDrawLayers.DrawPlayer_13_Leggings += HideLegs;
    }

    private void ArmorSetShadows(On_Player.orig_SetArmorEffectVisuals orig, Player self, Player drawPlayer)
    {
        orig(self, drawPlayer);

        if (self.GetModPlayer<SonicTheHedgehogPlayer>().enabled)
            self.armorEffectDrawShadow = true;
    }

    private void HideHead(On_PlayerDrawLayers.orig_DrawPlayer_21_Head orig, ref PlayerDrawSet drawinfo)
    {
        if (drawinfo.drawPlayer.head != EquipLoader.GetEquipSlot(Mod, nameof(PhantomRuby), EquipType.Head))
            orig(ref drawinfo);
    }

    private void HideLegs(On_PlayerDrawLayers.orig_DrawPlayer_13_Leggings orig, ref PlayerDrawSet drawinfo)
    {
        if (drawinfo.drawPlayer.legs != EquipLoader.GetEquipSlot(Mod, nameof(PhantomRuby), EquipType.Legs))
            orig(ref drawinfo);
    }

    public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
    {
        if (enabled)
        {
            drawInfo.cHead = cSonicShader;
            drawInfo.cBody = cSonicShader;
            drawInfo.cLegs = cSonicShader;
        }
    }

    public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
    {
        if (enabled && drawInfo.shadow > 0f)
        {
            a = 1f - drawInfo.shadow;

            if (cSonicShader <= 0)
            {
                r = 0.3f;
                g = 0.5f;
            }
        }
    }

    private void EnableSonic(On_Player.orig_UpdateVisibleAccessory orig, Player self, int itemSlot, Item item, bool modded)
    {
        orig(self, itemSlot, item, modded);

        if (item.ModItem is PhantomRuby sonicItem)
        {
            self.GetModPlayer<SonicTheHedgehogPlayer>().cSonicShader = self.dye[itemSlot % 10].dye;
            self.head = EquipLoader.GetEquipSlot(Mod, sonicItem.Name, EquipType.Head);
            self.body = EquipLoader.GetEquipSlot(Mod, sonicItem.Name, EquipType.Body);
            self.legs = EquipLoader.GetEquipSlot(Mod, sonicItem.Name, EquipType.Legs);
        }
    }

    internal float walkCounter;
    internal int walkFrame;
    internal float runCounter;
    internal int runFrame;

    private void SpecialRunAnimation(On_Player.orig_PlayerFrame orig, Player self)
    {
        orig(self);

        SonicTheHedgehogPlayer sonicPlayer = self.GetModPlayer<SonicTheHedgehogPlayer>();

        if (sonicPlayer.enabled)
        {
            if (self.velocity.Y == 0)
            {
                if (!Main.gameInactive)
                {
                    if (sonicPlayer.IsRunning)
                    {
                        self.bodyFrame.Y = self.bodyFrame.Height * 13;

                        sonicPlayer.runCounter += Math.Abs(self.velocity.X * 0.5f);

                        while (sonicPlayer.runCounter > 8)
                        {
                            sonicPlayer.runCounter -= 8;
                            sonicPlayer.runFrame = (++sonicPlayer.runFrame) % 7;
                        }
                    }

                    sonicPlayer.walkCounter += Math.Abs(self.velocity.X * 1.2f);
                }

                while (sonicPlayer.walkCounter > 8)
                {
                    sonicPlayer.walkCounter -= 8;
                    sonicPlayer.walkFrame += self.legFrame.Height;
                }

                if (sonicPlayer.walkFrame < self.legFrame.Height * 7)
                    sonicPlayer.walkFrame = self.legFrame.Height * 19;
                else if (sonicPlayer.walkFrame > self.legFrame.Height * 19)
                    sonicPlayer.walkFrame = self.legFrame.Height * 7;

                if (self.velocity.X != 0)
                {
                    self.bodyFrameCounter = 0.0;
                    self.legFrameCounter = 0.0;
                    self.legFrame.Y = sonicPlayer.walkFrame;
                }
                else
                {
                    sonicPlayer.walkCounter = 0;
                    sonicPlayer.walkFrame = 0;
                    sonicPlayer.runCounter = 0;
                    sonicPlayer.runFrame = 0;
                }
            }
        }
    }

    public override void PostUpdateRunSpeeds()
    {
        if (enabled && IsRunning)
        {
            int x = (int)Math.Round(Player.Center.X / 16f);
            int y = (int)Math.Round((Player.Center.Y + Player.gravDir * 16) / 16f);
            if (Main.rand.NextBool())
            {
                Dust cloud = Dust.NewDustDirect(new Vector2(x * 16 - Player.direction * 8, Player.Center.Y + Player.gravDir * 16), 0, 0, DustID.Cloud, Player.velocity.X * Main.rand.NextFloat(0.5f), -Main.rand.NextFloat(1f, 2f) * Player.gravDir, 0, Color.White, 1f);
                cloud.noGravity = true;
                cloud.velocity *= 0.5f;
            }

            if (WorldGen.InWorld(x, y))
            {
                if (Main.tile[x, y].HasTile)
                {
                    int index = WorldGen.KillTile_MakeTileDust(x, y, Main.tile[x, y]);
                    if (index < 6000)
                    {
                        Dust dust = Main.dust[index];
                        dust.fadeIn = 1f;
                        dust.noGravity = Main.rand.NextBool();
                        dust.scale *= Main.rand.NextFloat(0.5f, 1.5f) + (dust.noGravity ? 0.5f : 0);
                        dust.position.X -= Player.direction * 8;
                        dust.position.Y -= 8f * Player.gravDir;
                        dust.velocity.Y = -Main.rand.NextFloat(1f, 2f) * Player.gravDir;
                        dust.velocity.X = Player.velocity.X * Main.rand.NextFloat(0.5f);
                    }
                }
            }

        }
    }

    public override void ResetEffects()
    {
        enabled = false;
        cSonicShader = 0;
    }
}

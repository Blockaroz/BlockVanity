﻿using System.Collections.Generic;
using BlockVanity.Common.Utilities;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.ModLoader;

namespace BlockVanity;

public static class AllAssets
{
    public static Asset<T>[] RequestArray<T>(string name, int count, AssetRequestMode mode = AssetRequestMode.AsyncLoad) where T : class
    {
        Asset<T>[] assets = new Asset<T>[count];
        for (int i = 0; i < count; i++)
        {
            assets[i] = ModContent.Request<T>(name + i, mode);
        }

        return assets;
    }

    public static Asset<T>[] RequestArrayAuto<T>(string name, AssetRequestMode mode = AssetRequestMode.AsyncLoad) where T : class
    {
        List<Asset<T>> assets = new List<Asset<T>>();

        int i = 0;
        while (ModContent.RequestIfExists(name + i, out Asset<T> asset, mode))
        {
            assets.Add(asset);
            i++;
        }

        return assets.ToArray();
    }

    public static SlowAsset<T>[] RequestSlowArray<T>(string name, int count) where T : class
    {
        SlowAsset<T>[] assets = new SlowAsset<T>[count];
        for (int i = 0; i < count; i++)
        {
            assets[i] = new SlowAsset<T>(name + i);
        }

        return assets;
    }

    public static SlowAsset<T>[] RequestSlowArrayAuto<T>(string name) where T : class
    {
        List<SlowAsset<T>> assets = new List<SlowAsset<T>>();

        int i = 0;
        while (ModContent.HasAsset(name + i))
        {
            assets.Add(new SlowAsset<T>(name + i));
            i++;
        }

        return assets.ToArray();
    }

    public static void Load()
    {
        Textures.Glow = RequestSlowArrayAuto<Texture2D>($"{nameof(BlockVanity)}/Assets/Textures/Extras/Glow_");

        Textures.FishEyes = new SlowAsset<Texture2D>($"{nameof(BlockVanity)}/Assets/Textures/Extras/FishSkin/FishEyes");
        Textures.OrionNoise = new SlowAsset<Texture2D>($"{nameof(BlockVanity)}/Assets/Textures/Extras/OrionNoise");
        Textures.SeasideColorMap = new SlowAsset<Texture2D>($"{nameof(BlockVanity)}/Assets/Textures/Extras/SeasideColorMap");

        Textures.BlueFishSkin = [
            ModContent.Request<Texture2D>($"{nameof(BlockVanity)}/Assets/Textures/Extras/FishSkin/BlueFishSkin_Head", AssetRequestMode.ImmediateLoad),
            ModContent.Request<Texture2D>($"{nameof(BlockVanity)}/Assets/Textures/Extras/FishSkin/BlueFishSkin_Ears_High", AssetRequestMode.ImmediateLoad),
            ModContent.Request<Texture2D>($"{nameof(BlockVanity)}/Assets/Textures/Extras/FishSkin/BlueFishSkin_Ears_Low", AssetRequestMode.ImmediateLoad),
            ModContent.Request<Texture2D>($"{nameof(BlockVanity)}/Assets/Textures/Extras/FishSkin/BlueFishSkin_Eyes", AssetRequestMode.ImmediateLoad),
            ModContent.Request<Texture2D>($"{nameof(BlockVanity)}/Assets/Textures/Extras/FishSkin/BlueFishSkin_Body", AssetRequestMode.ImmediateLoad),
            ModContent.Request<Texture2D>($"{nameof(BlockVanity)}/Assets/Textures/Extras/FishSkin/BlueFishSkin_Arms", AssetRequestMode.ImmediateLoad),
            ModContent.Request<Texture2D>($"{nameof(BlockVanity)}/Assets/Textures/Extras/FishSkin/BlueFishSkin_Hands", AssetRequestMode.ImmediateLoad),
            ModContent.Request<Texture2D>($"{nameof(BlockVanity)}/Assets/Textures/Extras/FishSkin/BlueFishSkin_Hands_Back", AssetRequestMode.ImmediateLoad),
            ModContent.Request<Texture2D>($"{nameof(BlockVanity)}/Assets/Textures/Extras/FishSkin/BlueFishSkin_Legs", AssetRequestMode.ImmediateLoad),
            ModContent.Request<Texture2D>($"{nameof(BlockVanity)}/Assets/Textures/Extras/FishSkin/BlueFishSkin_Tail", AssetRequestMode.ImmediateLoad),
            ];

        Effects.BasicTrail = ModContent.Request<Effect>($"{nameof(BlockVanity)}/Assets/Effects/BasicTrail", AssetRequestMode.ImmediateLoad);
        Effects.OrionShader = ModContent.Request<Effect>($"{nameof(BlockVanity)}/Assets/Effects/OrionShader", AssetRequestMode.ImmediateLoad);
        Effects.SeasideHairShader = ModContent.Request<Effect>($"{nameof(BlockVanity)}/Assets/Effects/SeasideHairShader", AssetRequestMode.ImmediateLoad);

        //Sounds.FishyHit = new SoundStyle($"{nameof(BlockVanity)}/Assets/Sounds/HitSounds/FishyHit_", 0, 3);
    }

    public static class Textures
    {
        public static SlowAsset<Texture2D>[] Glow;
        public static SlowAsset<Texture2D> FishEyes;
        public static SlowAsset<Texture2D> OrionNoise;
        public static SlowAsset<Texture2D> SeasideColorMap;

        public static Asset<Texture2D>[] BlueFishSkin;
    }

    public static class Sounds
    {
        public static SoundStyle FishyHit;
    }

    public static class Effects
    {
        public static Asset<Effect> BasicTrail;
        public static Asset<Effect> OrionShader;
        public static Asset<Effect> SeasideHairShader;
    }
}
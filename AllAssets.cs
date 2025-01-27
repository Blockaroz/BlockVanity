using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria.Audio;
using Terraria.ModLoader;

namespace BlockVanity;

[Autoload]
public static class AllAssets
{
    public static Asset<T>[] RequestArray<T>(string name, int count, AssetRequestMode mode = AssetRequestMode.AsyncLoad) where T : class
    {
        Asset<T>[] assets = new Asset<T>[count];
        for (int i = 0; i < count; i++)
            assets[i] = ModContent.Request<T>(name + i, mode);

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

    public static void Load(Mod mod)
    {
        string assetsPath = $"{nameof(BlockVanity)}/Assets/";
        Textures.Pixel = ModContent.Request<Texture2D>(assetsPath + "Textures/Extras/Pixel");
        Textures.Glow = RequestArrayAuto<Texture2D>(assetsPath + "Textures/Extras/Glow_");

        Textures.MiscNoise = RequestArrayAuto<Texture2D>(assetsPath + "Textures/Extras/Noise_");
        Textures.SeasideColorMap = ModContent.Request<Texture2D>(assetsPath + "Textures/Extras/SeasideColorMap");

        Textures.Particle = RequestArrayAuto<Texture2D>(assetsPath + "Textures/Particles/Particle_");

        Textures.Bar = RequestArrayAuto<Texture2D>(assetsPath + "Textures/Extras/Bar_");
        Textures.VanityStar = ModContent.Request<Texture2D>(assetsPath + "Textures/UI/VanityStar");

        Effects.BasicTrail = ModContent.Request<Effect>(assetsPath + "Effects/BasicTrail", AssetRequestMode.ImmediateLoad);
        Effects.FrenziedFlameParticle = ModContent.Request<Effect>(assetsPath + "Effects/FrenziedFlameParticle", AssetRequestMode.ImmediateLoad);
        Effects.FrenziedFlameEye = ModContent.Request<Effect>(assetsPath + "Effects/FrenziedFlameEye", AssetRequestMode.ImmediateLoad);
        Effects.TransparencyMask = ModContent.Request<Effect>(assetsPath + "Effects/TransparencyMask", AssetRequestMode.ImmediateLoad);

        Effects.RadiationDye = ModContent.Request<Effect>(assetsPath + "Effects/Dyes/RadiationDye");
        Effects.PhantomDye = ModContent.Request<Effect>(assetsPath + "Effects/Dyes/PhantomDye");
        Effects.ChaosMatterDye = ModContent.Request<Effect>(assetsPath + "Effects/Dyes/ChaosMatterDye");
        Effects.OrionDye = ModContent.Request<Effect>(assetsPath + "Effects/Dyes/OrionDye");

        Effects.SeasideHairDye = ModContent.Request<Effect>(assetsPath + "Effects/Dyes/SeasideHairDye");

        Sounds.FishyHit = new SoundStyle(assetsPath + "Sounds/HitSounds/DemonSkin_Hurt", 1, 3) { PitchVariance = 0.4f, Volume = 0.7f };
        Sounds.DemonHit = new SoundStyle(assetsPath + "Sounds/HitSounds/DemonSkin_Hurt", 1, 3) { PitchVariance = 0.4f, Volume = 0.7f };
    }

    public static class Textures
    {
        public static readonly string Placeholder = $"{nameof(BlockVanity)}/Assets/Textures/Placeholder_Pearl";

        public static Asset<Texture2D>[] Glow;
        public static Asset<Texture2D> Pixel;

        public static Asset<Texture2D>[] MiscNoise;
        public static Asset<Texture2D> SeasideColorMap;

        public static Asset<Texture2D>[] Particle;

        public static Asset<Texture2D>[] Bar;

        public static Asset<Texture2D> VanityStar;
    }

    public static class Sounds
    {
        public static SoundStyle FishyHit;

        public static SoundStyle DemonHit;
    }

    public static class Effects
    {
        public static Asset<Effect> BasicTrail;
        public static Asset<Effect> FrenziedFlameParticle;
        public static Asset<Effect> FrenziedFlameEye;
        public static Asset<Effect> TransparencyMask;

        public static Asset<Effect> RadiationDye;
        public static Asset<Effect> PhantomDye;
        public static Asset<Effect> ChaosMatterDye;
        public static Asset<Effect> OrionDye;

        public static Asset<Effect> SeasideHairDye;
        public static Asset<Effect> SparklingAshHairDye;

    }
}
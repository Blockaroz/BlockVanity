using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria.Audio;
using Terraria.ModLoader;

namespace BlockVanity;

public readonly record struct LazyAsset<T>(string Path) where T : class
{
    public Asset<T> Asset => lazy.Value;

    public T Value => Asset.Value;

    private readonly Lazy<Asset<T>> lazy = new Lazy<Asset<T>>(() => ModContent.Request<T>(Path, AssetRequestMode.ImmediateLoad));

    public static implicit operator Asset<T>(LazyAsset<T> asset) => asset.Asset;
}

public static partial class AllAssets
{
    public static LazyAsset<T>[] LazyArray<T>(string name, int count) where T : class
    {
        LazyAsset<T>[] assets = new LazyAsset<T>[count];
        for (int i = 0; i < count; i++)
            assets[i] = new LazyAsset<T>(name + i);

        return assets;
    }

    public static LazyAsset<T>[] LazyArrayAuto<T>(string name) where T : class
    {
        List<LazyAsset<T>> assets = new List<LazyAsset<T>>();

        int i = 0;
        while (ModContent.HasAsset(name + i))
        {
            assets.Add(new LazyAsset<T>(name + i));
            i++;
        }

        return assets.ToArray();
    }

    public static readonly string AssetPath = $"{nameof(BlockVanity)}/Assets/";

    public static class Textures
    {
        public static readonly string Placeholder = $"{nameof(BlockVanity)}/Assets/Textures/Placeholder_Pearl";

        public static LazyAsset<Texture2D>[] Glow = LazyArrayAuto<Texture2D>(AssetPath + "Textures/Extras/Glow_");
        public static LazyAsset<Texture2D> Pixel = new LazyAsset<Texture2D>(AssetPath + "Textures/Extras/Pixel");

        public static LazyAsset<Texture2D>[] MiscNoise = LazyArrayAuto<Texture2D>(AssetPath + "Textures/Extras/Noise_");
        public static LazyAsset<Texture2D> MagmaPattern = new LazyAsset<Texture2D>(AssetPath + "Textures/Extras/MagmaPattern");
        public static LazyAsset<Texture2D> SeasideColorMap = new LazyAsset<Texture2D>(AssetPath + "Textures/Extras/SeasideColorMap");

        public static LazyAsset<Texture2D>[] Particle = LazyArrayAuto<Texture2D>(AssetPath + "Textures/Particles/Particle_");

        public static LazyAsset<Texture2D>[] Bar = LazyArrayAuto<Texture2D>(AssetPath + "Textures/Extras/Bar_");

        public static LazyAsset<Texture2D> VanityStar = new LazyAsset<Texture2D>(AssetPath + "Textures/UI/VanityStar");
    }

    public static class Sounds
    {
    }

    public static class Effects
    {
        public static LazyAsset<Effect> BasicTrail = new LazyAsset<Effect>(AssetPath + "Effects/BasicTrail");
        public static LazyAsset<Effect> ObliterationRayBeam = new LazyAsset<Effect>(AssetPath + "Effects/ObliterationRayBeam");
        public static LazyAsset<Effect> FrenziedFlameParticle = new LazyAsset<Effect>(AssetPath + "Effects/FrenziedFlameParticle");
        public static LazyAsset<Effect> FrenziedFlameEye = new LazyAsset<Effect>(AssetPath + "Effects/FrenziedFlameEye");
        public static LazyAsset<Effect> TransparencyMask = new LazyAsset<Effect>(AssetPath + "Effects/TransparencyMask");

        public static LazyAsset<Effect> RadiationDye = new LazyAsset<Effect>(AssetPath + "Effects/Dyes/RadiationDye");
        public static LazyAsset<Effect> PhantomDye = new LazyAsset<Effect>(AssetPath + "Effects/Dyes/PhantomDye");
        public static LazyAsset<Effect> ChaosMatterDye = new LazyAsset<Effect>(AssetPath + "Effects/Dyes/ChaosMatterDye");
        public static LazyAsset<Effect> WitheringDye = new LazyAsset<Effect>(AssetPath + "Effects/Dyes/WitheringDye");

        public static LazyAsset<Effect> SeasideHairDye = new LazyAsset<Effect>(AssetPath + "Effects/Dyes/SeasideHairDye");
        public static LazyAsset<Effect> SparklingAshHairDye;

    }
}
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

public static partial class Assets
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

        public static LazyAsset<Texture2D>[] Glow { get; } = LazyArrayAuto<Texture2D>(AssetPath + "Textures/Extras/Glow_");
        public static LazyAsset<Texture2D> Pixel { get; } = new LazyAsset<Texture2D>(AssetPath + "Textures/Extras/Pixel");

        public static LazyAsset<Texture2D>[] MiscNoise { get; } = LazyArrayAuto<Texture2D>(AssetPath + "Textures/Extras/Noise_");
        public static LazyAsset<Texture2D> MagmaPattern { get; } = new LazyAsset<Texture2D>(AssetPath + "Textures/Extras/MagmaPattern");

        public static LazyAsset<Texture2D>[] Particle { get; } = LazyArrayAuto<Texture2D>(AssetPath + "Textures/Particles/Particle_");

        public static LazyAsset<Texture2D> VanityStar { get; } = new LazyAsset<Texture2D>(AssetPath + "Textures/UI/VanityStar");
    }

    public static class Sounds
    {
    }

    public static class Effects
    {
        public static LazyAsset<Effect> BasicTrail { get; } = new LazyAsset<Effect>(AssetPath + "Effects/BasicTrail");
        public static LazyAsset<Effect> ObliterationRayBeam { get; } = new LazyAsset<Effect>(AssetPath + "Effects/ObliterationRayBeam");
        public static LazyAsset<Effect> TransparencyMask { get; } = new LazyAsset<Effect>(AssetPath + "Effects/TransparencyMask");
    }
}
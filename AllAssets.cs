using System.Collections.Generic;
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

    public static void Load()
    {
        string assetpath = $"{nameof(BlockVanity)}/Assets/";
        Textures.Glow = RequestArrayAuto<Texture2D>(assetpath + "Textures/Extras/Glow_");

        Textures.FishEyes = ModContent.Request<Texture2D>(assetpath + "Textures/Extras/FishSkin/FishEyes");
        Textures.FireDissolveNoise = RequestArrayAuto<Texture2D>(assetpath + "Textures/Extras/FireDissolveNoise_");
        Textures.OrionNoise = ModContent.Request<Texture2D>(assetpath + "Textures/Extras/OrionNoise");
        Textures.SeasideColorMap = ModContent.Request<Texture2D>(assetpath + "Textures/Extras/SeasideColorMap");

        Textures.Particle = RequestArrayAuto<Texture2D>(assetpath + "Textures/Particles/Particle_");
        Textures.Bar = RequestArrayAuto<Texture2D>(assetpath + "Textures/Extras/Bar_");

        Textures.BlueFishSkin = [
            ModContent.Request<Texture2D>(assetpath + "Textures/Extras/FishSkin/BlueFishSkin_Head"),
            ModContent.Request<Texture2D>(assetpath + "Textures/Extras/FishSkin/BlueFishSkin_Eyes"),
            ModContent.Request<Texture2D>(assetpath + "Textures/Extras/FishSkin/BlueFishSkin_Body"),
            ModContent.Request<Texture2D>(assetpath + "Textures/Extras/FishSkin/BlueFishSkin_Arms"),
            ModContent.Request<Texture2D>(assetpath + "Textures/Extras/FishSkin/BlueFishSkin_Hands"),
            ModContent.Request<Texture2D>(assetpath + "Textures/Extras/FishSkin/BlueFishSkin_Hands_Back"),
            ModContent.Request<Texture2D>(assetpath + "Textures/Extras/FishSkin/BlueFishSkin_Legs"),
            ModContent.Request<Texture2D>(assetpath + "Textures/Extras/FishSkin/BlueFishSkin_Legs"),//Slim
            ModContent.Request<Texture2D>(assetpath + "Textures/Extras/FishSkin/BlueFishSkin_Ears_High"),
            ModContent.Request<Texture2D>(assetpath + "Textures/Extras/FishSkin/BlueFishSkin_Ears_Low"),
            ModContent.Request<Texture2D>(assetpath + "Textures/Extras/FishSkin/BlueFishSkin_Tail")
            ];

        Effects.BasicTrail = ModContent.Request<Effect>(assetpath + "Effects/BasicTrail", AssetRequestMode.ImmediateLoad);
        Effects.Dissolve = ModContent.Request<Effect>(assetpath + "Effects/Dissolve", AssetRequestMode.ImmediateLoad);

        Effects.OrionShader = ModContent.Request<Effect>(assetpath + "Effects/OrionShader");
        Effects.RadiationDyeShader = ModContent.Request<Effect>(assetpath + "Effects/RadiationDyeShader");
        Effects.SeasideHairShader = ModContent.Request<Effect>(assetpath + "Effects/SeasideHairShader");

        //Sounds.FishyHit = new SoundStyle(assetpath + "Sounds/HitSounds/FishySkin_Hurt", 1, 3) { PitchVariance = 0.4f, Volume = 0.7f };
        Sounds.DemonHit = new SoundStyle(assetpath + "Sounds/HitSounds/DemonSkin_Hurt", 1, 3) { PitchVariance = 0.4f, Volume = 0.7f };
    }

    public static class Textures
    {
        public static readonly string Placeholder = $"{nameof(BlockVanity)}/Assets/Textures/Placeholder_Pearl";

        public static Asset<Texture2D>[] Glow;
        public static Asset<Texture2D> FishEyes;
        public static Asset<Texture2D>[] FireDissolveNoise;
        public static Asset<Texture2D> OrionNoise;
        public static Asset<Texture2D> SeasideColorMap;

        public static Asset<Texture2D>[] Particle;

        public static Asset<Texture2D>[] Bar;

        public static Asset<Texture2D>[] BlueFishSkin;
    }

    public static class Sounds
    {
        public static SoundStyle FishyHit;

        public static SoundStyle DemonHit;
    }

    public static class Effects
    {
        public static Asset<Effect> BasicTrail;
        public static Asset<Effect> Dissolve;

        public static Asset<Effect> OrionShader;
        public static Asset<Effect> RadiationDyeShader;
        public static Asset<Effect> SeasideHairShader;
    }
}

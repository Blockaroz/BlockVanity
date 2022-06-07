using Terraria.ModLoader;

namespace BlockVanity
{
	public class BlockVanity : Mod
	{
        public override void Load()
        {
            ParticleEngine.ParticleLoader.Load();
        }

        public override void Unload()
        {
            ParticleEngine.ParticleLoader.Unload();
        }
    }
}
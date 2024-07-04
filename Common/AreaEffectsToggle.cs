using Terraria.Localization;
using Terraria.ModLoader;

namespace BlockVanity.Common;

public class AreaEffectsToggle : BuilderToggle
{
    public override string DisplayValue() => Mod.GetLocalization("AreaEffectsToggle.DisplayText").Value;

    public override Position OrderPosition => new After(BuilderToggle.TorchBiome);

    public override int NumberOfStates => 2;
}

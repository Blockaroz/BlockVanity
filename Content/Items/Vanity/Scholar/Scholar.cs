using BlockVanity.Common.Utilities;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlockVanity.Content.Items.Vanity.Scholar;

[AutoloadEquip(EquipType.Head)]
public class ScholarHood : VanityItem
{
    public ScholarHood() : base(ItemRarityID.Blue) { }
}

[AutoloadEquip(EquipType.Body)]
public class ScholarCloak : VanityItem
{
    public ScholarCloak() : base(ItemRarityID.Blue) { }
}

//[AutoloadEquip(EquipType.Legs)]
//public class ScholarShorts : VanityItem
//{
//    public ScholarShorts() : base("Scholar's Shorts", ItemRarityID.Blue, "'Oth...'") { }
//}

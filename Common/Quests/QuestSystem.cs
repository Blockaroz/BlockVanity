using BlockVanity.Common.UI.QuestUI;
using BlockVanity.Content;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace BlockVanity.Common.Quests;

public class QuestSystem : ModSystem
{
    public ModKeybind testBind;

    public QuestUI questUI;
    public static QuestDatabase database;

    public override void Load()
    {
        if (Main.dedServ)
            return;

        testBind = KeybindLoader.RegisterKeybind(Mod, "QuestUI Test Bind", Microsoft.Xna.Framework.Input.Keys.Y);

        database = new QuestDatabase();
    }

    public override void PostAddRecipes()
    {
        if (Main.dedServ)
            return;

        AllQuests.AddQuests(ref database);
        questUI = new QuestUI(database);
    }

    private int checkCounter;
    public bool RequestCheck { get; set; }

    public override void PostUpdatePlayers()
    {
        if (Main.netMode != NetmodeID.Server)
        {
            if (checkCounter++ > 6 || RequestCheck)
            {
                database.CheckQuests();

                RequestCheck = false;
                checkCounter = 0;
            }
        }
    }

    public override void UpdateUI(GameTime gameTime)
    {
        if (testBind.JustPressed && !Main.inFancyUI)
        {
            SoundEngine.PlaySound(SoundID.MenuOpen);

            questUI = new QuestUI(database);

            IngameFancyUI.OpenUIState(questUI);
            questUI.UpdateContents();
        }
    }
}

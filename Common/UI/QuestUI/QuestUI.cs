using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlockVanity.Common.Quests;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.UI.Elements;
using Terraria.GameContent.UI.States;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;

namespace BlockVanity.Common.UI.QuestUI;

public class QuestUI : UIState
{
    private bool _clicked;

    private QuestUIInfoPage _infoPage;

    private QuestUIEntryGrid _entryGrid;

    private List<QuestEntry> _originalEntries;
    private List<QuestEntry> _workingEntries;

    private EntryFilterer<QuestEntry, IEntryFilter<QuestEntry>> _filterer;
    private EntrySorter<QuestEntry, IEntrySortStep<QuestEntry>> _sorter;

    private UIText _pageIndexesText;
    private UIText _sortText;

    private UISearchBar _searchBar;
    private UIPanel _searchBoxPanel;
    private string _searchString;
    private bool _clickedSearchBar;

    public QuestUI(QuestDatabase questDatabase)
    {
        _filterer = new EntryFilterer<QuestEntry, IEntryFilter<QuestEntry>>();
        _sorter = new EntrySorter<QuestEntry, IEntrySortStep<QuestEntry>>();

        _originalEntries = new List<QuestEntry>(questDatabase.Entries);
        _workingEntries = new List<QuestEntry>(questDatabase.Entries);
        BuildPage();
    }

    private void BuildPage()
    {
        RemoveAllChildren();
        UIElement mainElement = new UIElement();
        mainElement.Width.Set(0f, 0.875f);
        mainElement.Height.Set(-200f, 1f);
        mainElement.MaxWidth.Set(1000f, 0f);
        mainElement.MinWidth.Set(900f, 0f);
        mainElement.Top.Set(150, 0.05f);
        mainElement.HAlign = 0.5f;
        Append(mainElement);
        MakeExitButton(mainElement);

        UIPanel mainPanel = new UIPanel();
        mainPanel.Width.Set(0f, 1f);
        mainPanel.Height.Set(-100f, 1f);
        mainPanel.BackgroundColor = new Color(33, 43, 79) * 0.8f;
        mainPanel.PaddingTop -= 4f;
        mainElement.Append(mainPanel);

        UIElement topPanel = new UIElement
        {
            Width = new StyleDimension(0f, 1f),
            Height = new StyleDimension(24f, 0f),
            VAlign = 0f
        };

        topPanel.SetPadding(0f);

        QuestUIInfoPage infoPageUI = _infoPage = new QuestUIInfoPage()
        {
            Height = new StyleDimension(12f, 1f),
            HAlign = 1f
        };

        AddSortButton(topPanel, infoPageUI);
        AddSearchBar(topPanel, infoPageUI);

        UIElement infoPanel = new UIElement
        {
            Width = new StyleDimension(0f, 1f),
            Height = new StyleDimension(-(topPanel.Height.Pixels + 6), 1f),
            VAlign = 1f
        };

        infoPanel.SetPadding(0f);
        infoPanel.Append(infoPageUI);

        UIElement gridSpace = new UIElement
        {
            Width = new StyleDimension(-12f - infoPageUI.Width.Pixels, 1f),
            Height = new StyleDimension(-(topPanel.Height.Pixels + 12), 1f),
            VAlign = 1f
        };
        QuestUIEntryGrid entryGrid = _entryGrid = new QuestUIEntryGrid(_workingEntries, null);
        entryGrid.OnGridContentsChanged += UpdateRange;
        AddBackAndForwardButtons(topPanel);
        gridSpace.Append(entryGrid);

        mainPanel.Append(gridSpace);
        mainPanel.Append(infoPanel);
        mainPanel.Append(topPanel);

        _searchBar.SetContents(null, forced: true);

        UpdateContents();
    }

    public override void Recalculate()
    {
        base.Recalculate();
        FillInEntries();
    }

    public void UpdateContents()
    {
        //_sortText.SetText(_sorter.GetDisplayName());
        FilterEntries();
        FillInEntries();
    }

    private void FillInEntries()
    {

    }

    private void FilterEntries()
    {
        _workingEntries.Clear();
        _workingEntries.AddRange(_originalEntries.Where(_filterer.FitsFilter));
    }

    private void SortEntries() => _workingEntries.Sort(_sorter);

    private void UpdateRange()
    {
        _pageIndexesText?.SetText(_entryGrid.GetRangeText());
    }

    private void MakeExitButton(UIElement outerContainer)
    {
        UITextPanel<LocalizedText> uITextPanel = new UITextPanel<LocalizedText>(Language.GetText("UI.Back"), 0.7f, large: true)
        {
            Width = StyleDimension.FromPixelsAndPercent(300f, 0f),
            Height = StyleDimension.FromPixels(50f),
            VAlign = 1f,
            HAlign = 0.5f,
            Top = StyleDimension.FromPixels(-25f)
        };

        uITextPanel.OnMouseOver += FadedMouseOver;
        uITextPanel.OnMouseOut += FadedMouseOut;
        uITextPanel.OnLeftMouseDown += Click_GoBack;
        uITextPanel.SetSnapPoint("ExitButton", 0);
        outerContainer.Append(uITextPanel);
    }

    private void Click_GoBack(UIMouseEvent evt, UIElement listeningElement)
    {
        SoundEngine.PlaySound(SoundID.MenuClose);
        if (Main.gameMenu)
            Main.menuMode = 0;
        else
            IngameFancyUI.Close();
    }

    private void AddBackAndForwardButtons(UIElement innerTopContainer)
    {
        UIImageButton backButton = new UIImageButton(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Button_Back"));
        backButton.SetHoverImage(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Button_Border"));
        backButton.SetVisibility(1f, 1f);
        backButton.SetSnapPoint("BackPage", 0);
        _entryGrid.MakeButtonGoByOffset(backButton, -1);
        innerTopContainer.Append(backButton);
        UIImageButton forwardButton = new UIImageButton(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Button_Forward"))
        {
            Left = new StyleDimension(backButton.Width.Pixels + 1f, 0f)
        };

        forwardButton.SetHoverImage(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Button_Border"));
        forwardButton.SetVisibility(1f, 1f);
        forwardButton.SetSnapPoint("NextPage", 0);
        _entryGrid.MakeButtonGoByOffset(forwardButton, 1);
        innerTopContainer.Append(forwardButton);
        UIPanel pageTextPanel = new UIPanel
        {
            Left = new StyleDimension(backButton.Width.Pixels + forwardButton.Width.Pixels + 7f, 0f),
            Width = new StyleDimension(135f, 0f),
            Height = new StyleDimension(0f, 1f),
            VAlign = 0.5f
        };

        pageTextPanel.BackgroundColor = new Color(35, 40, 83);
        pageTextPanel.BorderColor = new Color(35, 40, 83);
        pageTextPanel.SetPadding(0f);
        innerTopContainer.Append(pageTextPanel);
        UIText pageIndexesText = new UIText("", 0.8f)
        {
            HAlign = 0.5f,
            VAlign = 0.5f
        };

        pageTextPanel.Append(pageIndexesText);
        _pageIndexesText = pageIndexesText;
    }

    private void AddSearchBar(UIElement innerTopContainer, QuestUIInfoPage infoSpace)
    {
        UIImageButton searchManualButton = new UIImageButton(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Button_Search"))
        {
            Left = new StyleDimension(-(infoSpace.Width.Pixels + 2), 1f),
            VAlign = 0.5f
        };

        searchManualButton.OnLeftClick += Click_SearchArea;
        searchManualButton.SetHoverImage(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Button_Search_Border"));
        searchManualButton.SetVisibility(1f, 1f);
        searchManualButton.SetSnapPoint("SearchButton", 0);
        innerTopContainer.Append(searchManualButton);
        UIPanel searchPanel = _searchBoxPanel = new UIPanel
        {
            Left = new StyleDimension(-infoSpace.Width.Pixels + searchManualButton.Width.Pixels + 3f, 1f),
            Width = new StyleDimension(infoSpace.Width.Pixels - searchManualButton.Width.Pixels - 3f, 0f),
            Height = new StyleDimension(0f, 1f),
            VAlign = 0.5f
        };

        searchPanel.BackgroundColor = new Color(35, 40, 83);
        searchPanel.BorderColor = searchPanel.BackgroundColor;
        searchPanel.SetPadding(0f);
        innerTopContainer.Append(searchPanel);
        UISearchBar searchBar = _searchBar = new UISearchBar(Language.GetText("UI.PlayerNameSlot"), 0.8f)
        {
            Width = new StyleDimension(0f, 1f),
            Height = new StyleDimension(0f, 1f),
            HAlign = 0f,
            VAlign = 0.5f,
            Left = new StyleDimension(0f, 0f),
            IgnoresMouseInteraction = true
        };

        searchPanel.OnLeftClick += Click_SearchArea;
        searchBar.OnContentsChanged += OnSearchContentsChanged;
        searchPanel.Append(searchBar);
        searchBar.OnStartTakingInput += OnStartTakingInput;
        searchBar.OnEndTakingInput += OnEndTakingInput;
        searchBar.OnNeedingVirtualKeyboard += OpenVirtualKeyboardWhenNeeded;
        UIImageButton cancelButton = new UIImageButton(Main.Assets.Request<Texture2D>("Images/UI/SearchCancel"))
        {
            HAlign = 1f,
            VAlign = 0.5f,
            Left = new StyleDimension(-2f, 0f)
        };

        cancelButton.OnMouseOver += searchCancelButton_OnMouseOver;
        cancelButton.OnLeftClick += searchCancelButton_OnClick;
        searchPanel.Append(cancelButton);
    }

    private void searchCancelButton_OnClick(UIMouseEvent evt, UIElement listeningElement)
    {
        if (_searchBar.HasContents)
        {
            _searchBar.SetContents(null, forced: true);
            SoundEngine.PlaySound(SoundID.MenuClose);
        }
        else
            SoundEngine.PlaySound(SoundID.MenuTick);

    }

    private void searchCancelButton_OnMouseOver(UIMouseEvent evt, UIElement listeningElement)
    {
        SoundEngine.PlaySound(SoundID.MenuTick);
    }

    private void OpenVirtualKeyboardWhenNeeded()
    {
        int maxInputLength = 40;
        UIVirtualKeyboard uIVirtualKeyboard = new UIVirtualKeyboard(Language.GetText("UI.PlayerNameSlot").Value, _searchString, OnFinishedSettingName, GoBackHere, 0, allowEmpty: true);
        uIVirtualKeyboard.SetMaxInputLength(maxInputLength);
        UserInterface.ActiveInstance.SetState(uIVirtualKeyboard);
    }

    private void OnFinishedSettingName(string name)
    {
        string contents = name.Trim();
        _searchBar.SetContents(contents);
        GoBackHere();
    }

    private void GoBackHere()
    {
        UserInterface.ActiveInstance.SetState(this);
        _searchBar.ToggleTakingText();
    }

    private void OnStartTakingInput()
    {
        _searchBoxPanel.BorderColor = Main.OurFavoriteColor;
    }

    private void OnEndTakingInput()
    {
        _searchBoxPanel.BorderColor = new Color(35, 40, 83);
    }

    private void OnSearchContentsChanged(string contents)
    {
        _searchString = contents;
        _filterer.SetSearchFilter(contents);
        UpdateContents();
    }

    private void Click_SearchArea(UIMouseEvent evt, UIElement listeningElement)
    {
        if (evt.Target.Parent != _searchBoxPanel)
        {
            _searchBar.ToggleTakingText();
            _clickedSearchBar = true;
        }
    }

    private void AttemptStoppingUsingSearchbar(UIMouseEvent evt)
    {
        _clicked = true;
    }

    private void AddSortButton(UIElement mainElement, QuestUIInfoPage infoSpace)
    {
        UIImageButton sortButton = new UIImageButton(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Button_Sorting"))
        {
            Left = new StyleDimension(-(infoSpace.Width.Pixels + 20), 0f),
            HAlign = 1f
        };

        sortButton.SetHoverImage(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Button_Wide_Border"));
        sortButton.SetVisibility(1f, 1f);
        sortButton.SetSnapPoint("SortButton", 0);
        //uIImageButton2.OnLeftClick += OpenOrCloseSortingOptions;
        mainElement.Append(sortButton);
        UIText sortText = _sortText = new UIText("", 0.8f)
        {
            Left = new StyleDimension(34f, 0f),
            Top = new StyleDimension(2f, 0f),
            VAlign = 0.5f,
            TextOriginX = 0f,
            TextOriginY = 0f
        };

        sortButton.Append(sortText);
    }

    public override void LeftClick(UIMouseEvent evt)
    {
        base.LeftClick(evt);
        AttemptStoppingUsingSearchbar(evt);
    }

    public override void RightClick(UIMouseEvent evt)
    {
        base.RightClick(evt);
        AttemptStoppingUsingSearchbar(evt);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        if (_clicked && !_clickedSearchBar && _searchBar.IsWritingText)
            _searchBar.ToggleTakingText();

        _clicked = false;
        _clickedSearchBar = false;
    }

    private void FadedMouseOver(UIMouseEvent evt, UIElement listeningElement)
    {
        SoundEngine.PlaySound(SoundID.MenuTick);
        ((UIPanel)evt.Target).BackgroundColor = new Color(73, 94, 171);
        ((UIPanel)evt.Target).BorderColor = Colors.FancyUIFatButtonMouseOver;
    }

    private void FadedMouseOut(UIMouseEvent evt, UIElement listeningElement)
    {
        ((UIPanel)evt.Target).BackgroundColor = new Color(63, 82, 151) * 0.8f;
        ((UIPanel)evt.Target).BorderColor = Color.Black;
    }
}

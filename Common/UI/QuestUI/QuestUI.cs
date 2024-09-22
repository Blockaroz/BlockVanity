using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BlockVanity.Common.Quests;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.GameContent.UI.States;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace BlockVanity.Common.UI.QuestUI;

public class QuestUI : UIState
{
    private bool _clicked;

    private UIPanel _mainPanel;

    private QuestUIInfoPage _infoPage;

    private UIElement _entryGridSpace;
    private QuestUIEntryGrid _entryGrid;

    private List<QuestEntry> _originalEntries;
    private List<QuestEntry> _workingEntries;

    private QuestSorter _sorter;
    private QuestUISortingGrid _sortingGrid;

    private UIText _pageIndexesText;
    private UIText _sortText;

    private UISearchBar _searchBar;
    private UIPanel _searchBoxPanel;
    private string _searchString;
    private bool _clickedSearchBar;

    public QuestUI(QuestDatabase questDatabase)
    {
        _sorter = new QuestSorter();
        _originalEntries = new List<QuestEntry>(questDatabase.Entries);
        _workingEntries = new List<QuestEntry>(questDatabase.Entries);
        BuildPage();
    }

    private void BuildPage()
    {
        RemoveAllChildren();
        UIElement mainElement = new UIElement();
        mainElement.Width.Set(980f, 0f);
        mainElement.MinWidth.Set(650f, 0f);
        mainElement.MaxWidth.Set(980f, 0f);
        mainElement.Height.Set(-250f, 1f);
        mainElement.MinHeight.Set(500f, 0f);
        mainElement.HAlign = 0.5f;
        mainElement.VAlign = 0.7f;
        Append(mainElement);

        UIPanel mainPanel = _mainPanel = new UIPanel();
        mainPanel.Width.Set(0f, 1f);
        mainPanel.Height.Set(-100f, 1f);
        mainPanel.BackgroundColor = new Color(33, 43, 79) * 0.8f;
        mainPanel.PaddingTop -= 4f;

        UIElement topPanel = new UIElement
        {
            Width = new StyleDimension(0f, 1f),
            Height = new StyleDimension(24f, 0f),
            VAlign = 0f
        };

        topPanel.SetPadding(0f);

        const int miscWidth = 200;

        QuestUIInfoPage infoPageUI = _infoPage = new QuestUIInfoPage()
        {
            Width = new StyleDimension(600, 0f),
            Height = new StyleDimension(-(topPanel.Height.Pixels + 12), 1f),
            VAlign = 1f,
            HAlign = 1f,
        };
        mainPanel.Append(infoPageUI);
        infoPageUI.SetPadding(0f);

        AddBackButton(mainElement);
        AddSortButton(topPanel, miscWidth);
        AddSearchBar(topPanel, miscWidth);

        UIElement gridSpace = _entryGridSpace = new UIElement
        {
            Width = new StyleDimension(-(infoPageUI.Width.Pixels + 12), 1f),
            Height = new StyleDimension(-(topPanel.Height.Pixels + 12), 1f),
            VAlign = 1f
        };
        QuestUIEntryGrid entryGrid = _entryGrid = new QuestUIEntryGrid(_workingEntries, ClickEntryGridButton);
        entryGrid.OnUpdateGrid += UpdateGrid;
        AddBackAndForwardButtons(topPanel);
        gridSpace.Append(entryGrid);

        mainPanel.Append(gridSpace);
        mainPanel.Append(topPanel);

        _searchBar.SetContents(null, forced: true);

        QuestUISortingGrid sortingGrid = _sortingGrid = new QuestUISortingGrid(_sorter);
        sortingGrid.OnLeftClick += ClickCloseSortingGrid;
        sortingGrid.OnClickingOption += UpdateContents;

        _sorter.Steps.Clear();

        for (int i = 0; i < QuestSortSteps.DefaultSteps.Count; i++)
        {
            if (QuestSortSteps.DefaultSteps[i].Hidden)
                _sorter.Steps.Add(QuestSortSteps.DefaultSteps[i]);
        }

        mainElement.Append(mainPanel);

        UpdateContents();
    }

    public override void Recalculate()
    {
        base.Recalculate();
        UpdateContents();
    }

    public void UpdateContents()
    {
        _sortText?.SetText($"{Language.GetOrRegister($"Mods.{nameof(BlockVanity)}.UI.SortBy").Value} ({_sortingGrid?.Selections?.Count ?? 0})");
        SortEntries();
        FillInEntries();
    }

    private void FillInEntries()
    {
        if (_entryGrid != null && _entryGrid.Parent != null)
        { 
            _entryGrid.SetWorkingEntries(_workingEntries);
            _entryGrid.FillInEntries(); 
        }
    }

    private void SortEntries() => _workingEntries?.Sort(_sorter);

    private void UpdateGrid()
    {
        _pageIndexesText?.SetText(_entryGrid.GetRangeText());
    }

    private void AddBackButton(UIElement outerContainer)
    {
        UITextPanel<LocalizedText> backButton = new UITextPanel<LocalizedText>(Language.GetText("UI.Back"), 0.7f, large: true)
        {
            Width = StyleDimension.FromPixelsAndPercent(300f, 0f),
            Height = StyleDimension.FromPixels(50f),
            VAlign = 1f,
            HAlign = 0.5f,
            Top = StyleDimension.FromPixels(-25f)
        };

        backButton.OnMouseOver += FadedMouseOver;
        backButton.OnMouseOut += FadedMouseOut;
        backButton.OnLeftMouseDown += Click_GoBack;
        backButton.SetSnapPoint("ExitButton", 0);
        outerContainer.Append(backButton);
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

    private void AddSearchBar(UIElement innerTopContainer, int width)
    {
        UIImageButton searchManualButton = new UIImageButton(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Button_Search"))
        {
            Left = new StyleDimension(-width, 1f),
            VAlign = 0.5f
        };

        searchManualButton.OnLeftClick += ClickSearchBar;
        searchManualButton.SetHoverImage(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Button_Search_Border"));
        searchManualButton.SetVisibility(1f, 1f);
        searchManualButton.SetSnapPoint("SearchButton", 0);
        innerTopContainer.Append(searchManualButton);
        UIPanel searchPanel = _searchBoxPanel = new UIPanel
        {
            Left = new StyleDimension(-width + searchManualButton.Width.Pixels + 3f, 1f),
            Width = new StyleDimension(width - searchManualButton.Width.Pixels - 3f, 0f),
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

        searchPanel.OnLeftClick += ClickSearchBar;
        searchPanel.Append(searchBar);

        searchBar.OnContentsChanged += OnSearchContentsChanged;
        searchBar.OnStartTakingInput += OnStartTakingInput;
        searchBar.OnEndTakingInput += OnEndTakingInput;
        searchBar.OnNeedingVirtualKeyboard += OpenVirtualKeyboardWhenNeeded;
        UIImageButton cancelButton = new UIImageButton(Main.Assets.Request<Texture2D>("Images/UI/SearchCancel"))
        {
            HAlign = 1f,
            VAlign = 0.5f,
            Left = new StyleDimension(-2f, 0f)
        };

        cancelButton.OnMouseOver += MouseOverTickSound;
        cancelButton.OnLeftClick += ClickCancelSearch;
        searchPanel.Append(cancelButton);
    }

    private void ClickCancelSearch(UIMouseEvent evt, UIElement listeningElement)
    {
        if (_searchBar.HasContents)
        {
            _searchBar.SetContents(null, forced: true);
            SoundEngine.PlaySound(SoundID.MenuClose);
        }
        else
            SoundEngine.PlaySound(SoundID.MenuTick);

    }

    //private void ClickExpandInfoPage(UIMouseEvent evt, UIElement listeningElement)
    //{

    //}

    private void MouseOverTickSound(UIMouseEvent evt, UIElement listeningElement)
    {
        SoundEngine.PlaySound(SoundID.MenuTick);
    }

    private void OpenVirtualKeyboardWhenNeeded()
    {
        int maxInputLength = 64;
        UIVirtualKeyboard uIVirtualKeyboard = new UIVirtualKeyboard(Language.GetText("UI.PlayerNameSlot").Value, _searchString, OnFinishedSettingName, CloseKeyboard, 0, allowEmpty: true);
        uIVirtualKeyboard.SetMaxInputLength(maxInputLength);
        UserInterface.ActiveInstance.SetState(uIVirtualKeyboard);
    }

    private void OnFinishedSettingName(string name)
    {
        string contents = name.Trim();
        _searchBar.SetContents(contents);
        CloseKeyboard();
    }

    private void CloseKeyboard()
    {
        GoBackHere();
        _searchBar.ToggleTakingText();
    }

    private void GoBackHere()
    {
        UserInterface.ActiveInstance.SetState(this);
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
        _sorter.searchString = _searchString;

        UpdateContents();
    }

    private void ClickSearchBar(UIMouseEvent evt, UIElement listeningElement)
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

    private void AddSortButton(UIElement mainElement, int width)
    {
        UIImageButton sortButton = new UIImageButton(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Button_Sorting"))
        {
            Left = new StyleDimension(-(width + 20), 0f),
            HAlign = 1f
        };

        sortButton.SetHoverImage(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Button_Wide_Border"));
        sortButton.SetVisibility(1f, 1f);
        sortButton.SetSnapPoint("SortButton", 0);

        sortButton.OnLeftClick += OpenOrCloseSortingOptions;
        UIText sortText = _sortText = new UIText("", 0.8f)
        {
            Left = new StyleDimension(34f, 0f),
            Top = new StyleDimension(2f, 0f),
            VAlign = 0.5f,
            TextOriginX = 0f,
            TextOriginY = 0f
        };

        sortButton.Append(sortText);

        UIImageButton sortOrderButton = new UIImageButton(ModContent.Request<Texture2D>($"{nameof(BlockVanity)}/Assets/Textures/UI/Quests/SortAscending"))
        {
            Left = new StyleDimension(-(width + 140), 0f),
            HAlign = 1f
        };

        sortOrderButton.SetHoverImage(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Button_Border"));
        sortOrderButton.SetVisibility(1f, 1f);
        sortOrderButton.SetSnapPoint("SortOrderButton", 1);
        sortOrderButton.OnLeftClick += SwitchSortOrder;

        mainElement.Append(sortOrderButton);
        mainElement.Append(sortButton);
    }

    private void OpenOrCloseSortingOptions(UIMouseEvent evt, UIElement listeningElement)
    {
        if (_sortingGrid.Parent != null)
        {
            CloseSortingGrid();
            return;
        }

        _mainPanel?.RemoveChild(_sortingGrid);
        _mainPanel?.Append(_sortingGrid);
    }

    private void ClickCloseSortingGrid(UIMouseEvent evt, UIElement listeningElement)
    {
        if (evt.Target == _sortingGrid)
            CloseSortingGrid();
    }

    private void CloseSortingGrid()
    {
        UpdateContents();
        _mainPanel?.RemoveChild(_sortingGrid);
    }

    private void SwitchSortOrder(UIMouseEvent evt, UIElement listeningElement)
    {
        UIImageButton button = (UIImageButton)evt.Target;
        _sorter.descending = !_sorter.descending;

        if (_sorter.descending)
            button.SetImage(ModContent.Request<Texture2D>($"{nameof(BlockVanity)}/Assets/Textures/UI/Quests/SortDescending"));
        else
            button.SetImage(ModContent.Request<Texture2D>($"{nameof(BlockVanity)}/Assets/Textures/UI/Quests/SortAscending"));

        UpdateContents();
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

    private void ClickEntryGridButton(UIMouseEvent evt, UIElement listeningElement)
    {
        QuestUIEntryGridButton entryButton = (QuestUIEntryGridButton)listeningElement;

        _infoPage.OpenEntry(entryButton.Entry);

        SoundEngine.PlaySound(SoundID.MenuTick);
    }
}

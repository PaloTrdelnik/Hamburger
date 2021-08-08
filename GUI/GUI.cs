using Godot;
using System;

public class GUI : CanvasLayer
{
    [Signal]
    public delegate void SLoadlevel(string resPath);

    [Signal]
    public delegate void SUnloadLevel();

    [Signal]
    public delegate void SRestartLevel(bool firstPlay);

    [Signal]
    public delegate void SQuitLevel();

    [Signal]
    public delegate void SBuyGLock(Player player, string gLockKey);

    [Signal]
    public delegate void SBuyItem(Player player, string itemKeyToBuy, int amount);

    [Signal]
    public delegate void SSellItem(Player player, string itemKeyToSell, int amount);

    [Signal]
    public delegate void SUpdateShopPrizes(Shop shop);

    public string ActualGUI = "MainMenuGUI";

    private string _previusGUI = "MainMenuGUI";
    private string _levelResPath = "res://Levels/Level.tscn";

    private bool _bAllowedBackgroundInput = false; // screen cover input fix
    private bool _bAllowedForegroundInput = false;
    //double restart fix
    private bool _actionJustPressed = false;
    private bool _bButtonReleased = false;
    //music
    private bool _bMusicFadeWithoutCover = false;

    private Player _player;
    private Shop _shop;
    private Level _level;
    private GameplayUnlocker _gUnlocker;

    private bool _mainMenuGUIFirstAppear = true;
    private bool _gameOverGUIFirstAppear = true;
    private AnimationPlayer _mainScreenCaptAnimPlayer;
    private AnimationPlayer _gameOverScreenCaptAnimPlayer;
    private AnimationPlayer _finishedGameUnbScreenCaptAnimPlayer;
    private AnimationPlayer _finishedGameFinishScreenCaptAnimPlayer;
    private AnimationPlayer _finishedGameThanksScreenCaptAnimPlayer;
    private MenuV _mainMenuV;
    private MenuV _gameOverMenuV;
    private MenuV _inGameMenuV;
    private MenuV _finishedGameMenuV;
    private ButtonImageMute _muteButton;
    private Notification _mainMenuImprovementNotify;
    private Notification _gameOverImprovementNotify;
    private Notification _mainMenuKeyPlayNotify;
    private Notification _mainMenuKeyShopNotify;
    private Notification _mainMenuKeyQuitNotify;
    private Notification _gameOverKeyRestartNotify;
    private Notification _gameOverKeyMainMenuNotify;
    private ContextualInfoConfirm _contextualInfoConfirm;
    private ContextualInfoNotify _contextualInfoNotify;
    private ItemAmountImageContainer _moneyAmount;
    private ItemAmountImageContainer _moneyShopAmount;
    private PropertyUsageHGUI _invUsage;
    private PropertyUsageHGUI _invShopUsage;
    private ItemContainer _ItemContainer;
    private BigPropertyAmount _bigScoreProp;
    private PropertyAmountH _inGameScoreProp;
    private PropertyAmountH _finishrdGameScoreProp;
    private PropertyAmountH _finishrdGameTimeOfAttempt;
    private PropertyAmountH _finishrdGameNumberOfAttempts;
    private GameplayLockConainer _gLockContainer;
    private ToolBar _toolBar;
    private ScreenCover _chanScreenCover;
    private StartScreenCover _startScreenCover;
    private MusicManager _musicManager;
    private AudioStreamPlayer _mainMenuAPlayer;
    private AudioStreamPlayer _inGameAPlayer;
    private AudioStreamPlayer _gameOverAPlayer;
    private AudioStreamPlayer _gameFinishedAPlayer;

    private PropertyAmountH _creditsLabel;
    private PropertyAmountH _versionLabel;

    public async void OnPlayButtonDown()
    {
        if (_bAllowedBackgroundInput)
        {
            _chanScreenCover.PlayHideScreenAnim();
            await ToSignal(GetTree().CreateTimer(_chanScreenCover.AnimLength + 0.1f), "timeout");

            EmitSignal(nameof(SLoadlevel), _levelResPath);
            EmitSignal(nameof(SRestartLevel), true);

            ChangeGUI("InGameGUI");
        }
    }

    public void OnQuitButtonDown()
    {
        if (_bAllowedBackgroundInput)
        {
            if (!_actionJustPressed)
            {
                _bAllowedBackgroundInput = false;
                _bAllowedForegroundInput = true;
                _contextualInfoConfirm.ShowInfo(_contextualInfoConfirm.GetInfoText());
            }
        }
    }

    public void OnQuitContextualInfoConfirmSConfirmed()
    {
        if (_bAllowedForegroundInput)
            GetTree().Quit();
    }

    public void OnContextualInfoConfirmSCanceled()
    {
        _bAllowedBackgroundInput = true;
        _bAllowedForegroundInput = false;
        _actionJustPressed = true;
        _bButtonReleased = true;
    }

    public void OnRestartButtonDown()
    {
        if (_bAllowedBackgroundInput)
        {
            //Double restart fix
            if (!_actionJustPressed)
            {
                EmitSignal(nameof(SRestartLevel), false);

                _actionJustPressed = true;
                ChangeGUI("InGameGUI");
            }
        }
    }

    public void OnRestartButtonTextButtonUp()
    {
        _bButtonReleased = true;
    }

    public async void OnMainMenuButtonDown()
    {
        if (_bAllowedBackgroundInput)
        {
            //Double restart fix
            if (!_actionJustPressed)
            {
                _chanScreenCover.PlayHideScreenAnim();
                await ToSignal(GetTree().CreateTimer(_chanScreenCover.AnimLength), "timeout");

                EmitSignal(nameof(SUnloadLevel));
                EmitSignal(nameof(SQuitLevel));

                ChangeGUI("MainMenuGUI");
            }
        }
    }

    public void OnBackButtonDown()
    {
        if (_bAllowedBackgroundInput)
        {
            //Double restart fix
            if (!_actionJustPressed)
            {
                ChangeGUI(_previusGUI);
                _actionJustPressed = true;
            }
        }
    }

    public void OnShopButtonDown()
    {
        _moneyShopAmount.UpdateAmount(_player);
        ChangeGUI("ShopGUI");
    }
    public void OnGameplayLockContainerSRequestUnlockGLock(string gLockKey)
    {
        EmitSignal(nameof(SBuyGLock),_player, gLockKey);
    }

    public void OnGameplayUnlockerSGameplayLockUnlocked(string gLockKey)
    {
        //update gLockContainer of improvements
        _gLockContainer.UpdateAvailableGLocksVisibility(_gUnlocker.GetDictOfLocksAvilability());
    }

    public void OnItemContainerSRequestBuyItem(string itemKey)
    {
        EmitSignal(nameof(SBuyItem), _player, itemKey, 1);
    }

    public void OnItemContainerSRequestSellItem(string itemKey)
    {
        EmitSignal(nameof(SSellItem), _player, itemKey, 1);
    }

    public void OnMuteButtonTextButtonDown()
    {
        _muteButton.SwapTexture(_musicManager.GetMuteForActual());

        _musicManager.SetMuteForActual(!_musicManager.GetMuteForActual());
    }

    //temporary label changing
    public void OnShopSItemBought(string itemKey)
    {
        _ItemContainer.UpdateAmount(_player, itemKey);

        _moneyShopAmount.UpdateAmount(_player);
        _moneyShopAmount.PlayUpdateAmountAnim();
        //update inventory usage
        _invShopUsage.UpdateAmount(_player.Inv.UsageProp);
        _invShopUsage.PlayUpdateAmountAnim();
    }
    //temporary label changing
    public void OnShopSItemSold(string itemKey)
    {
        _ItemContainer.UpdateAmount(_player, itemKey);

        _moneyShopAmount.UpdateAmount(_player);
        _moneyShopAmount.PlayUpdateAmountAnim();
        //update inventory usage
        _invShopUsage.UpdateAmount(_player.Inv.UsageProp);
        _invShopUsage.PlayUpdateAmountAnim();
    }

    public void OnShopSLackOfMoneyToBuy()
    {
        _contextualInfoNotify.ShowInfo("You haven't got enough money to buy this.");
    }

    public void OnShopSLackOfItemToSell()
    {
        _contextualInfoNotify.ShowInfo("You haven't got enough items to sell them.");
    }

    public void OnLackOfContextualInfoNotifySConfirmed()
    {
        _actionJustPressed = true;
        _bButtonReleased = true;
    }

    public async void OnMainSLevelLoaded(Node level)
    {
        _level = (Level)level;

        level.Connect("SPlayerGO", this, nameof(OnLevelSPlayerGO));
        level.Connect("SPlayerUpdateScore", this, nameof(OnLevelSPlayerUpdateScore));
        level.Connect("SGameFinished", this, nameof(OnLevelSGameFinished));

        await ToSignal(GetTree().CreateTimer(0.5f), "timeout");
        _chanScreenCover.PlayShowScreenAnim();
        await ToSignal(GetTree().CreateTimer(_chanScreenCover.AnimLength), "timeout");

        //player in level
        //level.Connect("SPlayerInvItemChange", this, nameof(OnLevelSPlayerInvItemChange));
    }

    public void OnMainSLevelUnloaded()
    {
        _chanScreenCover.PlayShowScreenAnim();
    }

    public void OnPlayerSInvItemChange(Player player, string itemInvKey)
    { 
        if (itemInvKey == "Money")
        {
            _moneyAmount.UpdateAmount(_player);
            _moneyAmount.PlayUpdateAmountAnim();
        }

        if (itemInvKey == "TimeDilation")
        {
            _toolBar.UpdateAmount(player, itemInvKey);
        }
        //update ussage of inventory
        _invUsage.UpdateAmount(_player.Inv.UsageProp.GetAmount());
        _invUsage.PlayUpdateAmountAnim();
    }

    public void OnPlayerSInvResized()
    {
        _invShopUsage.UpdateMaximum(_player.Inv.UsageProp);
        _invShopUsage.PlayUpdateMaximum();
    }

    public void OnPlayerSItemUseDurabilityBegin(string itemKey)
    {
        _toolBar.GetNode<ToolBarItem>("ToolBar" + itemKey).PlayUseItemAnim(_player);
    }

    public void OnPlayerSItemUseDurabilityEnd(string itemKey)
    {
        _toolBar.GetNode<ToolBarItem>("ToolBar" + itemKey).ResetAnim("UseItem_anim");
    }

    public void OnLevelSPlayerGO(Player player)
    { 
        _bigScoreProp.UpdatePropertyAmount(player.ScoreProp);

        ChangeGUI("GameOverGUI");
    }

    public void OnLevelSGameFinished()
    {
        _finishrdGameNumberOfAttempts.UpdateAmount(_level.NumerOfAttempts);
        _finishrdGameScoreProp.UpdateAmount(_player.ScoreProp.GetAmount());
        _finishrdGameTimeOfAttempt.UpdateAmount(SecToDHMS(_level.TimeOfAttempt));

        ChangeGUI("GameFinishedGUI");
    }

    public void OnLevelSPlayerUpdateScore(Player player)
    {
        _inGameScoreProp.UpdateAmount(player.ScoreProp.GetAmount());
        //chesk if anim is playing to solve transparency issues caused by not finished ShowUp_anim
        if(!_inGameScoreProp.AnimPlayer.IsPlaying())
        {
            _inGameScoreProp.PlayUpdateAmountAnim();
        }
    }

    public void OnScreenCoverCoveringStarted()
    {
        _bAllowedBackgroundInput = false;

        _musicManager.FadeTime = 0.3f;
        _musicManager.FadeOut();
    }

    public void OnScreenCoverCoveringFinished()
    {
        _bAllowedBackgroundInput = true;
        _musicManager.FadeTime = 0.1f;
        _musicManager.FadeIn();
    }

    public async void OnMainFinalReady()
    {
        await ToSignal(GetTree().CreateTimer(1.5f), "timeout");

        _startScreenCover.PlayBlinkAnim();

        await ToSignal(GetTree().CreateTimer(5.0f), "timeout");

        _startScreenCover.PlayShowScreenAnim();

        await ToSignal(GetTree().CreateTimer(0.25f), "timeout");

        _mainScreenCaptAnimPlayer.Play("ShowUp_anim", -1, 0.9f);
        _mainMenuV.PlayShowUpAnim();

        _muteButton.PlayShowUpAnim();
        _creditsLabel.PlayShowUpAnim();
        _versionLabel.PlayShowUpAnim();

        //// LEVELDASIGN SHORTCUT
        //_startScreenCover.Hide();
        //OnPlayButtonDown();
    }

    public void ChangeGUI(string nameOfGUI)
    {
        //variable for improvement notify check
        //because of gUnlocker.IsNewGLockAvailable() is little bit more exxpensive i save its result to this variable
        //this bool is placed here bacause definition in switch can couse some problems
        bool newAvailabImrovements = false;

        if (nameOfGUI != ActualGUI)
        {
            switch (ActualGUI)
            {
                case "GameOverGUI":
                    _bMusicFadeWithoutCover = true;

                    GetNode<Control>("GameOverGUIControl").Hide();

                    //hide new record
                    _bigScoreProp.HideNewRecord();

                    //Double restart fix
                    _bButtonReleased = true;

                    _gameOverGUIFirstAppear = false;

                    break;

                case "InGameGUI":
                    _bMusicFadeWithoutCover = false;

                    GetNode<Control>("InGameGUIControl").Hide();
                    //animation
                    _inGameScoreProp.ResetAnim();
                    _inGameMenuV.ResetAnim();
                    _toolBar.ResetAnim();
                    break;

                case "MainMenuGUI":
                    _bMusicFadeWithoutCover = false;

                    GetNode<Control>("MainMenuGUIControl").Hide();

                    _mainMenuGUIFirstAppear = false;

                    break;

                case "ShopGUI":
                    _bButtonReleased = true;
                    _bMusicFadeWithoutCover = false;

                    GetNode<Control>("ShopGUIControl").Hide();
                    break;

                case "GameFinishedGUI":
                    _bMusicFadeWithoutCover = true;

                    GetNode<Control>("GameFinishedGUIControl").Hide();

                    //reset animations
                    //this code can go in to the caption script as ResetAnim() function
                    _finishedGameUnbScreenCaptAnimPlayer.Play("ShowUp_anim");
                    _finishedGameUnbScreenCaptAnimPlayer.Seek(0.0f, true);
                    _finishedGameUnbScreenCaptAnimPlayer.Stop();

                    _finishedGameFinishScreenCaptAnimPlayer.Play("ShowUp_anim");
                    _finishedGameFinishScreenCaptAnimPlayer.Seek(0.0f, true);
                    _finishedGameFinishScreenCaptAnimPlayer.Stop();

                    _finishedGameThanksScreenCaptAnimPlayer.Play("ShowUp_anim");
                    _finishedGameThanksScreenCaptAnimPlayer.Seek(0.0f, true);
                    _finishedGameThanksScreenCaptAnimPlayer.Stop();

                    _finishrdGameScoreProp.ResetAnim();
                    _finishrdGameTimeOfAttempt.ResetAnim();
                    _finishrdGameNumberOfAttempts.ResetAnim();
                    _finishedGameMenuV.ResetAnim();
                    break;

            }
            
            switch (nameOfGUI)
            {
                case "GameOverGUI":
                    _bMusicFadeWithoutCover = true;

                    _musicManager.FutureForeground = _gameOverAPlayer;
                    _musicManager.FutureBackground = _mainMenuAPlayer;
                    _musicManager.FadeTime = 0f;

                    //animation
                    _gameOverMenuV.ResetAnim();
                    _bigScoreProp.ResetAnim();

                    GetNode<Control>("GameOverGUIControl").Show();

                    //Double restart fix
                    _actionJustPressed = false;

                    //show and hide notifications
                    newAvailabImrovements = _gUnlocker.IsNewGLockAvailable();
                    _gameOverImprovementNotify.CheckSatisfaction(newAvailabImrovements);
                    _mainMenuImprovementNotify.CheckSatisfaction(newAvailabImrovements);

                    _gameOverKeyRestartNotify.CheckSatisfaction(_gameOverGUIFirstAppear);
                    _gameOverKeyMainMenuNotify.CheckSatisfaction(_gameOverGUIFirstAppear);

                    //animation
                    _gameOverScreenCaptAnimPlayer.Play("ShowUp_anim", -1, 0.8f);
                    _gameOverMenuV.PlayShowUpAnim();
                    _bigScoreProp.PlayShowUpAnim();
                    _bigScoreProp.PlayNewRecAnim();

                    break;

                case "InGameGUI":
                    _musicManager.FutureForeground = _inGameAPlayer;

                    GetNode<Control>("InGameGUIControl").Show();

                    _toolBar.UpdateAmountOfAllitems(_player);
                    _toolBar.PlayShowUpAnim();
                    //update money amount
                    _moneyAmount.UpdateAmount(_player);
                    _moneyAmount.PlayShowUpAnim();
                    //update ussage of inventory
                    _invUsage.UpdateMaximum(_player.Inv.UsageProp);
                    _invUsage.UpdateAmount(_player.Inv.UsageProp);
                    _invUsage.PlayShowUpAnim();
                    //animation
                    _inGameScoreProp.PlayShowUpAnim();
                    _inGameMenuV.PlayShowUpAnim();
                    break;

                case "MainMenuGUI":
                    _bButtonReleased = true;
                    _bMusicFadeWithoutCover = false;

                    _musicManager.FutureForeground = _mainMenuAPlayer;

                    GetNode<Control>("MainMenuGUIControl").Show();

                    //show and hide notifications
                    newAvailabImrovements = _gUnlocker.IsNewGLockAvailable();
                    _gameOverImprovementNotify.CheckSatisfaction(newAvailabImrovements);
                    _mainMenuImprovementNotify.CheckSatisfaction(newAvailabImrovements);

                    _mainMenuKeyShopNotify.CheckSatisfaction(_mainMenuGUIFirstAppear);
                    _mainMenuKeyPlayNotify.CheckSatisfaction(_mainMenuGUIFirstAppear);
                    _mainMenuKeyQuitNotify.CheckSatisfaction(_mainMenuGUIFirstAppear);

                    break;

                case "ShopGUI":
                    _bMusicFadeWithoutCover = false;

                    GetNode<Control>("ShopGUIControl").Show();

                    //update gLockContainer of improvements
                    _gLockContainer.UpdateAvailableGLocksVisibility(_gUnlocker.GetDictOfLocksAvilability());

                    EmitSignal(nameof(SUpdateShopPrizes), _shop);

                    _ItemContainer.UpdateAmount(_player);
                    //update ussage of inventory
                    _invShopUsage.UpdateMaximum(_player.Inv.UsageProp);
                    _invShopUsage.UpdateAmount(_player.Inv.UsageProp);
                    break;

                case "GameFinishedGUI":
                    _bMusicFadeWithoutCover = true;

                    _musicManager.FutureForeground = _gameFinishedAPlayer;
                    _musicManager.FutureBackground = _mainMenuAPlayer;
                    _musicManager.FadeTime = 0.1f;

                    GetNode<Control>("GameFinishedGUIControl").Show();

                    //play animations
                    _finishedGameUnbScreenCaptAnimPlayer.Play("ShowUp_anim", -1, 0.8f);
                    _finishedGameFinishScreenCaptAnimPlayer.Play("ShowUp_anim", -1, 0.8f);
                    _finishedGameThanksScreenCaptAnimPlayer.Play("ShowUp_anim", -1, 0.8f);

                    _finishrdGameScoreProp.PlayShowUpAnim();
                    _finishrdGameTimeOfAttempt.PlayShowUpAnim();
                    _finishrdGameNumberOfAttempts.PlayShowUpAnim();
                    _finishedGameMenuV.PlayShowUpAnim();

                    break;
            }

            if (_bMusicFadeWithoutCover)
            {
                _musicManager.Fade();
            }    

            _previusGUI = ActualGUI;
            ActualGUI = nameOfGUI;
        }
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _player = GetParent().GetNode<Player>("Player");
        _shop = GetParent().GetNode<Shop>("Shop");
        _gUnlocker = GetParent().GetNode<GameplayUnlocker>("GameplayUnlocker");

        _mainScreenCaptAnimPlayer = GetNode<AnimationPlayer>("MainMenuGUIControl/CenterContainer/Caption/AnimationPlayer");
        _gameOverScreenCaptAnimPlayer = GetNode<AnimationPlayer>("GameOverGUIControl/CenterContainer/VBoxContainer/Caption/AnimationPlayer");
        _finishedGameUnbScreenCaptAnimPlayer = GetNode<AnimationPlayer>("GameFinishedGUIControl/CenterContainer/VBoxContainer/Caption/AnimationPlayer");
        _finishedGameFinishScreenCaptAnimPlayer = GetNode<AnimationPlayer>("GameFinishedGUIControl/CenterContainer/VBoxContainer/Caption2/AnimationPlayer");
        _finishedGameThanksScreenCaptAnimPlayer = GetNode<AnimationPlayer>("GameFinishedGUIControl/CenterContainer/VBoxContainer/Caption3/AnimationPlayer");

        _mainMenuV = GetNode<MenuV>("MainMenuGUIControl/MenuV");
        _gameOverMenuV = GetNode<MenuV>("GameOverGUIControl/CenterContainer/VBoxContainer/MenuV");
        _inGameMenuV = GetNode<MenuV>("InGameGUIControl/MenuV");
        _finishedGameMenuV = GetNode<MenuV>("GameFinishedGUIControl/CenterContainer/VBoxContainer/MenuV");

        _muteButton = GetNode<ButtonImageMute>("MainMenuGUIControl/ButtonImageMute");

        _gameOverImprovementNotify = GetNode<Notification>("GameOverGUIControl/CenterContainer/VBoxContainer/MenuV/CenterContainer3/ShopButtonText/Notification");
        _mainMenuImprovementNotify = GetNode<Notification>("MainMenuGUIControl/MenuV/CenterContainer2/ShopButtonText/Notification");

        _mainMenuKeyPlayNotify = GetNode<Notification>("MainMenuGUIControl/MenuV/CenterContainer/PlayButtonText/KeyNotification");
        _mainMenuKeyShopNotify = GetNode<Notification>("MainMenuGUIControl/MenuV/CenterContainer2/ShopButtonText/KeyNotification");
        _mainMenuKeyQuitNotify = GetNode<Notification>("MainMenuGUIControl/MenuV/CenterContainer3/QuitButtonText/KeyNotification");
        _gameOverKeyRestartNotify = GetNode<Notification>("GameOverGUIControl/CenterContainer/VBoxContainer/MenuV/CenterContainer2/RestartButtonText/KeyNotification");
        _gameOverKeyMainMenuNotify = GetNode<Notification>("GameOverGUIControl/CenterContainer/VBoxContainer/MenuV/CenterContainer/MainMenuButtonText/KeyNotification");

        _contextualInfoConfirm = GetNode<ContextualInfoConfirm>("Cover/CenterContainer/ContextualInfoConfirm");
        _contextualInfoNotify = GetNode<ContextualInfoNotify>("Cover/CenterContainer/ContextualInfoNotify");

        _moneyAmount = GetNode<ItemAmountImageContainer>("InGameGUIControl/VBoxContainer/MoneyAmountImageContainer");
        _moneyShopAmount = GetNode<ItemAmountImageContainer>("ShopGUIControl/VBoxContainer/MoneyAmountImageContainer");

        _invUsage = GetNode<PropertyUsageHGUI>("InGameGUIControl/VBoxContainer/InventoryPropertyUsageHImageGUI");
        _invShopUsage = GetNode<PropertyUsageHGUI>("ShopGUIControl/VBoxContainer/InventoryPropertyUsageHImageGUI");

        _ItemContainer = GetNode<ItemContainer>("ShopGUIControl/ItemContainer");

        _bigScoreProp = GetNode<BigPropertyAmount>("GameOverGUIControl/CenterContainer/VBoxContainer/BigScoreAmount");
        _inGameScoreProp = GetNode<PropertyAmountH>("InGameGUIControl/PropertyAmountH");

        _finishrdGameScoreProp = GetNode<PropertyAmountH>("GameFinishedGUIControl/CenterContainer/VBoxContainer/PropertyAmountH");
        _finishrdGameTimeOfAttempt = GetNode<PropertyAmountH>("GameFinishedGUIControl/CenterContainer/VBoxContainer/PropertyAmountH2");
        _finishrdGameNumberOfAttempts = GetNode<PropertyAmountH>("GameFinishedGUIControl/CenterContainer/VBoxContainer/PropertyAmountH3");

        _toolBar = GetNode<ToolBar>("InGameGUIControl/ToolBar");

        _chanScreenCover = GetNode<ScreenCover>("Cover/ChangeScreenCover");
        _startScreenCover = GetNode<StartScreenCover>("Cover/StartScreenCover");

        //music
        _musicManager = GetNode<MusicManager>("MusicManager");
        _mainMenuAPlayer = GetNode<AudioStreamPlayer>("MainMenuGUIControl/MainMenuSndPlayer");
        _gameOverAPlayer =  GetNode<AudioStreamPlayer>("GameOverGUIControl/GameOverSndPlayer");
        _inGameAPlayer = GetNode<AudioStreamPlayer>("InGameGUIControl/InGameSndPlayer");
        _gameFinishedAPlayer = GetNode<AudioStreamPlayer>("GameFinishedGUIControl/GameFinishedSndPlayer");
        _musicManager.ActualForeground = _mainMenuAPlayer;
        _musicManager.PlayActual();

        _gLockContainer = GetNode<GameplayLockConainer>("ShopGUIControl/GameplayLockContainer");

        _creditsLabel = GetNode<PropertyAmountH>("MainMenuGUIControl/PropertyAmountH");
        _versionLabel = GetNode<PropertyAmountH>("MainMenuGUIControl/PropertyAmountH2");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        //Double restart fix
        if (Input.IsActionJustReleased ("Game_Restart") || Input.IsActionJustReleased("Game_Back") || _bButtonReleased)
        {
            _actionJustPressed = false;
            _bButtonReleased = false;
        }
    }

    private string SecToDHMS(UInt64 timeInSec)
    {
        UInt64 tInSec = timeInSec;

        UInt64 days = 0;
        UInt64 hours = 0;
        UInt64 mins = 0;

        UInt64 dayInseconds = 60 * 60 * 24;
        UInt64 hoursInseconds = 60 * 60;
        UInt64 minsInseconds = 60;

        string result = "";

        if (tInSec >= dayInseconds)
        {
            days = tInSec / dayInseconds;
            tInSec = tInSec % dayInseconds;

            result += Convert.ToString(days) + "d";
        }

        if (tInSec >= hoursInseconds)
        {
            hours = tInSec / hoursInseconds;
            tInSec = tInSec % hoursInseconds;

            result += Convert.ToString(hours) + "h";
        }

        if (tInSec >= minsInseconds)
        {
            mins = tInSec / minsInseconds;
            tInSec = tInSec % minsInseconds;

            result += Convert.ToString(mins) + "m";
        }

        result += Convert.ToString(tInSec) + "s";

        return result;
    }

}

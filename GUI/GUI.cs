using Godot;
using System;

public class GUI : CanvasLayer
{
    [Signal]
    public delegate void SLoadlevel(string resPath);

    [Signal]
    public delegate void SUnloadLevel();

    [Signal]
    public delegate void SRestartLevel();

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
    private bool _bMuted = false;

    private string _previusGUI = "MainMenuGUI";
    private string _levelResPath = "res://Levels/Level.tscn";

    private bool _bAllowedInput = false; // screen cover input fix
    //double restart fix
    private bool _actionJustPressed = false;
    private bool _bButtonReleased = false;

    private Player _player;
    private Shop _shop;
    private Level _level;
    private GameplayUnlocker _gUnlocker;

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
    private Notification _gameOverImprovementNotify;
    private Notification _mainMenuImprovementNotify;
    private ItemAmountImageContainer _moneyAmount;
    private ItemAmountImageContainer _moneyShopAmount;
    private PropertyUsageHGUI _invUsage;
    private PropertyUsageHGUI _invShopUsage;
    private BuySellItemGUI _buySellTDGui;
    private BigPropertyAmount _bigScoreProp;
    private PropertyAmountH _inGameScoreProp;
    private PropertyAmountH _finishrdGameScoreProp;
    private PropertyAmountH _finishrdGameTimeOfAttempt;
    private PropertyAmountH _finishrdGameNumberOfAttempts;
    private GameplayLockConainer _gLockContainer;
    private ToolBar _toolBar;
    private ScreenCover _chanScreenCover;
    private StartScreenCover _startScreenCover;


    public async void OnPlayButtonDown()
    {
        if (_bAllowedInput)
        {
            _chanScreenCover.PlayHideScreenAnim();
            await ToSignal(GetTree().CreateTimer(_chanScreenCover.AnimLength + 0.1f), "timeout");

            EmitSignal(nameof(SLoadlevel), _levelResPath);
            EmitSignal(nameof(SRestartLevel));

            ChangeGUI("InGameGUI");
        }
    }

    public void OnQuitButtonDown()
    {
        GetTree().Quit();
    }

    public void OnRestartButtonDown()
    {
        if (_bAllowedInput)
        {
            //Double restart fix
            if (!_actionJustPressed)
            {
                EmitSignal(nameof(SRestartLevel));

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
        _chanScreenCover.PlayHideScreenAnim();
        await ToSignal(GetTree().CreateTimer(_chanScreenCover.AnimLength), "timeout");

        EmitSignal(nameof(SUnloadLevel));
        EmitSignal(nameof(SQuitLevel));

        ChangeGUI("MainMenuGUI");
    }

    public void OnBackButtonDown()
    {
        ChangeGUI(_previusGUI);
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

    public void OnBuySellItemGUISRequestBuyItem(string itemKey)
    {
        EmitSignal(nameof(SBuyItem), _player, itemKey, 1);
    }

    public void OnBuySellItemGUISRequestSellItem(string itemKey)
    {
        EmitSignal(nameof(SSellItem), _player, itemKey, 1);
    }

    public void OnMuteButtonTextButtonDown()
    {
        _muteButton.SwapTexture(_bMuted);

        SetMuted(!_bMuted);
    }

    //temporary label changing
    public void OnShopSItemBought(string itemKey)
    {
        if (itemKey == "TimeDilation")
        {
            _buySellTDGui.UpdateAmount(_player);
        }

        _moneyShopAmount.UpdateAmount(_player);
        _moneyShopAmount.PlayUpdateAmountAnim();
        //update inventory usage
        _invShopUsage.UpdateAmount(_player.Inv.UsageProp);
        _invShopUsage.PlayUpdateAmountAnim();
    }
    //temporary label changing
    public void OnShopSItemSold(string itemKey)
    {
        if (itemKey == "TimeDilation")
        {
            _buySellTDGui.UpdateAmount(_player);
        }

        _moneyShopAmount.UpdateAmount(_player);
        _moneyShopAmount.PlayUpdateAmountAnim();
        //update inventory usage
        _invShopUsage.UpdateAmount(_player.Inv.UsageProp);
        _invShopUsage.PlayUpdateAmountAnim();
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
        _bAllowedInput = false;
    }

    public void OnScreenCoverCoveringFinished()
    {
        _bAllowedInput = true;
    }

    public async void OnMainFinalReady()
    {
        await ToSignal(GetTree().CreateTimer(1.5f), "timeout");

        //AnimationPlayer animP = GetNode<AnimationPlayer>("Cover/BeginScreenCover/ColorRect/CenterContainer/AnimationPlayer");
        //animP.Play("Blink_anim");

        _startScreenCover.PlayBlinkAnim();

        await ToSignal(GetTree().CreateTimer(5.0f), "timeout");

        _startScreenCover.PlayShowScreenAnim();

        await ToSignal(GetTree().CreateTimer(0.25f), "timeout");

        _mainScreenCaptAnimPlayer.Play("ShowUp_anim", -1, 0.9f);
        _mainMenuV.PlayShowUpAnim();

        _muteButton.PlayShowUpAnim();

        //// LEVELDASIGN SHORTCUT
        //_startScreenCover.Hide();
        //OnPlayButtonDown();
    }

    public void ChangeGUI(string nameOfGUI)
    {
        if (nameOfGUI != ActualGUI)
        {
            switch (ActualGUI)
            {
                case "GameOverGUI":
                    if (!_bMuted)
                        GetNode<AudioStreamPlayer>("GameOverGUIControl/GameOverSndPlayer").Stop();

                    GetNode<Control>("GameOverGUIControl").Hide();

                    //Double restart fix
                    _bButtonReleased = true;

                    break;

                case "InGameGUI":
                    if (!_bMuted)
                        GetNode<AudioStreamPlayer>("InGameGUIControl/InGameSndPlayer").Stop();

                    GetNode<Control>("InGameGUIControl").Hide();
                    //animation
                    _inGameScoreProp.ResetAnim();
                    _inGameMenuV.ResetAnim();
                    _toolBar.ResetAnim();
                    break;

                case "MainMenuGUI":
                    if (!_bMuted)
                        GetNode<AudioStreamPlayer>("MainMenuGUIControl/MainMenuSndPlayer").Stop();

                    GetNode<Control>("MainMenuGUIControl").Hide();
                    break;

                case "ShopGUI":
                    if (!_bMuted)
                        GetNode<AudioStreamPlayer>("ShopGUIControl/ShopSndPlayer").Stop();

                    GetNode<Control>("ShopGUIControl").Hide();
                    break;

                case "GameFinishedGUI":
                    if (!_bMuted)
                        GetNode<AudioStreamPlayer>("GameFinishedGUIControl/GameFinishedSndPlayer").Stop();

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
                    if (!_bMuted)
                        GetNode<AudioStreamPlayer>("GameOverGUIControl/GameOverSndPlayer").Play();

                    _bigScoreProp.HideNewRecord();
                    //animation
                    _gameOverMenuV.ResetAnim();
                    _bigScoreProp.ResetAnim();
                    GD.Print("animatoin reset");

                    GetNode<Control>("GameOverGUIControl").Show();

                    //Double restart fix
                    _actionJustPressed = false;

                    //show and hide notifications
                    if (_gUnlocker.IsNewGLockAvailable())
                    {
                        _gameOverImprovementNotify.UnSatisfy();
                        _mainMenuImprovementNotify.UnSatisfy();
                    }
                    else
                    {
                        _gameOverImprovementNotify.Satisfy();
                        _mainMenuImprovementNotify.Satisfy();
                    }

                    //animation
                    _gameOverScreenCaptAnimPlayer.Play("ShowUp_anim", -1, 0.8f);
                    _gameOverMenuV.PlayShowUpAnim();
                    _bigScoreProp.PlayShowUpAnim();
                    _bigScoreProp.PlayNewRecAnim();

                    break;

                case "InGameGUI":
                    if (!_bMuted)
                        GetNode<AudioStreamPlayer>("InGameGUIControl/InGameSndPlayer").Play();

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
                    if (!_bMuted)
                        GetNode<AudioStreamPlayer>("MainMenuGUIControl/MainMenuSndPlayer").Play();

                    GetNode<Control>("MainMenuGUIControl").Show();

                    //show and hide notifications
                    if (_gUnlocker.IsNewGLockAvailable())
                    {
                        _gameOverImprovementNotify.UnSatisfy();
                        _mainMenuImprovementNotify.UnSatisfy();
                    }
                    else
                    {
                        _gameOverImprovementNotify.Satisfy();
                        _mainMenuImprovementNotify.Satisfy();
                    }

                    break;

                case "ShopGUI":
                    if (!_bMuted)
                        GetNode<AudioStreamPlayer>("ShopGUIControl/ShopSndPlayer").Play();

                    GetNode<Control>("ShopGUIControl").Show();

                    EmitSignal(nameof(SUpdateShopPrizes), _shop);

                    //update gLockContainer of improvements
                    _gLockContainer.UpdateAvailableGLocksVisibility(_gUnlocker.GetDictOfLocksAvilability());

                    _buySellTDGui.UpdateAmount(_player);
                    //update ussage of inventory
                    _invShopUsage.UpdateMaximum(_player.Inv.UsageProp);
                    _invShopUsage.UpdateAmount(_player.Inv.UsageProp);
                    break;

                case "GameFinishedGUI":
                    if (!_bMuted)
                        GetNode<AudioStreamPlayer>("GameFinishedGUIControl/GameFinishedSndPlayer").Play();

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

        _moneyAmount = GetNode<ItemAmountImageContainer>("InGameGUIControl/VBoxContainer/MoneyAmountImageContainer");
        _moneyShopAmount = GetNode<ItemAmountImageContainer>("ShopGUIControl/VBoxContainer/MoneyAmountImageContainer");

        _invUsage = GetNode<PropertyUsageHGUI>("InGameGUIControl/VBoxContainer/InventoryPropertyUsageHImageGUI");
        _invShopUsage = GetNode<PropertyUsageHGUI>("ShopGUIControl/VBoxContainer/InventoryPropertyUsageHImageGUI");

        _buySellTDGui = GetNode<BuySellItemGUI>("ShopGUIControl/HBoxContainer/CenterContainer/BuySellTimeDilationGUI");

        _bigScoreProp = GetNode<BigPropertyAmount>("GameOverGUIControl/CenterContainer/VBoxContainer/BigScoreAmount");
        _inGameScoreProp = GetNode<PropertyAmountH>("InGameGUIControl/PropertyAmountH");

        _finishrdGameScoreProp = GetNode<PropertyAmountH>("GameFinishedGUIControl/CenterContainer/VBoxContainer/PropertyAmountH");
        _finishrdGameTimeOfAttempt = GetNode<PropertyAmountH>("GameFinishedGUIControl/CenterContainer/VBoxContainer/PropertyAmountH2");
        _finishrdGameNumberOfAttempts = GetNode<PropertyAmountH>("GameFinishedGUIControl/CenterContainer/VBoxContainer/PropertyAmountH3");

        _toolBar = GetNode<ToolBar>("InGameGUIControl/ToolBar");

        _chanScreenCover = GetNode<ScreenCover>("Cover/ChangeScreenCover");
        _startScreenCover = GetNode<StartScreenCover>("Cover/StartScreenCover");

        _gLockContainer = GetNode<GameplayLockConainer>("ShopGUIControl/GameplayLockContainer");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        //Double restart fix
        if (Input.IsActionJustReleased ("Game_Restart") || _bButtonReleased)
        {
            _actionJustPressed = false;
            _bButtonReleased = false;
        }
    }

    private void SetMuted(bool bMuted)
    {
        if (_bMuted != bMuted)
        {
            _bMuted = bMuted;
            if (_bMuted)
            {
                switch (ActualGUI)
                {
                    case "GameOverGUI":
                        GetNode<AudioStreamPlayer>("GameOverGUIControl/GameOverSndPlayer").Stop();
                        break;

                    case "InGameGUI":
                        GetNode<AudioStreamPlayer>("InGameGUIControl/InGameSndPlayer").Stop();
                        break;

                    case "MainMenuGUI":
                        GetNode<AudioStreamPlayer>("MainMenuGUIControl/MainMenuSndPlayer").Stop();
                        break;

                    case "ShopGUI":
                        GetNode<AudioStreamPlayer>("ShopGUIControl/ShopSndPlayer").Stop();
                        break;

                    case "GameFinishedGUI":
                        GetNode<AudioStreamPlayer>("GameFinishedGUIControl/GameFinishedSndPlayer").Stop();
                        break;
                }
            }
            else
            {
                switch (ActualGUI)
                {
                    case "GameOverGUI":
                        GetNode<AudioStreamPlayer>("GameOverGUIControl/GameOverSndPlayer").Play();
                        break;

                    case "InGameGUI":
                        GetNode<AudioStreamPlayer>("InGameGUIControl/InGameSndPlayer").Play();
                        break;

                    case "MainMenuGUI":
                        GetNode<AudioStreamPlayer>("MainMenuGUIControl/MainMenuSndPlayer").Play();
                        break;

                    case "ShopGUI":
                        GetNode<AudioStreamPlayer>("ShopGUIControl/ShopSndPlayer").Play();
                        break;

                    case "GameFinishedGUI":
                        GetNode<AudioStreamPlayer>("GameFinishedGUIControl/GameFinishedSndPlayer").Play();
                        break;
                }
            }
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

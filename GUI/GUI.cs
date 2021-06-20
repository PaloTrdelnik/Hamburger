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
    public delegate void SBuyItem(Player player, string itemKeyToBuy, int amount);

    [Signal]
    public delegate void SSellItem(Player player, string itemKeyToSell, int amount);

    [Signal]
    public delegate void SUpdateShopPrizes(Shop shop);

    public string ActualGUI = "MainMenuGUI";

    private string _levelResPath = "res://Levels/Level.tscn";

    private Player _player;
    private Shop _shop;

    private AnimationPlayer _mainScreenCaptAnimPlayer;
    private AnimationPlayer _gameOverScreenCaptAnimPlayer;
    private MenuV _mainMenuV;
    private MenuV _gameOverMenuV;
    private MenuV _inGameMenuV;
    private ItemAmountImageContainer _moneyAmount;
    private ItemAmountImageContainer _moneyShopAmount;
    private PropertyUsageHGUI _invUsage;
    private PropertyUsageHGUI _invShopUsage;
    private BuySellItemGUI _buySellTDGui;
    private BigPropertyAmount _bigScoreProp;
    private PropertyAmountH _inGamescoreProp;
    private ToolBar _toolBar;
    private ScreenCover _chanScreenCover;
    private StartScreenCover _startScreenCover;


    public async void OnPlayButtonDown()
    {
        _chanScreenCover.PlayHideScreenAnim();
        await ToSignal(GetTree().CreateTimer(_chanScreenCover.AnimLength + 0.1f), "timeout");

        EmitSignal(nameof(SLoadlevel), _levelResPath);
        EmitSignal(nameof(SRestartLevel));

        ChangeGUI("InGameGUI");
    }

    public void OnQuitButtonDown()
    {
        GetTree().Quit();
    }

    public void OnRestartButtonDown()
    {
        EmitSignal(nameof(SRestartLevel));

        ChangeGUI("InGameGUI");
    }

    public async void OnMainMenuButtonDown()
    {
        _chanScreenCover.PlayHideScreenAnim();
        await ToSignal(GetTree().CreateTimer(_chanScreenCover.AnimLength), "timeout");

        EmitSignal(nameof(SUnloadLevel));
        EmitSignal(nameof(SQuitLevel));

        ChangeGUI("MainMenuGUI");
    }

    public void OnMainMenuButtonDownWithoutUnload()
    {
        ChangeGUI("MainMenuGUI");
    }

    public void OnShopButtonDown()
    {
        _moneyShopAmount.UpdateAmount(_player);
        ChangeGUI("ShopGUI");
    }

    public void OnBuySellItemGUISRequestBuyItem(string itemKey)
    {
        EmitSignal(nameof(SBuyItem), _player, itemKey, 1);
    }
    public void OnBuySellItemGUISRequestSellItem(string itemKey)
    {
        EmitSignal(nameof(SSellItem), _player, itemKey, 1);
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
        level.Connect("SPlayerGO", this, nameof(OnLevelSPlayerGO));
        level.Connect("SPlayerUpdateScore", this, nameof(OnLevelSPlayerUpdateScore));

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

    public void OnLevelSPlayerUpdateScore(Player player)
    {
        _inGamescoreProp.UpdateAmount(player.ScoreProp.GetAmount());
        //chesk if anim is playing to solve transparency issues caused by not finished ShowUp_anim
        if(!_inGamescoreProp.AnimPlayer.IsPlaying())
        {
            _inGamescoreProp.PlayUpdateAmountAnim();
        }
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
    }

    public void ChangeGUI(string nameOfGUI)
    {
        if (nameOfGUI != ActualGUI)
        {
            switch (ActualGUI)
            {
                case "GameOverGUI":
                    GetNode<AudioStreamPlayer>("GameOverGUIControl/GameOverSndPlayer").Stop();
                    GetNode<Control>("GameOverGUIControl").Hide();

                    _bigScoreProp.HideNewRecord();
                    //animation
                    _gameOverMenuV.ResetAnim();
                    _bigScoreProp.ResetAnim();
                    break;

                case "InGameGUI":
                    GetNode<AudioStreamPlayer>("InGameGUIControl/InGameSndPlayer").Stop();
                    GetNode<Control>("InGameGUIControl").Hide();
                    //animation
                    _inGamescoreProp.ResetAnim();
                    _inGameMenuV.ResetAnim();
                    _toolBar.ResetAnim();
                    break;

                case "MainMenuGUI":
                    GetNode<AudioStreamPlayer>("MainMenuGUIControl/MainMenuSndPlayer").Stop();
                    GetNode<Control>("MainMenuGUIControl").Hide();
                    break;

                case "ShopGUI":
                    GetNode<AudioStreamPlayer>("ShopGUIControl/ShopSndPlayer").Stop();
                    GetNode<Control>("ShopGUIControl").Hide();
                    break;
            }
            
            switch (nameOfGUI)
            {
                case "GameOverGUI":
                    GetNode<AudioStreamPlayer>("GameOverGUIControl/GameOverSndPlayer").Play();
                    GetNode<Control>("GameOverGUIControl").Show();
                    //animation
                    _gameOverScreenCaptAnimPlayer.Play("ShowUp_anim", -1, 0.8f);
                    _gameOverMenuV.PlayShowUpAnim();
                    _bigScoreProp.PlayShowUpAnim();
                    _bigScoreProp.PlayNewRecAnim();

                    break;

                case "InGameGUI":
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
                    _inGamescoreProp.PlayShowUpAnim();
                    _inGameMenuV.PlayShowUpAnim();
                    break;

                case "MainMenuGUI":
                    GetNode<AudioStreamPlayer>("MainMenuGUIControl/MainMenuSndPlayer").Play();
                    GetNode<Control>("MainMenuGUIControl").Show();
                    break;

                case "ShopGUI":
                    GetNode<AudioStreamPlayer>("ShopGUIControl/ShopSndPlayer").Play();
                    GetNode<Control>("ShopGUIControl").Show();

                    EmitSignal(nameof(SUpdateShopPrizes), _shop);

                    _buySellTDGui.UpdateAmount(_player);
                    //update ussage of inventory
                    _invShopUsage.UpdateMaximum(_player.Inv.UsageProp);
                    _invShopUsage.UpdateAmount(_player.Inv.UsageProp);
                    break;
            }

            ActualGUI = nameOfGUI;
        }
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _player = GetParent().GetNode<Player>("Player");
        _shop = GetParent().GetNode<Shop>("Shop");

        _mainScreenCaptAnimPlayer = GetNode<AnimationPlayer>("MainMenuGUIControl/CenterContainer/Caption/AnimationPlayer");
        _gameOverScreenCaptAnimPlayer = GetNode<AnimationPlayer>("GameOverGUIControl/CenterContainer/VBoxContainer/Caption/AnimationPlayer");

        _mainMenuV = GetNode<MenuV>("MainMenuGUIControl/MenuV");
        _gameOverMenuV = GetNode<MenuV>("GameOverGUIControl/CenterContainer/VBoxContainer/MenuV");
        _inGameMenuV = GetNode<MenuV>("InGameGUIControl/MenuV");

        _moneyAmount = GetNode<ItemAmountImageContainer>("InGameGUIControl/VBoxContainer/MoneyAmountImageContainer");
        _moneyShopAmount = GetNode<ItemAmountImageContainer>("ShopGUIControl/VBoxContainer/MoneyAmountImageContainer");

        _invUsage = GetNode<PropertyUsageHGUI>("InGameGUIControl/VBoxContainer/InventoryPropertyUsageHImageGUI");
        _invShopUsage = GetNode<PropertyUsageHGUI>("ShopGUIControl/VBoxContainer/InventoryPropertyUsageHImageGUI");

        _buySellTDGui = GetNode<BuySellItemGUI>("ShopGUIControl/HBoxContainer/CenterContainer/BuySellTimeDilationGUI");

        _bigScoreProp = GetNode<BigPropertyAmount>("GameOverGUIControl/CenterContainer/VBoxContainer/BigScoreAmount");
        _inGamescoreProp = GetNode<PropertyAmountH>("InGameGUIControl/PropertyAmountH");

        _toolBar = GetNode<ToolBar>("InGameGUIControl/ToolBar");

        _chanScreenCover = GetNode<ScreenCover>("Cover/ChangeScreenCover");
        _startScreenCover = GetNode<StartScreenCover>("Cover/StartScreenCover");
    }

    //// Called every frame. 'delta' is the elapsed time since the previous frame.
    //public override void _Process(float delta)
    //{
    //}


    private void HandleInput()
    {

    }
}

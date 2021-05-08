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
    public delegate void SBuyItem(Player player, string itemKey);

    [Signal]
    public delegate void SSellItem(Player player, string itemKey);

    [Signal]
    public delegate void SUpdateShopPrizes(Shop shop);

    public string ActualGUI = "MainMenuGUI";

    private string _levelResPath = "res://Levels/Level.tscn";

    private Player _player;
    private Shop _shop;

    private ItemAmountGUI _moneyAmount;
    private ItemAmountGUI _moneyShopAmount;
    private Taskbar _taskBar;

    public void OnPlayButtonDown()
    {
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

    public void OnMainMenuButtonDown()
    {
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
        EmitSignal(nameof(SBuyItem), _player, itemKey);
    }
    public void OnBuySellItemGUISRequestSellItem(string itemKey)
    {
        EmitSignal(nameof(SSellItem), _player, itemKey);
    }
    //temporary label changing
    public void OnShopSItemBought(string itemKey)
    {
        if (itemKey == "TimeDilation")
        {
            BuySellItemGUI buySellTDGui = GetNode<BuySellItemGUI>("ShopGUIControl/HBoxContainer/CenterContainer/BuySellTimeDilationGUI");

            buySellTDGui.UpdateAmount(_player);
        }

        _moneyAmount.UpdateAmount(_player);
        _moneyShopAmount.UpdateAmount(_player);
    }
    //temporary label changing
    public void OnShopSItemSold(string itemKey)
    {
        if (itemKey == "TimeDilation")
        {
            BuySellItemGUI buySellTDGui = GetNode<BuySellItemGUI>("ShopGUIControl/HBoxContainer/CenterContainer/BuySellTimeDilationGUI");

            buySellTDGui.UpdateAmount(_player);
        }

        _moneyAmount.UpdateAmount(_player);
        _moneyShopAmount.UpdateAmount(_player);
    }

    public void OnMainSLevelLoaded(Node level)
    {
        level.Connect("SPlayerGO", this, nameof(OnLevelSPlayerGO));
     
        //player in level
        //level.Connect("SPlayerInvItemChange", this, nameof(OnLevelSPlayerInvItemChange));
    }

    public void OnPlayerSInvItemChange(Player player, string itemInvKey)
    { 
        if (itemInvKey == "Money")
        {
            _moneyAmount.UpdateAmount(_player);
        }

        if (itemInvKey == "TimeDilation")
        {
            GD.Print("GUI");
            _taskBar.UpdateAmount(player, itemInvKey);
        }
    }

    public void OnPlayerSItemDurabilityUpdateBegin(string itemKey)
    {
        _taskBar.GetNode<TaskBarItem>("TaskBar" + itemKey).StartUpdateUseDurabilityBar(_player);
    }

    public void OnPlayerSItemDurabilityUpdateEnd(string itemKey)
    {
        _taskBar.GetNode<TaskBarItem>("TaskBar" + itemKey).StopUpdateUseDurabilityBar();
    }

    public void OnLevelSPlayerGO(Player player)
    { 
        ChangeGUI("GameOverGUI");
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
                    break;

                case "InGameGUI":
                    GetNode<AudioStreamPlayer>("InGameGUIControl/InGameSndPlayer").Stop();
                    GetNode<Control>("InGameGUIControl").Hide();
                    break;

                case "MainMenuGUI":
                    GD.Print("ad");
                    GetNode<AudioStreamPlayer>("MainMenuGUIControl/MainMenuSndPlayer").Stop();
                    GetNode<Control>("MainMenuGUIControl").Hide();
                    break;

                case "ShopGUI":
                    GD.Print("ad");
                    GetNode<AudioStreamPlayer>("ShopGUIControl/ShopSndPlayer").Stop();
                    GetNode<Control>("ShopGUIControl").Hide();
                    break;
            }
            
            switch (nameOfGUI)
            {
                case "GameOverGUI":
                    GetNode<AudioStreamPlayer>("GameOverGUIControl/GameOverSndPlayer").Play();
                    GetNode<Control>("GameOverGUIControl").Show();
                    break;

                case "InGameGUI":
                    GetNode<AudioStreamPlayer>("InGameGUIControl/InGameSndPlayer").Play();
                    GetNode<Control>("InGameGUIControl").Show();
                    _taskBar.UpdateAmountOfAllitems(_player);
                    break;

                case "MainMenuGUI":
                    GetNode<AudioStreamPlayer>("MainMenuGUIControl/MainMenuSndPlayer").Play();
                    GetNode<Control>("MainMenuGUIControl").Show();
                    break;

                case "ShopGUI":
                    GetNode<AudioStreamPlayer>("ShopGUIControl/ShopSndPlayer").Play();
                    GetNode<Control>("ShopGUIControl").Show();
                    EmitSignal(nameof(SUpdateShopPrizes), _shop);
                    BuySellItemGUI buySellTDGui = GetNode<BuySellItemGUI>("ShopGUIControl/HBoxContainer/CenterContainer/BuySellTimeDilationGUI");
                    buySellTDGui.UpdateAmount(_player);
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

        _moneyAmount = GetNode<ItemAmountGUI>("InGameGUIControl/MoneyAmountContainer");
        _moneyShopAmount = GetNode<ItemAmountGUI>("ShopGUIControl/MoneyAmountContainer");

        _taskBar = GetNode<Taskbar>("InGameGUIControl/Taskbar");
    }

    //// Called every frame. 'delta' is the elapsed time since the previous frame.
    //public override void _Process(float delta)
    //{
    //}


    private void HandleInput()
    {

    }
}

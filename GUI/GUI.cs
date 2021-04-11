using Godot;
using System;

public class GUI : CanvasLayer
{
    [Signal]
    public delegate void SLoadlevel(string resPath);

    public string ActualGUI = "MainMenuGUI";

    private string _levelResPath = "res://Levels/Level.tscn";
    private Label _moneyValLab;

    public void OnPlayButtonDown()
    {
        EmitSignal(nameof(SLoadlevel), _levelResPath);
        ChangeGUI("InGameGUI");
    }

    public void OnQuitButtonDown()
    {
        GetTree().Quit();
    }

    public void OnMainSLevelLoaded(Node level)
    {
        level.Connect("SPlayerGO", this, nameof(OnLevelSPlayerGO));
        level.Connect("SPlayerInvItemChange", this, nameof(OnLevelSPlayerInvItemChange));
    }

    public void OnLevelSPlayerInvItemChange(Player player, string itemInvKey)
    { 
        if (itemInvKey == "Money")
        {
            Item MoneyInv = (Item)player.Inv.GetItem(itemInvKey);

            _moneyValLab.Text = Convert.ToString(MoneyInv.Amount);
            _moneyValLab.Update();
        }
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
                    break;

                case "MainMenuGUI":
                    GetNode<AudioStreamPlayer>("MainMenuGUIControl/MainMenuSndPlayer").Play();
                    GetNode<Control>("MainMenuGUIControl").Show();
                    break;
            }

            ActualGUI = nameOfGUI;
        }

            

    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _moneyValLab = GetNode<Label>("InGameGUIControl/HSplitContainer/Value");
    }

    //// Called every frame. 'delta' is the elapsed time since the previous frame.
    //public override void _Process(float delta)
    //{
    //}
}

using Godot;
using System;


public class Level : Node2D
{
    [Signal]
    public delegate void SPlayerGO(Player player);

    //player in level
    //[Signal]
    //public delegate void SPlayerInvItemChange(Player player, string itemInvKey);

    [Signal]
    public delegate void SSlowDownTimeBegin(float amount, float length);

    public float TimeSpeed = 1.0f;

    private float _timeSpeedUp = 0.0f;
    private float _timeSlowDownDifference = 0.0f;

    private Player _player;

    public void OnGUISRestartLevel()
    {
        //reset acid scale
        Vector2 higherScale = new Vector2(1, 1);
        GetNode<Area2D>("Acid").Scale = higherScale;

        //reset time
        TimeSpeed = 1.0f;
        _timeSpeedUp = 0.0f;
        _timeSlowDownDifference = 0.0f;

        //player in level
        //Player player = GetNode<Player>("Player");
        //Reset player position
        _player.Position = GetNode<Position2D>("PlayerStart").Position;
        //unfreeze the player
        _player.bFreeze = false;
        //reset itemuse
        _player.GetNode<ItemUser>("ItemUser").ResetItemUse();
    }
    public void OnGUISQuitLevel()
    {
        _player.bFreeze = true;
    }

    //player in level
    //public void OnPlayerSInvItemChange(Player player, Item item)
    //{
    //	EmitSignal(nameof(SPlayerInvItemChange), player, item.InvKey);
    //}

    public void OnAcidBodyEntered(Node body)
    {
        if (body is Player)
        {
            Player player = (Player)body;

            EmitSignal(nameof(SPlayerGO), body);
            player.bFreeze = true;
        }
    }

    private void OnSlowDownTime(int amount, int length)
    {
        TimeSpeed /= amount;
        _timeSlowDownDifference = 1.0f - TimeSpeed;
        _timeSpeedUp = 1.0f / (length / 1000);
    }

    public override void _Ready()
    {
        //get player reference
        _player = GetParent().GetNode<Player>("Player");
        //connect the signals from player
        _player.Connect("SSlowDownTime", this, nameof(OnSlowDownTime));
        //connect the signals from GUI
        GUI gui = GetParent().GetNode<GUI>("GUI");

        //connect signals from GUI
        gui.Connect("SRestartLevel", this, nameof(OnGUISRestartLevel));
        gui.Connect("SQuitLevel", this, nameof(OnGUISQuitLevel));
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (TimeSpeed < 1.0f)
        {
            ReturnTimeToNormal(delta);
        }
    }

    private void ReturnTimeToNormal(float delta)
    {
        TimeSpeed += _timeSpeedUp * delta * _timeSlowDownDifference;
    }

    //private float Lerp(float firstFloat, float secondFloat, float by)
    //{
    //    return firstFloat * (1 - by) + secondFloat * by;
    //}
}

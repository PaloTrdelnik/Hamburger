using Godot;
using System;


public class Level : Node2D
{
    [Signal]
    public delegate void SPlayerGO(Player player);

    [Signal]
    public delegate void SPlayerUpdateScore(Player player);

    //player in level
    //[Signal]
    //public delegate void SPlayerInvItemChange(Player player, string itemInvKey);

    [Signal]
    public delegate void SSlowDownTimeBegin(float amount, float length);

    public float TimeSpeed = 1.0f;
    public float Difficulty = 5.0f;

    private float _timeSpeedUp = 0.0f;
    private float _timeSlowDownDifference = 0.0f;

    private Player _player;

    private MoveIntervalChecker _diffInterChecker;
    private MoveIntervalChecker _scoreInterChecker;

    private ItemSpawner2D _moneySpawner;
    private ItemSpawner2D _timeDilSpawner;

    private Acid _acid;

    public void OnGUISRestartLevel()
    {
        //reset acid scale
        _acid.ResetAcid();

        //reset time
        TimeSpeed = 1.0f;
        _timeSpeedUp = 0.0f;
        _timeSlowDownDifference = 0.0f;

        //reset difficulty
        Difficulty = 5.0f;
        _diffInterChecker.ResetIntervals();

        //player in level
        //Player player = GetNode<Player>("Player");
        //Reset player position
        _player.Position = GetNode<Position2D>("PlayerStart").Position;
        //unfreeze the player
        _player.bFreeze = false;
        //reset itemuse
        _player.GetNode<ItemUser>("ItemUser").ResetItemUse();
        //reset score
        _player.ScoreProp.SetAmount(0);
        _scoreInterChecker.ResetIntervals();
        EmitSignal(nameof(SPlayerUpdateScore), _player);

        //randomize item spawn
        _moneySpawner.RemoveItems();
        _moneySpawner.RandomizeSpawnLevel(0, 2);

        _timeDilSpawner.RemoveItems();
        _timeDilSpawner.RandomizeSpawnLevel(0, 2);
        //spawn items
        _moneySpawner.SpawnItemByLevel();
        _timeDilSpawner.SpawnItemByLevel();
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

    public void OnScoreIntervalCheckerSIntervalReached()
    {
        _player.ScoreProp += 1;
        EmitSignal(nameof(SPlayerUpdateScore), _player);
        GD.Print("Score Increased");
    }

    public void OnMoveIntervalCheckerSIntervalReached()
    {
        Difficulty += 10.0f;
        GD.Print("Difficulty increased");
    }

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
        //get ref for acid
        _acid = GetNode<Acid>("Acid");
        //get player reference
        _player = GetParent().GetNode<Player>("Player");
        //connect the signals from player
        _player.Connect("SSlowDownTime", this, nameof(OnSlowDownTime));
        //connect the signals from GUI
        GUI gui = GetParent().GetNode<GUI>("GUI");
        //setup score interval
        _scoreInterChecker = GetNode<MoveIntervalChecker>("ScoreIntervalChecker");
        _scoreInterChecker.InstanceToCheck = _player;

        //connect signals from GUI
        gui.Connect("SRestartLevel", this, nameof(OnGUISRestartLevel));
        gui.Connect("SQuitLevel", this, nameof(OnGUISQuitLevel));

        //setup difficulty Interval
        _diffInterChecker = GetNode<MoveIntervalChecker>("DifficultyIntervalChecker");
        _diffInterChecker.InstanceToCheck = _player;

        //find refs for itm spawn
        _moneySpawner = GetNode<ItemSpawner2D>("MoneySpawner2D");
        _timeDilSpawner = GetNode<ItemSpawner2D>("TimeDilationSpawner2D");

        //randomize item spawn
        _moneySpawner.RandomizeSpawnPointsLevels(0, 2);
        _timeDilSpawner.RandomizeSpawnPointsLevels(0, 2);
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

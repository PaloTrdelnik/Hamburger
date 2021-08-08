using Godot;
using System;


public class Level : Node2D
{
    [Signal]
    public delegate void SPlayerGO(Player player);

    [Signal]
    public delegate void SPlayerUpdateScore(Player player);

    [Signal]
    public delegate void SGameFinished();

    [Signal]
    public delegate void SRespawnItems();

    //player in level
    //[Signal]
    //public delegate void SPlayerInvItemChange(Player player, string itemInvKey);

    [Signal]
    public delegate void SSlowDownTimeBegin(float amount, float length);

    //stats
    public int NumerOfAttempts = 0;
    public UInt64 StartTimeOfAttempt = 0;
    public UInt64 TimeOfAttempt = 0;

    public float TimeSpeed = 1.0f;
    public float Difficulty = 5.0f;
    public float DifficultyStep = 5.0f; // this variable can be changed by PreloadedGameplay
    public bool bFirstPlay = true; // this variable can be changed by PreloadedGameplay


    private float _timeSpeedUp = 0.0f;
    private float _timeSlowDownDifference = 0.0f;

    private Player _player;
    private Position2D _playerStart;

    private MoveIntervalChecker _diffInterChecker;
    private MoveIntervalChecker _scoreInterChecker;

    private Notification _keyNotification;

    //private ItemSpawner2D _moneySpawner;
    //private ItemSpawner2D _timeDilSpawner;

    private Acid _acid;

    public void OnGUISRestartLevel(bool firstPlay)
    {
        //reset acid scale
        _acid.ResetAcid();
        _acid.StopRaise();

        //reset time
        TimeSpeed = 1.0f;
        _timeSpeedUp = 0.0f;
        _timeSlowDownDifference = 0.0f;

        //reset difficulty
        Difficulty = DifficultyStep;
        _diffInterChecker.ResetIntervals();

        //reset stat variables
        NumerOfAttempts++;
        StartTimeOfAttempt = OS.GetUnixTime();

        //player in level
        //Player player = GetNode<Player>("Player");
        //reset player position
        _player.Position = _playerStart.Position;
        //unfreeze the player
        _player.bFreeze = false;
        //reset moved status
        _player.bMoved = false;
        //reset player animations
        _player.ResetAnim();
        //reset item use
        _player.ItemUser.ResetItemUse();
        //reset score
        _player.ScoreProp.SetAmount(0);
        _scoreInterChecker.ResetIntervals();
        EmitSignal(nameof(SPlayerUpdateScore), _player);

        //Respawn items in block challenges
        EmitSignal(nameof(SRespawnItems));

        //refer that this is not first game
        //this can be changed if I use different implementation of Play and Restart signals
        if (bFirstPlay)
            bFirstPlay = firstPlay;
        //check for key binding tutorial visibility
        _keyNotification.CheckSatisfaction(bFirstPlay);
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
    }

    public void OnMoveIntervalCheckerSIntervalReached()
    {
        Difficulty += 10.0f;
    }

    public void OnAcidBodyEntered(Node body)
    {
        if (body is Player)
        {
            Player player = (Player)body;

            EmitSignal(nameof(SPlayerGO), body);
            player.bFreeze = true;
            //reset item use
            player.ItemUser.ResetItemUse();
            //refer that this is not first game
            bFirstPlay = false;
        }
    }

    public async void OnEndBodyEntered(Node body)
    {
        if (body is Player)
        {
            UInt64 actualTime = OS.GetUnixTime();
            TimeOfAttempt = actualTime - StartTimeOfAttempt;

            if (TimeOfAttempt > 5)
            {
                _player.bFreeze = true;
                _player.PlayAnim("Disappear_anim");

                await ToSignal(GetTree().CreateTimer(1f), "timeout");

                EmitSignal(nameof(SGameFinished));
            }
        }
    }

    public override void _Ready()
    {
        //get ref for acid
        _acid = GetNode<Acid>("Acid");
        //settup acid for level
        _acid.MaxHeight = 128 * 182.5f;
        _acid.DistToPlayer_Offset = 700f;
        //get player reference
        _player = GetParent().GetNode<Player>("Player");
        //get playerStart referemce
        _playerStart = GetNode<Position2D>("PlayerStart");
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

        _keyNotification = GetNode<Notification>("PlayerStart/KeyNotification");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (TimeSpeed < 1.0f)
        {
            ReturnTimeSlowlyToNormal(delta);
        }
        if (_player.IsReadyForGameplay(_playerStart.Position) && !_acid.IsRaising())
        {
            _acid.StartRaise();
        }
    }

    private void ReturnTimeSlowlyToNormal(float delta)
    {
        TimeSpeed += _timeSpeedUp * delta * _timeSlowDownDifference;
    }

    private void OnSlowDownTime(int amount, int length)
    {
        TimeSpeed /= amount;
        _timeSlowDownDifference = 1.0f - TimeSpeed;
        _timeSpeedUp = 1.0f / (length / 1000);
    }

    //private float Lerp(float firstFloat, float secondFloat, float by)
    //{
    //    return firstFloat * (1 - by) + secondFloat * by;
    //}
}

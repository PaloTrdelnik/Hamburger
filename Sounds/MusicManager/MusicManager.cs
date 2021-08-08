using Godot;
using System;

public class MusicManager : Node
{
    private Tween _animTweenOut;
    private Tween _animTweenOutIn;

    public AudioStreamPlayer ActualForeground;
    public AudioStreamPlayer ActualBackground;
    public AudioStreamPlayer FutureForeground;
    public AudioStreamPlayer FutureBackground;
    public float FadeTime = 0;
    private bool _bMuted = false;

    public void PlayActual()
    {
        ActualForeground.Play();
        if (ActualBackground != null)
            ActualBackground.Play();
    }

    public void SetMuteForActual(bool bMute)
    {
        if (bMute != _bMuted)
        {
            _bMuted = bMute;

            if (ActualForeground.Playing)
            {
                ActualForeground.StreamPaused = _bMuted;
            }
            if (ActualBackground != null)
            {
                if (ActualBackground.Playing)
                {
                    ActualBackground.StreamPaused = _bMuted;
                }
            }
        }
    }

    public bool GetMuteForActual()
    {
        return _bMuted;
    }

    public async void FadeOut()
    {
        if (!_bMuted)
        {
            _animTweenOut.InterpolateProperty(ActualForeground, "volume_db", 0f, -80f, FadeTime, Tween.TransitionType.Sine,
                                   Tween.EaseType.In, 0);
            if (ActualBackground != null)
            {
                _animTweenOut.InterpolateProperty(ActualBackground, "volume_db", -13f, -80f, FadeTime, Tween.TransitionType.Sine,
                               Tween.EaseType.In, 0);
            }
            _animTweenOut.Start();

            await ToSignal(GetTree().CreateTimer(FadeTime), "timeout");

            ActualForeground.Stop();

            if (ActualBackground != null)
                ActualBackground.Stop();
        }
    }

    public async void FadeIn()
    {
        if (FutureForeground != null)
        {
            if (!_bMuted)
            {
                _animTweenOutIn.InterpolateProperty(FutureForeground, "volume_db", -80f, 0f, FadeTime, Tween.TransitionType.Sine,
                               Tween.EaseType.In, 0);

                if (FutureBackground != null)
                {
                    _animTweenOutIn.InterpolateProperty(FutureBackground, "volume_db", -80f, -13f, FadeTime, Tween.TransitionType.Sine,
                                   Tween.EaseType.In, 0);
                }

                _animTweenOutIn.Start();
                FutureForeground.Play();

                if (FutureBackground != null)
                    FutureBackground.Play();

                await ToSignal(GetTree().CreateTimer(FadeTime), "timeout");
            }

            ActualForeground = FutureForeground;
            ActualBackground = FutureBackground;
            FutureForeground = null;
            FutureBackground = null;
        }
    }

    public async void Fade()
    {
        FadeOut();

        await ToSignal(GetTree().CreateTimer(FadeTime), "timeout");

        FadeIn();
    }

    public async void FadeAudioPlayers(AudioStreamPlayer fromPlayer, AudioStreamPlayer toPlayer, float time)
    {
        _animTweenOut.InterpolateProperty(fromPlayer, "volume_db", 0f, -80f, time, Tween.TransitionType.Sine,
                                       Tween.EaseType.In, 0);
        _animTweenOut.Start();

        await ToSignal(GetTree().CreateTimer(time), "timeout");
        fromPlayer.Stop();

        _animTweenOut.InterpolateProperty(toPlayer, "volume_db", -80f, 0f, time, Tween.TransitionType.Linear,
                                       Tween.EaseType.InOut);
        _animTweenOut.Start();
        toPlayer.Play();
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _animTweenOut = GetNode<Tween>("TweenIn");
        _animTweenOutIn = GetNode<Tween>("TweenOut");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}

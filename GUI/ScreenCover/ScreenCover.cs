using Godot;
using System;

public class ScreenCover : Control
{
    [Signal]
    public delegate void CoveringStarted();

    [Signal]
    public delegate void CoveringFinished();

    enum PlayingAnimation
    {
        ShowScreenAnim,
        HideScreenAnim,
        None
    }

    public float AnimLength = 0.30f;

    private Vector2 _startPos;
    private Vector2 _endPos;
    protected ColorRect _cRect;
    protected Tween _animTween;
    private PlayingAnimation _actualAnim = PlayingAnimation.None;

    public async void PlayShowScreenAnim()
    {
        _startPos = new Vector2(0.0f, 0.0f);
        _endPos = new Vector2(0.0f, _cRect.RectSize.y);

        Show();
        PlayScreenTransition();

        _actualAnim = PlayingAnimation.ShowScreenAnim;
        await ToSignal(GetTree().CreateTimer(AnimLength), "timeout");
        _actualAnim = PlayingAnimation.None;

        Hide();
        EmitSignal(nameof(CoveringFinished));
    }

    public async void PlayHideScreenAnim()
    {
        _startPos = new Vector2(0.0f, _cRect.RectSize.y * -1);
        _endPos = new Vector2(0.0f, 0.0f);

        Show();
        PlayScreenTransition();

        _actualAnim = PlayingAnimation.HideScreenAnim;

        EmitSignal(nameof(CoveringStarted));
        await ToSignal(GetTree().CreateTimer(AnimLength), "timeout");
        _actualAnim = PlayingAnimation.None;
    }

    public void OnScreenCoverResized()
    {
        // check for null because of crash on startup
        if (_animTween != null)
        {
            if (_animTween.IsActive())
            {
                // adjusting animation for different screen size
                // this changes only position not speed, so animation will be slower after resize
                if (_actualAnim == PlayingAnimation.ShowScreenAnim)
                {
                    _startPos = new Vector2(0.0f, _cRect.RectPosition.y);
                    _endPos = new Vector2(0.0f, _cRect.RectSize.y);
                }

                else if (_actualAnim == PlayingAnimation.HideScreenAnim)
                {
                    _startPos = new Vector2(0.0f, _cRect.RectPosition.y);
                    _endPos = new Vector2(0.0f, 0.0f);
                }

                // first we have to stop actual playing animations because tween do not update animation automaticaly
                _animTween.StopAll();
                //then start it again
                PlayScreenTransition();
            }
        }
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _cRect = GetNode<ColorRect>("ColorRect");
        _animTween = GetNode<Tween>("Tween");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
    }

    private void PlayScreenTransition()
    {
        _animTween.InterpolateProperty(_cRect,
                                       "rect_position",
                                       _startPos,
                                       _endPos,
                                       AnimLength,
                                       Tween.TransitionType.Linear,
                                       Tween.EaseType.InOut);
        _animTween.Start();
    }
}

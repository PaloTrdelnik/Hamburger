using Godot;
using System;

public class Player : KinematicBody2D
{
    [Export]
    int MOVE_SPEED = 500;

    [Export]
    int JUMP_FORCE = 1200;

    [Export]
    int GRAVITY = 80;
    
    [Export]
    int MAX_FALL_SPEED = 1000;

    private int _yVelo = 0;
    private bool _facingRight = true;

    Inventory Inv = new Inventory();


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }
    public override void _PhysicsProcess(float delta)
    {
        int moveDir = 0;
        if (Input.IsActionPressed("ui_right"))
        {
            moveDir += 1;
            
        }

        if (Input.IsActionPressed("ui_left"))
        {
            moveDir -= 1;
            
        }

        Vector2 moveVector = new Vector2(moveDir * MOVE_SPEED, _yVelo);
        Vector2 upDirection = new Vector2(0, -1);

        MoveAndSlide(moveVector, upDirection);

        bool grounded = IsOnFloor();

        _yVelo += GRAVITY;

        if (grounded && Input.IsActionJustPressed("ui_up"))
        {
            _yVelo = -JUMP_FORCE;
        }
        if (grounded && _yVelo > 5)
        {
            _yVelo = 5;
        }
        if (_yVelo > MAX_FALL_SPEED)
        {
            _yVelo = MAX_FALL_SPEED;
        }

        if (!_facingRight && moveDir > 0)
        {
            Flip();
        }

        if (_facingRight && moveDir < 0)
        {
            Flip();
        }

        if (grounded)
        {
            if (moveDir == 0)
            {
                PlayAnim("idle");
            }

            else 
            {
                PlayAnim("walk");
            }
        }
        else
        {
            PlayAnim("jump");
        }
            
    }

    private void Flip()
    {
        var pSprite = GetNode<Sprite>("Sprite");

        _facingRight = !_facingRight;
        pSprite.FlipH = !pSprite.FlipH;

    }

    private void PlayAnim(string animName)
    {
        var pAnimPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

        if (pAnimPlayer.IsPlaying() && pAnimPlayer.CurrentAnimation == animName)
        {
            return;
        }
        else
        {
            pAnimPlayer.Play(animName);
        }
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}

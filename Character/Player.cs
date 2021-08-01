using Godot;
using System;

public class Player : KinematicBody2D
{
    [Export]
    public int MOVE_SPEED = 20;

    [Export]
    public int JUMP_FORCE = 1200;

    [Export]
    public int GRAVITY = 80;
    
    [Export]
    public int MAX_FALL_SPEED = 1000;

    [Signal]
    public delegate void SInvItemChange(Player player, string itemInvKey);

    [Signal]
    public delegate void SSlowDownTime(int amount, int length); // length in milliseconds

    [Signal]
    public delegate void SItemUseDurabilityBegin(string itemKey);

    [Signal]
    public delegate void SItemUseDurabilityEnd(string itemKey);

    public bool bFreezeInput = false;
    public bool bFreeze = true;
    public bool bMoved = false;
    public Inventory Inv = new Inventory();
    public Property ScoreProp = new Property();
    public ItemUser ItemUser;
    public AnimationPlayer AnimPlayer;

    private GUI _gui;
    private int _yVelo = 0;
    private float _pushForce = 200;
    private bool _facingRight = true;

    public void OnItemUserSItemUseEnd(string itemKey)
    {
        EmitSignal(nameof(SItemUseDurabilityEnd), itemKey);
    }

    public void OnInteractionSAddToInv(Item item)
    {
        if (!Inv.ISInvFull())
        {
            Inv.Add(item);
            if (Inv.bAddedItem ||Inv.bRemovedItem)
            {
                EmitSignal(nameof(SInvItemChange),this, item.InvKey);
                item.QueueFree();
            }
        }
    }

    public bool IsReadyForGameplay(Vector2 playerStartPos)
    {
        if (playerStartPos.DistanceTo(GlobalPosition) > 35 && bMoved)
            return true;
        else
            return false;
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _gui = GetParent().GetNode<GUI>("GUI");
        AnimPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        ItemUser = GetNode<ItemUser>("ItemUser");
    }

    public override void _PhysicsProcess(float delta)
    {
        int moveDir = 0;
        if (Input.IsActionPressed("Game_Right"))
        {
            moveDir += 1;
            bMoved = true;
        }

        if (Input.IsActionPressed("Game_Left"))
        {
            moveDir -= 1;
            bMoved = true;
        }

        //handle RigidBody2D collisions
        for (int index = 0; index < GetSlideCount(); index++)
        {
            KinematicCollision2D collision = GetSlideCollision(index);
            if (collision.Collider != null)
            {
                if (collision.Collider.IsClass("RigidBody2D"))
                {
                    RigidBody2D body = (RigidBody2D)collision.Collider;
                    body.ApplyCentralImpulse(-collision.Normal * _pushForce);
                }
            }
        }

        bool grounded = IsOnFloor();

        _yVelo += GRAVITY;

        if (grounded && Input.IsActionJustPressed("Game_Jump"))
        {
            if (bFreezeInput)
            {
                _yVelo = 0;
            }
            else
            {
                _yVelo = -JUMP_FORCE;
            }
        }

        if (grounded && _yVelo > 5)
        {
            _yVelo = 5;
        }

        if (_yVelo > MAX_FALL_SPEED)
        {
            _yVelo = MAX_FALL_SPEED;
        }

        if (bFreezeInput)
        {
            moveDir = 0;
        }
        if (bFreeze)
        {
            moveDir = 0;
            _yVelo = 0;
        }

        Vector2 moveVector = new Vector2(moveDir * MOVE_SPEED, _yVelo);
        Vector2 upDirection = new Vector2(0, -1);

        MoveAndSlide(moveVector, upDirection, infiniteInertia: false);

        if (!_facingRight && moveDir > 0)
        {
            Flip();
        }

        if (_facingRight && moveDir < 0)
        {
            Flip();
        }
        if (!bFreeze)
        {
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

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        CheckForAbilityInput();
    }
    private void Flip()
    {
        var pSprite = GetNode<Sprite>("Sprite");

        _facingRight = !_facingRight;
        pSprite.FlipH = !pSprite.FlipH;

    }

    public void PlayAnim(string animName)
    {
        if (AnimPlayer.IsPlaying() && AnimPlayer.CurrentAnimation == animName)
        {
            return;
        }
        else
        {
            AnimPlayer.Play(animName);
        }
    }
    public void ResetAnim()
    {
        AnimPlayer.Play("Disappear_anim");
        AnimPlayer.Seek(0.0f, true);
        AnimPlayer.Stop();
    }

    private void CheckForAbilityInput()
    {
        ToolBar tBar = _gui.GetNode<ToolBar>("InGameGUIControl/ToolBar");

        if (Input.IsActionJustPressed("Game_ToolBar_item1"))
        {
            UseItemFromToolBar(tBar, 0);
        }
    }

    private void UseItemFromToolBar(ToolBar tBar, int itemSlot)
    {
        if (itemSlot + 1 <= tBar.GetChildCount())
        {
            ToolBarItem slot = tBar.GetChild<ToolBarItem>(0);

            string slotItemKey = slot.ItemKey;

            UseItem(slotItemKey);
        }
    }

    private void UseItem(string itemKey)
    {
        Item rem = new Item { InvKey = itemKey, Amount = 1 };

        if (Inv.IsInInv(rem) && !bFreeze)
        {
            if (itemKey == "TimeDilation")
            {
                if(ItemUser.UseItem(itemKey))
                {
                    EmitSignal(nameof(SInvItemChange), this, itemKey);
                    EmitSignal(nameof(SSlowDownTime), 5, 10000);
                    EmitSignal(nameof(SItemUseDurabilityBegin), itemKey);
                }
            }
        }
    }
}

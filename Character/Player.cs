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

    [Signal]
    public delegate void SInvItemChange(Player player, string itemInvKey);

    [Signal]
    public delegate void SSlowDownTime(int amount, int length); // length in milliseconds

    [Signal]
    public delegate void SItemDurabilityUpdateBegin(string itemKey);

    [Signal]
    public delegate void SItemDurabilityUpdateEnd(string itemKey);

    public bool bFreezeInput = false;
    public bool bFreeze = true;
    public Inventory Inv = new Inventory();

    private GUI _gui;
    private ItemUser _itemUser;

    private int _yVelo = 0;
    private bool _facingRight = true;

    public void OnItemUserSItemUseEnd(string itemKey)
    {
        GD.Print("ItemUseEnd");
        EmitSignal(nameof(SItemDurabilityUpdateEnd), itemKey);
    }

    public void OnInteractionSAddToInv(Item item)
    {
        Inv.Add(item);
        if (Inv.bAddedItem || Inv.bRemovedItem)
        {
            EmitSignal(nameof(SInvItemChange),this, item.InvKey);
        }
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _gui = GetParent().GetNode<GUI>("GUI");
        _itemUser = GetNode<ItemUser>("ItemUser");
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

        MoveAndSlide(moveVector, upDirection);

        bool grounded = IsOnFloor();

        _yVelo += GRAVITY;

        if (grounded && Input.IsActionJustPressed("ui_up"))
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

    private void CheckForAbilityInput()
    {
        Taskbar tBar = _gui.GetNode<Taskbar>("InGameGUIControl/Taskbar");

        if (Input.IsActionJustPressed("Game_Taskbar_item1"))
        {
            UseItemFromTaskBar(tBar, 0);
        }
    }

    private void UseItemFromTaskBar(Taskbar tBar, int itemSlot)
    {
        if (itemSlot + 1 <= tBar.GetChildCount())
        {
            TaskBarItem slot = tBar.GetChild<TaskBarItem>(0);

            string slotItemKey = slot.ItemKey;

            UseItem(slotItemKey);
        }
    }

    private void UseItem(string itemKey)
    {
        Item rem = new Item { InvKey = itemKey, Amount = 1 };

        if (Inv.IsInInv(rem))
        {
            if (itemKey == "TimeDilation")
            {
                if(_itemUser.UseItem(itemKey))
                {
                    GD.Print("aaa");
                    EmitSignal(nameof(SInvItemChange), this, itemKey);
                    EmitSignal(nameof(SSlowDownTime), 5, 10000);
                    EmitSignal(nameof(SItemDurabilityUpdateBegin), itemKey);
                }

            }
        }
    }
}

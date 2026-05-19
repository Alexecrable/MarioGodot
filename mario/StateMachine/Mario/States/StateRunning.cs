using Godot;
using System;
using System.Security.AccessControl;

public partial class StateRunning : MarioState
{
    private Timer coyoteTimer;
    private bool isCoyote;
    private MovementComponent movementComponent;
    public StateRunning(Mario _mario, MovementComponent _movementComponent) : base(_mario)
    {
        coyoteTimer = new Timer();
        coyoteTimer.OneShot = true;
        isCoyote = false;
        coyoteTimer.WaitTime = 0.1;
        coyoteTimer.Timeout += CoyoteEnd;
        AddChild(coyoteTimer);
        movementComponent = _movementComponent;
    }

    // Called when the node enters the scene tree for the first time.
    private void CoyoteEnd()
    {
        EmitSignal(SignalName.Finished, (int)Mario.StateEnum.FALL);
    }

    override public void Enter(int _stateID)
    {
        mario.animation.Animation = "Running";
       // mario.animation.Modulate = new Color(0,0,1);
        mario.animation.Play();
        //mario.SetGoingNeutral();
        isCoyote = false;
        movementComponent.CurrentSpeedY = mario.startingGravity;
        movementComponent.CurrentSpeedX = mario.speed;
        mario.chapeau.Offset = new Vector2(13 * (mario.rightInput - mario.leftInput),4);

        mario.feetBox.CollisionLayer = 0;
        mario.feetBox.CollisionMask = 0;
    }

    override public void Exit(int _stateID)
    {
       // mario.animation.Modulate = new Color(1,1,1);
        coyoteTimer.Stop();
        mario.chapeau.Offset = new Vector2(0,0);
    }

    override public void PhysicsProcess(double _delta)
    {
        movementComponent.SpeedX();
        movementComponent.Advance();
        if (mario.rightInput - mario.leftInput < 0)
        {
            mario.animation.FlipH = true;
            mario.chapeau.FlipH = true;
        }
        else
        {
            if ((mario.rightInput - mario.leftInput > 0))
            {
                mario.animation.FlipH = false;
                mario.chapeau.FlipH = false;
            }
        }
        if (!mario.IsOnFloor() && !isCoyote)
        {
            isCoyote = true;
            coyoteTimer.Start();
        }
        else
        {
            if (!movementComponent.IsMoving())
            {
                EmitSignal(SignalName.Finished, (int)Mario.StateEnum.IDLE);
            }
            else
            {
                if (movementComponent.WantsJump())
                {
                    EmitSignal(SignalName.Finished, (int)Mario.StateEnum.JUMP);
                }
            }

        }
        //GD.Print("MARIO VELOC " + right + " " + left);
    }




}

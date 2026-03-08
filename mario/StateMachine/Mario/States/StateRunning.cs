using Godot;
using System;
using System.Security.AccessControl;

public partial class StateRunning : MarioState
{
    private Timer coyoteTimer;
    private bool isCoyote;
    public StateRunning(Mario _mario) : base(_mario)
    {
        coyoteTimer = new Timer();
        coyoteTimer.OneShot = true;
        isCoyote = false;
        coyoteTimer.WaitTime = 0.1;
        coyoteTimer.Timeout += CoyoteEnd;
        AddChild(coyoteTimer);
    }

    // Called when the node enters the scene tree for the first time.
    private void CoyoteEnd()
    {
        GD.Print("coyote ENDING");
        EmitSignal(SignalName.Finished, (int)Mario.StateEnum.FALL);
    }

    override public void Enter(int _stateID)
    {
        GD.Print("enter running");
        mario.animation.Animation = "Running";
        mario.animation.Play();
        mario.SetGoingNeutral();
        isCoyote = false;
        mario.yVelocity = mario.startingGravity;
    }

    override public void Exit(int _stateID)
    {
        GD.Print("stopp coyoteTimer exiting running");
        coyoteTimer.Stop();
    }

    override public void PhysicsProcess(double delta)
    {
       
        bool isRunning = mario.rightInput + mario.leftInput > 0;
        mario.currentHorizontalVelocity = (mario.rightInput - mario.leftInput) * mario.speed;
		mario.Velocity = new Vector2(mario.currentHorizontalVelocity, mario.yVelocity);
        if (mario.rightInput - mario.leftInput < 0)
        {
            mario.animation.FlipH = true;
        }
        else
        {
            if ((mario.rightInput - mario.leftInput > 0))
            {
                mario.animation.FlipH = false;
            }
        }
        if (!mario.IsOnFloor() && !isCoyote)
        {
            isCoyote = true;
            coyoteTimer.Start();
        }
        else
        {
            if (!isRunning)
            {
                GD.Print("gotoidle");
                EmitSignal(SignalName.Finished, (int)Mario.StateEnum.IDLE);
            }
            else
            {
                if (Input.IsActionJustPressed("jump"))
                {
                    EmitSignal(SignalName.Finished, (int)Mario.StateEnum.JUMP);
                }
            }

        }
        //GD.Print("MARIO VELOC " + right + " " + left);
    }




}

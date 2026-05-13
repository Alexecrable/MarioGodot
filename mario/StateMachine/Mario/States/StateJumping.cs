using Godot;
using System;

public partial class StateJumping : MarioState
{

    private Timer timer;
    private AudioStreamMP3 jumpSFX;
    MovementComponent movementComponent;
    public StateJumping(Mario _mario, MovementComponent _movementComponent) : base(_mario)
    {

        timer = new Timer();
        timer.WaitTime = mario.jumpTime;
        timer.Timeout += timerEnd;
        timer.OneShot = true;
        jumpSFX = ResourceLoader.Load<AudioStreamMP3>("res://StateMachine/Mario/Sounds/Jump.mp3");

        jumpSFX.Loop = false;

        movementComponent = _movementComponent;

        AddChild(timer);
    }


    private void timerEnd()
    {
        EmitSignal(SignalName.Finished, (int)Mario.StateEnum.FALL);
        movementComponent.CurrentSpeedY = mario.endJumpGravity;
    }

    override public void Enter(int _stateID)
    {
        //mario.Velocity += new Vector2(0, 50);
        GD.Print("enter jump");
        GD.Print(movementComponent.CurrentSpeedX);
        mario.animation.Animation = "Jumping";
        mario.sfxPlayer.Stream = jumpSFX;

        mario.animation.Play();
        mario.sfxPlayer.Play();
        mario.SetGoingUp();
        timer.Start();
        movementComponent.CurrentSpeedY = mario.startJumpGravity;

        mario.feetBox.CollisionLayer = 4;
        mario.feetBox.CollisionMask = 8;
    }








    override public void Exit(int _stateID)
    {
        //mario.SetHeadActivated(false);
        GD.Print("exitJump");
    }

    override public void PhysicsProcess(double _delta)
    {
        movementComponent.AccelerateToSpeedX(_delta);
        //movementComponent.AccelerateToSpeedY(_delta);
        
        
        GD.Print(mario.jumpInput);
        if (!movementComponent.WantsJump())
        {
            timer.Stop();
            EmitSignal(SignalName.Finished, (int)Mario.StateEnum.FALL);

        }
        if (movementComponent.IsOnCeiling())
        {
            timer.Stop();
            movementComponent.CurrentSpeedY = 0;
            mario.SetGoingDown();
            EmitSignal(SignalName.Finished, (int)Mario.StateEnum.FALL);

        }
        movementComponent.Advance();
        GD.Print(mario.Velocity);
    }




}

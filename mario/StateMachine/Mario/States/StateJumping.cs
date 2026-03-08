using Godot;
using System;

public partial class StateJumping : MarioState
{

    private Timer timer;
    private AudioStreamMP3 jumpSFX;
    public StateJumping(Mario _mario) : base(_mario)
    {

        timer = new Timer();
        timer.WaitTime = mario.jumpTime;
        timer.Timeout += timerEnd;
        timer.OneShot = true;
        jumpSFX = ResourceLoader.Load<AudioStreamMP3>("res://StateMachine/Mario/Sounds/Jump.mp3");

        jumpSFX.Loop = false;


        AddChild(timer);
    }


    private void timerEnd()
    {
        EmitSignal(SignalName.Finished, (int)Mario.StateEnum.FALL);
        mario.yVelocity = mario.endJumpGravity;
    }

    override public void Enter(int _stateID)
    {
        //mario.Velocity += new Vector2(0, 50);
        GD.Print("enter jump");
        mario.animation.Animation = "Jumping";
        mario.sfxPlayer.Stream = jumpSFX;

        mario.animation.Play();
        mario.sfxPlayer.Play();
        mario.SetGoingUp();
        timer.Start();
        mario.yVelocity = mario.startJumpGravity;
    }








    override public void Exit(int _stateID)
    {
        //mario.SetHeadActivated(false);
    }

    override public void PhysicsProcess(double delta)
    {
        if (mario.maxHorizontalVelocity > mario.currentHorizontalVelocity * (mario.rightInput - mario.leftInput))
        {
            mario.currentHorizontalVelocity += mario.airborneHorizontalAccel * (mario.rightInput - mario.leftInput) * (float)delta;
        }
        

        if (mario.jumpInput == 0)
        {
            timer.Stop();
            EmitSignal(SignalName.Finished, (int)Mario.StateEnum.FALL);

        }
        if (mario.IsOnCeiling())
        {
            timer.Stop();
            mario.yVelocity = 0;
            mario.SetGoingDown();
            EmitSignal(SignalName.Finished, (int)Mario.StateEnum.FALL);

        }
        mario.Velocity = new Vector2(mario.currentHorizontalVelocity, mario.yVelocity);

    }




}

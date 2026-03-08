using Godot;
using System;

public partial class StateWallSlide : MarioState
{

    private AudioStreamMP3 wallSlideSFX;
    public StateWallSlide(Mario _mario) : base(_mario)
    {
        wallSlideSFX = GD.Load<AudioStreamMP3>("res://StateMachine/Mario/Sounds/WallSlide.mp3");
        wallSlideSFX.Loop = true;
        wallSlideSFX.LoopOffset = 0.3;
        
    }




    override public void Enter(int _stateID)
    {
        mario.sfxPlayer.Stream = wallSlideSFX;
        mario.SetGoingDown();
        GD.Print("enter wallslide");
        mario.animation.Animation = "WallSlide";
        mario.animation.Play();
        mario.Velocity = new Vector2(0, 100);
        mario.currentHorizontalVelocity = 0;
        GD.Print("wall direction");
        mario.animation.FlipH = mario.raycastLeft.IsColliding();
        mario.particles.Emitting = true;
        if (mario.animation.FlipH)
        {
            mario.particles.Position = new Vector2(-6, -14);
        }
        else
        {
            mario.particles.Position = new Vector2(6, -14);
        }
        mario.sfxPlayer.Play();

    }



    override public void Exit(int _stateID)
    {
        mario.particles.Emitting = false;
        mario.sfxPlayer.Stop();

    }

    override public void PhysicsProcess(double delta)
    {


        //GD.Print("floor " + mario.IsOnFloor());
        bool marioGoesRight = mario.rightInput - mario.leftInput > 0;
        bool marioGoesLeft = mario.rightInput - mario.leftInput < 0;
        bool marioGrabsWall = (mario.raycastLeft.IsColliding() && marioGoesLeft) || (mario.raycastRight.IsColliding() && marioGoesRight);
        GD.Print("climbLog :"+
                "\n\tMarioRight : " + marioGoesRight +
                "\n\tMarioLeft : " + marioGoesLeft +
                "\n\tMarioGrabs : " + marioGrabsWall);
        if (!marioGrabsWall)
        {
            //falling
            mario.yVelocity = 100;
            EmitSignal(SignalName.Finished, (int)Mario.StateEnum.FALL);
        }

        else
        {
            if (mario.IsOnFloor())
            {
                GD.Print("le probleme");
                EmitSignal(SignalName.Finished, (int)Mario.StateEnum.IDLE);
            }
            else
            {
                if (Input.IsActionJustPressed("jump"))
                {
                    mario.animation.FlipH = !mario.animation.FlipH;
                    mario.currentHorizontalVelocity = mario.raycastRight.IsColliding() ? -mario.speed : mario.speed;
                    EmitSignal(SignalName.Finished, (int)Mario.StateEnum.JUMP);
                    GD.Print("new speed " + mario.currentHorizontalVelocity + " " + mario.speed);
                }

            }
        }

    }





}

using Godot;
using System;

public partial class StateWallSlide : MarioState
{

    private AudioStreamMP3 wallSlideSFX;
    private MovementComponent movementComponent;
    public StateWallSlide(Mario _mario, MovementComponent _movementComponent) : base(_mario)
    {
        wallSlideSFX = GD.Load<AudioStreamMP3>("res://StateMachine/Mario/Sounds/WallSlide.mp3");
        wallSlideSFX.Loop = true;
        wallSlideSFX.LoopOffset = 0.3;
        movementComponent = _movementComponent;
        
    }




    override public void Enter(int _stateID)
    {
        mario.sfxPlayer.Stream = wallSlideSFX;
        //mario.SetGoingDown();
        mario.animation.Animation = "WallSlide";
        mario.animation.Play();
        movementComponent.CurrentSpeedX = 0;
        movementComponent.CurrentSpeedY = 100;
        movementComponent.Advance();
        mario.animation.FlipH = mario.raycastLeft.IsColliding();
        mario.particles.Emitting = true;
        if (mario.animation.FlipH)
        {
            mario.particles.Position = new Vector2(-6, -14);
            mario.chapeau.Offset = new Vector2(10,-3);
            mario.chapeau.FlipH = false;
        }
        else
        {
            mario.particles.Position = new Vector2(6, -14);
            mario.chapeau.Offset = new Vector2(-10,-3);
            mario.chapeau.FlipH = true;

        }
        mario.sfxPlayer.Play();

        mario.feetBox.CollisionLayer = 4;
        mario.feetBox.CollisionMask = 8;

    }

// 10 3

    override public void Exit(int _stateID)
    {
        mario.particles.Emitting = false;
        mario.sfxPlayer.Stop();
        mario.chapeau.Offset = new Vector2(0,0);
    }

    override public void PhysicsProcess(double delta)
    {


        //GD.Print("floor " + mario.IsOnFloor());
       
        if (!mario.IsGrabbingWall())
        {
           
            EmitSignal(SignalName.Finished, (int)Mario.StateEnum.FALL);
        }

        else
        {
            if (mario.IsOnFloor())
            {
                EmitSignal(SignalName.Finished, (int)Mario.StateEnum.IDLE);
            }
            else
            {
                if (movementComponent.WantsJump())
                {
                    mario.animation.FlipH = !mario.animation.FlipH;
                    movementComponent.CurrentSpeedX = mario.raycastRight.IsColliding() ? -mario.speed : mario.speed;
                    EmitSignal(SignalName.Finished, (int)Mario.StateEnum.JUMP);
                }

            }
        }

    }





}

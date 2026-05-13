using Godot;
using System;

public partial class GoombaStateWalk : GoombaState
{
    private bool flipBufferActive;
    private Timer flipBufferTimer;
    private float velocitySave;
    public GoombaStateWalk(Goomba _goomba, MovementComponent _movementComponent) : base(_goomba, _movementComponent)
    {
        VisibleOnScreenNotifier2D notifier = goomba.getNotifier();
        notifier.ScreenExited += ScreenExited;
        goomba.Hit += HitBoxTouched;
        flipBufferActive = false;
        flipBufferTimer = new Timer();
        flipBufferTimer.WaitTime = 0.1;
        AddChild(flipBufferTimer);
        flipBufferTimer.Timeout += FlipBufferEnd;
        velocitySave = 60;
    }

    private void FlipBufferEnd()
    {
        flipBufferActive = false;
    }

    
    public override void Enter(int _previousStateId)
    {
        goomba.skin.Play();
        movementComponent.CurrentSpeedX = velocitySave;
    }

    public override void Exit(int _previousStateId)
    {
        velocitySave = movementComponent.CurrentSpeedX;
        GD.Print("Screen Goomb " + movementComponent.CurrentSpeedX);
    }

    public override void PhysicsProcess(double _delta)
    {
        movementComponent.CurrentSpeedY = goomba.IsOnFloor() ? 0 : 200;
        if (goomba.IsOnWall() && !flipBufferActive)
        {
            flipGoomb();

        }
        movementComponent.Advance();
    }


    private void flipGoomb()
    {
        movementComponent.CurrentSpeedX = -movementComponent.CurrentSpeedX;
        goomba.Scale = new Vector2(-goomba.Scale.X, goomba.Scale.Y);
        flipBufferTimer.Start();
        flipBufferActive = true;

    }

    private void ScreenExited()
    {
        
        EmitSignal(SignalName.Finished, (int)GoombaStateEnum.IDLE);
        
    }

    private void HitBoxTouched()
    {
        EmitSignal(SignalName.Finished, (int)GoombaStateEnum.DIE);
    }
}

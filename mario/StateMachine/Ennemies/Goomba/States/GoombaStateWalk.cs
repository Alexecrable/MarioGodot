using Godot;
using System;

public partial class GoombaStateWalk : GoombaState
{
    private bool flipBufferActive;
    private Timer flipBufferTimer;
    public GoombaStateWalk(Goomba _goomba) : base(_goomba)
    {
        VisibleOnScreenNotifier2D notifier = goomba.getNotifier();
        notifier.ScreenExited += ScreenExited;
        goomba.Hit += HitBoxTouched;
        flipBufferActive = false;
        flipBufferTimer = new Timer();
        flipBufferTimer.WaitTime = 0.1;
        AddChild(flipBufferTimer);
        flipBufferTimer.Timeout += FlipBufferEnd;
    }

    private void FlipBufferEnd()
    {
        flipBufferActive = false;
    }

    
    public override void Enter(int _previousStateId)
    {
        goomba.skin.Play();
        goomba.currentXVelocity = 60;
    }

    public override void Exit(int _previousStateId)
    {
    }

    public override void PhysicsProcess(double _delta)
    {
        goomba.currentYVelocity = goomba.IsOnFloor() ? 0 : 200;
        if (goomba.IsOnWall() && !flipBufferActive)
        {
            flipGoomb();

        }
        goomba.Velocity = new Vector2(goomba.currentXVelocity, goomba.currentYVelocity);

    }


    private void flipGoomb()
    {
        goomba.currentXVelocity = -goomba.currentXVelocity;
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

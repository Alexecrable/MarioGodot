using Godot;
using System;

public partial class KoopaStateShellMove : KoopaState
{
    private bool flipBufferActive;
    private Timer flipBufferTimer;

    public KoopaStateShellMove(Koopa _koopa, MovementComponent _movementComponent) : base(_koopa, _movementComponent)
    {
        koopa = _koopa;
        flipBufferActive = false;
        flipBufferTimer = new Timer();
        flipBufferTimer.WaitTime = 0.1;
        AddChild(flipBufferTimer);
        flipBufferTimer.Timeout += FlipBufferEnd;

    }

    public override void Enter(int _previousStateId)
    {
        GD.Print("enter state : ShellMove" + this.Name);

    }

    public override void Exit(int _previousStateId)
    {
    }
    public override void PhysicsProcess(double _delta)
    {
        if (koopa.IsOnWall() && !flipBufferActive)
        {
            GD.Print("enter state : xddd");
            FlipKoopa();


        }
        movementComponent.CurrentSpeedY = (koopa.IsOnFloor()) ? 0 : 200;
        movementComponent.Advance();



    }
    private void FlipKoopa()
    {
        koopa.Scale = new Vector2(-koopa.Scale.X, koopa.Scale.Y);
        movementComponent.CurrentSpeedX = -movementComponent.CurrentSpeedX;

        flipBufferTimer.Start();
        flipBufferActive = true;
    }
    private void FlipBufferEnd()
    {
        flipBufferActive = false;
    }

}

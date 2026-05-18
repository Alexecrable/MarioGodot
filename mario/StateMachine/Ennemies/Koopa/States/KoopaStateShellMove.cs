using Godot;
using System;

public partial class KoopaStateShellMove : KoopaState
{
    private bool flipBufferActive;
    private Timer flipBufferTimer, enterStateDelay;

    public KoopaStateShellMove(Koopa _koopa, MovementComponent _movementComponent) : base(_koopa, _movementComponent)
    {
        koopa = _koopa;
        flipBufferActive = false;
        flipBufferTimer = new Timer();
        flipBufferTimer.WaitTime = 0.1;
        AddChild(flipBufferTimer);
        flipBufferTimer.Timeout += FlipBufferEnd;
        enterStateDelay = new Timer();
        enterStateDelay.WaitTime = 1;
        AddChild(enterStateDelay);

    }

    public override void Enter(int _previousStateId)
    {
        GD.Print("enter state : ShellMove" + this.Name);
        koopa.hurtBox.AreaEntered += Hittt;
        enterStateDelay.Start();
        enterStateDelay.Timeout += EndDelay;


    }

    public void EndDelay()
    {
        koopa.hurtBox.CollisionLayer = 8;
        //koopa.CollisionLayer = 16;
    }
    private void Hittt(Node2D _body)
    {
        if (_body.Name == "FeetBox")
        {

            EmitSignal(SignalName.Finished, (int)KoopaStateEnum.SHELL_IDLE);
        }

    }
    public override void Exit(int _previousStateId)
    {
        enterStateDelay.Timeout -= EndDelay;
        koopa.hurtBox.AreaEntered -= Hittt;

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

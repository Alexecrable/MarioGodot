using Godot;
using System;

public partial class KoopaStateWalk : KoopaState
{
    private bool flipBufferActive;
    private float velocitySave;
    private Timer flipBufferTimer;
    private VisibleOnScreenNotifier2D notifier;
    public KoopaStateWalk(Koopa _koopa, MovementComponent _movementComponent) : base(_koopa, _movementComponent)
    {
        notifier = koopa.getNotifier();

        velocitySave = koopa.xVel;
        flipBufferActive = false;
        flipBufferTimer = new Timer();
        flipBufferTimer.WaitTime = 0.1;
        AddChild(flipBufferTimer);
        flipBufferTimer.Timeout += FlipBufferEnd;

    }
    private void TurnAreaExited(Node _body)
    {
        GD.Print("exited " + _body);
        FlipKoopa();
    }

    private void FlipKoopa()
    {
        koopa.Scale = new Vector2(-koopa.Scale.X, koopa.Scale.Y);
        movementComponent.CurrentSpeedX = -movementComponent.CurrentSpeedX;
        movementComponent.Advance();
        flipBufferTimer.Start();
        flipBufferActive = true;
    }
    private void FlipBufferEnd()
    {
        flipBufferActive = false;
    }

    public override void Enter(int _previousStateId)
    {
        GD.Print("enter state : Walk" + this.Name);
        koopa.legs.Play();
        koopa.eyes.Play();
        movementComponent.CurrentSpeedX = velocitySave;
        movementComponent.Advance();
        koopa.turnAroundArea.BodyExited += TurnAreaExited;
        notifier.ScreenExited += ScreenExited;
        koopa.hurtBox.AreaEntered += HitBoxTouched;




    }

    public override void Exit(int _previousStateId)
    {
        velocitySave = movementComponent.CurrentSpeedX;
        koopa.turnAroundArea.BodyExited -= TurnAreaExited;
        notifier.ScreenExited -= ScreenExited;
        koopa.hurtBox.AreaEntered -= HitBoxTouched;


    }

    public override void PhysicsProcess(double _delta)
    {
        if (koopa.IsOnWall() && !flipBufferActive)
        {
            FlipKoopa();


        }

    }




    private void ScreenExited()
    {
        EmitSignal(SignalName.Finished, (int)KoopaStateEnum.IDLE);
    }

    private void HitBoxTouched(Node _body)
    {
        GD.Print("enter state: ici" + _body.Name + koopa.hurtBox.CollisionLayer + koopa.hurtBox.CollisionMask);

        EmitSignal(SignalName.Finished, (int)KoopaStateEnum.SHELL_IDLE);
    }
}
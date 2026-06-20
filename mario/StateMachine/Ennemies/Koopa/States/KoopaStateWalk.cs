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
        koopa.legs.Play();
        koopa.eyes.Play();
        movementComponent.CurrentSpeedX = velocitySave;
        movementComponent.Advance();
        notifier.ScreenExited += ScreenExited;
        koopa.hurtBox.AreaEntered += HitBoxTouched;




    }

    public override void Exit(int _previousStateId)
    {
        velocitySave = movementComponent.CurrentSpeedX;
        notifier.ScreenExited -= ScreenExited;
        koopa.hurtBox.AreaEntered -= HitBoxTouched;


    }

    public override void PhysicsProcess(double _delta)
    {

        if (koopa.IsOnWall() && !flipBufferActive)
        {
            FlipKoopa();


        }
        else
        {
            if (!koopa.turnAroundRay.IsColliding())
            {
                FlipKoopa();
            }
        }

    }




    private void ScreenExited()
    {
        EmitSignal(SignalName.Finished, (int)KoopaStateEnum.IDLE);
    }

    private void HitBoxTouched(Node _body)
    {
                koopa.GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D").Play();
        GD.Print("xbox");
        EmitSignal(SignalName.Finished, (int)KoopaStateEnum.SHELL_IDLE);
    }
}
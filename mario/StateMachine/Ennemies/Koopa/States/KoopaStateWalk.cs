using Godot;
using System;

public partial class KoopaStateWalk : KoopaState
{
    private bool flipBufferActive;
    private Timer flipBufferTimer;
    public KoopaStateWalk(Koopa _koopa) : base(_koopa)
    {
        VisibleOnScreenNotifier2D notifier = koopa.getNotifier();
        notifier.ScreenExited += ScreenExited;
        koopa.turnAroundArea.BodyExited += TurnAreaExited;
        
        flipBufferActive = false;
        flipBufferTimer = new Timer();
        flipBufferTimer.WaitTime = 0.1;
        AddChild(flipBufferTimer);
        flipBufferTimer.Timeout += FlipBufferEnd;

        koopa.hurtBox.BodyEntered += HitBoxTouched;
    }
    private void TurnAreaExited(Node _body)
    {
        GD.Print("exited " + _body);
        FlipKoopa();
    }

    private void FlipKoopa()
    {
        koopa.Scale = new Vector2(-koopa.Scale.X, koopa.Scale.Y);
        koopa.xVel = -koopa.xVel;
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
        koopa.Velocity = new Vector2(koopa.xVel, koopa.IsOnFloor() ? 0 : 200);


    }

    public override void Exit(int _previousStateId)
    {
    }

    public override void PhysicsProcess(double _delta)
    {
        if (koopa.IsOnWall() && !flipBufferActive)
        {
            FlipKoopa();


        }
        koopa.Velocity = new Vector2(koopa.xVel, koopa.IsOnFloor() ? 0 : 200);

    }




    private void ScreenExited()
    {
        EmitSignal(SignalName.Finished, (int)KoopaStateEnum.IDLE);
    }

    private void HitBoxTouched(Node _body)
    {
        GD.Print("ici");
        EmitSignal(SignalName.Finished, (int)KoopaStateEnum.SHELL_IDLE);
    }
}
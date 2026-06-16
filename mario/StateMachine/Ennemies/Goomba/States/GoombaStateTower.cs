using Godot;
using System;

public partial class GoombaStateTower : GoombaState
{
    private bool flipBufferActive;
    private Timer flipBufferTimer;
    private float velocitySave;
    public GoombaStateTower(Goomba _goomba, MovementComponent _movementComponent) : base(_goomba, _movementComponent)
    {
        VisibleOnScreenNotifier2D notifier = goomba.getNotifier();
        notifier.ScreenExited += ScreenExited;
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
        movementComponent.CurrentSpeedX = 0;
        movementComponent.CurrentSpeedY = 0;
        movementComponent.Advance();
        goomba.hurtBox.Hurt += HitBoxTouched;
    }

    public override void Exit(int _previousStateId)
    {
        //velocitySave = movementComponent.CurrentSpeedX;
        goomba.hurtBox.Hurt -= HitBoxTouched;
    }

    public override void PhysicsProcess(double _delta)
    {
        for (int i = 0; i < goomba.GetSlideCollisionCount(); i++)
        {
            KinematicCollision2D coll = goomba.GetSlideCollision(i);
            GD.Print("normal : " + i + " | " + coll.GetNormal() );
            if (coll.GetNormal() == Vector2.Up)
            {
                try
                {
                    Goomba otherGoomba = (Goomba)coll.GetCollider();
                    GD.Print("normal : Succ " + goomba.Position + " : " + otherGoomba.Position);
                   
                   
                    return;
                }
                catch (Exception e)
                {
                    GD.Print("normal : goomb tower fail " + e);
                }
            }
        }
        GD.Print("normal : goombend" + goomba.Position);
        EmitSignal(SignalName.Finished, (int)GoombaStateEnum.WALK);
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

    private void HitBoxTouched(Node _Area)
    {
        EmitSignal(SignalName.Finished, (int)GoombaStateEnum.DIE);
    }
}

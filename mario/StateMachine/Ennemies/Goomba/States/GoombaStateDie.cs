using Godot;
using System;

public partial class GoombaStateDie : GoombaState
{
    private Timer deathTimer;
    public GoombaStateDie(Goomba _goomba, MovementComponent _movementComponent) : base(_goomba, _movementComponent)
    {
        deathTimer = new Timer();
        deathTimer.WaitTime = 2;
        deathTimer.Timeout += deathTimerTimeout;
        AddChild(deathTimer);
    }

    public override void Enter(int _previousStateId)
    {
        goomba.hurtBox.CollisionLayer = 0;
        goomba.hurtBox.CollisionMask = 0;
        goomba.skin.Animation = "DIE";
        goomba.CollisionLayer = 0;
        
        movementComponent.CurrentSpeedX = 0;
        movementComponent.AccelY = 100;

        goomba.chapeau.SautChapeau();
        goomba.chapeau.CallDeferred("reparent", goomba.GetParent());
        //goomba.chapeau.Position = goomba.Position;
        GD.Print("ololol", goomba.chapeau.Position);

        //goomba.chapeau.Reparent(goomba.GetParent());
        goomba.GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D").Play();

        deathTimer.Start();
    }

    public override void Exit(int _previousStateId)
    {

    }

    public override void PhysicsProcess(double _delta)
    {
       

        //movementComponent.CurrentSpeedY = (movementComponent.IsOnFloor()) ? 0 : 200;
        movementComponent.AccelerateToSpeedY(_delta);
        movementComponent.Advance();
    }


    private void deathTimerTimeout()
    {

        goomba.QueueFree();
        //

    }

}

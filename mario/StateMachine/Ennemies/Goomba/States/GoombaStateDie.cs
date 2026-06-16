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

        ScoreAnim scoreAnim = new ScoreAnim();
        scoreAnim.ScoreToShow = 100;
        scoreAnim.GlobalPosition = goomba.GlobalPosition ;
        goomba.AddSibling(scoreAnim);
        GD.Print(scoreAnim.GlobalPosition + "score a");

        goomba.EmitSignal(Goomba.SignalName.DiedSignal, 100);
        goomba.skin.Animation = "DIE";
        
        movementComponent.CurrentSpeedX = 0;
        movementComponent.AccelY = 100;
        
        goomba.chapeau.SautChapeau();
        goomba.chapeau.CallDeferred("reparent", goomba.GetParent());
        //goomba.chapeau.Position = goomba.Position;
        goomba.CollisionLayer = 0;
        goomba.CollisionMask = 1;
        goomba.hurtBox.CollisionLayer = 0;
        goomba.hitBox.CollisionLayer = 0;
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

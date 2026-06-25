using Godot;
using System;

public partial class KoopaStateDie : KoopaState
{   Node2D koopaBody;

    public KoopaStateDie(Koopa _koopa, MovementComponent _movementComponent) : base(_koopa, _movementComponent)
    {
        koopa = _koopa;
        koopaBody = koopa.GetNode<Node2D>("Body");
    }

    public override void Enter(int _previousStateId)
    {
        ScoreAnim scoreAnim = ResourceLoader.Load<PackedScene>("res://World/ScoreAnim.tscn").Instantiate<ScoreAnim>();
        scoreAnim.ScoreToShow = 200;
        koopaBody.Hide();
        
        koopa.shell.Animation = "die";
        koopa.shell.Play();
        scoreAnim.GlobalPosition = koopa.GlobalPosition- new Vector2(16,16); ;
        koopa.AddSibling(scoreAnim);
        movementComponent.CurrentSpeedX = 0;
        GD.Print(scoreAnim.GlobalPosition + "score a");
        koopa.CollisionLayer = 0;
        koopa.CollisionMask = 1;
        koopa.EmitSignal(Koopa.SignalName.DiedSignal, 200);
        Timer timer = new Timer();
        timer.WaitTime = 4;
        movementComponent.Advance();
        
        timer.Timeout += () =>
        {
            koopa.QueueFree();
        };
        AddChild(timer);
        timer.Start();

    }

    public override void Exit(int _previousStateId)
    {
        //throw new NotImplementedException();
    }

    public override void PhysicsProcess(double _delta)
    {
        //throw new NotImplementedException();
    }
}

using Godot;
using System;

public partial class KoopaStateDie : KoopaState
{

    public KoopaStateDie(Koopa _koopa, MovementComponent _movementComponent) : base(_koopa, _movementComponent)
    {
        koopa = _koopa;
    }

    public override void Enter(int _previousStateId)
    {
        ScoreAnim scoreAnim = ResourceLoader.Load<PackedScene>("res://World/ScoreAnim.tscn").Instantiate<ScoreAnim>();
        scoreAnim.ScoreToShow = 200;
        scoreAnim.GlobalPosition = koopa.GlobalPosition- new Vector2(16,16); ;
        koopa.AddSibling(scoreAnim);
        GD.Print(scoreAnim.GlobalPosition + "score a");
        koopa.CollisionLayer = 0;
        koopa.CollisionMask = 1;
        koopa.EmitSignal(Koopa.SignalName.DiedSignal, 200);
        koopa.QueueFree();
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

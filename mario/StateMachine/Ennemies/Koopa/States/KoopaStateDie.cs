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
        GD.Print("enter state : Die" + this.Name);
    }

    public override void Exit(int _previousStateId)
    {
        throw new NotImplementedException();
    }

    public override void PhysicsProcess(double _delta)
    {
        throw new NotImplementedException();
    }
}

using Godot;
using System;

public partial class KoopaStateWakeUp : KoopaState
{
	

    public KoopaStateWakeUp(Koopa _koopa, MovementComponent _movementComponent) : base(_koopa, _movementComponent)
    {
    }

    public override void Enter(int _previousStateId)
    {
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

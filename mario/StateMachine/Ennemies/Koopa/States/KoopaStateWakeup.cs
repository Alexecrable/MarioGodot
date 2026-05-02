using Godot;
using System;

public partial class KoopaStateWakeUp : KoopaState
{
	
    private Koopa koopa;

    public KoopaStateWakeUp(Koopa _koopa) : base(_koopa)
    {
        koopa = _koopa;
    }

    public override void Enter(int _previousStateId)
    {
        throw new NotImplementedException();
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

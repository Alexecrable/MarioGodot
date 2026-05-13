using Godot;
using System;

public abstract partial class KoopaState : State
{
	
    protected Koopa koopa;
    protected MovementComponent movementComponent;

    public KoopaState(Koopa _koopa, MovementComponent _movementComponent)
    {
        koopa = _koopa;
        movementComponent = _movementComponent;
    }

	
	
}

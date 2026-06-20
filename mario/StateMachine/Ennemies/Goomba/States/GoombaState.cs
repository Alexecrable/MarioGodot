using Godot;
using System;

public abstract partial class GoombaState : State
{
	
    protected Goomba goomba;
    protected MovementComponent movementComponent;

    public GoombaState(Goomba _goomba, MovementComponent _movementComponent)
    {
        goomba = _goomba;
        movementComponent = _movementComponent;
    }

	
	
}

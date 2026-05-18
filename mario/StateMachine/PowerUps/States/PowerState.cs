using Godot;
using System;
using System.Threading;


public abstract partial class PowerState : Node
{
    protected Powerup powerUp;
    protected MovementComponent movementComponent;
    [Signal]
    public delegate void finishedEventHandler(int stateIndex);

    public PowerState(Powerup _powerUp, MovementComponent _movementComponent)
    {
        powerUp = _powerUp;
        movementComponent = _movementComponent;
    }

    public abstract void enter();
    public abstract void exit();
    public abstract void PhysicsProcess(double delta);
	
	
}

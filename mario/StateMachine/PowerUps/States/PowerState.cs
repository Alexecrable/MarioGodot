using Godot;
using System;
using System.Threading;


public abstract partial class PowerState : Node
{
    protected Powerup powerUp;
    [Signal]
    public delegate void finishedEventHandler(int stateIndex);

    public PowerState(Powerup _powerUp)
    {
        powerUp = _powerUp;
    }

    public abstract void enter();
    public abstract void exit();
    public abstract void PhysicsProcess(double delta);
	
	
}

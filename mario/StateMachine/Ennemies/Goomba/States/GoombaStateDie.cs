using Godot;
using System;

public partial class GoombaStateDie : GoombaState
{
	private Timer deathTimer;
	public GoombaStateDie(Goomba _goomba) : base(_goomba)
    {
        deathTimer = new Timer();
        deathTimer.WaitTime = 20;
        deathTimer.Timeout +=deathTimerTimeout;

        AddChild(deathTimer);
    }

    public override void Enter(int _previousStateId)
    {
        goomba.skin.Animation = "DeathAnim";
        deathTimer.Start();
    }

    public override void Exit(int _previousStateId)
    {
        throw new NotImplementedException();
    }

    public override void PhysicsProcess(double _delta)
    {
        throw new NotImplementedException();
    }
	

    private void deathTimerTimeout()
    {
        EmitSignal(SignalName.Finished, -1);
        //
        goomba.QueueFree();
    }
	
}

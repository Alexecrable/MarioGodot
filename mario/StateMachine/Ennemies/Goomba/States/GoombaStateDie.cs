using Godot;
using System;

public partial class GoombaStateDie : GoombaState
{
	private Timer deathTimer;
	public GoombaStateDie(Goomba _goomba) : base(_goomba)
    {
        deathTimer = new Timer();
        deathTimer.WaitTime = 2;
        deathTimer.Timeout +=deathTimerTimeout;

        AddChild(deathTimer);
    }

    public override void Enter(int _previousStateId)
    {
        goomba.skin.Animation = "DIE";
        deathTimer.Start();
    }

    public override void Exit(int _previousStateId)
    {
        
    }

    public override void PhysicsProcess(double _delta)
    {
    }
	

    private void deathTimerTimeout()
    {
        goomba.QueueFree();
        //
        
    }
	
}

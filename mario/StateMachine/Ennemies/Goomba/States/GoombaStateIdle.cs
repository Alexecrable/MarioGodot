using Godot;
using System;

public partial class GoombaStateIdle : GoombaState
{

	public GoombaStateIdle(Goomba _goomba) : base(_goomba)
    {
        VisibleOnScreenNotifier2D notifier = goomba.getNotifier();
        notifier.ScreenEntered += ScreenEntered;
    }

    public override void Enter(int _previousStateId)
    {
        goomba.skin.Pause();
    }

    public override void Exit(int _previousStateId)
    {
        
    }
	
	public override void PhysicsProcess(double _delta)
    {
        
    }

    private void ScreenEntered()
    {
        EmitSignal(SignalName.Finished, (int)GoombaStateEnum.WALK);
    }
}

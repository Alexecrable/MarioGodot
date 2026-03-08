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
        throw new NotImplementedException();
    }
	
	public override void PhysicsProcess(double _delta)
    {
        throw new NotImplementedException();
    }

    private void ScreenEntered()
    {
        EmitSignal(SignalName.Finished, 1);
    }
}

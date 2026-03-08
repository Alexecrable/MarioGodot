using Godot;
using System;

public partial class GoombaStateWalk : GoombaState
{
	
	public GoombaStateWalk(Goomba _goomba) : base(_goomba)
    {
        VisibleOnScreenNotifier2D notifier = goomba.getNotifier();
        notifier.ScreenExited += ScreenExited;
    }

    public override void Enter(int _previousStateId)
    {
        goomba.skin.Play();
    }

    public override void Exit(int _previousStateId)
    {
        throw new NotImplementedException();
    }
	
	public override void PhysicsProcess(double _delta)
    {
    
    }

    private void ScreenExited()
    {
        EmitSignal(SignalName.Finished, 0);
    }
}

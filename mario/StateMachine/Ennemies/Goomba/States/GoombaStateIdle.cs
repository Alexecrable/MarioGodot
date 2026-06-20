using Godot;
using System;

public partial class GoombaStateIdle : GoombaState
{

	public GoombaStateIdle(Goomba _goomba, MovementComponent _movementComponent) : base(_goomba, _movementComponent)
    {
        VisibleOnScreenNotifier2D notifier = goomba.getNotifier();
        notifier.ScreenEntered += ScreenEntered;
    }

    public override void Enter(int _previousStateId)
    {
        goomba.skin.Pause();
        movementComponent.CurrentSpeedX = 0;
        movementComponent.Advance();
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

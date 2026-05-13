using Godot;
using System;

public partial class KoopaStateIdle : KoopaState
{

    public KoopaStateIdle(Koopa _koopa, MovementComponent _movementComponent) : base(_koopa, _movementComponent)
    {
        VisibleOnScreenNotifier2D notifier = koopa.getNotifier();
        notifier.ScreenEntered += ScreenEntered;
    }

    public override void Enter(int _previousStateId)
    {
        GD.Print("enter state : Idle" + this.Name);

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
        EmitSignal(SignalName.Finished, (int)KoopaStateEnum.WALK);
    }
}


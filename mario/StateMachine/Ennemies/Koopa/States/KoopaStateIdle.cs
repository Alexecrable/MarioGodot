using Godot;
using System;

public partial class KoopaStateIdle : KoopaState
{
    VisibleOnScreenNotifier2D notifier;
    public KoopaStateIdle(Koopa _koopa, MovementComponent _movementComponent) : base(_koopa, _movementComponent)
    {
        notifier = koopa.getNotifier();
    }

    public override void Enter(int _previousStateId)
    {
        GD.Print("enter state : Idle" + this.Name);
        notifier.ScreenEntered += ScreenEntered;

        movementComponent.CurrentSpeedX = 0;
        movementComponent.Advance();
    }
    public override void Exit(int _previousStateId)
    {
        notifier.ScreenEntered -= ScreenEntered;

    }

    public override void PhysicsProcess(double _delta)
    {
    }

    private void ScreenEntered()
    {
        EmitSignal(SignalName.Finished, (int)KoopaStateEnum.WALK);
    }
}


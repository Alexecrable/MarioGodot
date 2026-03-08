using Godot;
using Godot.Collections;
using System;
using System.Runtime.CompilerServices;

public partial class Koopa : Ennemi
{

    [Export]
    private int baseExportTest = 3;
    public enum KoopaStateEnum
    {
        IDLE = 0,
        WALK = 1,
        DIE = 2,
        SHELL_IDLE = 3,
        SHELL_MOVE = 4,
        WAKEUP = 5
    }

    private Array<KoopaState> states;
    private VisibleOnScreenNotifier2D notifier;


    public override void _Ready()
    {
        InitState();
    }

    public override void InitState()
    {
        states = [
            new KoopaStateIdle(this),
            new KoopaStateWalk(this),
            new KoopaStateDie(this),
            new KoopaStateShellIdle(this),
            new KoopaStateShellMove(this),
            new KoopaStateWakeUp(this)
        ];

        foreach (KoopaState state in states)
        {
            AddChild(state);
            state.Finished += ChangeState;

        }
        currentStateIndex = 0;
        lastStateIndex = 0;
        states[currentStateIndex].Enter(lastStateIndex);

    }

    public override void ChangeState(int _stateIndex)
    {
        int lastIndex = currentStateIndex;
        states[currentStateIndex].Exit(_stateIndex);
        currentStateIndex = _stateIndex;
        states[currentStateIndex].Enter(lastIndex);

    }

    public override void _PhysicsProcess(double _delta)
    {
        states[currentStateIndex].PhysicsProcess(_delta);
        MoveAndSlide();
    }

    public VisibleOnScreenNotifier2D getNotifier()
    {
        return notifier;
    }

    public override void MakeHit()
    {
        EmitSignal(SignalName.Hit);
    }



}

using Godot;
using Godot.Collections;
using System;
using System.Runtime.CompilerServices;
 public enum GoombaStateEnum
    {
        IDLE = 0,
        WALK = 1,
        DIE = 2
    }
public partial class Goomba : Ennemi
{
   

    private Array<GoombaState> states;
    private VisibleOnScreenNotifier2D notifier;
    public int currentYVelocity = 0;
    public int currentXVelocity = 0;

    public override void _Ready()
    {
        
        notifier = GetNode<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier2D");
        skin = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        hitBox = GetNode<Area2D>("HitBox");
        skin.Animation = "WALK";
        skin.Pause();
        InitState();
    }

    public override void InitState()
    {
        states = [
            new GoombaStateIdle(this),
            new GoombaStateWalk(this),
            new GoombaStateDie(this)
        ];

        foreach (GoombaState state in states)
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




}

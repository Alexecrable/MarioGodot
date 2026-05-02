using Godot;
using System;
using Godot.Collections;
public abstract partial class Ennemi : CharacterBody2D, IStateMachine
{
    [Export]
    private int xdddddd = 32;
    [Signal]
    public delegate void HitEventHandler();
	public AnimatedSprite2D skin;
    public int currentStateIndex, lastStateIndex;

    public abstract void InitState();
    public abstract void ChangeState(int _stateId);

    public abstract void MakeHit();
	
	
}

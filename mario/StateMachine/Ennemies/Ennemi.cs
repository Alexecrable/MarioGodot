using Godot;
using System;
using Godot.Collections;
public abstract partial class Ennemi : CharacterBody2D, IStateMachine
{
    [Export]
    private int xdddddd = 32;
	public AnimatedSprite2D skin;
    public Area2D hitBox;
    public int currentStateIndex, lastStateIndex;

    public abstract void InitState();
    public abstract void ChangeState(int _stateId);
	
	
}

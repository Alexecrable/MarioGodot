using Godot;
using System;

public abstract partial class MarioState : State
{
    // Called when the node enters the scene tree for the first time.

    protected Mario mario;

    public MarioState(Mario _mario)
    {
        mario = _mario;
    }
	
}

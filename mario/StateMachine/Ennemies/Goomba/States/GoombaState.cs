using Godot;
using System;

public abstract partial class GoombaState : State
{
	
    protected Goomba goomba;

    public GoombaState(Goomba _goomba)
    {
        goomba = _goomba;
    }

	
	
}

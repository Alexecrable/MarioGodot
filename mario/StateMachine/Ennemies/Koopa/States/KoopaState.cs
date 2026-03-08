using Godot;
using System;

public abstract partial class KoopaState : State
{
	
    private Koopa koopa;

    public KoopaState(Koopa _koopa)
    {
        koopa = _koopa;
    }

	
	
}

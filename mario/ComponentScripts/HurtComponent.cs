using Godot;
using System;
[GlobalClass]
public partial class HurtComponent : Area2D
{
	[Signal]
public delegate void HurtEventHandler(Area2D area);
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AreaEntered += MakeHurt;
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void MakeHurt(Area2D _area)
	{
		GD.Print("xbox2" + _area.Name);
		EmitSignal(SignalName.Hurt, _area);
	}

	
}

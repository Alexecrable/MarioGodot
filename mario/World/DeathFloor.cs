using Godot;
using System;

public partial class DeathFloor : Area2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//AreaEntered += KillEveryArea;
		BodyEntered += KillEveryBody;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}


	private void KillEveryBody(Node _body)
	{
		IInstaKillableObject instaKillableObject = (IInstaKillableObject)_body;
		instaKillableObject.InstaKill();
	}

	
}

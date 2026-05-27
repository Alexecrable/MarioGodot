using Godot;
using System;

public partial class DeathFloor : Area2D
{
	// Called when the node enters the scene tree for the first time.
	private Timer timer;
	public override void _Ready()
	{
		//AreaEntered += KillEveryArea;
		timer = new Timer();
		
		AddChild(timer);
		timer.Start();
		//BodyEntered += KillEveryBody
		//setup();
		GD.Print("insta : I AM READY" + timer.TimeLeft);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		GD.Print("insta : process " + Monitoring);
	}

	public void setup()
	{	
		GD.Print("loaad " + this);
		BodyEntered += KillEveryBody;
	}


	private void KillEveryBody(Node _body)
	{
	
		//BodyEntered -= KillEveryBody;
		IInstaKillableObject instaKillableObject = (IInstaKillableObject)_body;
		CharacterBody2D body = (CharacterBody2D)_body;
		//body.Position = Vector2.Zero;
		GD.Print("insta : " + timer.TimeLeft + " | " + body.Position);
		instaKillableObject.InstaKill();

		
	}
	

	
}

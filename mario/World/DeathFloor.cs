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
		GD.Print("insta : process; " + Monitoring);
	}

	public void setup()
	{	
		GD.Print("loaad " + this);
		BodyEntered += KillEveryBody;
	}


	private void KillEveryBody(Node _body)
	{
	
		try
		{
			Mario mario = (Mario)_body;
			mario.InstaKill();
		}
		catch 
		{
			try
			{
				FireBall fireBall = (FireBall)_body;
				fireBall.Disappear("deahtfloor");
			}
			catch
			{
				
			_body.QueueFree();
			}
		}
	}
	

	
}

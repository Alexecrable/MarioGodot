using Godot;
using System;

public partial class Star : Powerup
{

	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
        powerEnum = (int)Mario.PowerEnum.STAR;
		xVelocity = 100;
		yVelocity = 0;
	}

	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
           
			if (IsOnFloor())
			{
				movementComponent.CurrentSpeedY = yVelocity;
			}
        base._PhysicsProcess(delta);

			//movementComponent.AccelerateToSpeedY(delta);
	}

	public void Start()
	{
		yVelocity = -300;
	}


}

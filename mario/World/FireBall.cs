using Godot;
using System;

public partial class FireBall : CharacterBody2D
{

	private MovementComponent movementComponent;
	private Area2D area2D;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		movementComponent = GetNode<MovementComponent>("MovementComponent");
		movementComponent.Init(this);
		movementComponent.CurrentSpeedX = 300;
		movementComponent.CurrentSpeedY = 200;
		movementComponent.MaxSpeedY = 200;
		movementComponent.AccelY = 1000;
		area2D = GetNode<Area2D>("Area2D");
		area2D.BodyEntered += EnnemiKill;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (IsOnWall())
		{
			QueueFree();
		}
		else
		{

			if (IsOnFloor())
			{
				movementComponent.CurrentSpeedY = -300;
			}

			movementComponent.AccelerateToSpeedY(delta);
			movementComponent.Advance();
			MoveAndSlide();
		}

	}
	private void EnnemiKill(Node _body)
	{
		Ennemi ennemi = (Ennemi)_body;
		ennemi.InstaKill();
		QueueFree();
	}
	public void SetDirection(int direction)
	{
		movementComponent.CurrentSpeedX = movementComponent.CurrentSpeedX * direction;
	}
}

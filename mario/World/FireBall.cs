using Godot;
using System;

public partial class FireBall : CharacterBody2D
{

	private MovementComponent movementComponent;
	private Area2D area2D;
	private Vector2 restingPosition = new Vector2(0,0);
	public bool Used {get; set;}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Used = false;
		movementComponent = GetNode<MovementComponent>("MovementComponent");
		movementComponent.Init(this);
		movementComponent.CurrentSpeedX = 0;
		movementComponent.CurrentSpeedY = 0;
		area2D = GetNode<Area2D>("Area2D");
		area2D.BodyEntered += EnnemiKill;
		area2D.AreaEntered += KillFloorReached;
		movementComponent.MaxSpeedY = 0;
		movementComponent.AccelY = 1000;
		//Position = restingPosition;
		GlobalPosition = restingPosition;
		movementComponent.Advance();
	}

	private void KillFloorReached(Node _area)
	{
		Disappear("killFloor");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (IsOnWall())
		{
			GD.Print("fireball : Position " + GlobalPosition + " " + IsOnWall());	
			Disappear("wall");
		}
		else
		{

			if (IsOnFloor())
			{
				movementComponent.CurrentSpeedY = -300;
			}

			movementComponent.AccelerateToSpeedY(delta);
			movementComponent.Advance();
			
		}
		MoveAndSlide();

	}
	private void EnnemiKill(Node _body)
	{
		Ennemi ennemi = (Ennemi)_body;
		ennemi.InstaKill();
		Disappear("ennemiKill");
	}

	public void Launch(Vector2 _position, int _direction)
	{
		GD.Print("FireBall : Launch" + Name);
		Position = _position;
		Used = true;
		movementComponent.CurrentSpeedX = 300 * _direction;
		movementComponent.MaxSpeedY = 200;
	}

	private void Disappear(string str)
	{
		GD.Print("Fireball : Disappear"+str);
		GlobalPosition = restingPosition;
		Used = false;
		movementComponent.CurrentSpeedX = 0;
		movementComponent.CurrentSpeedY = 0;
		movementComponent.MaxSpeedY = 0;	
	}


}

using Godot;
using System;
using System.Dynamic;
[GlobalClass]
public partial class MovementComponent : Node
{

	private float direction, isJumping, maxSpeedX, maxSpeedY, xAccel, yAccel, currentySpeed;
	public float CurrentXSpeed{get; set;}
	


	private CharacterBody2D body;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		direction = 0;
		
		
	}

	public void Init(CharacterBody2D _body)
	{
		body = _body;
		maxSpeedX = 0;
		maxSpeedY = 0;
		xAccel = 0;
		yAccel = 0;
		CurrentXSpeed = 0;
		currentySpeed = 0;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public void Advance(double _delta)
	{
		GD.Print("je suis làaaa");
		body.Velocity = new Vector2(CurrentXSpeed, currentySpeed);
		GD.Print(body.Velocity);
	}

	public void AccelerateToSpeedX(double _delta)
	{
		GD.Print("maxSpeed : " + maxSpeedX);
		GD.Print("current : " + CurrentXSpeed * direction);
		if(maxSpeedX > CurrentXSpeed * direction)
		{
			CurrentXSpeed += xAccel * direction * (float) _delta;
		}
		else
		{
			CurrentXSpeed = maxSpeedX * direction;
		}
	}


	public void AccelerateToSpeedY(double _delta)
	{
		GD.Print(maxSpeedY, currentySpeed, yAccel);
		if(maxSpeedY > currentySpeed)
		{
			currentySpeed += yAccel * (float) _delta;
			
		}
		else
		{
			currentySpeed = maxSpeedY;
		}
	}
	public void SetDirection(float _leftInput, float _rightInput)
	{
		direction = _rightInput -_leftInput;
	}

	public void SetCurrentYVel(float _yVel)
	{
		currentySpeed = _yVel;
	}


	public void SetxAccel(float _xAccel)
	{
		xAccel = _xAccel;
	}

	public void SetyAccel(float _yAccel)
	{
		yAccel = _yAccel;
	}


	public void SetMaxSpeedX(float _maxSpeedX)
	{
		maxSpeedX = _maxSpeedX;
	}

	public void SetMaxSpeedY(float _maxSpeedY)
	{
		maxSpeedY = _maxSpeedY;
	}
	public void SetJump(float _jumpInput)
	{
		isJumping = _jumpInput;
	}
}

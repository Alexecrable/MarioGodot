using Godot;
using System;
using System.Dynamic;
using System.Text.Json.Serialization;
[GlobalClass]
public partial class MovementComponent : Node
{

	private float direction, wantsToJump, wantsToCrouch;
	public float CurrentSpeedX{get; set;}
	public float CurrentSpeedY{get; set;}
	public float MaxSpeedX{get; set;}
	public float MaxSpeedY{get; set;}
	public float BaseSpeedX{get; set;}
	public float AccelX{get; set;}
	public float AccelY{get; set;}
	


	private CharacterBody2D body;
	// Called when the node enters the scene tree for the first time.
	
	public override void _Ready()
	{
		direction = 0;
		
		
	}

	public void Init(CharacterBody2D _body)
	{
		body = _body;
		BaseSpeedX = 250;
		MaxSpeedX = 300;
		MaxSpeedY = 700;
		AccelX = 700;
		AccelY = 2000;
		CurrentSpeedX = 250;
		CurrentSpeedY = 0;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public void Advance()
	{
		body.Velocity = new Vector2(CurrentSpeedX, CurrentSpeedY);
	}

	public bool IsOnCeiling()
	{
		return body.IsOnCeiling();
	}

	public bool IsOnFloor()
	{
		return body.IsOnFloor();
	}

	public void SpeedX()
	{
		CurrentSpeedX = BaseSpeedX * direction;
	}

	public void AccelerateToSpeedX(double _delta)
	{
		
		CurrentSpeedX += AccelX * direction * (float) _delta;
		if(MaxSpeedX < CurrentSpeedX * direction)
		{
			CurrentSpeedX = MaxSpeedX * direction;
		}
	}

	


	public void AccelerateToSpeedY(double _delta)
	{
		
		if(MaxSpeedY > CurrentSpeedY)
		{
			CurrentSpeedY += AccelY * (float) _delta;
			
		}
		else
		{
			CurrentSpeedY = MaxSpeedY;
		}
	}
	public void SetDirection(float _leftInput, float _rightInput)
	{
		direction = _rightInput -_leftInput;
	}

	
	public void SetJump(float _jumpInput)
	{
		wantsToJump = _jumpInput;
	}

	public void SetDownInput(float _downInput)
	{
		wantsToCrouch = _downInput;
	}

	public bool WantsJump()
	{
		return wantsToJump == 1;
	}

	public bool IsMoving()
	{
		return direction != 0;
	}

	public bool WantsToCrouch()
	{
		return wantsToCrouch > 0;
	}
    

	
}

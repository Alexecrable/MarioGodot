using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Security.AccessControl;

public partial class Mario : CharacterBody2D
{
	// Called when the node enters the scene tree for the first time.
public enum StateEnum
{
	IDLE = 0,
	JUMP = 1,
	FALL = 2,
	MOVE = 3,
	WALLSLIDE = 4
}

	[Export]
	public int
		speed = 250,
		startingGravity = 20,
		terminalGravity = 700,
		gravityAccel = 2000,
		endJumpGravity = -300,
		startJumpGravity = -200,
		maxHorizontalVelocity = 300,
		airborneHorizontalAccel = 700,
		groundedHorizontalAccel = 5000;
	[Export]
	public float jumpTime = 1;
	public float yVelocity, rightInput, leftInput, jumpInput, currentHorizontalVelocity = 250;
	private int currentStateIndex;
	private Array<MarioState> states;
	public GpuParticles2D particles;

	public AnimatedSprite2D animation;
	
	private Camera2D camera;
	public RayCast2D raycastRight;
	public RayCast2D raycastLeft;
	public AudioStreamPlayer2D sfxPlayer;
	private Area2D hitBox;
	private bool isHitting;

	
	public override void _Ready()
	{

		yVelocity = startingGravity;
						GD.Print("sfx2");

		
						GD.Print("sfx3");

		animation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		camera = GetNode<Camera2D>("Camera2D");
		raycastLeft = GetNode<RayCast2D>("RayCast2D2");
		raycastRight = GetNode<RayCast2D>("RayCast2D");
		particles = GetNode<GpuParticles2D>("GPUParticles2D");
		particles.Emitting = false;
		sfxPlayer = GetNode<AudioStreamPlayer2D>("SFXPlayer");
		hitBox = GetNode<Area2D>("HitBox");
		hitBox.BodyEntered += Hitting;
		isHitting = false;
		InitStates();

	}

	private void Hitting(Node _body)
	{

		GD.Print("bodddd " + _body.GetGroups());
		
		
		if (_body.IsInGroup("Blocks") && !IsGoingDown() && !isHitting)
		{
			isHitting = true;
			PowerBlock powerBlock = (PowerBlock)_body;
			powerBlock.Collision();
		}
	}

	private void InitStates()
	{

		states = [
			new StateIdle(this),
			new StateJumping(this),
			new StateFalling(this),
			new StateRunning(this),
			new StateWallSlide(this)
			];

		foreach (MarioState state in states)
		{
			AddChild(state);
			state.Finished += ChangeState;
		}
		currentStateIndex = 0;
	}

	private void ChangeState(int stateIndex)
	{
		int lastIndex = currentStateIndex;
		states[currentStateIndex].Exit(stateIndex);
		currentStateIndex = stateIndex;
		states[currentStateIndex].Enter(lastIndex); 

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		camera.GlobalPosition = new Vector2(Position.X + 30, 450);
		
	}

	public override void _PhysicsProcess(double delta)
	{
		takeInputs();

		states[currentStateIndex].PhysicsProcess(delta);


		
		
		MoveAndSlide();
		
		
	}

	private void takeInputs()
	{
		rightInput = Input.GetActionStrength("right");
		leftInput = Input.GetActionStrength("left");
		jumpInput = Input.GetActionStrength("jump");
		
	}


	public void SetGoingUp()
	{
		GD.Print("goingUp");
		hitBox.Position = new Vector2(0,-28);
		//head.SetDeferred("monitorable", _decision);
		isHitting = false;
	}

	public void SetGoingDown()
	{
		GD.Print("GoingDown");
		hitBox.Position = new Vector2(0,0);
		isHitting = false;
	}
	public bool IsGoingDown()
	{
		return (hitBox.Position.Y == 0) ? true : false;
	}

	public void SetGoingNeutral()
	{
		isHitting = false;
		GD.Print("goingNeutral");
		hitBox.Position = new Vector2(0,-14);
	}

	
	


}

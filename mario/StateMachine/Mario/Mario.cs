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
	private Area2D headBox, feetBox;
	private bool isHittingUp, isHittingDown;

	
	public override void _Ready()
	{

		yVelocity = startingGravity;

		animation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		camera = GetNode<Camera2D>("Camera2D");
		raycastLeft = GetNode<RayCast2D>("RayCast2D2");
		raycastRight = GetNode<RayCast2D>("RayCast2D");
		particles = GetNode<GpuParticles2D>("GPUParticles2D");
		particles.Emitting = false;
		sfxPlayer = GetNode<AudioStreamPlayer2D>("SFXPlayer");
		headBox = GetNode<Area2D>("HeadBox");
		headBox.BodyEntered += HittingUp;
		feetBox = GetNode<Area2D>("FeetBox");
		feetBox.BodyEntered += HittingDown;
		
		isHittingUp = false;

		InitStates();

	}

	private void HittingUp(Node _body)
	{
		
		//headBox.Monitoring = false;
		if (!isHittingUp)
		{
			isHittingUp = true;
			
			PowerBlock powerBlock = (PowerBlock)_body;
			powerBlock.Collision();
		}
		
	}

	private void HittingDown(Node _body)
	{
		
		//headBox.Monitoring = false;
		if (!isHittingDown)
		{
			isHittingDown = true;
			
			Ennemi ennemi = (Ennemi)_body;
			ennemi.MakeHit();
			ChangeState((int)StateEnum.JUMP);
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
		headBox.SetDeferred("monitoring",true);
		feetBox.SetDeferred("monitoring",false);
		//head.SetDeferred("monitorable", _decision);
		isHittingUp = false;
		isHittingDown = false;
	}

	public void SetGoingDown()
	{
		headBox.SetDeferred("monitoring",false);
		feetBox.SetDeferred("monitoring",true);
		isHittingUp = false;
		isHittingDown = false;

	}
	public bool IsGoingDown()
	{
		return feetBox.Monitoring;
	}

	public void SetGoingNeutral()
	{
		isHittingUp = false;
		isHittingDown = false;
		headBox.SetDeferred("monitoring",false);
		feetBox.SetDeferred("monitoring",false);
	}

	
	


}

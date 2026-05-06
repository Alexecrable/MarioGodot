using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Security.AccessControl;

public partial class Mario : CharacterBody2D
{
	// Called when the node enters the scene tree for the first time.
	public enum PowerEnum
	{
		BASE = 0,
		BIG = 1,
		FIRE = 2
	}
	public enum StateEnum
	{
		IDLE = 0,
		JUMP = 1,
		FALL = 2,
		MOVE = 3,
		WALLSLIDE = 4,
		HURT = 5
	}
	InputComponent inputComponent;
	MovementComponent movementComponent;
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
	private Tween tween;

	private Camera2D camera;
	public RayCast2D raycastRight;
	public RayCast2D raycastLeft;
	public AudioStreamPlayer2D sfxPlayer;
	private Area2D headBox, feetBox, hurtBox;
	private bool isHittingUp, isHittingDown;
	private Timer invincibilityTimer;
	public int currentPowerUpIndex;

	public override void _Ready()
	{



		currentPowerUpIndex = 1;
		Scale = new Vector2(1, 1.5f);

		yVelocity = startingGravity;
		inputComponent = GetNode<InputComponent>("InputComponent");
		movementComponent = GetNode<MovementComponent>("MovementComponent");
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
		hurtBox = GetNode<Area2D>("HurtBox");
		hurtBox.BodyEntered += GetHurt;


		isHittingUp = false;

		InitStates();

		invincibilityTimer = new Timer();
		invincibilityTimer.WaitTime = 3;
		invincibilityTimer.Timeout += InvincibilityEnd;
		invincibilityTimer.OneShot = true;
		AddChild(invincibilityTimer);

		movementComponent.Init(this);
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
		GD.Print("hittt" + _body);
		//headBox.Monitoring = false;
		if (!isHittingDown)
		{
			isHittingDown = true;

			//Ennemi ennemi = (Ennemi)_body;
			//ennemi.MakeHit();
			ChangeState((int)StateEnum.JUMP);
		}

	}

	private void GetHurt(Node _body)
	{
		if (currentPowerUpIndex == 0)
		{
			Die();
		}
		else
		{
			currentPowerUpIndex--;
			hurtBox.Monitoring = false;
			invincibilityTimer.Start();
			ChangeState((int)StateEnum.HURT);
			tween = CreateTween();
			tween.SetTrans(Tween.TransitionType.Sine);
			tween.SetLoops();
			tween.TweenProperty(animation, "modulate", new Color(0, 0, 0), 0.2);
			tween.TweenProperty(animation, "modulate", new Color(1, 1, 1), 0.2);
		}
	}

	private void InvincibilityEnd()
	{
		hurtBox.Monitoring = true;
		tween.Stop();
		animation.Modulate = new Color(1,1,1);
	}

	private void Die()
	{
		QueueFree();
	}

	private void InitStates()
	{

		states = [
			new StateIdle(this),
			new StateJumping(this),
			new StateFalling(this, movementComponent),
			new StateRunning(this),
			new StateWallSlide(this),
			new StateHurt(this)
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
		inputComponent.takeInputs();
		leftInput = inputComponent.getLeftInput();
		rightInput = inputComponent.getRightInput();
		jumpInput = inputComponent.getJumpInput();
		
		movementComponent.SetDirection(inputComponent.getLeftInput(), inputComponent.getRightInput());
		

		states[currentStateIndex].PhysicsProcess(delta);




		MoveAndSlide();


	}

	


	public void SetGoingUp()
	{
		headBox.SetDeferred("monitoring", true);
		feetBox.SetDeferred("monitoring", false);
		//head.SetDeferred("monitorable", _decision);
		isHittingUp = false;
		isHittingDown = false;
	}

	public void SetGoingDown()
	{
		headBox.SetDeferred("monitoring", false);
		feetBox.SetDeferred("monitoring", true);
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
		headBox.SetDeferred("monitoring", false);
		feetBox.SetDeferred("monitoring", false);
	}


	public bool IsGrabbingWall()
	{
		bool marioGoesRight = rightInput - leftInput > 0;
		bool marioGoesLeft = rightInput - leftInput < 0;
		bool marioGrabsWall = (raycastLeft.IsColliding() && marioGoesLeft) || (raycastRight.IsColliding() && marioGoesRight);
		return marioGrabsWall;
	}

	public bool IsRunning()
	{
		bool isRunning = rightInput + leftInput > 0;
		return isRunning;
	}







}

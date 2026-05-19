using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Security.AccessControl;


/// BUGS
/// 
/// Mario couleur ne va pas bien su rla fleur si il la prend pendant un HIT (save une variable de couleur pr qu'ils puissent adapter le tween)
/// -> Couleur var créée, mais c degueu, idealement je fais un etat powerUp pour casser l'invincibilité et faire ça proprement
/// 
/// CHapeau de Mario pas toujours dans la bonne dir
public partial class Mario : CharacterBody2D
{

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
	// Composants externes (Composition) //
	private InputComponent inputComponent;
	private MovementComponent movementComponent;
	private HurtComponent hurtComponent;
	/////////////////////////////

	public Area2D headBox, feetBox;
	public int
		speed = 250,
		startingGravity = 0,
		terminalGravity = 700,
		gravityAccel = 2000,
		endJumpGravity = -300,
		startJumpGravity = -400,
		maxHorizontalVelocity = 300,
		airborneHorizontalAccel = 700,
		groundedHorizontalAccel = 5000,
		currentStateIndex,
		currentPowerUpIndex = 1;
	public float jumpTime = 0.2f,
		yVelocity,
		rightInput,
		leftInput,
		jumpInput,
		currentHorizontalVelocity = 250;
	public bool actionInput,
		isHittingDown,
		isHittingUp = false;
	private Array<MarioState> states;

	//// Misc. ////
	public GpuParticles2D particles;
	public AnimatedSprite2D animation;
	public Sprite2D chapeau;
	private Tween tween;

	private Camera2D camera;
	public RayCast2D raycastRight, raycastLeft;
	public AudioStreamPlayer2D sfxPlayer;
	PackedScene fireScene;

	private Timer invincibilityTimer, fireTimer;
	private Color currentColor = new Color(1,1,1);
	/////////


	public override void _Ready()
	{


		//n'avoir que 2 boules de feu qui cyclent entre elles
		//leur faire lacher un ptit effet d'explosion on contact
		//les TP a un endroit random le temps d'etre rappellée
		fireTimer = new Timer();

		inputComponent = GetNode<InputComponent>("InputComponent");
		movementComponent = GetNode<MovementComponent>("MovementComponent");
		movementComponent.Init(this);
		hurtComponent = GetNode<HurtComponent>("HurtComponent");




		animation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		camera = GetNode<Camera2D>("Camera2D");
		raycastLeft = GetNode<RayCast2D>("RayCast2D2");
		raycastRight = GetNode<RayCast2D>("RayCast2D");
		particles = GetNode<GpuParticles2D>("GPUParticles2D");
		sfxPlayer = GetNode<AudioStreamPlayer2D>("SFXPlayer");
		headBox = GetNode<Area2D>("HeadBox");
		feetBox = GetNode<Area2D>("FeetBox");
		chapeau = GetNode<Sprite2D>("Chapeau");
		fireScene = ResourceLoader.Load<PackedScene>("res://World/FireBall.tscn");


		Scale = new Vector2(1, 1.5f);

		yVelocity = startingGravity;
		particles.Emitting = false;



		invincibilityTimer = new Timer();
		invincibilityTimer.WaitTime = 3;
		invincibilityTimer.OneShot = true;
		AddChild(invincibilityTimer);
		ConnectSignals();
		InitStates();
	}

	private void ConnectSignals()
	{
		headBox.BodyEntered += HittingUp;
		feetBox.AreaEntered += HittingDownArea;
		hurtComponent.Hurt += GetHurt;
		invincibilityTimer.Timeout += InvincibilityEnd;
	}

	private void DisconnectSignals()
	{
		headBox.BodyEntered -= HittingUp;
		feetBox.AreaEntered -= HittingDownArea;
		hurtComponent.Hurt -= GetHurt;
	}

	private void HittingUp(Node _body)
	{

		//headBox.Monitoring = false;
		if (!isHittingUp)
		{
			isHittingUp = true;

			PowerBlock powerBlock = (PowerBlock)_body;
			powerBlock.Collision(currentPowerUpIndex);
		}



	}



	private void HittingDownArea(Node _body)
	{
		//headBox.Monitoring = false;
		if (!isHittingDown)
		{
			isHittingDown = true;

			ChangeState((int)StateEnum.JUMP);
		}

	}

	private void GetHurt()
	{
		if (currentPowerUpIndex == 0)
		{
			DisconnectSignals();
			Die();
		}
		else
		{
			currentColor = new Color(1,1,1);
			animation.Modulate = currentColor;
			currentPowerUpIndex--;
			hurtComponent.SetDeferred("monitoring", false);
			invincibilityTimer.Start();
			ChangeState((int)StateEnum.HURT);
			tween = CreateTween();
			tween.SetTrans(Tween.TransitionType.Sine);
			tween.SetLoops();
			tween.TweenProperty(animation, "modulate", new Color(0, 0, 0), 0.2);
			tween.TweenProperty(animation, "modulate", new Color(1, 1, 1), 0.2);
			
		}
	}

	public Texture2D GetHat(Texture2D _texture)
	{
		Texture2D textToReturn = chapeau.Texture;
		chapeau.Texture = _texture;

		return textToReturn;
	}
	public void GetPower(int _powerId)
	{
		currentPowerUpIndex = _powerId;
		if(invincibilityTimer.TimeLeft > 0)
		{
			invincibilityTimer.Stop();

		}
		switch (currentPowerUpIndex)
		{
			case (int)Mario.PowerEnum.BIG:
				Scale = new Vector2(1, 1.5f);
				break;
			case (int)Mario.PowerEnum.FIRE:
				currentColor = new Color(1, 0, 1);
				animation.Modulate = currentColor;
				Scale = new Vector2(1, 1.5f);
				break;

		}
	}

	private void InvincibilityEnd()
	{

		hurtComponent.SetDeferred("monitoring", true);
		tween.Stop();
		animation.Modulate = currentColor;
	}

	private void Die()
	{
		QueueFree();
	}

	private void InitStates()
	{

		states = [
			new StateIdle(this, movementComponent),
			new StateJumping(this, movementComponent),
			new StateFalling(this, movementComponent),
			new StateRunning(this, movementComponent),
			new StateWallSlide(this, movementComponent),
			new StateHurt(this, movementComponent),
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
		actionInput = inputComponent.getActionInput();

		movementComponent.SetDirection(inputComponent.getLeftInput(), inputComponent.getRightInput());
		movementComponent.SetJump(inputComponent.getJumpInput());

		states[currentStateIndex].PhysicsProcess(delta);




		if (actionInput && currentPowerUpIndex == (int)PowerEnum.FIRE)
		{
			LaunchFireBall();
		}

		CheckForHitting();
		MoveAndSlide();


	}

	private void CheckForHitting()
	{
		if (movementComponent.CurrentSpeedY < 0)
		{
			headBox.Monitoring = true;
			feetBox.Monitoring = true;
			feetBox.Modulate = new Color(1, 1, 1, 1);

		}
		else
		{
			if (movementComponent.CurrentSpeedY > 0)
			{
				headBox.Monitoring = false;
				feetBox.Monitoring = true;
				feetBox.Modulate = new Color(1, 1, 1, 1);

			}
			else
			{
				headBox.Monitoring = false;
				feetBox.Monitoring = false;
				feetBox.Modulate = new Color(0, 0, 0, 0);
			}
		}
	}


	private void LaunchFireBall()
	{
		FireBall fireBall = fireScene.Instantiate<FireBall>();
		AddSibling(fireBall);
		int direction = (animation.FlipH) ? -1 : 1;
		fireBall.Position = Position + new Vector2(30 * direction, -30);
		fireBall.SetDirection(direction);
	}


	//public void SetGoingUp()
	//{
	//	headBox.SetDeferred("monitoring", true);
	//	feetBox.SetDeferred("monitoring", false);
	//	//head.SetDeferred("monitorable", _decision);
	//	isHittingUp = false;
	//	isHittingDown = false;
	//}
	//
	//public void SetGoingDown()
	//{
	//	headBox.SetDeferred("monitoring", false);
	//	feetBox.SetDeferred("monitoring", true);
	//	isHittingUp = false;
	//	isHittingDown = false;
	//
	//}
	//public bool IsGoingDown()
	//{
	//	return feetBox.Monitoring;
	//}
	//
	//public void SetGoingNeutral()
	//{
	//	isHittingUp = false;
	//	isHittingDown = false;
	//	headBox.SetDeferred("monitoring", false);
	//	feetBox.SetDeferred("monitoring", false);
	//}


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

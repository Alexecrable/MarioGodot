using System;
using System.ComponentModel;
using System.Linq.Expressions;
using Godot;
using Godot.Collections;


/// BUGS
/// 
/// Mario couleur ne va pas bien su rla fleur si il la prend pendant un HIT (save une variable de couleur pr qu'ils puissent adapter le tween)
/// -> Couleur var créée, mais c degueu, idealement je fais un etat powerUp pour casser l'invincibilité et faire ça proprement
/// 
/// CHapeau de Mario pas toujours dans la bonne dir
public partial class Mario : CharacterBody2D, IInstaKillableObject
{
	[Signal]
	public delegate void RespawnSignalEventHandler();

	[Signal]
	public delegate void MarioLifeUpdateEventHandler(int lives);
	private PackedScene jumpEffet;
	public enum PowerEnum
	{
		BASE = 0,
		BIG = 1,
		FIRE = 2,
		STAR = 3,
		UP = 4
	}
	public enum StateEnum
	{
		IDLE = 0,
		JUMP = 1,
		FALL = 2,
		MOVE = 3,
		WALLSLIDE = 4,
		HURT = 5,
		DIE = 6,
		CROUCH = 7
	}
	// Composants externes (Composition) //
	private InputComponent inputComponent;
	private MovementComponent movementComponent;
	public HurtComponent hurtComponent;
	/////////////////////////////

	public Area2D headBox, feetBox;

	public int Lives { get; set; }
	public bool invincible = false;
	public int
		speed = 250,
		startingGravity = 100,
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
		downInput,
		currentHorizontalVelocity = 250;
	public bool actionInput,
		isHittingDown,
		isHittingUp = false;
	private Array<MarioState> states;
	private Array<FireBall> fireBalls;

	//// Misc. ////
	public GpuParticles2D particles;
	public AnimatedSprite2D animation;
	public Sprite2D chapeau;
	private Tween tween;

	private Camera2D camera;
	public RayCast2D raycastRight, raycastLeft, ceilingChecker;
	public AudioStreamPlayer2D sfxPlayer;

	private Timer invincibilityTimer, fireTimer;
	private Color currentColor = new Color(1, 1, 1);

	private float cameraBaseline;

	private uint holdLayer, holdMask;
	/////////
	/// 


	public override void _Ready()
	{


		//n'avoir que 2 boules de feu qui cyclent entre elles
		//leur faire lacher un ptit effet d'explosion on contact
		//les TP a un endroit random le temps d'etre rappellée
		fireTimer = new Timer();

		inputComponent = GetNode<InputComponent>("InputComponent");
		movementComponent = GetNode<MovementComponent>("MovementComponent");
		movementComponent.Init(this);
		movementComponent.BaseSpeedX = speed;
		hurtComponent = GetNode<HurtComponent>("HurtComponent");

		jumpEffet = ResourceLoader.Load<PackedScene>("res://World/jumpEffect.tscn");

		Lives = 3;
		animation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		camera = GetNode<Camera2D>("Camera2D");
		raycastLeft = GetNode<RayCast2D>("RayCast2D2");
		raycastRight = GetNode<RayCast2D>("RayCast2D");
		ceilingChecker = GetNode<RayCast2D>("CeilingChecker");
		particles = GetNode<GpuParticles2D>("GPUParticles2D");
		sfxPlayer = GetNode<AudioStreamPlayer2D>("SFXPlayer");
		headBox = GetNode<Area2D>("HeadBox");
		feetBox = GetNode<Area2D>("FeetBox");
		chapeau = GetNode<Sprite2D>("Chapeau");


		Scale = new Vector2(1, 1.5f);

		yVelocity = startingGravity;
		particles.Emitting = false;



		invincibilityTimer = new Timer();
		invincibilityTimer.WaitTime = 3;
		invincibilityTimer.OneShot = true;
		AddChild(invincibilityTimer);
		InitStates();
		InitFireBalls();

		EmitSignal(SignalName.MarioLifeUpdate, Lives);

	}

	public void Spawn()
	{
		cameraBaseline = 450;
		ConnectSignals();
		currentPowerUpIndex = (int)PowerEnum.BASE;
		Scale = new Vector2(1, 1);
		animation.Modulate = new Color(1, 1, 1);
		currentColor = Modulate;
		ChangeState((int)StateEnum.IDLE);
		//CollisionLayer = 4 + 128 + 256;
	}

	private void InitFireBalls()
	{
		PackedScene fireScene = ResourceLoader.Load<PackedScene>("res://World/FireBall.tscn");

		fireBalls = [
			fireScene.Instantiate<FireBall>(),
			fireScene.Instantiate<FireBall>(),
		];
		foreach (FireBall fireBall in fireBalls)
		{
			CallDeferred("add_sibling", fireBall);
			///AddSibling(fireBall);
		}

	}

	private void ConnectSignals()
	{
		headBox.BodyEntered += HittingUp;
		feetBox.AreaEntered += HittingDownArea;
		hurtComponent.Hurt += GetHurt;
		invincibilityTimer.Timeout += InvincibilityEnd;
	}

	public void DisconnectSignals()
	{
		headBox.BodyEntered -= HittingUp;
		feetBox.AreaEntered -= HittingDownArea;
		hurtComponent.Hurt -= GetHurt;
	}

	private void HittingUp(Node _body)
	{

		//headBox.Monitoring = false;

		isHittingUp = true;

		IBlock block = (IBlock)_body;
		block.Collision(currentPowerUpIndex);




	}



	private void HittingDownArea(Node _body)
	{
		GD.Print("xbox1");
		if (!invincible && (currentStateIndex != (int)Mario.StateEnum.DIE))
		{
			GpuParticles2D particles = jumpEffet.Instantiate<GpuParticles2D>();
			particles.Position = Position;
			particles.Emitting = true;
			AddSibling(particles);
			ChangeState((int)StateEnum.JUMP);
		}


	}
	private bool respawning = false;


	public void EmittingRespawnSignal()
	{
		if (!respawning)
		{
			respawning = true;
			Timer timer = new Timer();
			timer.WaitTime = 2;
			timer.OneShot = true;
			AddChild(timer);
			timer.Timeout += () =>
			{
				respawning = false;
				CollisionLayer = holdLayer;
				CollisionMask = holdMask;
			};
			timer.Start();
			EmitSignal(SignalName.RespawnSignal);
		}

	}

	public void InstaKill()
	{

		Die();
	}

	private void GetHurt(Area2D area)
	{
		if (currentPowerUpIndex == 0 && !invincible)
		{
			Die();
		}
		else
		{
			if (!invincible)
			{

				currentColor = new Color(1, 1, 1);
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
			else
			{
				IInstaKillableObject mort = (IInstaKillableObject)area.GetParent();
				mort.InstaKill();
			}

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
		int save = currentPowerUpIndex;
		if (invincibilityTimer.TimeLeft > 0 && currentPowerUpIndex != (int)PowerEnum.STAR)
		{
			invincibilityTimer.Stop();

		}
		if (_powerId > currentPowerUpIndex)
		{
			currentPowerUpIndex = _powerId;

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
			case (int)Mario.PowerEnum.STAR:

				//Changer ça pr lancer une fonction ou je tue tout sur mon passage plutot
				tween = CreateTween();
				tween.SetTrans(Tween.TransitionType.Sine);
				tween.SetLoops();
				tween.TweenProperty(animation, "modulate", new Color(1, 1, 1), 0.1);
				tween.TweenProperty(animation, "modulate", new Color(1, 0, 0), 0.1);
				tween.TweenProperty(animation, "modulate", new Color(0, 1, 0), 0.1);
				tween.TweenProperty(animation, "modulate", new Color(1, 1, 0), 0.1);
				tween.TweenProperty(animation, "modulate", new Color(0, 0, 1), 0.1);
				tween.TweenProperty(animation, "modulate", new Color(1, 0, 1), 0.1);
				Timer starTimer = new Timer();
				starTimer.WaitTime = 10;
				starTimer.OneShot = true;
				invincible = true;
				starTimer.Timeout += () =>
				{
					invincible = false;
					tween.Stop();
					animation.Modulate = currentColor;
				};
				AddChild(starTimer);
				starTimer.Start();
				currentPowerUpIndex = save;

				break;
			case (int)Mario.PowerEnum.UP:
				Lives++;
				EmittingLifeUpdate();

				currentPowerUpIndex = save;
				break;


		}
	}



	private void InvincibilityEnd()
	{

		hurtComponent.SetDeferred("monitoring", true);
		tween.Stop();
		animation.Modulate = currentColor;
	}

	public void GameOver()
	{
		DisconnectSignals();
		QueueFree();
	}

	private void Die()
	{
		holdLayer = CollisionLayer;
		holdMask = CollisionMask;
		DisconnectSignals();
		ChangeState((int)StateEnum.DIE);
	}

	public void EmittingLifeUpdate()
	{
		EmitSignal(SignalName.MarioLifeUpdate, Lives);
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
			new StateDie(this, movementComponent),
			new StateCrouching(this, movementComponent)
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
		float camX = camera.GlobalPosition.X;
		float camY = camera.GlobalPosition.Y;

		if (Position.Y <= 150)
		{
			cameraBaseline = 225;
		}
		else
		{
			if (Position.Y >= 750)
			{
				cameraBaseline = 675;
			}
			else
			{
				if (Position.Y < cameraBaseline - 75)
				{
					GD.Print("cam : " + Position.Y + "  /  " + cameraBaseline);
					cameraBaseline = Position.Y + 75;
					GD.Print("cam : " + Position.Y + "  /  " + cameraBaseline);
				}
				else
				{
					if (Position.Y > cameraBaseline + 75)
					{
						cameraBaseline = Position.Y - 75;
					}
				}

			}
		}





		camera.GlobalPosition = new Vector2(Position.X, cameraBaseline);
		//if (camY <= 225)
		//{
		//	camera.GlobalPosition = new Vector2(camX, 225);
		//}
		//else
		//{
		//	if (camY >= 700)
		//	{
		//		camera.GlobalPosition = new Vector2(camX, 650);
		//	}
		//	else
		//	{
		//		camera.GlobalPosition = new Vector2(Position.X, Position.Y);
		//	}
		//}


	}


	public override void _PhysicsProcess(double delta)
	{



		if (ceilingChecker.IsColliding() && currentStateIndex != (int)Mario.StateEnum.CROUCH)
		{
			ChangeState((int)Mario.StateEnum.CROUCH);
		}

		inputComponent.takeInputs();
		leftInput = inputComponent.getLeftInput();
		rightInput = inputComponent.getRightInput();
		jumpInput = inputComponent.getJumpInput();
		downInput = inputComponent.getDownInput();
		actionInput = inputComponent.getActionInput();

		movementComponent.SetDirection(inputComponent.getLeftInput(), inputComponent.getRightInput());
		movementComponent.SetJump(inputComponent.getJumpInput());
		movementComponent.SetDownInput(downInput);
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
		if (movementComponent.CurrentSpeedY < startingGravity)
		{
			headBox.Monitoring = true;
			//feetBox.Monitoring = true;
			//feetBox.Monitorable = true;

			feetBox.CollisionLayer = 16;
			feetBox.CollisionMask = 32;
			feetBox.Modulate = new Color(1, 1, 1, 1);

		}
		else
		{
			if (movementComponent.CurrentSpeedY > startingGravity)
			{
				headBox.Monitoring = false;
				//feetBox.Monitoring = true;
				//feetBox.Monitorable = true;
				feetBox.CollisionLayer = 16;
				feetBox.CollisionMask = 32;
				feetBox.Modulate = new Color(1, 1, 1, 1);

			}
			else
			{
				headBox.Monitoring = false;
				//feetBox.Monitoring = false;
				//feetBox.Monitorable = false;
				feetBox.CollisionLayer = 0;
				feetBox.CollisionMask = 0;
				feetBox.Modulate = new Color(0, 0, 0, 0);
			}
		}
	}

	public override void _ExitTree()
	{
		//base._ExitTree();
	}
	private void LaunchFireBall()
	{


		//Prevoie si une boule lancée apres disaparait avant
		//pour etre reutilisée direct
		//pourrait etre gourmand si on a des millins de boules ?

		//Pb : la fire Ball disappear en boucle si touche le wall ??
		foreach (FireBall fireBall in fireBalls)
		{
			if (!fireBall.Used)
			{
				int direction = (animation.FlipH) ? -1 : 1;
				Vector2 position = Position + new Vector2(30 * direction, -30);
				fireBall.Launch(position, direction);
				break;
			}
		}
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

using Godot;
using System;
using Godot.Collections;


public abstract partial class Powerup : CharacterBody2D, IStateMachine, IInstaKillableObject
{

	public enum StateEnum
	{
		SPAWN = 0,
		MOVE = 1
	}

	private Array<PowerState> states;
	private AnimatedSprite2D skin;
	Area2D area;
	public Timer spawnTimer;
	private float spawnTime = 0.5f;
	private AudioStreamPlayer2D sfxPlayer;
	protected MovementComponent movementComponent;
	protected int powerEnum;
	private int currentStateIndex;
	public float xVelocity, yVelocity, currentYVelocity, currentXVelocity;
	

	public override void _Ready()
	{

		movementComponent = new MovementComponent();
		AddChild(movementComponent);
		movementComponent.Init(this);
		CollisionMask = 0;
		skin = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		sfxPlayer = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
		spawnTimer = new Timer();
		spawnTimer.WaitTime = spawnTime;
		area = GetNode<Area2D>("Area2D");
		movementComponent.CurrentSpeedX = 0;
		movementComponent.CurrentSpeedY = 0;
		area.BodyEntered += MarioTouch;


		AddChild(spawnTimer);
		skin.Pause();

		InitState();
		spawnTimer.Start();
		

	}

	private void MarioTouch(Node _body)
	{
		((Mario)_body).GetPower(powerEnum);
		QueueFree();
	}

	public void InitState()
	{
		states = [
			new StateSpawn(this, movementComponent),
			new StateMove(this, movementComponent)
		];
		foreach (PowerState state in states)
		{
			AddChild(state);
			state.finished += ChangeState;
		}
		currentStateIndex = 0;
		states[currentStateIndex].enter();
	}



	public void ChangeState(int stateIndex)
	{
		states[currentStateIndex].exit();
		currentStateIndex = stateIndex;
		states[currentStateIndex].enter();

	}

	public void InstaKill()
	{
		QueueFree();
	}

	public override void _Process(double delta)
	{
		
		base._Process(delta);

	}
	public override void _PhysicsProcess(double delta)
	{

		states[currentStateIndex].PhysicsProcess(delta);

		if (IsOnWall())
		{
			movementComponent.CurrentSpeedX = -movementComponent.CurrentSpeedX;
			if(movementComponent.CurrentSpeedX < 0)
			{
				skin.PlayBackwards();
			}
			else
			{
				skin.Play();
			}
		}
		GD.Print("poweradvance " + Velocity);
		movementComponent.Advance();
		MoveAndSlide();


	}
	public AnimatedSprite2D GetSprite()
	{
		return skin;
	}





}

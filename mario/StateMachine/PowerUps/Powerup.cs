using Godot;
using System;
using Godot.Collections;


public abstract partial class Powerup : CharacterBody2D, IStateMachine
{

	public enum StateEnum
	{
		SPAWN = 0,
		MOVE = 1
	}

	private Array<PowerState> states;
	private AnimatedSprite2D skin;
	public Timer spawnTimer;
	private float spawnTime = 0.5f;
	private AudioStreamPlayer2D sfxPlayer;
	private int currentStateIndex;
	public float xVelocity, yVelocity, currentYVelocity, currentXVelocity;

	public override void _Ready()
	{

		CollisionMask = 0;
		skin = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		sfxPlayer = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
		spawnTimer = new Timer();
		spawnTimer.WaitTime = spawnTime;
		yVelocity = 200;
		currentYVelocity = 0;
		currentXVelocity = 0;

		AddChild(spawnTimer);
		skin.Pause();

		InitState();
		spawnTimer.Start();

	}

	public void InitState()
	{
		states = [
			new StateSpawn(this),
			new StateMove(this)
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

	public override void _Process(double delta)
	{
		
		base._Process(delta);

	}
	public override void _PhysicsProcess(double delta)
	{

		states[currentStateIndex].PhysicsProcess(delta);

		if (IsOnWall())
		{
			currentXVelocity = -currentXVelocity;
			if(currentXVelocity < 0)
			{
				skin.PlayBackwards();
			}
			else
			{
				skin.Play();
			}
		}
		Velocity = new Vector2(currentXVelocity, currentYVelocity);
		MoveAndSlide();


	}
	public AnimatedSprite2D GetSprite()
	{
		return skin;
	}





}

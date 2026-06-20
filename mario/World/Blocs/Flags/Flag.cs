using Godot;
using System;

public partial class Flag : AnimatedSprite2D
{
	[Signal]
	public delegate void NewSpawnEventHandler(Flag flag);
	public Area2D area2D { get; set; }
	public bool Reached {get; set;}
	private GpuParticles2D particles;
	private AudioStreamPlayer2D player;

	private void FlagReached(Node _body)
	{
		GD.Print("flaggy " + Reached + this);
		if (!Reached)
		{
			EmitSignal(SignalName.NewSpawn, this);
			particles.Emitting = true;
			player.Play();
		}
		

	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
		Reached = false;
		GetNode<Area2D>("Area2D").BodyEntered += FlagReached;
		Play();
		particles = GetNode<GpuParticles2D>("GPUParticles2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

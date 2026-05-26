using Godot;
using System;

public partial class Coin : AnimatedSprite2D
{
	[Signal]
	public delegate void CoinPickedEventHandler(int score);
	private AudioStreamPlayer2D player;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player = GetNode<AudioStreamPlayer2D>("GPUParticles2D/AudioStreamPlayer2D");

		GetNode<Area2D>("Area2D").BodyEntered += PickUp;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}


	private void PickUp(Node _body)
	{
		GpuParticles2D gpu = GetNode<GpuParticles2D>("GPUParticles2D");
		gpu.Reparent(GetParent());
		player.Play();
		gpu.Emitting = true;
		EmitSignal(SignalName.CoinPicked);
		QueueFree();
	}
}

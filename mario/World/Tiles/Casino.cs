using Godot;
using System;

public partial class Casino : Sprite2D
{
	[Signal]
	public delegate void LevelFinishEventHandler();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Area2D area = GetNode<Area2D>("Area2D");
		area.BodyEntered += LevelFinished;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	private void LevelFinished(Node2D mario)
	{
		EmitSignal(SignalName.LevelFinish);
	}
}

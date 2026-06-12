using Godot;
using System;

public partial class MainMenu : CanvasLayer
{
	// Called when the node enters the scene tree for the first time.
	public Button button;

	[Signal]
	public delegate void LaunchGameEventHandler();
	public override void _Ready()
	{
		button = GetNode<Button>("Button");
		button.Pressed += StartGame;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void StartGame()
	{
		EmitSignal(SignalName.LaunchGame);
	}
}

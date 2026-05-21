using Godot;
using System;

public partial class MainMenu : Node2D
{
	// Called when the node enters the scene tree for the first time.
	private Button button;
	public override void _Ready()
	{
		button = GetNode<Button>("Button");
		button.Pressed += LaunchGame;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void LaunchGame()
	{
	GetTree().ChangeSceneToFile("res://World/world.tscn");
	}
}

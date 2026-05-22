using Godot;
using System;

public partial class PauseMenu : CanvasLayer
{
	public Button resumeButton;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		resumeButton = GetNode<Button>("Control/Button");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

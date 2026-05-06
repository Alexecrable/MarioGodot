using Godot;
using System;
using System.Numerics;

[GlobalClass]
public partial class InputComponent : Node
{	
	private float rightInput, leftInput, jumpInput;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		rightInput = 0;
		leftInput = 0;
		jumpInput = 0;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public void takeInputs()
	{
		rightInput = Input.GetActionStrength("right");
		leftInput = Input.GetActionStrength("left");
		jumpInput = Input.GetActionStrength("jump");

	}

	public float getLeftInput()
	{
		return leftInput;
	}

	public float getRightInput()
	{
		return rightInput;
	}

	public float getJumpInput()
	{
		return jumpInput;
	}

}

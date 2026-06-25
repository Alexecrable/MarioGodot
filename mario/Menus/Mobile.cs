using Godot;
using System;

public partial class Mobile : Control
{

	private Button right, left, down, jump, leftDown, rightDown;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		right = GetNode<Button>("Right");
		left = GetNode<Button>("Left");
		jump = GetNode<Button>("Jump");
		down = GetNode<Button>("Down");
		leftDown = GetNode<Button>("LeftDown");
		rightDown = GetNode<Button>("RightDown");

		right.ButtonUp += () => {Input.ActionRelease("right");};
		right.ButtonDown += () => {Input.ActionPress("right");};

		left.ButtonUp += () => {Input.ActionRelease("left");};
		left.ButtonDown += () => {Input.ActionPress("left");};

		jump.ButtonUp += () => {Input.ActionRelease("jump");};
		jump.ButtonDown += () => {Input.ActionPress("jump");};

		down.ButtonUp += () => {Input.ActionRelease("down");};
		down.ButtonDown += () => {Input.ActionPress("down");};

		rightDown.ButtonUp += () => {Input.ActionRelease("right"); Input.ActionRelease("down");};
		rightDown.ButtonDown += () => {Input.ActionPress("right"); Input.ActionPress("down");};

		leftDown.ButtonUp += () => {Input.ActionRelease("left"); Input.ActionRelease("down");};
		leftDown.ButtonDown += () => {Input.ActionPress("left"); Input.ActionPress("down");};
	}

	
}

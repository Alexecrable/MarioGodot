using Godot;
using System;
using System.Transactions;

public abstract partial class State : Node
{
	[Signal]
	public delegate void FinishedEventHandler(int _stateIndex);
	public abstract void Enter(int _previousStateId);
	public abstract void Exit(int _previousStateId);
	public abstract void PhysicsProcess(double _delta);
	
	
}

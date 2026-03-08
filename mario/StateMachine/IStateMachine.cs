using Godot;
using System;
using Godot.Collections;

public interface IStateMachine
{
	void ChangeState(int _newStateIndex);
	void InitState();
	
	
}

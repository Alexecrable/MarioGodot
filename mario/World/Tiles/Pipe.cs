using Godot;
using System;

public partial class Pipe : StaticBody2D
{
	// Called when the node enters the scene tree for the first time.
	public Pipe otherPipe { get; set; }


	public void EnterAnim(Mario mario)
	{
		GetTree().Paused = true;
		mario.Position = Position + new Vector2(0, -16);

		Tween tween = CreateTween();
		tween.SetTrans(Tween.TransitionType.Linear);
		tween.TweenProperty(mario, "position", Position + new Vector2(0,6), 1);
		tween.Finished += () =>
		{
			if(otherPipe != null)
			{
				otherPipe.ExitAnim(mario);
			}
			else
			{
				ExitAnim(mario);
			}
		};
	}



	public void ExitAnim(Mario mario)
	{
		mario.Position = Position + new Vector2(0,6);
		Tween tween = CreateTween();
		tween.SetTrans(Tween.TransitionType.Linear);
		tween.TweenProperty(mario, "position", Position  + new Vector2(0, -16), 1);
		tween.Finished += () =>
		{
			GetTree().Paused = false;
		};
	}
}

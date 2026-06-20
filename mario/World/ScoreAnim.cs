using Godot;
using System;

public partial class ScoreAnim : Label
{
	// Called when the node enters the scene tree for the first time.
	public int ScoreToShow { get; set; } = 0;
	public override void _Ready()
	{
		Text = ScoreToShow.ToString();
		GD.Print("label: " + LabelSettings);
		Tween tween = CreateTween();
		tween.SetTrans(Tween.TransitionType.Sine);
		tween.SetParallel();
		
		tween.TweenProperty(this, "global_position", GlobalPosition + new Vector2(0,-40), 0.3);
		tween.TweenProperty(this, "modulate", new Color(1, 1, 1, 0), 1);

		tween.Finished += () => {
			QueueFree();
		};
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

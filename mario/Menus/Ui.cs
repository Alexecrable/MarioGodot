using Godot;
using System;

public partial class Ui : CanvasLayer
{
	private Control control;
	private Timer timer;
	private Label timerLabel, LifeLabel, ScoreLabel;

	private int timeLeft;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		control = GetNode<Control>("Control");
		timer = control.GetNode<Timer>("Timer");
		timerLabel = control.GetNode<Label>("TimerLabel");
		LifeLabel = control.GetNode<Label>("Lives");
		ScoreLabel = control.GetNode<Label>("Score");
		timeLeft = 999;
		timer.Timeout += timeUpdate;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		GD.Print("Canvas : Je SUI SOU " + control.Position + " | " + control.GlobalPosition);
	}

	private void timeUpdate()
	{
		timeLeft--;
		timerLabel.Text = "TIME : " + timeLeft;	
	}

	public void LifeUpdate(int _life)
	{
		LifeLabel.Text = ": " + _life;
	}

	public void ScoreUpdate(int _score)
	{
		ScoreLabel.Text = "SCORE : " + _score;
	}
}

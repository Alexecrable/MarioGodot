using Godot;
using System;

public partial class TitleCard : CanvasLayer
{

	private Label titre, vies, score;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		titre = GetNode<Label>("VSplitContainer/Title");
		vies = GetNode<Label>("VSplitContainer/HBoxContainer/Lives");
		score = GetNode<Label>("VSplitContainer/HBoxContainer2/Score");
	}

	public void setTitle(String _titre, int _vies, int _score)
	{
		titre.Text = _titre;
		vies.Text = " : " + _vies;
		score.Text = " : " + _score;
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

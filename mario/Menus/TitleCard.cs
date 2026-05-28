using Godot;
using System;

public partial class TitleCard : CanvasLayer
{

	private Label titre, vies;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		titre = GetNode<Label>("Title");
		vies = GetNode<Label>("Lives");
	}

	public void setTitle(String _titre, int _vies)
	{
		titre.Text = _titre;
		vies.Text = " : " + _vies;
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

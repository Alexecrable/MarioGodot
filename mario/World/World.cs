using Godot;
using System;

public partial class World : Node2D
{
	//Powerup power;
	Mario mario;
	private Ui ui;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ui = ResourceLoader.Load<PackedScene>("res://Menus/UI.tscn").Instantiate<Ui>();
		mario = GetNode<Mario>("Mario");
		//power = GetNode<Powerup>("Power");
		mario.TreeExited += MarioJustDied;
		ui.Scale = new Vector2(0.5f,0.5f);
		mario.MarioLifeUpdate += ui.LifeUpdate;
		AddChild(ui);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	private void MarioJustDied()
	{
		GD.Print("WORLD : PRET A LANCER UN MENU");
		GetTree().ChangeSceneToFile("res://Menus/MainMenu.tscn");
	}
	
	
}

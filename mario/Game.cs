using Godot;
using GodotPlugins.Game;
using System;

public partial class Game : Node2D
{
	//Powerup power;
	private Mario mario;
	private Ui ui;
	private Level level;
	private MainMenu mainMenu;
	private Level currentLevel;
	private PauseMenu pauseMenu;
	private bool isPaused = false;


	//FAUDRA SEPARER LA CAMERA DE MARIO POUR LA DONNER AU JEU GLOBALEMENT JE PENSE C MIEUX ??,
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		pauseMenu = GetNode<PauseMenu>("PauseMenu");
		pauseMenu.resumeButton.Pressed += PauseMenu;
		mainMenu = GetNode<MainMenu>("MainMenu");
		mainMenu.LaunchGame += LaunchGame;
		ui = ResourceLoader.Load<PackedScene>("res://Menus/UI.tscn").Instantiate<Ui>();
		mario = ResourceLoader.Load<PackedScene>("res://StateMachine/Mario/mario.tscn").Instantiate<Mario>();
		//power = GetNode<Powerup>("Power");
		mario.TreeExited += MarioJustDied;
		ui.Scale = new Vector2(0.5f, 0.5f);
		mario.MarioLifeUpdate += ui.LifeUpdate;
		ui.Visible = false;
		mario.Visible = false;
		currentLevel = ResourceLoader.Load<PackedScene>("res://World/Levels/levelTest.tscn").Instantiate<Level>();
		//GetTree().Paused = true;
		AddChild(ui);
		AddChild(mario);

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("pause"))
		{
			PauseMenu();
		}

	}


	private void PauseMenu()
	{
		ui.Visible = !ui.Visible;
		pauseMenu.Visible = !pauseMenu.Visible;
		GetTree().Paused = !GetTree().Paused;
		isPaused = !isPaused;
	}

	private void LaunchGame()
	{
		GD.Print("LevelSetup : ");
		ui.StartTimer(125);
		GetTree().Paused = false;
		AddChild(currentLevel);
		currentLevel.LoadLevel();
		mario.Position = currentLevel.spawnPoint;
		mario.Visible = true;
		ui.Visible = true;
		mainMenu.Visible = false;
	}

	private void MarioJustDied()
	{
		GD.Print("WORLD : PRET A LANCER UN MENU");
		GetTree().ChangeSceneToFile("res://Menus/MainMenu.tscn");
		mainMenu.Visible = true;
		RemoveChild(currentLevel);
	}


}

using Godot;
using GodotPlugins.Game;
using System;

public partial class Game : Node2D
{
	//Powerup power;
	private Mario mario;
	private Ui ui;
	private MainMenu mainMenu;
	private Level currentLevel;
	private PauseMenu pauseMenu;
	private AudioStreamPlayer2D player;
	private PackedScene levelScene;
	private TitleCard titleCard;

	private Vector2 spawnPoint = Vector2.Zero;

	public override void _Ready()
	{

		//recuperation des Nodes de la scene
		titleCard = GetNode<TitleCard>("TitleCard");
		player = GetNode<AudioStreamPlayer2D>("Music");
		pauseMenu = GetNode<PauseMenu>("PauseMenu");
		mainMenu = GetNode<MainMenu>("MainMenu");
		ui = GetNode<Ui>("UI");
		//Chargement de scenes externes
		levelScene = ResourceLoader.Load<PackedScene>("res://World/Levels/levelTest.tscn");
		mario = ResourceLoader.Load<PackedScene>("res://StateMachine/Mario/mario.tscn").Instantiate<Mario>();



		titleCard.Visible = false;
		mario.Visible = false;
		ui.Scale = new Vector2(0.5f, 0.5f);
		ui.Visible = false;




		mario.MarioLifeUpdate += ui.LifeUpdate;
		pauseMenu.resumeButton.Pressed += PauseMenu;
		mainMenu.LaunchGame += LaunchGame;


		SwitchMusic("title.ogg");
	}

	private void SwitchMusic(String name)
	{

		player.Stream = ResourceLoader.Load<AudioStream>("res://SoundsBeta/" + name);
		player.Play();
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
		player.Playing = !player.Playing;
		GD.Print("pause : " + GetChildren());
	}


	private void LaunchGame()
	{
		GD.Print("LevelSetup : ");
		ui.StartTimer(125);
		GetTree().Paused = false;

		AddChild(mario);


		ShowLevelCard();

		mario.Visible = true;
		ui.Visible = true;
		mainMenu.Visible = false;

		mario.RespawnSignal += () =>
		{
			spawnPoint = currentLevel.spawnPoint;
			mario.Position = Vector2.Zero;
			

			RemoveChild(currentLevel);
			ShowLevelCard();



		};
		SwitchMusic("maintheme.mp3");
	}
	private void ShowLevelCard()
	{

		currentLevel = levelScene.Instantiate<Level>();

		

		currentLevel.init();
		currentLevel.LoadLevel();
		if(spawnPoint != Vector2.Zero)
		{
		currentLevel.spawnPoint = spawnPoint;
			
		}
		currentLevel.CheckSpawn(currentLevel.spawnPoint);
		titleCard.setTitle(currentLevel.Name, mario.Lives);
		//player.Stop();
		player.Play();
		//GetTree().Paused = true;
		titleCard.Visible = true;
		Timer timer = new Timer();
		timer.WaitTime = 4;
		AddChild(timer);
		timer.Timeout += () =>
		{






			ui.StartTimer(currentLevel.GetTime());


			AddChild(currentLevel);

			mario.Position = currentLevel.spawnPoint + new Vector2(100,0);
			mario.Spawn();
			timer.QueueFree();
			titleCard.Visible = false;
			GetTree().Paused = false;
		};
		timer.Start();

	}


}

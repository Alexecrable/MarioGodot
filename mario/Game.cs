using Godot;
using GodotPlugins.Game;
using System;

public partial class Game : Node2D
{
	//Powerup power;
	private Mario mario;
	private Ui ui;
	private int totalScore;
	private MainMenu mainMenu;
	private Level currentLevel;
	private PauseMenu pauseMenu;
	private AudioStreamPlayer2D player;
	private PackedScene levelScene;
	private TitleCard titleCard;

	private Vector2 spawnPoint = Vector2.Zero;

	public override void _Ready()
	{
		//Engine.TimeScale = 0.5;
		//recuperation des Nodes de la scene
		titleCard = GetNode<TitleCard>("TitleCard");
		player = GetNode<AudioStreamPlayer2D>("Music");
		pauseMenu = GetNode<PauseMenu>("PauseMenu");
		mainMenu = GetNode<MainMenu>("MainMenu");
		ui = GetNode<Ui>("UI");
		//Chargement de scenes externes
		levelScene = ResourceLoader.Load<PackedScene>("res://World/Levels/level1.tscn");
		mario = ResourceLoader.Load<PackedScene>("res://StateMachine/Mario/mario.tscn").Instantiate<Mario>();

		totalScore = 0;

		titleCard.Visible = false;
		mario.Visible = false;
		ui.Scale = new Vector2(0.5f, 0.5f);
		ui.Visible = false;
		ui.TimeEnd += mario.InstaKill;




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
		player.StreamPaused = !player.StreamPaused;
	}


	private void LaunchGame()
	{
		ui.StartTimer(125);
		GetTree().Paused = false;

		AddChild(mario);


		ShowLevelCard("init launch");

		mario.Visible = true;
		ui.Visible = true;
		mainMenu.Visible = false;

		mario.RespawnSignal += () =>
		{

			spawnPoint = currentLevel.spawnPoint;
			mario.Position = Vector2.Zero;
			currentLevel.LevelFinish -= LevelFinished;
			RemoveChild(currentLevel);
			ShowLevelCard("respanw signal");



		};
		SwitchMusic("maintheme.mp3");
	}

	private void ScoreUpdate(int _score)
	{
		totalScore = _score;
		ui.ScoreUpdate(_score);
	}
	private void ShowLevelCard(String origin)
	{

		currentLevel = levelScene.Instantiate<Level>();


		currentLevel.ScoreUpdate += ScoreUpdate;

		currentLevel.init(totalScore);
		currentLevel.LoadLevel();
		if (spawnPoint != Vector2.Zero)
		{
			currentLevel.spawnPoint = spawnPoint;

		}

		titleCard.setTitle(currentLevel.Name, mario.Lives, totalScore);
		//player.Stop();
		player.Play();
		//GetTree().Paused = true;
		titleCard.Visible = true;
		Timer timer = new Timer();
		timer.WaitTime = 4;
		AddChild(timer);
		ui.PauseTimer();
		timer.Timeout += () =>
		{

			ui.StartTimer(currentLevel.GetTime());

			currentLevel.LevelFinish += LevelFinished;
			AddChild(currentLevel);
			currentLevel.CheckSpawn(spawnPoint);
			mario.Position = currentLevel.spawnPoint;
			mario.Spawn();
			timer.QueueFree();
			titleCard.Visible = false;
			GetTree().Paused = false;
		};
		timer.Start();

	}

	private void LevelFinished()
	{
		currentLevel.LevelFinish -= LevelFinished;
		spawnPoint = Vector2.Zero;
		GD.Print("niveau fini oep");
		levelScene = currentLevel.GetNextLevel();
		RemoveChild(currentLevel);
		if (levelScene == null)
		{
			GD.Print("CACAFIN");
			//faire fin du jeu bravo oeeee
			titleCard.setTitle("BRAVO", mario.Lives, totalScore);
			//player.Stop();
			//GetTree().Paused = true;
			titleCard.Visible = true;
		}
		else
		{
			mario.DisconnectSignals();
			ShowLevelCard("salut");
		}


	}
	

}

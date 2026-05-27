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
	private AudioStreamPlayer2D player;
	private PackedScene levelScene;
	private TitleCard titleCard;
	private bool isPaused = false;


	//FAUDRA SEPARER LA CAMERA DE MARIO POUR LA DONNER AU JEU GLOBALEMENT JE PENSE C MIEUX ??,
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		titleCard = GetNode<TitleCard>("TitleCard");
		titleCard.Visible = false;
		levelScene = ResourceLoader.Load<PackedScene>("res://World/Levels/levelTest.tscn");
		player = GetNode<AudioStreamPlayer2D>("Music");
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
		//currentLevel = ResourceLoader.Load<PackedScene>("res://World/Levels/levelTest.tscn").Instantiate<Level>();
		//GetTree().Paused = true;
		AddChild(ui);

		mario.MarioLifeUpdate += ui.LifeUpdate;


		player.Stream = ResourceLoader.Load<AudioStream>("res://SoundsBeta/title.ogg");
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
		isPaused = !isPaused;
		player.Playing = !player.Playing;
		GD.Print("pause : " + GetChildren());
	}


	private void LaunchGame()
	{
		GD.Print("LevelSetup : ");
		ui.StartTimer(125);
		GetTree().Paused = false;
		currentLevel = levelScene.Instantiate<Level>();
		titleCard.setTitle(currentLevel.Name, mario.Lives);
		
		Level saveLevel = (Level)currentLevel.Duplicate();
		ShowLevelCard();
		
		mario.Visible = true;
		ui.Visible = true;
		mainMenu.Visible = false;

		AddChild(mario);
		mario.Spawn();
		mario.RespawnSignal += () =>
		{
			
			GD.Print("respacn");
			//uint save = mario.CollisionLayer;
			//mario.CollisionLayer = 0;
			//RemoveChild(mario);
			//ResourceLoader.Load<PackedScene>("res://StateMachine/Mario/mario.tscn").Instantiate<Mario>();
			titleCard.setTitle(currentLevel.Name, mario.Lives);

			ShowLevelCard();

			Vector2 spawn = currentLevel.spawnPoint;
			RemoveChild(currentLevel);
			mario.Position = spawn;
			
			//RemoveChild(currentLevel);
			//currentLevel = (Level)saveLevel.Duplicate();
		};

		player.Stream = ResourceLoader.Load<AudioStream>("res://SoundsBeta/maintheme.mp3");
		player.Play();
	}
	private void ShowLevelCard()
	{
		//player.Stop();
		player.Play();
		//GetTree().Paused = true;
		titleCard.Visible = true;
		Timer timer = new Timer();
		timer.WaitTime = 4;
		AddChild(timer);
		timer.Timeout += () =>
		{

			//RemoveChild(currentLevel);

			//Timer timer = new Timer();
			//timer.OneShot = true;
			//timer.WaitTime = 2;
			//timer.Timeout += () =>
			//{
			//	currentLevel = levelScene.Instantiate<Level>();
			//	AddChild(currentLevel);
			//	currentLevel.LoadLevel();
			//	mario.Position = spawn;
			//	mario.CollisionLayer = save;
			//	AddChild(mario);
			//	GD.Print("timeout : " + mario.Position);
			//	RemoveChild(timer);
			//};
			//AddChild(timer);
			//timer.Start();


			//AddChild(currentLevel);
			currentLevel = levelScene.Instantiate<Level>();
			currentLevel.spawnPoint = mario.Position;
			AddChild(currentLevel);
			currentLevel.init();
			currentLevel.LoadLevel();
			currentLevel.CheckSpawn(currentLevel.spawnPoint);
			mario.Position = currentLevel.spawnPoint;
			mario.Spawn();
			timer.QueueFree();
			titleCard.Visible = false;
			GetTree().Paused = false;
		};
		timer.Start();

	}
	private void MarioJustDied()
	{
		//GD.Print("WORLD : PRET A LANCER UN MENU");
		//GetTree().ChangeSceneToFile("res://Menus/MainMenu.tscn");
		//mainMenu.Visible = true;
		//RemoveChild(currentLevel);
	}


}

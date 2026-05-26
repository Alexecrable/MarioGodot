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
	private bool isPaused = false;


	//FAUDRA SEPARER LA CAMERA DE MARIO POUR LA DONNER AU JEU GLOBALEMENT JE PENSE C MIEUX ??,
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
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
		AddChild(currentLevel);
		currentLevel.LoadLevel();
		mario.Position = currentLevel.spawnPoint;
		mario.Visible = true;
		ui.Visible = true;
		mainMenu.Visible = false;
		AddChild(mario);
		mario.RespawnSignal += () =>
		{
			GD.Print("insta : bef" + currentLevel.spawnPoint);
			mario.GlobalPosition = Vector2.Zero;
			GD.Print("insta : after" + currentLevel.spawnPoint);
			GD.Print("CONNARD : mario bougé au vecteur 0");

			GetTree().Paused = true;
			Vector2 pos = currentLevel.spawnPoint;

			//GD.Print("Respawn : ");
			RemoveChild(currentLevel);
			CallDeferred("remove_child", currentLevel);
			//currentLevel.QueueFree();
			Level swapLevel = levelScene.Instantiate<Level>();

			AddChild(swapLevel);
			GD.Print("CONNARD : tree exited after addchild");

			swapLevel.LoadLevel();
			swapLevel.CheckSpawn(pos);
			mario.Position = pos;
			GetTree().Paused = false;
			currentLevel = swapLevel;

			//RemoveChild(currentLevel);



		};

		player.Stream = ResourceLoader.Load<AudioStream>("res://SoundsBeta/maintheme.mp3");
		player.Play();
	}

	private void MarioJustDied()
	{
		GD.Print("WORLD : PRET A LANCER UN MENU");
		GetTree().ChangeSceneToFile("res://Menus/MainMenu.tscn");
		mainMenu.Visible = true;
		RemoveChild(currentLevel);
	}


}

using Godot;
using Godot.Collections;
public partial class Level : Node
{

	/// Un niveau doit avoir ///
	/// - un spawn point initial
	/// - checkpoints ? (liste ?)
	/// -Fin de niveau
	/// 
	/// -un tilemapLayer a parse pour init le niveau correctement
	/// (genre, la si c un tile de bloc, tu le remplaces par le vrai objet bloc)
	/// 
	/// 
	/// donc faut qu'un niveau puisse envoyer des signaux pr dire que genre, le checkpoint est REACHED
	/// 
	/// un niveau decide du temps imparti pr le finir ?
	/// 
	/// le niveau peut se charger d'envoyer tous les signaux d'update de score ?
	/// ///////////////////////
	// Called when the node enters the scene tree for the first time.


	[Signal]
	public delegate void CheckPointEventHandler(Vector2 pos);
	public Vector2 spawnPoint, checkPoint;
	private Vector2 offSetVector = new Vector2(16, 16);
	private PackedScene koopaScene, goombaScene, powerBlockScene, starBlockScene, flagScene, brickScene, coinBlockScene, coinScene;
	[Export]
	private int time;
	private TileMapLayer tiles, saveTiles;

	private Array<Flag> flags;
	private DeathFloor deathFloor;
	private Array<Goomba> goombas;
	


	public void ReloadLevel()
	{
		tiles = (TileMapLayer)saveTiles.Duplicate();
		LoadLevel();
	}
	public override void _Ready()
	{
		
	}

	public void init()
	{
		tiles = GetNode<TileMapLayer>("TileMapLayer");
		saveTiles = (TileMapLayer)tiles.Duplicate();
		koopaScene = ResourceLoader.Load<PackedScene>("res://StateMachine/Ennemies/Koopa/koopa.tscn");
		goombaScene = ResourceLoader.Load<PackedScene>("res://StateMachine/Ennemies/Goomba/goomba.tscn");
		powerBlockScene = ResourceLoader.Load<PackedScene>("res://World/Blocs/PowerBlock/PowerBlock.tscn");
		brickScene = ResourceLoader.Load<PackedScene>("res://World/Blocs/BrickBlock/BrickBlock.tscn");
		flagScene = ResourceLoader.Load<PackedScene>("res://World/Blocs/Flags/flag.tscn");
		coinScene = ResourceLoader.Load<PackedScene>("res://World/Tiles/coin.tscn");
		flags = new Array<Flag>();
		deathFloor = GetNode<DeathFloor>("DeathFloor");
		GD.Print("loaad " + deathFloor);
		goombas = new Array<Goomba>();
		
	}	



	public void LoadLevel()
	{
		GD.Print("load : " + tiles + " | " + saveTiles);
		//tiles = (TileMapLayer)saveTiles.Duplicate();
		for (int i = 0; i < 300; i++)
		{
			for (int y = 0; y < 300; y++)
			{
				Vector2I positionVector = new Vector2I(i, y);
				Vector2I lol = tiles.GetCellAtlasCoords(positionVector);


				switch (lol)
				{
					case Vector2I(0, 1):
						tiles.EraseCell(new Vector2I(i, y));
						SpawnBrickBlock(positionVector);
						break;
					case Vector2I(1, 1):
						tiles.EraseCell(new Vector2I(i, y));
						SpawnCoinBrickBlock(positionVector);
						break;
					case Vector2I(0, 2):
						tiles.EraseCell(new Vector2I(i, y));
						SpawnPowerBlock(positionVector);
						break;
					case Vector2I(1, 2):
						tiles.EraseCell(new Vector2I(i, y));
						SpawnStarBlock(positionVector);
						break;
					case Vector2I(0, 3):
						tiles.EraseCell(new Vector2I(i, y));
						SpawnGoomba(positionVector);
						break;
					case Vector2I(1, 3):
						tiles.EraseCell(new Vector2I(i, y));
						SpawnKoopa(positionVector);
						break;
					case Vector2I(0, 4):
						tiles.EraseCell(new Vector2I(i, y));
						SpawnSpawnPoint(positionVector);
						break;
					case Vector2I(1, 4):
						tiles.EraseCell(new Vector2I(i, y));
						SpawnCheckPoint(positionVector);
						break;
					case Vector2I(0, 5):
						tiles.EraseCell(new Vector2I(i, y));
						SpawnCoin(positionVector);
						break;
					case Vector2I(1, 5):
						tiles.EraseCell(new Vector2I(i, y));
						//SpawnCoinBrickBlock(positionVector);
						break;
				}


			}
			
		}
		//Timer timer = new Timer();
		//timer.WaitTime = 2;
		//timer.Timeout += () =>
		//{
		//	GetNode<DeathFloor>("DeathFloor").setup();
		//	RemoveChild(timer);
		//};
		//AddChild(timer);
//
		//timer.OneShot = true;
		//timer.Start();
		deathFloor.setup();
		
	}

	private void SpawnBrickBlock(Vector2I position)
	{
		BrickBlock brickBlock = brickScene.Instantiate<BrickBlock>();
		brickBlock.Position = position * 32 + offSetVector;
		AddChild(brickBlock);
	}

	private void SpawnCoinBrickBlock(Vector2I position)
	{
		GD.Print("LevelCoin : ");
		BrickBlock brickBlock = brickScene.Instantiate<BrickBlock>();
		brickBlock.Position = position * 32 + offSetVector;
		brickBlock.HasCoin = true;
		AddChild(brickBlock);
	}

	private void SpawnCoin(Vector2I position)
	{
		Coin coin = coinScene.Instantiate<Coin>();
		coin.Position = position * 32 + offSetVector;
		coin.Play();

		AddChild(coin);
	}

	private void Spawn1Up(Vector2I position)
	{

	}
	private void SpawnSpawnPoint(Vector2I position)
	{

		spawnPoint = position * 32 + offSetVector;

	}
	private void SpawnCheckPoint(Vector2I position)
	{
		Flag flag = flagScene.Instantiate<Flag>();
		checkPoint = position * 32 + offSetVector;
		flag.Position = checkPoint;
		flag.NewSpawn += SetSpawnPoint;
		flags.Add(flag);
		AddChild(flag);
		GD.Print("flaggy " + flag);

	}

	public void CheckSpawn(Vector2 position)
	{
		foreach (Flag flaggy in flags)
		{
			if (position == flaggy.Position)
			{	
				GD.Print("Flaggy success" + flaggy);
				flaggy.Modulate = new Color(0.5f, 1, 0.5f);
				spawnPoint = flaggy.Position;
				flaggy.Reached = true;
			}

		}
	}
	private void SetSpawnPoint(Flag flag)
	{
		foreach (Flag flaggy in flags)
		{
			flaggy.Modulate = new Color(1, 1, 1);
			flaggy.Reached = false;
		}
		flag.Modulate = new Color(0.5f, 1, 0.5f);
		spawnPoint = flag.Position;
		flag.Reached = true;

	}
	private void SpawnStarBlock(Vector2I position)
	{

	}
	private void SpawnPowerBlock(Vector2I position)
	{
		PowerBlock powerBlock = powerBlockScene.Instantiate<PowerBlock>();
		powerBlock.Position = position * 32 + offSetVector;
		AddChild(powerBlock);
	}
	private void SpawnGoomba(Vector2I position)
	{
		Goomba goomba = goombaScene.Instantiate<Goomba>();
		goomba.Position = position * 32 + offSetVector;
		goombas.Add(goomba);
		AddChild(goomba);
	}
	private void SpawnKoopa(Vector2I position)
	{
		Koopa koopa = koopaScene.Instantiate<Koopa>();
		koopa.Position = position * 32 + offSetVector;
		AddChild(koopa);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	public int GetTime()
	{
		return time;
	}
}

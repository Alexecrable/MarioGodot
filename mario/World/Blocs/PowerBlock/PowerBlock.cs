using Godot;
using System;

public partial class PowerBlock : StaticBody2D
{
	// Called when the node enters the scene tree for the first time.
	private Sprite2D sprite;
	private Tween jumpTween;
	private Area2D area;
	private Powerup powerup;
	private Texture2D emptyTexture;
	private bool isEmpty;

	public override void _Ready()
	{
		area = GetNode<Area2D>("Area2D");
		sprite = GetNode<Sprite2D>("Sprite2D");
		emptyTexture = ResourceLoader.Load<Texture2D>("res://World/Blocs/PowerBlock/Sprites/EmptyPowerBlock.jpg");
		//jumpTween.SetLoops();

		isEmpty = false;
	}

	public void Collision(int _currentPowerUpIndex)
	{
		if (_currentPowerUpIndex == (int)Mario.PowerEnum.BASE)
		{
			powerup = ResourceLoader.Load<PackedScene>("res://StateMachine/PowerUps//BeerWalker/Shroom.tscn").Instantiate<BeerWalker>();

		}
		else
		{
			powerup = ResourceLoader.Load<PackedScene>("res://StateMachine/PowerUps//FireFlower/fireFlower.tscn").Instantiate<FireFlower>();

		}
		if (!isEmpty)
		{
			powerup.Position = Position + new Vector2(0, -30);
			GD.Print("collision");
			CallDeferred("add_sibling", powerup);

			jumpTween = CreateTween();
			jumpTween.SetTrans(Tween.TransitionType.Linear);
			jumpTween.TweenProperty(sprite, "position", new Vector2(0, -3), 0.1);
			jumpTween.Parallel().TweenProperty(sprite, "scale", new Vector2(1.1f, 1.1f), 0.1);

			jumpTween.TweenProperty(sprite, "position", new Vector2(0, 0), 0.1);
			jumpTween.Parallel().TweenProperty(sprite, "scale", new Vector2(1, 1), 0.1);
			jumpTween.Finished += MakeEmpty;
			isEmpty = true;
			TryToKill();
		}


	}

	private void TryToKill()
	{
		GD.Print("trytokill");
		foreach (CharacterBody2D body in area.GetOverlappingBodies())
		{
			try
			{
				Ennemi ennemi = (Ennemi)body;
				ennemi.InstaKill();
			}
			catch (Exception e)
			{
				GD.Print(body + " n'est pas un ennemi ? " + e);
			}
		}
	}

	private void MakeEmpty()
	{

		sprite.Texture = emptyTexture;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		//GD.Print("runningtween" + jumpTween.IsRunning());
	}


}

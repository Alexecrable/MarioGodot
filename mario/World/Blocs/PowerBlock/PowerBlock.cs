using Godot;
using System;

public partial class PowerBlock : StaticBody2D
{
	// Called when the node enters the scene tree for the first time.
	private Sprite2D sprite;
	private Tween jumpTween;
	private Area2D area;
	private Powerup powerUp;
	private BeerWalker shroom;
	private Texture2D emptyTexture;
	private bool isEmpty;

	public override void _Ready()
	{
		area = GetNode<Area2D>("Area2D");
		sprite = GetNode<Sprite2D>("Sprite2D");
		shroom = ResourceLoader.Load<PackedScene>("res://StateMachine/PowerUps//BeerWalker/Shroom.tscn").Instantiate<BeerWalker>();
		emptyTexture = ResourceLoader.Load<Texture2D>("res://World/Blocs/PowerBlock/Sprites/EmptyPowerBlock.jpg");
		//jumpTween.SetLoops();
		GD.Print("setloops");
		isEmpty = false;
	}

	public void Collision()
	{
		if (!isEmpty)
		{
			shroom.Position = Position + new Vector2(0,-30);

			CallDeferred("add_sibling", shroom);
			GD.Print("collided");

			jumpTween = CreateTween();
			jumpTween.SetTrans(Tween.TransitionType.Linear);
			jumpTween.TweenProperty(sprite, "position", new Vector2(0, -3), 0.1);
			jumpTween.Parallel().TweenProperty(sprite, "scale", new Vector2(1.1f, 1.1f), 0.1);

			jumpTween.TweenProperty(sprite, "position", new Vector2(0, 0), 0.1);
			jumpTween.Parallel().TweenProperty(sprite, "scale", new Vector2(1, 1), 0.1);
			jumpTween.Finished += MakeEmpty;
			isEmpty = true;
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

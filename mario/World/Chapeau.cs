using Godot;
using System;

public partial class Chapeau : RigidBody2D
{

	private Area2D area;
	private Sprite2D sprite2D;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		area = GetNode<Area2D>("Area2D");
		area.BodyEntered += PickUp;
		sprite2D = GetNode<Sprite2D>("Sprite2D");
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void SautChapeau()
	{
		CollisionMask = 3;
		area.CollisionMask = 128;
        SetDeferred("freeze",false);
        float rand = (GD.Randf()*2) - 1;
        GD.Print("rand " + rand );
        CallDeferred("apply_impulse",new Vector2(150 * rand,-120));
        CallDeferred("apply_torque",50000*rand);
        
        
	}

	private void PickUp(Node _body)
	{
		Mario mario = (Mario)_body;
		sprite2D.Texture = mario.GetHat(sprite2D.Texture);
		SautChapeau();

	}
}

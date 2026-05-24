using Godot;
using System;

public partial class BrickBlock : StaticBody2D, IBlock
{
    // Called when the node enters the scene tree for the first time.
    protected Sprite2D sprite;
    private Tween jumpTween;
    private Area2D area;
    private Powerup powerup;
    private Texture2D emptyTexture;
    protected bool isEmpty;

    public override void _Ready()
    {
        area = GetNode<Area2D>("Area2D");
        sprite = GetNode<Sprite2D>("Sprite2D");

    }

    public void Collision(int _currentPowerUpIndex)
    {



        TryToKill();
        GpuParticles2D explosion = GetNode<GpuParticles2D>("Explosion");
        explosion.Reparent(GetParent());

        explosion.Emitting = true;
        QueueFree();

    }

    protected void TryToKill()
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

   

   


}

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
    private AudioStreamPlayer2D player;


    protected bool isEmpty;
    private bool canHit = false;


    public bool HasCoin { get; set; }

    public override void _Ready()
    {
        player = GetNode<AudioStreamPlayer2D>("Explosion/AudioStreamPlayer2D");
        area = GetNode<Area2D>("Area2D");
        sprite = GetNode<Sprite2D>("Sprite2D");

        isEmpty = false;

        canHit = true;

    }

    private void timerEnd()
    {
        canHit = true;
    }

    private void coinExplosion()
    {

    }

    public void Collision(int _currentPowerUpIndex)
    {
        TryToKill();

        GD.Print("COIN ? : " + HasCoin);
        if (HasCoin)
        {
            Timer timer = new Timer();
            timer.WaitTime = 0.2;
            timer.OneShot = true;
            canHit = false;
            timer.Timeout += timerEnd;
            AddChild(timer);
            timer.Start();


            jumpTween = CreateTween();
            jumpTween.SetTrans(Tween.TransitionType.Linear);
            jumpTween.TweenProperty(sprite, "position", new Vector2(0, -3), 0.1);
            jumpTween.Parallel().TweenProperty(sprite, "scale", new Vector2(1.1f, 1.1f), 0.1);

            jumpTween.TweenProperty(sprite, "position", new Vector2(0, 0), 0.1);
            jumpTween.Parallel().TweenProperty(sprite, "scale", new Vector2(1, 1), 0.1);

            GpuParticles2D particles = new GpuParticles2D();
            particles.Emitting = false;
            particles.OneShot = true;
            particles.Amount = 90;
            particles.Explosiveness = 1;
            particles.Lifetime = 0.4;
            //particles.Position = Position;
            particles.ProcessMaterial = ResourceLoader.Load<ParticleProcessMaterial>("res://World/Tiles/coinExplosion.tres");
            HasCoin = false;
            SpriteFrames frames = ResourceLoader.Load<SpriteFrames>("res://World/Tiles/CoinFrames.tres");
            AnimatedSprite2D coinSprite = new AnimatedSprite2D();
            coinSprite.SpriteFrames = frames;
            coinSprite.Scale = new Vector2(0.7f, 0.7f);
            coinSprite.ZIndex = -3;
            coinSprite.Animation = "default";
            AddChild(coinSprite);
            coinSprite.Play();
            Tween coinTween = CreateTween();
            coinTween.SetTrans(Tween.TransitionType.Linear);
            coinTween.TweenProperty(coinSprite, "position", new Vector2(0, -50), 0.25);
            player.Stream = ResourceLoader.Load<AudioStream>("res://SoundsBeta/coin.ogg");
            player.Play();
            coinTween.Finished += () =>
            {
                particles.Position = Position + new Vector2(0, -50);
                AddSibling(particles);
                particles.Emitting = true;

                particles.Finished += particles.QueueFree;

            };
            coinTween.Finished += coinSprite.QueueFree;

        }
        else
        {
            if (_currentPowerUpIndex == 0 && canHit)
            {
                jumpTween = CreateTween();
                jumpTween.SetTrans(Tween.TransitionType.Linear);
                jumpTween.TweenProperty(sprite, "position", new Vector2(0, -3), 0.1);
                jumpTween.Parallel().TweenProperty(sprite, "scale", new Vector2(1.1f, 1.1f), 0.1);

                jumpTween.TweenProperty(sprite, "position", new Vector2(0, 0), 0.1);
                jumpTween.Parallel().TweenProperty(sprite, "scale", new Vector2(1, 1), 0.1);
            }
            else
            {
                if (canHit)
                {
                    GpuParticles2D explosion = GetNode<GpuParticles2D>("Explosion");
                    explosion.Reparent(GetParent());

                    explosion.Emitting = true;
                    player.Stream = ResourceLoader.Load<AudioStream>("res://SoundsBeta/brickExplosion.ogg");
                    player.Play();
                    QueueFree();
                }

            }
        }




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

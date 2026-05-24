using Godot;
using System;

public partial class StarBlock : PowerBlock
{
    // Called when the node enters the scene tree for the first time.


    public new void  Collision(int _currentPowerUpIndex)
    {
        //Changer cte ligne pr load un objet ETOILE plutot
        Powerup powerup = ResourceLoader.Load<PackedScene>("res://StateMachine/PowerUps//FireFlower/fireFlower.tscn").Instantiate<FireFlower>();
        if (!isEmpty)
        {
            powerup.Position = Position + new Vector2(0, -30);
            GD.Print("collision");
            CallDeferred("add_sibling", powerup);

            Tween jumpTween = CreateTween();
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

}
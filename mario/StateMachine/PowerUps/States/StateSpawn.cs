using Godot;
using System;
using Godot.Collections;


public partial class StateSpawn : PowerState
{
    public StateSpawn(Powerup _powerUp) : base(_powerUp)
    {
        
    }

    public override void enter()
    {
        Tween spawnTweener = CreateTween();
        spawnTweener.SetTrans(Tween.TransitionType.Linear);
        spawnTweener.TweenProperty(powerUp.GetSprite(), "position", new Vector2(0,0),0.4);
        spawnTweener.Finished += endTween;
        
    }
    public override void exit()
    {
        
    }
    public override void PhysicsProcess(double delta)
    {
        
        
        
    }
	
    private void endTween()
    {
        EmitSignal(SignalName.finished, (int)Powerup.StateEnum.MOVE);
    }


	
}

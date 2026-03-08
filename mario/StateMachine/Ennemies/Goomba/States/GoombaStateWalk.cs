using Godot;
using System;

public partial class GoombaStateWalk : GoombaState
{
	
	public GoombaStateWalk(Goomba _goomba) : base(_goomba)
    {
        VisibleOnScreenNotifier2D notifier = goomba.getNotifier();
        notifier.ScreenExited += ScreenExited;
        goomba.Hit += HitBoxTouched;
    }

    public override void Enter(int _previousStateId)
    {
        goomba.skin.Play();
        goomba.currentXVelocity = 100;
    }

    public override void Exit(int _previousStateId)
    {
    }
	
	public override void PhysicsProcess(double _delta)
    {
        goomba.currentYVelocity = goomba.IsOnFloor() ? 0 : 200;
        if (goomba.IsOnWall()){
            flipGoomb();
        }
        goomba.Velocity = new Vector2(goomba.currentXVelocity, goomba.currentYVelocity);
        
    }


    private void flipGoomb()
    {
        goomba.currentXVelocity = -goomba.currentXVelocity;
        goomba.skin.FlipH = !goomba.skin.FlipH;
    }

    private void ScreenExited()
    {
        EmitSignal(SignalName.Finished, (int)GoombaStateEnum.IDLE);
    }

    private void HitBoxTouched()
    {
        EmitSignal(SignalName.Finished, (int)GoombaStateEnum.DIE);
    }
}

using Godot;
using System;

public partial class GoombaStateDie : GoombaState
{
	private Timer deathTimer;
	public GoombaStateDie(Goomba _goomba) : base(_goomba)
    {
        deathTimer = new Timer();
        deathTimer.WaitTime = 2;
        deathTimer.Timeout +=deathTimerTimeout;
        AddChild(deathTimer);
    }

    public override void Enter(int _previousStateId)
    {
        goomba.skin.Animation = "DIE";
        goomba.CollisionLayer = 0;
        goomba.CollisionMask = 0;
        goomba.Velocity = new Vector2(0,0);
        goomba.chapeau.CollisionMask = 3;

        goomba.chapeau.SetDeferred("freeze",false);
        float rand = (GD.Randf()*2) - 1;
        GD.Print("rand " + rand );
        goomba.chapeau.CallDeferred("apply_impulse",new Vector2(150 * rand,-120));
        goomba.chapeau.CallDeferred("apply_torque",50000*rand);
        goomba.chapeau.CallDeferred("reparent",goomba.GetParent());
        //goomba.chapeau.Position = goomba.Position;
        GD.Print("ololol",goomba.chapeau.Position);
        
        //goomba.chapeau.Reparent(goomba.GetParent());

        deathTimer.Start();
    }

    public override void Exit(int _previousStateId)
    {
        
    }

    public override void PhysicsProcess(double _delta)
    {
    }
	

    private void deathTimerTimeout()
    {

        goomba.QueueFree();
        //
        
    }
	
}

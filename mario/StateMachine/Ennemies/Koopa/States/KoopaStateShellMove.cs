using Godot;
using System;

public partial class KoopaStateShellMove : KoopaState
{
	private bool flipBufferActive;
    private Timer flipBufferTimer;

    public KoopaStateShellMove(Koopa _koopa) : base(_koopa)
    {
        koopa = _koopa;
        flipBufferActive = false;
        flipBufferTimer = new Timer();
        flipBufferTimer.WaitTime = 0.1;
        AddChild(flipBufferTimer);
        flipBufferTimer.Timeout += FlipBufferEnd;

    }

    public override void Enter(int _previousStateId)
    {
        
    }

    public override void Exit(int _previousStateId)
    {
    }
	public override void PhysicsProcess(double _delta)
    {
         if (koopa.IsOnWall() && !flipBufferActive)
        {
            GD.Print("xddd");
            FlipKoopa();


        }
        koopa.Velocity = new Vector2(koopa.xVel, koopa.IsOnFloor() ? 0 : 200);

    }
    private void FlipKoopa()
    {
        koopa.Scale = new Vector2(-koopa.Scale.X, koopa.Scale.Y);
        koopa.xVel = -koopa.xVel;
        if(koopa.xVel > 0)
        {
            
        }
        flipBufferTimer.Start();
        flipBufferActive = true;
    }
    private void FlipBufferEnd()
    {
        flipBufferActive = false;
    }
	
}

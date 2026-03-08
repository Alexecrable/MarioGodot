using Godot;
using System;
using Godot.Collections;


public partial class StateMove : PowerState
{
    public StateMove(Powerup _powerUp) : base(_powerUp)
    {
       
    }

    public override void enter()
    {
        powerUp.GetSprite().Play();
        powerUp.currentXVelocity = 200;
        powerUp.CollisionMask = 1;
    }
    public override void exit()
    {
       
    }
    public override void PhysicsProcess(double delta)
    {
        powerUp.currentYVelocity = powerUp.IsOnFloor() ? 0 : 200;
       

    }
	
    


	
}

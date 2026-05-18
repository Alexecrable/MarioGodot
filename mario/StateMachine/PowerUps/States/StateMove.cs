using Godot;
using System;
using Godot.Collections;


public partial class StateMove : PowerState
{
    public StateMove(Powerup _powerUp, MovementComponent _movementComponent) : base(_powerUp, _movementComponent)
    {
       
    }

    public override void enter()
    {
        powerUp.GetSprite().Play();
        powerUp.CollisionMask = 3;
        movementComponent.CurrentSpeedX = powerUp.xVelocity;
    }
    public override void exit()
    {
       
    }
    public override void PhysicsProcess(double delta)
    {
        movementComponent.CurrentSpeedY = powerUp.IsOnFloor() ? 0 : 200;
       

    }
	
    


	
}

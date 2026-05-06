using Godot;
using System;

public partial class StateFalling : MarioState
{

    private MovementComponent movementComponent;
    
    public StateFalling(Mario _mario, MovementComponent _movement) : base(_mario)
    {
        movementComponent = _movement;
                GD.Print("mariogravAccel" + mario.gravityAccel);

    }




    override public void Enter(int _stateID)
    {
        mario.animation.Animation = "Falling";
        mario.animation.Play();
        movementComponent.SetxAccel(mario.airborneHorizontalAccel);
        movementComponent.SetyAccel(mario.gravityAccel);
        movementComponent.SetMaxSpeedX(mario.maxHorizontalVelocity);
        movementComponent.SetMaxSpeedY(mario.terminalGravity);
        movementComponent.CurrentXSpeed = mario.currentHorizontalVelocity;
        movementComponent.SetCurrentYVel(mario.yVelocity);
    }



    override public void Exit(int _stateID)
    {

    }

    override public void PhysicsProcess(double _delta)
    {
        GD.Print("falling");
        if (!mario.IsGoingDown() && mario.yVelocity > 0)
        {
            mario.SetGoingDown();
        }
        
        movementComponent.AccelerateToSpeedX(_delta);
        movementComponent.AccelerateToSpeedY(_delta);
        
        if (mario.IsOnCeiling())
        {
            movementComponent.SetCurrentYVel(10);
            mario.SetGoingDown();
            GD.Print("mari  " + mario.yVelocity);
        }
        movementComponent.Advance(_delta);
        //GD.Print("floor " + mario.IsOnFloor());
        if (mario.IsOnFloor())
        {

            if (mario.IsRunning())
            {
                EmitSignal(SignalName.Finished, (int)Mario.StateEnum.MOVE);

            }
            else
            {
                EmitSignal(SignalName.Finished, (int)Mario.StateEnum.IDLE);
            }
        }
        else
        {
            if (mario.IsOnWall())
            {
               if (mario.Velocity.Y > 0 && mario.IsGrabbingWall())
                {
                    EmitSignal(SignalName.Finished, (int)Mario.StateEnum.WALLSLIDE);
                }
                else
                {
                    if (Input.IsActionJustPressed("jump"))
                    {
                        mario.animation.FlipH = !mario.animation.FlipH;
                        if (mario.raycastLeft.IsColliding())
                        {
                            if (mario.raycastRight.IsColliding())
                            {
                                movementComponent.CurrentXSpeed = 0;
                            }
                            else
                            {
                                mario.currentHorizontalVelocity = mario.speed;
                                movementComponent.CurrentXSpeed = mario.speed;
                            }
                        }
                        else
                        {
                            if (mario.raycastRight.IsColliding())
                            {
                                movementComponent.CurrentXSpeed = -mario.speed;
                            }
                            else
                            {
                                movementComponent.CurrentXSpeed = 0;
                            }
                            
                        }
                        EmitSignal(SignalName.Finished, (int)Mario.StateEnum.JUMP);
                    }
                }
            }
        }

    }





}

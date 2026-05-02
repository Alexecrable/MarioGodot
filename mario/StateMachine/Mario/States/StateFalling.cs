using Godot;
using System;

public partial class StateFalling : MarioState
{

    
    public StateFalling(Mario _mario) : base(_mario)
    {
        
    }




    override public void Enter(int _stateID)
    {
        mario.animation.Animation = "Falling";
        mario.animation.Play();
            


    }



    override public void Exit(int _stateID)
    {

    }

    override public void PhysicsProcess(double _delta)
    {

        if (!mario.IsGoingDown() && mario.yVelocity > 0)
        {
            mario.SetGoingDown();
        }
        if (mario.yVelocity < mario.terminalGravity)
        {
            mario.yVelocity += mario.gravityAccel * (float)_delta;
        }

        if (mario.maxHorizontalVelocity > mario.currentHorizontalVelocity * (mario.rightInput - mario.leftInput))
        {
            mario.currentHorizontalVelocity += mario.airborneHorizontalAccel * (mario.rightInput - mario.leftInput) * (float)_delta;
        }
        if (mario.IsOnCeiling())
        {
            mario.yVelocity = 10;
            mario.SetGoingDown();
            GD.Print("mari  " + mario.yVelocity);
        }
        mario.Velocity = new Vector2(mario.currentHorizontalVelocity, mario.yVelocity);
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
                                mario.currentHorizontalVelocity = 0;
                            }
                            else
                            {
                                mario.currentHorizontalVelocity = mario.speed;
                            }
                        }
                        else
                        {
                            if (mario.raycastRight.IsColliding())
                            {
                                mario.currentHorizontalVelocity = -mario.speed;
                            }
                            else
                            {
                                mario.currentHorizontalVelocity = 0;
                            }
                            
                        }
                        EmitSignal(SignalName.Finished, (int)Mario.StateEnum.JUMP);
                    }
                }
            }
        }

    }





}

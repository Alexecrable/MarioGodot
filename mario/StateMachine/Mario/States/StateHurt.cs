using Godot;
using System;

public partial class StateHurt : MarioState
{
    public StateHurt(Mario _mario) : base(_mario)
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

    override public void PhysicsProcess(double delta)
    {

        if (mario.yVelocity < mario.terminalGravity)
        {
            mario.yVelocity += mario.gravityAccel * (float)delta;
        }
        bool isRunning = mario.rightInput + mario.leftInput > 0;

        if (mario.maxHorizontalVelocity > mario.currentHorizontalVelocity * (mario.rightInput - mario.leftInput))
        {
            mario.currentHorizontalVelocity += mario.airborneHorizontalAccel * (mario.rightInput - mario.leftInput) * (float)delta;
        }
        mario.Velocity = new Vector2(mario.currentHorizontalVelocity, mario.yVelocity);
        //GD.Print("floor " + mario.IsOnFloor());
        if (mario.IsOnFloor())
        {

            if (isRunning)
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
                bool marioGoesRight = mario.rightInput - mario.leftInput > 0;
                bool marioGoesLeft = mario.rightInput - mario.leftInput < 0;
                bool marioGrabsWall = (mario.raycastLeft.IsColliding() && marioGoesLeft) || (mario.raycastRight.IsColliding() && marioGoesRight);
                if (mario.Velocity.Y > 0 && marioGrabsWall)
                {
                    EmitSignal(SignalName.Finished, (int)Mario.StateEnum.WALLSLIDE);
                }
                else
                {
                    if (mario.jumpInput > 0)
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

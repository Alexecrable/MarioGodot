using Godot;
using System;

public partial class StateIdle : MarioState
{
    public StateIdle(Mario _mario) : base(_mario)
    {
    }




    override public void Enter(int _stateID)
    {
        mario.animation.Animation = "Idle";
        mario.animation.Play();

        mario.yVelocity = mario.startingGravity;
        mario.SetGoingNeutral();
    }

    override public void Exit(int _stateID)
    {

    }

    override public void PhysicsProcess(double delta)
    {


        mario.currentHorizontalVelocity = (mario.rightInput - mario.leftInput) * mario.speed;
        mario.Velocity = new Vector2(mario.currentHorizontalVelocity, mario.yVelocity);

        if (!mario.IsOnFloor())
        {
            EmitSignal(SignalName.Finished, (int)Mario.StateEnum.FALL);
        }
        else
        {
            if (mario.IsRunning())
            {
                EmitSignal(SignalName.Finished, (int)Mario.StateEnum.MOVE);
            }
            else
            {
                if (Input.IsActionJustPressed("jump"))
                {
                    EmitSignal(SignalName.Finished, (int)Mario.StateEnum.JUMP);
                }
            }

        }
    }





}

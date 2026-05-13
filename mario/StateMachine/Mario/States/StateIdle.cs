using Godot;
using System;

public partial class StateIdle : MarioState
{

    private MovementComponent movementComponent;

    public StateIdle(Mario _mario, MovementComponent _movementComponent) : base(_mario)
    {
        movementComponent = _movementComponent;
    }




    override public void Enter(int _stateID)
    {
        mario.animation.Animation = "Idle";
        mario.animation.Play();

        
        movementComponent.CurrentSpeedY = 0;
        mario.SetGoingNeutral();

        mario.feetBox.CollisionLayer = 0;
        mario.feetBox.CollisionMask = 0;
    }

    override public void Exit(int _stateID)
    {

    }

    override public void PhysicsProcess(double _delta)
    {   
        movementComponent.SpeedX();
        movementComponent.Advance();
        if (!mario.IsOnFloor())
        {
            EmitSignal(SignalName.Finished, (int)Mario.StateEnum.FALL);
        }
        else
        {
            if (movementComponent.IsMoving())
            {
                EmitSignal(SignalName.Finished, (int)Mario.StateEnum.MOVE);
            }
            else
            {
                if (movementComponent.WantsJump())
                {
                    EmitSignal(SignalName.Finished, (int)Mario.StateEnum.JUMP);
                }
            }

        }
    }





}

using Godot;
using System;

public partial class StateDie : MarioState
{

    private float XVelocitySave, yVelocitySave;
    private Timer hurtTimer;
    private Tween tween;
    private int previousStateId;
    private MovementComponent movementComponent;



    public StateDie(Mario _mario, MovementComponent _movementComponent) : base(_mario)
    {

        movementComponent = _movementComponent;
    }




    override public void Enter(int _stateID)
    {
        mario.animation.Animation = "Die";
        mario.animation.Play();
        mario.animation.AnimationFinished += DeathFinished;
        movementComponent.CurrentSpeedX = 0;
        movementComponent.CurrentSpeedY = 0;
        movementComponent.Advance();
        mario.animation.Animation = "Die";
        mario.animation.Play();
        //mario.CollisionLayer = 0;
    }

    private void DeathFinished()
    {
        mario.animation.AnimationFinished -= DeathFinished;
        if (mario.Lives == 0)
        {
            mario.GameOver();
        }
        else
        {
            mario.Lives--;
            mario.EmittingLifeUpdate();
            Respawn();
        }
        //EmitSignal(SignalName.Finished, (int)Mario.StateEnum.IDLE);
    }

    private void Respawn()
    {
        mario.EmittingRespawnSignal();
    }

   
    override public void Exit(int _stateID)
    {
        //mario.Velocity = VelocitySave;
        //tween.Stop();
        //GetTree().Paused = false;
    }

    override public void PhysicsProcess(double delta)
    {
        GD.Print("Dying : " + mario.Velocity, " | " + mario.Position);

        //movementComponent.Advance();
    }





}

using Godot;
using System;

public partial class StateHurt : MarioState
{

    private float XVelocitySave, yVelocitySave;
    private Timer hurtTimer;
    private Tween tween;
    private int previousStateId;
    private MovementComponent movementComponent;

    

    public StateHurt(Mario _mario, MovementComponent _movementComponent) : base(_mario)
    {
        hurtTimer = new Timer();
        hurtTimer.WaitTime = 1;
        hurtTimer.Timeout += HurtTimeOut;
        hurtTimer.OneShot = true;
        AddChild(hurtTimer);
        hurtTimer.ProcessMode = Node.ProcessModeEnum.WhenPaused;
        movementComponent = _movementComponent;

    }




    override public void Enter(int _stateID)
    {
        //XVelocitySave = movementComponent.CurrentSpeedX;
        //yVelocitySave = movementComponent.CurrentSpeedY;
        previousStateId = _stateID;
        //mario.animation.Pause();
        //mario.Velocity = new Vector2(0, 0);
        //tween = CreateTween();
        //tween.SetTrans(Tween.TransitionType.Sine);
        //tween.SetLoops();
        //tween.TweenProperty(mario.animation, "modulate", new Color(0, 0, 0), 0.2);
        //tween.TweenProperty(mario.animation, "modulate", new Color(1, 1, 1), 0.2);
        switch (mario.currentPowerUpIndex)
        {
            case (int)Mario.PowerEnum.BASE : mario.Scale = new Vector2(1,1);
            break;
            case (int)Mario.PowerEnum.FIRE : ;
            break;
        
        }
        GetTree().Paused = true;
        hurtTimer.Start();

        mario.feetBox.CollisionLayer = 0;
        mario.feetBox.CollisionMask = 0;
    }

    private void HurtTimeOut()
    {
        GetTree().Paused = false;

        EmitSignal(SignalName.Finished, previousStateId);
    }

    override public void Exit(int _stateID)
    {
        //mario.Velocity = VelocitySave;
        //tween.Stop();
        //GetTree().Paused = false;
    }

    override public void PhysicsProcess(double delta)
    {



    }





}

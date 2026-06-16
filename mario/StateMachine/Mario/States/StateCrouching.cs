using Godot;
using System;

public partial class StateCrouching : MarioState
{

    private MovementComponent movementComponent;
    private Vector2 scaleSave;

    public StateCrouching(Mario _mario, MovementComponent _movement) : base(_mario)
    {
        movementComponent = _movement;

    }


    private bool CheckForPipe()
    {
        int nbColl = mario.GetSlideCollisionCount();
        for (int i = 0; i < nbColl; i++)
        {
            KinematicCollision2D coll = mario.GetSlideCollision(i);
            if (coll.GetNormal() == Vector2.Up)
            {
                try
                {
                    Pipe pipe = (Pipe)coll.GetCollider();
                    pipe.EnterAnim(mario);
                    return true;
                }
                catch { }
            }
        }
        return false;
    }

    override public void Enter(int _stateID)
    {
        bool onPipe = CheckForPipe();

        CollisionShape2D marioshape = mario.GetNode<CollisionShape2D>("CollisionShape2D");
        marioshape.Scale *= new Vector2(1, 0.6f);
        marioshape.Position *= new Vector2(1, 0.6f);
        mario.hurtComponent.Scale *= new Vector2(1, 0.6f);
        mario.hurtComponent.Position *= new Vector2(1, 0.6f);
        mario.chapeau.Position *= new Vector2(1, 0.6f);
        mario.animation.Animation = "Crouching";
        mario.animation.Play();

        GD.Print("enter state : falling");
    }



    override public void Exit(int _stateID)
    {
        CollisionShape2D marioshape = mario.GetNode<CollisionShape2D>("CollisionShape2D");
        marioshape.Scale /= new Vector2(1, 0.6f);
        marioshape.Position /= new Vector2(1, 0.6f);
        mario.hurtComponent.Scale /= new Vector2(1, 0.6f);
        mario.hurtComponent.Position /= new Vector2(1, 0.6f);
        mario.chapeau.Position /= new Vector2(1, 0.6f);
    }

    override public void PhysicsProcess(double _delta)
    {
        //if (!mario.IsGoingDown() && mario.yVelocity > 0)
        //{
        //    mario.SetGoingDown();
        //}

        movementComponent.SpeedX();
        movementComponent.Advance();


        if (!mario.IsOnFloor())
        {
            EmitSignal(SignalName.Finished, (int)Mario.StateEnum.FALL);
        }
        else
        {
            if (!movementComponent.WantsToCrouch() && !mario.ceilingChecker.IsColliding())
            {
                EmitSignal(SignalName.Finished, (int)Mario.StateEnum.IDLE);
            }
        }

    }





}

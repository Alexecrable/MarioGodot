using Godot;
using System;

public partial class StateTransform : MarioState
{
    public StateTransform(Mario _mario) : base(_mario)
    {

    }




    override public void Enter(int _stateID)
    {
        mario.animation.Animation = "Transforming";
        mario.animation.Play();
        mario.feetBox.CollisionLayer = 0;
        mario.feetBox.CollisionMask = 0;

    }



    override public void Exit(int _stateID)
    {

    }

    override public void PhysicsProcess(double delta)
    {


    }





}

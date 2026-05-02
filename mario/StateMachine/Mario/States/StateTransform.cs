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

    }



    override public void Exit(int _stateID)
    {

    }

    override public void PhysicsProcess(double delta)
    {


    }





}

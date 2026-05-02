using Godot;
using System;

public partial class KoopaStateShellIdle : KoopaState
{
    Node2D koopaBody;
    private bool flipBufferActive;
    private Timer flipBufferTimer;
    private bool isShell;


    public KoopaStateShellIdle(Koopa _koopa) : base(_koopa)
    {
        koopa = _koopa;
        koopaBody = koopa.GetNode<Node2D>("Body");
        koopa.CollisionLayer = 0;
        koopa.hurtBox.BodyEntered += Hittt;
        isShell = false;
    }

    private void Hittt(Node2D _body)
    {
        if (_body.Name == "Mario" && isShell)
        {
            if (_body.GlobalPosition.X < koopa.GlobalPosition.X)
            {
                koopa.xVel = 100;
                koopa.Scale = new Vector2(1, 1);
                koopa.shell.Play();
            }
            else
            {
                koopa.xVel = -100;
                koopa.Scale = new Vector2(-1, 1);
                koopa.shell.PlayBackwards();
            }
            isShell = false;

            EmitSignal(SignalName.Finished, (int)KoopaStateEnum.SHELL_MOVE);
        }

    }
    public override void Enter(int _previousStateId)
    {
        koopaBody.Hide();
        koopa.Velocity = new Vector2(0, 0);
        koopa.CollisionLayer = 0;
        isShell = true;
    }


    public override void Exit(int _previousStateId)
    {
        koopa.CollisionLayer = 8;
    }


    public override void PhysicsProcess(double _delta)
    {

    }



}

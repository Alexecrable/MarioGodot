using Godot;
using System;

public partial class KoopaStateShellIdle : KoopaState
{
    Node2D koopaBody;
    private bool flipBufferActive;
    private Timer flipBufferTimer, enterStateDelay;
    private bool isShell;


    public KoopaStateShellIdle(Koopa _koopa, MovementComponent _movementComponent) : base(_koopa, _movementComponent)
    {
        koopa = _koopa;
        koopaBody = koopa.GetNode<Node2D>("Body");
        enterStateDelay = new Timer();
        enterStateDelay.WaitTime = 0.1;
        enterStateDelay.Timeout += EndDelay;
        AddChild(enterStateDelay);
        
        isShell = false;

    }

    private void EndDelay()
    {
        koopa.hurtBox.CollisionLayer = 8;   
        koopa.hurtBox.CollisionMask = 4;
    }

    private void Hittt(Node2D _body)
    {
        if (_body.Name == "FeetBox" && isShell)
        {
            if (_body.GlobalPosition.X < koopa.GlobalPosition.X)
            {
                movementComponent.CurrentSpeedX = 100;
                koopa.Scale = new Vector2(1, 1);
                koopa.shell.Play();
            }
            else
            {
                movementComponent.CurrentSpeedX = -100;
                koopa.Scale = new Vector2(-1, 1);
                koopa.shell.PlayBackwards();
            }
            isShell = false;

            EmitSignal(SignalName.Finished, (int)KoopaStateEnum.SHELL_MOVE);
        }

    }
    public override void Enter(int _previousStateId)
    {
        GD.Print("enter state : ShellIdle" + this.Name);
        koopaBody.Hide();
        movementComponent.CurrentSpeedX = 0;
        movementComponent.Advance();
        isShell = true;
        koopa.hurtBox.CollisionLayer = 0;
        koopa.hurtBox.CollisionMask = 0;
        enterStateDelay.Start();
        koopa.hurtBox.AreaEntered += Hittt;
    }


    public override void Exit(int _previousStateId)
    {
        koopa.hurtBox.AreaEntered -= Hittt;
    }


    public override void PhysicsProcess(double _delta)
    {

    }



}

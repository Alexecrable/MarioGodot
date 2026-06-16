using Godot;
using System;

public partial class KoopaStateShellIdle : KoopaState
{
    Node2D koopaBody;
    private bool flipBufferActive;
    private Timer flipBufferTimer, enterStateDelay;
    private uint saveLayerBody, saveMaskBody, saveLayerHurt, saveMaskHurt, saveLayerHit, saveMaskHit;
    private bool isShell;


    public KoopaStateShellIdle(Koopa _koopa, MovementComponent _movementComponent) : base(_koopa, _movementComponent)
    {
        koopaBody = koopa.GetNode<Node2D>("Body");
        enterStateDelay = new Timer();
        enterStateDelay.WaitTime = 0.1;
        enterStateDelay.OneShot = true;
        enterStateDelay.Timeout += EndDelay;


        AddChild(enterStateDelay);

        isShell = false;

    }

    private void EndDelay()
    {
        //koopa.CollisionLayer = saveLayerBody;
        //koopa.CollisionMask = saveMaskBody;
        koopa.hurtBox.CollisionLayer = saveLayerHurt;
        koopa.hurtBox.CollisionMask = saveMaskHurt;
        
    }

    private void Hittt(Node2D _body)
    {
        if (_body.Name == "FeetBox" || _body.Name == "Mario")
        {
            if (_body.GlobalPosition.X < koopa.GlobalPosition.X)
            {
                movementComponent.CurrentSpeedX = 300;
                koopa.Scale = new Vector2(1, 1);
                koopa.shell.Play();
            }
            else
            {
                movementComponent.CurrentSpeedX = -300;
                koopa.Scale = new Vector2(-1, 1);
                koopa.shell.Play();
            }
            isShell = false;

            EmitSignal(SignalName.Finished, (int)KoopaStateEnum.SHELL_MOVE);
        }

    }
    public override void Enter(int _previousStateId)
    {

        //saveLayerBody = koopa.CollisionLayer;
        //saveMaskBody = koopa.CollisionMask;
        saveLayerHurt = koopa.hurtBox.CollisionLayer;
        saveMaskHurt = koopa.hurtBox.CollisionMask;
        saveLayerHit = koopa.hitBox.CollisionLayer;
        saveMaskHit = koopa.hitBox.CollisionMask;

        //koopa.CollisionLayer = 0;
        //koopa.CollisionMask = 0;
        koopa.hurtBox.CollisionLayer = 0;
        koopa.hurtBox.CollisionMask = 0;
        koopa.hitBox.CollisionLayer = 0;
        koopa.hitBox.CollisionMask = 0;

        koopaBody.Hide();
        movementComponent.CurrentSpeedX = 0;
        movementComponent.Advance();
        isShell = true;

        enterStateDelay.Start();
        koopa.hurtBox.AreaEntered += Hittt;
        koopa.hurtBox.BodyEntered += Hittt;
        koopa.shell.Pause();
    }


    public override void Exit(int _previousStateId)
    {
        koopa.hurtBox.AreaEntered -= Hittt;
        koopa.hurtBox.BodyEntered -= Hittt;
        koopa.hitBox.CollisionLayer = saveLayerHit;
        koopa.hitBox.CollisionMask = saveMaskHit;
    }


    public override void PhysicsProcess(double _delta)
    {

    }



}

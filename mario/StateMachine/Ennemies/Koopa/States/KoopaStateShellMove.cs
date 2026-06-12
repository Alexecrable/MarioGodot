using Godot;
using System;

public partial class KoopaStateShellMove : KoopaState
{
    private bool flipBufferActive;
    private Timer flipBufferTimer, enterStateDelay;
    private uint collisionMaskHold, collisionLayerHold;

    public KoopaStateShellMove(Koopa _koopa, MovementComponent _movementComponent) : base(_koopa, _movementComponent)
    {
        koopa = _koopa;
        flipBufferActive = false;
        flipBufferTimer = new Timer();
        flipBufferTimer.WaitTime = 0.1;
        AddChild(flipBufferTimer);
        flipBufferTimer.Timeout += FlipBufferEnd;
        enterStateDelay = new Timer();
        enterStateDelay.WaitTime = 1;
        AddChild(enterStateDelay);

    }

    public override void Enter(int _previousStateId)
    {

        koopa.hurtBox.AreaEntered += Hittt;

        enterStateDelay.Start();
        enterStateDelay.Timeout += EndDelay;
        collisionLayerHold = koopa.CollisionLayer;
        collisionMaskHold = koopa.CollisionMask;



    }

    public void EndDelay()
    {
        //koopa.CollisionLayer = 16;
    }
    private void Hittt(Area2D _body)
    {

        EmitSignal(SignalName.Finished, (int)KoopaStateEnum.SHELL_IDLE);


    }
    public override void Exit(int _previousStateId)
    {
        enterStateDelay.Timeout -= EndDelay;
        koopa.hurtBox.AreaEntered -= Hittt;

    }
    public override void PhysicsProcess(double _delta)
    {
        ManualCollision();
        movementComponent.CurrentSpeedY = (koopa.IsOnFloor()) ? 0 : 200;
        movementComponent.Advance();





    }

    private void ManualCollision()
    {
        if (koopa.GetSlideCollisionCount() > 0)
        {
            GD.Print("slidecount: " + koopa.GetSlideCollisionCount());
        }
        for (int i = 0; i < koopa.GetSlideCollisionCount(); i++)
        {
            KinematicCollision2D coll = koopa.GetSlideCollision(i);
            if (coll.GetNormal() == Vector2.Left || coll.GetNormal() == Vector2.Right)
            {
                GodotObject godotObject = coll.GetCollider();
                try
                {
                    IInstaKillableObject ennemi = (IInstaKillableObject)godotObject;
                    GD.Print("essai meurtre : 1");
                    
                    try
                    {
                        Koopa otherKoop = (Koopa)ennemi;
                        GD.Print("essai meurtre : 2");

                        if (otherKoop.currentStateIndex == (int)KoopaStateEnum.SHELL_MOVE)
                        {
                            koopa.InstaKill();
                            GD.Print("essai meurtre : 3");   
                        }
                        else
                        {
                            otherKoop.InstaKill();
                            return;
                        }
                    }
                    catch (Exception e)
                    {
                        GD.Print("pas un ennemu");
                    }
                    ennemi.InstaKill();
                    return;
                }
                catch (Exception e)
                {
                    GD.Print("echecmeurtre" + e);
                }
                try
                {
                    IBlock block = (IBlock)godotObject;
                    block.Collision(1);

                }
                catch (Exception e)
                {
                    GD.Print("slideTest: Echec " + e);
                }
                FlipKoopa();
                return;

            }
        }
    }
    private void FlipKoopa()
    {
        GD.Print("koopa:flip");
        koopa.Scale = new Vector2(-koopa.Scale.X, koopa.Scale.Y);
        movementComponent.CurrentSpeedX = -movementComponent.CurrentSpeedX;

        flipBufferTimer.Start();
        flipBufferActive = true;
    }
    private void FlipBufferEnd()
    {
        flipBufferActive = false;
    }


}

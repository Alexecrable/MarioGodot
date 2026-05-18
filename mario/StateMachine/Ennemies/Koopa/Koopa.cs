using Godot;
using Godot.Collections;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;





///////////////////
/// //////////////////////////
/// 
/// 
/// 
/// 
/// 
/// 
/// 
/// 
/// 
/// FAUT CHANGER TON SYSTEME DE COLLISIONS POUR LES DEGATS
/// 
/// 
/// 
/// 
/// 
/// 
/// /////////////////////////:::::::
public enum KoopaStateEnum
    {
        IDLE = 0,
        WALK = 1,
        DIE = 2,
        SHELL_IDLE = 3,
        SHELL_MOVE = 4,
        WAKEUP = 5
    }
public partial class Koopa : Ennemi
{

    [Export]
    private int baseExportTest = 3;
    

    private Array<KoopaState> states;
    public Area2D turnAroundArea;
    private VisibleOnScreenNotifier2D notifier;
    public AnimatedSprite2D legs,eyes,shell;
    public Area2D marioNotif, hurtBox;
    private MovementComponent movementComponent;
    public int xVel;


    public override void _Ready()
    {
        legs = GetNode<AnimatedSprite2D>("Body/Legs");
        eyes = GetNode<AnimatedSprite2D>("Body/Eyes");
        shell = GetNode<AnimatedSprite2D>("Shell");
        notifier = GetNode<VisibleOnScreenNotifier2D>("Notifier");
        marioNotif = GetNode<Area2D>("MarioLookArea");
        marioNotif.BodyEntered += MarioSpotted;
        marioNotif.BodyExited += MarioLost;
        turnAroundArea = GetNode<Area2D>("TurnAroundArea");
        hurtBox = GetNode<Area2D>("HurtBox");
        movementComponent = GetNode<MovementComponent>("MovementComponent");
        movementComponent.Init(this);
        movementComponent.CurrentSpeedY = 200;
        xVel = 50;
        InitState();
    }

    
    

    private void MarioLost(Node body)
    {
        GD.Print("deconnexion");
        if(eyes.Animation == "SHOCKED")
        {
        eyes.AnimationFinished -= ShockFinish;
        }
        eyes.Animation = "NORMAL";
        eyes.Play();
    }
    private void MarioSpotted(Node body)
    {
        GD.Print("connection");
        eyes.AnimationFinished += ShockFinish;
        eyes.Animation = "SHOCKED";
        eyes.Play();
    }

    private void ShockFinish()
    {
        GD.Print("shocked !!!");
        eyes.AnimationFinished -= ShockFinish;
        eyes.Pause();
        eyes.Animation = "ANGRY";
    }

    public override void InitState()
    {
        states = [
            new KoopaStateIdle(this, movementComponent),
            new KoopaStateWalk(this, movementComponent),
            new KoopaStateDie(this, movementComponent),
            new KoopaStateShellIdle(this, movementComponent),
            new KoopaStateShellMove(this, movementComponent),
            new KoopaStateWakeUp(this, movementComponent)
        ];

        foreach (KoopaState state in states)
        {
            AddChild(state);
            state.Finished += ChangeState;

        }
        currentStateIndex = 0;
        lastStateIndex = 0;
        states[currentStateIndex].Enter(lastStateIndex);

    }

    public override void ChangeState(int _stateIndex)
    {
        int lastIndex = currentStateIndex;
        states[currentStateIndex].Exit(_stateIndex);
        currentStateIndex = _stateIndex;
        states[currentStateIndex].Enter(lastIndex);
        GD.Print("go to state " + currentStateIndex);

    }

    public override void _PhysicsProcess(double _delta)
    {
        states[currentStateIndex].PhysicsProcess(_delta);
        MoveAndSlide();
    }

    public VisibleOnScreenNotifier2D getNotifier()
    {
        return notifier;
    }

    public override void InstaKill()
    {
       ChangeState((int)KoopaStateEnum.DIE);
    }



}

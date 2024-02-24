using Godot;
using System;

public partial class MovingTurningPlatformConcept : Node3D 
{
    [Export] public float TurnSpeed {get; set;}
    [Export] public float MaxRotTime {get; set;}
    [Export] public int TotalPositions {get; set;}
    [Signal] public delegate void FinishedChangeEventHandler();

    private float RotationSpeed;
    private static int DEFAULT_ROT_POS = 4;
    private static float DEFAULT_TURN_SPEED = 1;
    private static float DEFAULT_MAX_ROT_TIME = 3;
    private float RotDegrees;
    private Tween tween;

    public override void _Ready()
    {
        if(TotalPositions <= 0 || TotalPositions > 360) TotalPositions = DEFAULT_ROT_POS;

        if(MaxRotTime <= 0) MaxRotTime = DEFAULT_MAX_ROT_TIME;

        if(TurnSpeed <= 0) TurnSpeed = DEFAULT_TURN_SPEED;

        RotDegrees = 360 / TotalPositions;
        RotationSpeed = MaxRotTime / TurnSpeed;

    }

    public void ChangeState(CharacterBody3D interactor)
    {
        tween = CreateTween();
        tween.TweenProperty(this, "rotation_degrees", new Vector3(0, RotDegrees,0) , (double) RotationSpeed).AsRelative();
        tween.TweenCallback(Callable.From(this.FinishChange));
        tween.Play();
    }

    public void FinishChange()
    {
        tween.Stop();
        EmitSignal(SignalName.FinishedChange);

    }
}
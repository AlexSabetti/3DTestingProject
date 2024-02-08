using Godot;
using System;

public partial class PlayerMovement : CharacterBody3D
{
	[Export] public int Speed {get; set;} = 12;
	
	Vector3 velocity = new();

	public void GetInput(){
		velocity = new();
		if(Input.IsActionPressed("move_right")){
			velocity.X += 1.0f;
		}

		if(Input.IsActionPressed("move_left")){
			velocity.X -= 1.0f;
		}

		if(Input.IsActionPressed("move_down")){
			velocity.Z += 1.0f;
		}

		if(Input.IsActionPressed("move_up")){
			velocity.Z -= 1.0f;
		}

		if(velocity.X != 0 && velocity.Z != 0){
			velocity.X /= 2;
			velocity.Z /= 2;
		}

		velocity = velocity.Normalized() * Speed;
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		GetInput();
		MoveTank(velocity);
	}

	public void MoveTank(Vector3 velocity){
		this.Velocity = velocity;
		MoveAndSlide();
	}

    public override void _Process(double delta)
    {
        float fps = (float) Engine.GetFramesPerSecond();
		Vector3 larp_interval = velocity / new Vector3(fps,fps,fps);
		Vector3 larp_position = GlobalTransform.Origin + larp_interval;
		

    }
}

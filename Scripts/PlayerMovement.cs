using Godot;
using System;
using System.Diagnostics;

public partial class PlayerMovement : CharacterBody3D
{
	[Export] public int Speed {get; set;} = 6;
	[Export] public float PersonalGrav {get; set;} = 18.8f;
	[Export] public float JumpSpeed {get; set;} = 6f;
	[Export] public float MouseSens {get; set;} = 0.003f;
	[Export] public float MaxSpeed {get; set;} = 8;
	[Export] public float AirSpeedDecay {get; set;} = 0.01f;
	[Export] public float AirControlPenalty {get; set;} = 0.03f;

	public Camera3D charCam;
	public Node3D Pivot;
	
	Vector3 velocity = new();
	public void Control(double delta)
    {

        float walkSpeedMod = Speed;

        if (!IsOnFloor()) 
        {
            velocity.Y -= PersonalGrav * (float)delta;
            //velocity.X -= velocity.X * AirSpeedDecay * (float)delta;
			//velocity.Z -= velocity.Z * AirSpeedDecay * (float)delta;

			Vector2 movement = Input.GetVector("left", "right", "forward", "back");
        
        	//GD.Print("Movement Initial Vector: " + movement);

        	Vector3 movementDir = (Transform.Basis * new Vector3(movement.X, 0, movement.Y)).Normalized();

        	if (IsOnFloor() && Input.IsActionJustPressed("jump"))
        	{
            	velocity.Y = JumpSpeed;
        	}
        
			walkSpeedMod *= AirControlPenalty;
        	velocity.X += movementDir.X * walkSpeedMod;
        	velocity.Z += movementDir.Z * walkSpeedMod;

			if(movementDir.X == 0) velocity.X = Mathf.Clamp(velocity.X, -1 * Speed, Speed);
			else velocity.X = Mathf.Clamp(velocity.X, Mathf.Abs(movementDir.X) * -1*Speed, Mathf.Abs(movementDir.X) * Speed);
			if(movementDir.Z == 0) velocity.Z = Mathf.Clamp(velocity.Z, -1 * Speed, Speed);
			else velocity.Z = Mathf.Clamp(velocity.Z, Mathf.Abs(movementDir.Z) * -1*Speed, Mathf.Abs(movementDir.Z) * Speed);
			
			velocity.Y = Mathf.Clamp(velocity.Y, -14, 1000);
        	this.Velocity = velocity;
			GD.Print(Velocity);

        } else {
			//GD.Print("Set VelY: " + velocity.Y);
    		Vector2 movement = Input.GetVector("left", "right", "forward", "back");
        	
        	//GD.Print("Movement Initial Vector: " + movement);

        	Vector3 movementDir = (Transform.Basis * new Vector3(movement.X, 0, movement.Y)).Normalized();

        	if (IsOnFloor() && Input.IsActionJustPressed("jump"))
        	{
            	velocity.Y = JumpSpeed;
        	}
        

        	velocity.X = movementDir.X * walkSpeedMod;
        	velocity.Z = movementDir.Z * walkSpeedMod;
			
        	this.Velocity = velocity;
			GD.Print("Standing Vel: " + Velocity);
		}
        
        MoveAndSlide();
    }
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		charCam = GetNode<Camera3D>("Pivot/MyEyes");
        Pivot = GetNode<Node3D>("Pivot");
        Input.MouseMode = Input.MouseModeEnum.Captured;
        charCam.MakeCurrent();

		AirSpeedDecay = Math.Clamp(AirSpeedDecay, 0.001f, 1f);
        AirControlPenalty = Math.Clamp(AirControlPenalty, -2, 2);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		Control(delta);
		//MoveTank(velocity);
	}

	public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseMotion && Input.MouseMode == Input.MouseModeEnum.Captured)
        {
            CameraRotate(mouseMotion.Relative * MouseSens);
        }
    }

    public void CameraRotate(Vector2 move)
    {
        RotateY(-move.X);
        //charCam.Orthonormalize();
        Pivot.RotateX(-move.Y);
        Pivot.Rotation = new Vector3(Mathf.Clamp(Pivot.Rotation.X, -Mathf.DegToRad(70), Mathf.DegToRad(70)), 0, 0);

        //GD.Print("CamXRot: " + Pivot.Rotation.X);
        //GD.Print("CamYRot: " + Pivot.Rotation.Y);
        //GD.Print("CamZRot: " + Pivot.Rotation.Z);
    }

    public override void _UnhandledKeyInput(InputEvent @keyPress)
    {
        if (@keyPress.IsActionPressed("ui_cancel"))
        {
            Input.MouseMode = Input.MouseModeEnum.Visible;
        } //else if (@keyPress.IsActionPressed("select")){
          //if(Input.MouseMode == Input.MouseModeEnum.Visible){
          // Input.MouseMode = Input.MouseModeEnum.Captured;
          // }
          // }
    }

    // public override void _Process(double delta)
    // {
    //     float fps = (float) Engine.GetFramesPerSecond();
	// 	Vector3 larp_interval = velocity / new Vector3(fps,fps,fps);
	// 	Vector3 larp_position = GlobalTransform.Origin + larp_interval;
		

    // }
}

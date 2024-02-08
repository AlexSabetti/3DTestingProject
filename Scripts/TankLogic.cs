using Godot;
using System;

public partial class TankLogic : CharacterBody3D
{
    [Export] public PackedScene Bullet {get; set;}

    [Export] public int Speed {get; set;}

    [Export] public float Firing_Cooldown {get; set;}

    [Export] public float MaxTankHealth {get; set;} = 100.0f;

    public Vector3 velocity = new();

    public bool can_Fire = true;

    public bool alive = true; 
    
    public override void _Ready(){
        this.GetNode<Timer>($"Reload_Timer").WaitTime = Firing_Cooldown;
    }

    public void Control(double delta){
        Viewport viewport = GetViewport();
        this.GetNode<Sprite3D>($"Turret").LookAt(new Vector3(viewport.GetMousePosition().X, this.GetParent().GetNode<CollisionObject3D>($"Tank_Hull").Position.Y, viewport.GetMousePosition().Y));
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

    public override void _PhysicsProcess(double delta){
        if(!alive) return;
        Control(delta);
        MoveAndSlide();
    }
}

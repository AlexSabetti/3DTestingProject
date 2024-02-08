using Godot;
using System;
using System.Collections.Generic;

public partial class PlayerInput : CharacterBody3D
{
    [Export] public float BaseSpeed { get; set; }
    [Export] public float RunSpeedMod { get; set; }
    [Export] public float JumpSpeed { get; set; }
    [Export] public float BonusSpeedFromJumping { get; set; }
    [Export] public float CrouchSpeedPenalty { get; set; }
    [Export] public int Hits { get; set; }
    [Export] public float PersonalGrav { get; set; }
    [Export] public float MouseSens { get; set; }
    [Signal] public delegate void PullupInvEventHandler();

    public string CurrentItemHeld = "Nothing";
    public Dictionary<string, Node3D> Items = new Dictionary<string, Node3D>()
    {
        {"Nothing",null},
        {"Compass",null},
        {"Flashlight",null},
        {"Map", null}
    };

    public Dictionary<int, string> Number_To_Item = new()
    {
        {0, "Compass"},
        {1, "Flashlight"},
        {2, "Map"},
        {3, "Nothing"}
    };

    public Dictionary<string, int> Item_To_Number = new()
    {
        {"Compass", 0},
        {"Flashlight", 1},
        {"Map", 2},
        {"Nothing", 3}
    };

    public string ItemToChangeTo = "Nothing";
    public bool ChangingHeld = false;

    public AnimationPlayer animationPlayer;

    public Camera3D charCam;
    public Vector3 velocity = new();

    public bool isRunning = false;
    public bool isCrouching = false;
    public override void _Ready()
    {
        charCam = GetNode<Camera3D>("Pivot/MyEyes");
        Input.MouseMode = Input.MouseModeEnum.Captured;
        charCam.MakeCurrent();
        animationPlayer = GetNode<AnimationPlayer>("Pivot/Model/AnimationPlayer");
        foreach (HeldItem item in Items.Values)
        {
            if (item != null)
            {
                item.player = this;
            }
        }

    }

    public override void _PhysicsProcess(double delta)
    {
        HandleInput(delta);
        Control(delta);

        HandleChanging(delta);

    }

    //Handles input for switching inbetween items
    public void HandleInput(double delta)
    {
        int itemChangeNum = -1;

        if (Input.IsActionJustPressed("map"))
        {
            itemChangeNum = 0;
        }

        if (Input.IsActionJustPressed("flashlight"))
        {
            itemChangeNum = 1;
        }

        if (Input.IsActionJustPressed("compass"))
        {
            itemChangeNum = 2;
        }

        if (Input.IsActionJustPressed("put_away"))
        {
            itemChangeNum = 3;
        }

        if (Input.IsActionJustPressed("shift_item_positive"))
        {
            itemChangeNum += 1;
        }

        if (Input.IsActionJustPressed("shift_item_negative"))
        {
            itemChangeNum -= 1;
        }

        itemChangeNum = Math.Clamp(itemChangeNum, 0, Number_To_Item.Count - 1);

        if (ChangingHeld == false)
        {
            if (Number_To_Item[itemChangeNum] != CurrentItemHeld)
            {
                ItemToChangeTo = Number_To_Item[itemChangeNum];
                ChangingHeld = true;
            }
        }

        if (Input.IsActionJustPressed("toggle_flashlight"))
        {
            if (ChangingHeld == false && CurrentItemHeld == "Flashlight")
            {
                Flashlight_Item light = (Flashlight_Item)Items[CurrentItemHeld];
                if (light != null)
                {
                    PlayerAnimation_Manager manager = (PlayerAnimation_Manager)animationPlayer;
                    if (manager.CurrentState == light.IdleHeldAnim)
                    {
                        manager.SetTargetAnimation(light.ToggleLightAnim);
                    }
                }
            }
        }
    }


    public void Control(double delta)
    {

        float walkSpeedMod = BaseSpeed;

        if (!IsOnFloor()) velocity.Y -= PersonalGrav * (float)delta;
        GD.Print("Set VelY: " + velocity.Y);
        Vector2 movement = Input.GetVector("left", "right", "forward", "back");

        if (Input.IsActionPressed("sprint"))
        {
            isRunning = true;

            walkSpeedMod *= RunSpeedMod; //Subject to change
        }
        else
        {
            isRunning = false;
        }

        Vector3 movementDir = (Transform.Basis * new Vector3(movement.X, 0, movement.Y)).Normalized();

        if (!isRunning && Input.IsActionPressed("crouch"))
        {
            isCrouching = true;

            walkSpeedMod *= CrouchSpeedPenalty; //Subject to change

        }
        else
        {
            isCrouching = false;
        }

        if (IsOnFloor() && Input.IsActionJustPressed("jump"))
        {
            if (isRunning)
            {
                velocity.Y = JumpSpeed - (JumpSpeed / 4);
            }
            else if (!isRunning && isCrouching)
            {
                velocity.Y = JumpSpeed + (JumpSpeed / 4);
            }
            else
            {
                velocity.Y = JumpSpeed;
            }

            walkSpeedMod *= BonusSpeedFromJumping; //Also subject to change
        }

        velocity.X = movementDir.X * walkSpeedMod;
        velocity.Z = movementDir.Z * walkSpeedMod;


        this.Velocity = velocity;
        GD.Print("Direction X: " + movementDir.X);
        GD.Print("Direction Z: " + movementDir.Z);

        GD.Print("Move X: " + Velocity.X);
        GD.Print("Move Z: " + Velocity.Z);
        //GD.Print("Actual VelY: " + Velocity.Y);
        MoveAndSlide();
    }

    public void HandleChanging(double delta)
    {
        if (ChangingHeld == true)
        {
            bool unEquipped = false;
            //As I get deeper and deeper into this C# translation rabbit hole, the more worried I get. This could very possibly not work, and I'll spend a week trying to find an alternative.
            HeldItem held = (HeldItem)Items[CurrentItemHeld];
            //Check if held item exists (It should) and if it isn't (somehow) count it as "Unequipped"
            if (held == null) unEquipped = true;
            else
            {
                //otherwise
                if (held.ItemIsHeld) unEquipped = held.UnequipItem();
                else unEquipped = true;

            }

            if(unEquipped) 
            {
                bool nowEquipped = false;
                HeldItem toEquip = (HeldItem) Items[ItemToChangeTo]; //Probably have to change this to variant, so we can catch a null later that probably won't be caught because of cast shenanigans

                if(toEquip == null) nowEquipped = true; 
                else 
                {
                    //Placeholder for a cast from variant to the HeldItem script

                    if(!toEquip.ItemIsHeld) nowEquipped = toEquip.EquipItem(); //If item is not equipped, equip it
                    else nowEquipped = true; //If it IS equipped, then clearly it is equipped
                }
                
                //Check if item was equipped
                if(nowEquipped)
                {
                    //Reset changing bool
                    ChangingHeld = false;
                    //Set the current item to the item we were changing to
                    CurrentItemHeld = ItemToChangeTo;
                    //Reset the container for item to change to
                    ItemToChangeTo = "";
                }
            }
        }
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
        charCam.RotateX(-move.Y);
        charCam.Rotation = new Vector3(Mathf.Clamp(charCam.Rotation.X, -Mathf.DegToRad(70), Mathf.DegToRad(70)), 0, 0);

        GD.Print("CamXRot: " + charCam.Rotation.X);
        GD.Print("CamYRot: " + charCam.Rotation.Y);
        GD.Print("CamZRot: " + charCam.Rotation.Z);
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

}
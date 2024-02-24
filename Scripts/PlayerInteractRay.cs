using Godot;
using System;
using System.Runtime.CompilerServices;

//NOTE: I learned this way of doing interaction from the youtuber Nagi, the video link is provided below:
// https://youtu.be/An3uHrAoHRw?si=CzLhaeNTjGiajpCP 
public partial class PlayerInteractRay : RayCast3D 
{

    public CharacterBody3D properOwner;
    public Label WhatAmILookingAt;
    public override void _Ready()
    {
        
        properOwner = GetParent<Node3D>().GetParent<CharacterBody3D>();
        WhatAmILookingAt = properOwner.GetNode<Label>("Pivot/MyEyes/MyThoughts/CenterPrompt");
        this.AddException(properOwner);
    }

    public override void _PhysicsProcess(double delta)
    {
        if(this.IsColliding()){
            var collision = GetCollider();

            if(collision is InteractableObject obj){
                WhatAmILookingAt.Text = obj.GetHoverTip();

                if(Input.IsActionJustPressed("Interact") && obj.Interactable){
                    obj.Activated(properOwner);
                }
            } else {
                WhatAmILookingAt.Text = "";
            }
        } else {
            WhatAmILookingAt.Text = "";
        }
    }
}
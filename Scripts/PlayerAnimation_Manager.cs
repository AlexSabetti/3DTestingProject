using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

public partial class PlayerAnimation_Manager : AnimationPlayer
{
    public Dictionary<String, string[]> HoldingAnims = new Dictionary<string, string[]>() 
    {
        {"Nothing_Held_Idle", new string[] { "Nothing_Held_Idle", "Compass_Equip", "Flashlight_Equip", "Map_Equip" }},
        {"Compass_Equip", new string[] { "Compass_Idle" }},
        {"Compass_Idle", new string[] { "Compass_Unequip", "Compass_Idle" }},
        {"Compass_Unequip", new string[] { "Nothing_Held_Idle" }},
        {"Flashlight_Equip", new string[] { "Flashlight_Idle" }},
        {"Flashlight_Idle", new string[] { "Flashlight_Toggle", "Flashlight_Unequip", "Flashlight_Idle" }},
        {"Flashlight_Toggle", new string[] { "Flashlight_Idle" }},
        {"Flashlight_Unequip", new string[] { "Nothing_Held_Idle" }},
        {"Map_Equip", new string[] { "Map_Idle" }},
        {"Map_Idle", new string[] { "Map_Unequip", "Map_Idle" }},
        {"Map_Unequip", new string[] { "Nothing_Held_Idle" }}
    };
    public Dictionary<string, double> AnimSpeeds = new Dictionary<string, double>()
    {
        //Default / Empty-Handed animation time
        {"Nothing_Held_Idle", 1.0},
        //Compass animation time
        {"Compass_Equip", 0.7},
        {"Compass_Idle", 1.0},
        {"Compass_Unequip", 0.7},
        //Flashlight animation time
        {"Flashlight_Equip", 0.7},
        {"Flashlight_Idle", 1.0},
        {"Flashlight_Toggle", 0.2},
        {"Flashlight_Unequip", 0.7},
        //Map animation time
        {"Map_Equip", 0.7},
        {"Map_Idle", 1.0},
        {"Map_Unequip", 0.7}
    };
    [Export] public string CurrentState = null; //Object type subject to change, might make it a custom object later

    public override void _Ready()
    {
        SetTargetAnimation("Nothing_Held_Idle");
        AnimationFinished += (StringName) => AnimationEnded(StringName);
    }

    

    public bool SetTargetAnimation(string animName)
    {
        //If we are already in the animation being called
        if (animName.Equals(CurrentState))
        {
            //Print out a warning
            GD.Print("PlayerAnimation_Manager.cs -- WARNING: animation is already " + animName);
            return true;
        }

        //Checks if the target has the animation we want to play
        if (HasAnimation(animName))
        {
            //Ensures the animation is not null (The initialized animation)
            if (CurrentState != null)
            {
                //Get all animation paths we can choose to go down from the one we are at
                string[] animPaths = HoldingAnims[CurrentState];
                //Check if the animation we are calling exists in the possible paths
                if (animPaths.Contains(animName))
                {
                    CurrentState = animName;
                    //Activate animation for that name here
                    return true;
                }
                else
                {
                    //Otherwise
                    GD.Print("PlayerAnimation_Manager.cs -- Warning: I Cannot change to " + animName + " from " + CurrentState + " as " + CurrentState + " does not have a path to it.");
                    return false;
                }

            } else {
                //Otherwise
                CurrentState = animName;
                //Play animation for that name here
                return true;
            }
        }
        return false;
    }

    public void AnimationEnded(string animName){
        /* Godot's fps tutorial has a long line of else if's to figure out which animation to swap to.
         * I was going to just do as it shows, but I think I'll use "switch" instead. This might be even less efficient, though */
        switch (CurrentState) {
            //Idle animation
            case "Nothing_Held_Idle":
                break;
            //Compass Animations    
            case "Compass_Equip":
                SetTargetAnimation("Compass_Idle");
                break;
            case "Compass_Idle":
                break;
            case "Compass_Unequip":
                SetTargetAnimation("Nothing_Held_Idle");
                break;
            //Flashlight animations
            case "Flashlight_Equip":
                SetTargetAnimation("Flashlight_Idle");
                break;
            case "Flashlight_Idle":
                break;
            case "Flashlight_Toggle":
                SetTargetAnimation("Flashlight_Idle");
                break;
            case "Flashlight_Unequip":
                SetTargetAnimation("Nothing_Held_Idle");
                break;
            //Map Animations
            case "Map_Equip":
                SetTargetAnimation("Map_Idle");
                break;
            case "Map_Idle":
                break;
            case "Map_Unequip":
                SetTargetAnimation("Nothing_Held_Idle");
                break;
            default:
                GD.Print("PlayerAnimation_Manager.cs -- WARNING: The current animation '" + CurrentState +"' has ended, but I do not have a case that recognizes it.");
                SetTargetAnimation("Nothing_Held_Idle");
                break;
        }
    }

}
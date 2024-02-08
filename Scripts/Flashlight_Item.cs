using Godot;
using System;

public partial class Flashlight_Item : HeldItem 
{
    public bool ToggleOn = false;
    //Animations
    public string ToggleLightAnim = "Flashlight_Toggle";

    public override void _Ready()
    {
        IdleHeldAnim = "Flashlight_Idle";
    }

    public override bool EquipItem(){
        PlayerAnimation_Manager manager = (PlayerAnimation_Manager) player.animationPlayer;
        if(manager.CurrentState == IdleHeldAnim){
            ItemIsHeld = true;
            return true;
        }

        if(manager.CurrentState == "Nothing_Held_Idle"){
            manager.SetTargetAnimation("Flashlight_Equip");
        }

        return false;
    }

    public override bool UnequipItem()
    {
        PlayerAnimation_Manager manager = (PlayerAnimation_Manager) player.animationPlayer;
        if(manager.CurrentState == IdleHeldAnim){
            manager.SetTargetAnimation("Flashlight_Unequip");
        }

        if(manager.CurrentState == "Nothing_Held_Idle"){
            ItemIsHeld = false;
            return true;
        }

        return false;
    }
}
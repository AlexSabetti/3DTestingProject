using Godot;
using System;

public partial class Compass_Item : HeldItem 
{
    public bool LyingToPlayer = false;
    

    public override void _Ready()
    {
        IdleHeldAnim = "Compass_Idle";
    }

    public override bool EquipItem(){
        PlayerAnimation_Manager manager = (PlayerAnimation_Manager) player.animationPlayer;
        if(manager.CurrentState == IdleHeldAnim){
            ItemIsHeld = true;
            return true;
        }

        if(manager.CurrentState == "Nothing_Held_Idle"){
            manager.SetTargetAnimation("Compass_Equip");
        }

        return false;
    }

    public override bool UnequipItem()
    {
        PlayerAnimation_Manager manager = (PlayerAnimation_Manager) player.animationPlayer;
        if(manager.CurrentState == IdleHeldAnim){
            manager.SetTargetAnimation("Compass_Unequip");
        }

        if(manager.CurrentState == "Nothing_Held_Idle"){
            ItemIsHeld = false;
            return true;
        }

        return false;
    }
}
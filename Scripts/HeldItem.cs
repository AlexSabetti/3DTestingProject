using Godot;
using System;

public partial class HeldItem : Node3D {
    [Export] public string Item_Name {get; set;}
    public string IdleHeldAnim;
    public bool ItemIsHeld = false;
    public PlayerInput player;

    public virtual bool EquipItem() {
        return false; //Must be overwritten
    }

    public virtual bool UnequipItem() {
        return false; //Must be overwritten
    }
}
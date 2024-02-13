using Godot;
using System;

/**
 * A base class that is to be extended from by every object we can interact with
 */
public partial class InteractableObject : StaticBody3D 
{
    [Export] public string ObjectName {get; set;}

    public bool interactable;

    public virtual bool MouseOn() 
    {
        return false; //Must be overwritten
    } 

    public virtual bool MouseOff()
    {
        return true; //Must be overwritten
    }
}
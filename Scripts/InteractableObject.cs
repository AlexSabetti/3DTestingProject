using Godot;
using System;

/**
 * A base class that is to be extended from by every object we can interact with
 */
public partial class InteractableObject : StaticBody3D 
{
    /** Name of the object*/
    [Export] public string ObjectName {get; set;}
    /** */
    [Export] public string PromptAction {get; set;}
    [Signal] public delegate void CanInteractEventHandler(Node3D node);
    [Signal] public delegate void CannotInteractEventHandler(Node3D node);

    [Export] public bool Interactable {get; set;} = true;
    [Export] public string InteractPrompt;

    public virtual bool Activated(CharacterBody3D interactor)
    {
        return false;
    }

    public virtual string GetHoverTip()
    {
        return string.Empty;
    }
}
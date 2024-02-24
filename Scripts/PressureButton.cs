using Godot;
using System;

public partial class PressureButton : InteractableObject 
{
    [Export] public bool ButtonOn {get; set;}
    [Signal] public delegate void ButtonWasPressedEventHandler(CharacterBody3D interactor);
    //[Export] public NodePath NodeThisButtonEffects {get; set;}
    [Export] public bool LocksAfterUse {get; set;}

    [Export] public bool Locked {get; set;}
    //public ButtonReceiver receiver;

    public override void _Ready()
    {
        if(ObjectName == null) ObjectName = "Button";
    }

    /** This method gets called when we receive a signal from PlayerInteractRay.
        This method will not be called if this object's current interact state is set to false*/
    public override bool Activated(CharacterBody3D interactor)
    {
        if(Locked) 
        {
            //Play a sound, but do nothing
            return false;
        }

        //We want to play an animation of the button being pushed in (Does not have to include the player, just the button going in)

        //Probably play a sound, too

        //Emit a signal to the button receiver
        EmitSignal(SignalName.ButtonWasPressed, interactor);
        ButtonOn = !ButtonOn; Interactable = false;
        return true;
    }
    /** This method is called when the button receiver is done with its part*/
    public void Finished()
    {
        if(LocksAfterUse) 
        {
            //Button stays in pressed in animation
            Locked = true;
            //But we reset it to interactable again
            Interactable = true;
        } 
        else 
        {
            //Play reverse of button press animation

            //Toggle button state
            ButtonOn = !ButtonOn;
            //Set to interactable
            Interactable = true;

        }
    }

    public override string GetHoverTip()
    {
        string keyName = "";
        foreach(InputEvent inputEvent in InputMap.ActionGetEvents(PromptAction)){
            if(inputEvent is InputEventKey key) {
                keyName = OS.GetKeycodeString(key.Keycode);
            }
        }
        string toReturn = ObjectName + "\n[" + keyName + "]";
       
        if(Locked) toReturn = "Locked " + toReturn;
        else if(!Interactable) toReturn = "In Use";

        return toReturn;
    }

    /** This method is called when we want a manual unlock, and it takes a bool that decides whether or not we remove the lockafteruse tag or not
        This method will be called when a signal is recieved to do it*/
    public void Unlock(bool permanentUnlock)
    {
        Locked = false;
        if(permanentUnlock) LocksAfterUse = false;
        GD.Print(this + " has been unlocked");
    }



}
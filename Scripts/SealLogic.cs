using Godot;
using System;

public partial class SealLogic : InteractableObject 
{
    [Signal] public delegate void FinishedEventHandler();
    public CollisionShape3D SealDoorCollider;
    public bool Opened;
    private Tween tween;

    public override void _Ready()
    {
        SealDoorCollider = GetNode<CollisionShape3D>("SealDoorCol");

        
    }

    public override bool Activated(CharacterBody3D interactor)
    {
        if(Interactable)
        {
            tween = CreateTween();
            tween.TweenProperty(this, "position", new Vector3(Position.X, Position.Y + 10, Position.Z), 3);
            tween.TweenCallback(Callable.From(this.FinishSeal));
            GD.Print(this.Name + " has started unsealing");
            Interactable = false;
            tween.Play();
            return true;
        }
        else return false;
    }

    private void FinishSeal(){
        tween.Stop();
        GD.Print(this.Name + " has finished unsealing.");
        EmitSignal(SignalName.Finished);
    }

    public override string GetHoverTip()
    {
        string keyName = "";
        foreach(InputEvent inputEvent in InputMap.ActionGetEvents(PromptAction)){
            if(inputEvent is InputEventKey key) {
                keyName = OS.GetKeycodeString(key.Keycode);
            }
        }
        return ObjectName + "\n[" + keyName + "]";
    }


    //The following appears to be not necessary, I overcomplicated this.

    /*public void OnAreaEntered(Node3D node)
    {
        //If what entered is one of our entities
        if(node is CharacterBody3D entity){
            //Check if it's the player
            if(entity.Name == "Player"){ //This might cause issues later, perhaps we make it mandatory on start to give a username, name the player node it on startup, and then save it to a file so accidently renaming the ingame node doesn't screw up the entire thing
                //If it is, we allow the sealdoor collidor to start broadcasting mouseevents
                SealDoorCollider.SetBlockSignals(false);
            }
        }
    } 

    public void OnAreaExited(Node3D node){
        //if what exited was an entity
        if(node is CharacterBody3D entity){
            if(entity.Name == "Player"){
                SealDoorCollider.SetBlockSignals(true);
                Interactable = false;

                //Emit turn off signal here as well, otherwise there could be an exploit
            }
        }
    }
    //Connects to Area3D's Collision3D Mouse entered event
    public override bool MouseOn(){
        //Program shouldn't reach here if we aren't in the area, that way we aren't constantly recieving signals that we will have to deny.
          
        //If the mouse is on the object, we want to let the player know that they can interact with it
            //Probably emit signal here?
        EmitSignal(SignalName.CanInteract);
        return true;
    }

    public override bool MouseOff()
    {
        Interactable = false;
        EmitSignal(SignalName.CannotInteract);
        return true;
    }*/
}
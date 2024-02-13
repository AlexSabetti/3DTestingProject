using Godot;
using System;

public partial class SealLogic : InteractableObject 
{
    [Signal] public delegate void WithinRangeEventHandler();
    public Area3D InteractableArea;
    public CollisionShape3D SealDoorCollider;
    public bool Opened;

    public override void _Ready()
    {
        interactable = false;
        InteractableArea = GetNode<Area3D>("InteractArea");
        SealDoorCollider = GetNode<CollisionShape3D>("SealDoorTex");

        InteractableArea.AreaEntered += OnAreaEntered;
        InteractableArea.MouseEntered += () => MouseOn();
        InteractableArea.MouseExited += () => 
    }
    
    public void OnAreaEntered(Node3D node)
    {
        //If what entered is one of our entities
        if(node is CharacterBody3D entity){
            //Check if it's the player
            if(entity.Name == "Player"){ //This might cause issues later, perhaps we make it mandatory on start to give a username, name the player node it on startup, and then save it to a file so accidently renaming the ingame node doesn't screw up the entire thing
                //If it is, we allow the sealdoor collidor to start broadcasting mouseevents
                SealDoorCollider.SetBlockSignals(false);
                interactable = true; //This bool may be redundant, might remove later
            }
        }
    } 

    public void OnAreaExited(Node3D node){
        //if what exited was an entity
        if(node is CharacterBody3D entity){
            if(entity.Name == "Player"){
                SealDoorCollider.SetBlockSignals(true);
                interactable = false;
            }
        }
    }
    //Connects to Area3D's Collision3D Mouse entered event
    public override bool MouseOn(){
        //Program shouldn't reach here if we aren't in the area, that way we aren't constantly recieving signals that we will have to deny.
          
          //If the mouse is on the object, we want to let the player know that they can interact with it
            //Probably emit signal here?
        
        

    }

    public override bool MouseOff()
    {
        
    }
}
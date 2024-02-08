using Godot;
using System;

public partial class StartButtonScript : StaticBody3D {
    [Export] public string TakesYouTo = "";
    public override void _Ready()
    {
        this.InputEvent += MouseEvents;
        //InputEvent += (Camera3D camera, InputEvent @event, Vector3 pos, Vector3 norm, int shapeIdx) => OnInputEvent;
    }

    private void MouseEvents(Node camera, InputEvent @event, Vector3 position, Vector3 normal, long shapeIdx)
    {
        if(@event is InputEventMouseButton input){
            if(input.ButtonIndex == MouseButton.Left){
                //We want to load the new scene
            }
        }
    }


}
using Godot;
using System;
using System.IO;

public partial class MainMenuScript : Node3D
{
    Variant MainMenu;
    [Export] public string StartPath;
    [Export] public StaticBody3D StartButton;
    [Export] public StaticBody3D SettingsButton;
    [Export] public StaticBody3D QuitButton;
    [Export] public Control SettingsMenu;

    public bool StartHoverToggle = false;
    public bool SettingsHoverToggle = false;
    public bool QuitHoverToggle = false;
    public bool SettingsOpen = false;


    public override void _Ready()
    {
        StartButton = GetNode<StaticBody3D>("InWorldNormalMenu/Start");
        SettingsButton = GetNode<StaticBody3D>("InWorldNormalMenu/Settings");
        QuitButton = GetNode<StaticBody3D>("InWorldNormalMenu/Quit");

        StartButton.InputEvent += InteractedStartButton;
        SettingsButton.InputEvent += InteractedSettingsButton;
        QuitButton.InputEvent += InteractedQuitButton;

        SettingsMenu = GetNode<Control>("Camera3D/SettingsHUD"); 
        SettingsMenu.Visible = false;
        //GetTree().ChangeSceneToFile(StartPath);
        GD.Print("Ready : Finished");
    }

    public void InteractedStartButton(Node camera, InputEvent @event, Vector3 position, Vector3 normal, long shapeIdx)
    {
        //GD.Print("Checkpoint");
        if (@event is InputEventMouseButton inputMouse)
        {
            if (inputMouse.ButtonIndex == MouseButton.Left && inputMouse.IsPressed())
            {
                GD.Print("Begin pressed");
            }
        }
    }

    public void InteractedSettingsButton(Node camera, InputEvent @event, Vector3 position, Vector3 normal, long shapeIdx)
    {
        if (@event is InputEventMouseButton inputMouse)
        {
            if(inputMouse.ButtonIndex == MouseButton.Left && inputMouse.IsPressed()){
                GD.Print("Settings Pressed");
            }
        }
    }

    public void InteractedQuitButton(Node camera, InputEvent @event, Vector3 position, Vector3 normal, long shapeIdx)
    {
        if (@event is InputEventMouseButton inputMouse)
        {
            if(inputMouse.ButtonIndex == MouseButton.Left && inputMouse.IsPressed()){
                GD.Print("Quit Pressed");
            }
        }
    }

    public void StartButtonHovered()
    {
        StartHoverToggle = !StartHoverToggle;
        if(StartHoverToggle){
            //StartButton.GetNode<MeshInstance3D>("Text").
        }
    }

    public void SettingsButtonHovered(){

    }

    public void QuitButtonHovered(){

    }

    private void ActivateSettings(){
        SettingsMenu.Visible = true;
        SettingsOpen = true;

    }

    private void DeactivateSettings() {
        SettingsMenu.Visible = false;
        SettingsOpen = false;
    }
}
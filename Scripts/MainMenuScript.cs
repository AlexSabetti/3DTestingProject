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
    [Signal] public delegate void SettingsToggleOnEventHandler();
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
        if(SettingsOpen) return;
        if (@event is InputEventMouseButton inputMouse)
        {
            if (inputMouse.ButtonIndex == MouseButton.Left && inputMouse.IsPressed())
            {
                GD.Print("Settings Pressed");
                ActivateSettings();
            }
        }
    }

    public void InteractedQuitButton(Node camera, InputEvent @event, Vector3 position, Vector3 normal, long shapeIdx)
    {
        //May or may not be needed, will find a way to condense later. Basically just makes it ignore signals from the scene when the settings menu is up.
        if(SettingsOpen) return;
        if (@event is InputEventMouseButton inputMouse)
        {
            if (inputMouse.ButtonIndex == MouseButton.Left && inputMouse.IsPressed())
            {
                GD.Print("Quit Pressed");
            }
        }
    }

    public void StartButtonHovered()
    {
        StartHoverToggle = !StartHoverToggle;
        if (StartHoverToggle)
        {
            //StartButton.GetNode<MeshInstance3D>("Text").
        }
    }

    public void SettingsButtonHovered()
    {

    }

    public void QuitButtonHovered()
    {

    }

    private void ActivateSettings()
    {
        
        /* This would be the other way, if we block out the other signals we don't have to put in a check to disregard them
         * Just need to make sure we unblock them later*/
        StartButton.SetBlockSignals(true); QuitButton.SetBlockSignals(true); SettingsButton.SetBlockSignals(true);
        //EmitSignal(SignalName.SettingsToggleOn);
        SettingsMenu.Visible = true;
        SettingsOpen = true;
    }

    /**
     * Should be connected to SettingsMenu (Specifically) toggleoff event. 
     * Re-hides the settings, toggles the bool, and unblocks all of the menu button signals
     *
     */
    private void DeactivateSettings()
    {
        //SettingsMenu.Visible = false;
        SettingsOpen = false;
        StartButton.SetBlockSignals(false); QuitButton.SetBlockSignals(false); SettingsButton.SetBlockSignals(false);
        GD.Print("Settings Closed");
    }

    public override void _UnhandledKeyInput(InputEvent @event)
    {
        if (@event is InputEventKey inputKey)
        {
            if (inputKey.Keycode == Key.Escape && inputKey.IsPressed())
            {
                if(SettingsOpen) DeactivateSettings();




            }
        }
    }
}
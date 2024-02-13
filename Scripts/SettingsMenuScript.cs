using Godot;
using System;

public partial class SettingsMenuScript : MarginContainer 
{
    [Signal] public delegate void SettingsToggleOffEventHandler();
    private VBoxContainer settingsContent;
    private HBoxContainer FPSSettings;
    private VBoxContainer SoundSettings;
    private VBoxContainer BrightnessSettings;
    private Button CloseOutButton;
    



    public override void _Ready()
    {
        settingsContent = GetNode<VBoxContainer>("VSep");
        CloseOutButton = GetNode<Button>("VSep/Header/HeaderBackground/Exit");
        FPSSettings = GetNode<HBoxContainer>("VSep/Body/HSep/LeftSide/FPSBox");
        BrightnessSettings = GetNode<VBoxContainer>("VSep/Body/HSep/LeftSide/BrightnessBox");
        SoundSettings = GetNode<VBoxContainer>("VSep/Body/HSep/LeftSide/SoundBox");



        CloseOutButton.Pressed += () => CloseOutSettings();
    }

    /**
     * Will be pre-connected to the close button (Top right) 
     * Emits the toggleoff signal. Should shut off all GUI buttons before finishing
     * so the menu it came from doesn't have to worry about it.
     */
    public void CloseOutSettings(){
        this.Hide();
    }

    public void UpdateFPS(float frames){
        GD.Print("New Max Framerate: " + frames);
        ProjectSettings.SetSetting("application/run/max_fps", (int) Math.Clamp(frames, 30, 144));
        GD.Print(ProjectSettings.GetSetting("application/run/max_fps"));
    }
    public void UpdateVolume(float percent){
        SoundSettings.GetNode<Label>("SoundHeader/SoundPercent").Text = percent + "%";
        //Have it send the new value directly to be saved so the sounds system can correctly adjust to it
    }

    public void UpdateBrightness(float percent) {
        GD.Print(BrightnessSettings);
        BrightnessSettings.GetNode<Label>("BrightnessHeader/BrightnessPercent").Text = percent + "%";
        //Have it saved just like the sound setting
    }

    /*
     * While this may seem a bit weird, this event handler is what turns all of this on and off
     */
    private void OnVisibilityChanged()
    {
        GD.Print(Visible);
        settingsContent.SetProcess(Visible);
        if(!Visible) EmitSignal(SignalName.SettingsToggleOff);
        
    }

    public override void _UnhandledKeyInput(InputEvent @event)
    {
        if(Visible) {
            if(@event is InputEventKey inputKey) {
                if(inputKey.Keycode == Key.Escape && inputKey.IsPressed()) {
                    CloseOutSettings();               
                }
            }
        }
    }





}
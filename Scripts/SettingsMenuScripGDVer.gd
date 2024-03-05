class_name SettingsMenuScriptGDVer
extends Control

@onready var settings_content: VBoxContainer = get_node("FullBox/VSep")
@onready var fps_settings: HBoxContainer = get_node("FullBox/VSep/Body/HSep/LeftSide/FPSBox")
@onready var sound_settings: VBoxContainer = get_node("FullBox/Vsep/Body/HSep/LeftSide/SoundBox")
@onready var brightness_settings: VBoxContainer = get_node("FullBox/VSep/Body/HSep/LeftSide/BrightnessBox")
@onready var close_out_button: Button = get_node("FullBox/Vsep/Header/HeaderBackground/Exit")
@onready var confirm_deny_changes: MarginContainer = get_node("ConfirmChangeBox")
@onready var focus_box: ColorRect = get_node("BlurOutFocus")
@onready var input_mapping_box: VBoxContainer = get_node("InputMappingBox")
var need_to_confirm_changes: bool = false
signal settings_toggle_off()

var changes = {}

var input_lookup = {
	"bright":"confirm_brightness",
	"volume":"confirm_volume",
	"fps":"confirm_fps"#,
	#"keyboard": confirm_keyboard_controls(),
	#"controller": confirm_controller_controls()
}

func _ready():
	close_out_button.pressed.connect(try_to_close_out_settings())

	var confirm_change_button: Button = confirm_deny_changes.get_node("Border/ActualContainer/Confirm")
	confirm_change_button.pressed.connect(confirm_changes())

	var discard_change_button: Button = confirm_deny_changes.get_node("Border/ActualContainer/Discard")
	discard_change_button.pressed.connect(discard_changes())


func try_to_close_out_settings():
	if need_to_confirm_changes:
		bring_up_confirm_change()
	else:
		hide()

func bring_up_confirm_change():
	confirm_deny_changes.visible = true

func confirm_changes():
	if not changes.is_empty():
		for key in changes:
			call(input_lookup[key])
	need_to_confirm_changes = false
	changes = {}
	close_out_settings()

func discard_changes():
	changes = {}
	fps_settings.get_node("FPSSpinInput").range.value = ProjectSettings.get_setting("application.application/run/max_fps")
	#Set the UI's setting to what the original saved value is for both brightness and volume
	need_to_confirm_changes = false
	close_out_settings()

func close_out_settings():
	settings_content.visible = false
	settings_toggle_off.emit()

func on_visibility_changed():
	settings_content.set_process(visible)
	if not visible:
		settings_toggle_off.emit()


func update_fps(frames: int):
	changes["fps"] = frames
	need_to_confirm_changes = true

func update_brightness(percent: float):
	changes["bright"] = percent
	need_to_confirm_changes = true
	brightness_settings.get_node("BrightnessHeader/BrightnessPercent").text = "{}%".format(percent)

func update_volume(percent: float):
	changes["volume"] = percent
	need_to_confirm_changes = true
	sound_settings.get_node("SoundHeader/SoundPercent").text = "{}%".format(percent)	

func confirm_brightness():
	#if we got to this point, we know we have brightness changes in the dictionary
	#placeholder
	print()

func confirm_volume():
	#if we got to this point, we know we have volume changes in the dictionary, no need to check for it
	#placeholder
	print()

func confirm_fps():
	#if we got here, dictionary must contain fps changes
	ProjectSettings.set_setting("application/run/max_fps", clampi(changes["fps"], 30, 144))


func _on_settings_input_event(camera, event, position, normal, shape_idx):
	pass # Replace with function body.

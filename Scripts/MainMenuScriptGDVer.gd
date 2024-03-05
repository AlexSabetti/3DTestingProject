class_name MainMenuScriptGDVer
extends Node3D

@export var start_path: String
@export var start_button: StaticBody3D
@export var settings_button: StaticBody3D
@export var quit_button: StaticBody3D
@export var settings_menu: Control
signal settings_toggle_on()

var start_hover_toggle: bool = false
var settings_hover_toggle: bool = false
var quit_hover_toggle: bool = false
var settings_open: bool = false

func _ready():
	start_button = get_node("InWorldNormalMenu/Start")
	settings_button = get_node("InWorldNormalMenu/Settings")
	quit_button = get_node("InWorldNormalMenu/Quit")
	settings_menu = get_node("Camera3D/SettingsHUD")
	start_button.input_event.connect(interacted_start_button)
	settings_menu = get_node("Camera3D/SettingsHUD")
	settings_menu.visible = false

@warning_ignore("unused_parameter")
func interacted_start_button(camera:Node, event: InputEvent, posvec:Vector3, vecnorm:Vector3, shapeidx:int):
	if event is InputEventMouseButton:
		var event_mouse: InputEventMouseButton = event
		if event_mouse.button_index == MOUSE_BUTTON_LEFT && event_mouse.is_pressed():
			print() #tba

@warning_ignore("unused_parameter")
func interacted_settings_button(camera:Node, event: InputEvent, posvec:Vector3, vecnorm:Vector3, shapeidx:int):
	if settings_open:
			return
	if event is InputEventMouseButton:
		var event_mouse: InputEventMouseButton = event
		if event_mouse.button_index == MOUSE_BUTTON_LEFT && event_mouse.is_pressed():
			activate_settings()

@warning_ignore("unused_parameter")
func interacted_quit_button(camera:Node, event: InputEvent, posvec:Vector3, vecnorm:Vector3, shapeidx:int):
	if settings_open:
		return
	if event is InputEventMouseButton:
		var event_mouse: InputEventMouseButton = event
		if event_mouse.button_index == MOUSE_BUTTON_LEFT && event_mouse.is_pressed():
			print() #tba

func activate_settings():
	start_button.set_block_signals(true)
	quit_button.set_block_signals(true)
	settings_button.set_block_signals(true)

	settings_menu.visible = true
	settings_open = true

func deactivate_settings():
	settings_open = false
	start_button.set_block_signals(false)
	settings_button.set_block_signals(false)
	quit_button.set_block_signals(false)

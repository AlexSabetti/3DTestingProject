class_name InputRemapButton
extends Button

@export var input_action: String
@onready var input_settings_popup: VBoxContainer = get_node("./SettingsHUD/InputMappingBox")
@onready var focus_box: ColorRect = get_node("./SettingsHUD/BlurOutFocus")
signal input_assign_call(button:Button, event, input_action)
func _init():
	toggle_mode = true

func _ready():
	set_process_unhandled_input(false)
	update_input_text()

func _toggled(button_pressed):
	set_process_unhandled_input(button_pressed)
	if button_pressed:
		input_settings_popup.visible = true
		focus_box.visible = true
		release_focus()
	else:
		input_settings_popup.visible = false
		focus_box.visible = false
		grab_focus()
		

func update_input_text():
	text = "%s" % InputMap.action_get_events(input_action)[0].as_text()

func _unhandled_input(event):
	if event.is_pressed():
		input_assign_call.emit(self, event, input_action)
		button_pressed = false


#Might need to put this aside for now
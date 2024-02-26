class_name PressureButtonGDVer
extends InteractableObjectGDVer

@export var button_on: bool = false
signal button_was_pressed(interactor: CharacterBody3D)
@export var locks_after_use: bool = false
@export var locked: bool = false

func _ready():
	if object_name == "": 
		object_name = "button"

func activated(interactor: CharacterBody3D) -> bool:
	if locked:
		#play a sound, but otherwise do nothing. Maybe a click?

		return false
	#Play an animation of the button being pushed in

	#play a sound as well

	#emit signal to receiver
	button_was_pressed.emit(interactor)
	button_on = !button_on
	can_interact = false
	return true

func finished():
	if locks_after_use:
		#makes it so the locked sound plays when attempted to be opened
		locked = true
		#Makes it so the interact ray the player uses can still recognize this as an "interactive" object
		can_interact = true
	else: 
		#play reverse of initial button animation
		button_on = !button_on
		can_interact = true

func get_hover_tip() -> String:
	var key_name = ""
	for inputevent: InputEvent in InputMap.action_get_events(prompt_action):
			var specific_key: InputEventKey
			specific_key = inputevent as InputEventKey

			key_name = OS.get_keycode_string(specific_key.keycode)
	var to_return = object_name + "\n[" + key_name + "]"
	if locked:
		to_return = "Locked" + to_return
	elif  not can_interact:
		to_return = "In Use"
	return to_return

func unlock(perm_unlock: bool):
	locked = false
	if perm_unlock: 
		locks_after_use = false


class_name DoorLogic
extends InteractableObjectGDVer


var door_anim_player: AnimationPlayer
var has_key: bool = false
@export var in_opened_mode: bool = false
@export var key_requirement: int = 1

func _ready():
	door_anim_player = get_node("DoorAnim")
	door_anim_player.connect("animation_finished", finished_movement)
	if in_opened_mode:
		change_to_proper_mode()

func activated(interactor: CharacterBody3D):
	if in_opened_mode:
		if interactor is PlayerControlsGDVer:
			var player: PlayerControlsGDVer = interactor
			player.return_keys(key_requirement)
			can_interact = false
			in_opened_mode = false
			door_anim_player.play_backwards("ToggleDoor")
	else: 	
		if interactor is PlayerControlsGDVer:
			var player: PlayerControlsGDVer = interactor
			if player.has_enough_keys(key_requirement):
				player.take_keys(key_requirement)
				can_interact = false
				door_anim_player.play("ToggleDoor")
				in_opened_mode = true

func change_to_proper_mode():
	door_anim_player.play("ToggleDoor")
	can_interact = false

func finished_movement(anim_name):
	print("Animation: " + anim_name + " finished.")
	can_interact = true
	#play a "chunk" like sound here

func get_hover_tip() -> String:
	var key_name = ""
	for inputevent: InputEvent in InputMap.action_get_events(prompt_action):
			var specific_key: InputEventKey
			specific_key = inputevent as InputEventKey

			key_name = OS.get_keycode_string(specific_key.keycode)
	var to_return = object_name + "\n[" + key_name + "]"
	if  not can_interact:
		to_return = "In Use"
	return to_return
	

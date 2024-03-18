class_name FeatureLock
extends InteractableObjectGDVer

var lock_anim_player: AnimationPlayer
@export var unlocked: bool = false

func _ready():
	lock_anim_player = get_node("LockAnim")
	lock_anim_player.connect("animation_finished", finished_movement)
	if unlocked:
		change_to_proper_mode()

func activated(interactor: CharacterBody3D):
	if unlocked:
		if interactor is PlayerControlsGDVer:
			var player: PlayerControlsGDVer = interactor
			player.return_keys(1)
			can_interact = false
			unlocked = false
			lock_anim_player.play_backwards("Unlocking")
	else:
		if interactor is PlayerControlsGDVer:
			var player: PlayerControlsGDVer = interactor
			if player.has_enough_keys(1):
				player.take_keys(1)
				can_interact = false
				lock_anim_player.play("Unlocking")
				unlocked = true

func change_to_proper_mode():
	lock_anim_player.play("Unlocking")
	can_interact = false

func finished_movement(anim_name):
	print("Animation: " + anim_name + " finished.")
	can_interact = true

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
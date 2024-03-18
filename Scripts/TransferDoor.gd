class_name TransferDoor
extends InteractableObjectGDVer

var door_anim_player: AnimationPlayer
var current_target: Node3D

var in_motion_sequence = false
var is_opened: bool = false
var eyes_are_active = false
var eyes_can_be_active = false

var left_eye_tween: Tween
var right_eye_tween: Tween

var left_eye: CharacterBody3D
var right_eye: CharacterBody3D
var local_watch: Area3D

var inhabitants: Array = []
@export var depends_on_these_feature_locks: Array = []

func _ready():
	door_anim_player = get_node("DoorAnim")
	local_watch = get_node("WatchfulZone")
	door_anim_player.connect("animation_finished", finished_movement)
	local_watch.connect("body_entered", entered)
	local_watch.connect("body_exited", exited)
	left_eye = get_node("CyclopsBlocks/Transfer_Room_Door_Eyes3/CharacterBody3D")
	right_eye = get_node("CyclopsBlocks/Transfer_Room_Door_Eyes2/CharacterBody3D")

@warning_ignore("unused_parameter")
func activated(interactor: CharacterBody3D):
	if is_opened:
		can_interact = false
		is_opened = false
		in_motion_sequence = true
		door_anim_player.play_backwards("Door_Unlock")
	else:
		if check_dependencies():
			can_interact = false
			in_motion_sequence = true
			door_anim_player.play("Door_Unlock")
			is_opened = true

func check_dependencies() -> bool:
	for lock in depends_on_these_feature_locks:
		var feature_lock: FeatureLock = lock
		if feature_lock.unlocked == false:
			print("Locked at {locker}.".format({"locker": feature_lock.name}))
			return false
		print("{locker} passed.".format({"locker": feature_lock.name}))
	return true

func finished_movement(anim_name):
	print("Animation: " + anim_name + " finished.")
	can_interact = true
	in_motion_sequence = false

func get_hover_tip() -> String:
	var key_name = ""
	for inputevent: InputEvent in InputMap.action_get_events(prompt_action):
			var specific_key: InputEventKey
			specific_key = inputevent as InputEventKey

			key_name = OS.get_keycode_string(specific_key.keycode)
	var to_return = object_name + "\n[" + key_name + "]"
	if  not can_interact:
		to_return = "The Eyes relieve you of their gaze; the gate opens."
	return to_return

@warning_ignore("unused_parameter")
func _physics_process(delta):
	if eyes_can_be_active:
		if current_target != null:
			if in_motion_sequence:
				return
			else:
				look_at_cur_target()
		

func look_at_cur_target():
	var tar_pos = current_target.global_transform.origin
	if left_eye_tween == null and right_eye_tween == null:
		print("Creating initial eye tweens: Left and Right")
		left_eye_tween = create_tween()
		right_eye_tween = create_tween()
	elif left_eye_tween == null:
		left_eye_tween = create_tween()
		print("Creating initial eye tween: Left")
	else:
		right_eye_tween = create_tween()
		print("Creating initial eye tween: Right")
	var des_rot_left = left_eye.global_transform.looking_at(tar_pos, Vector3.UP).basis.get_euler()
	var des_rot_right = right_eye.global_transform.looking_at(tar_pos, Vector3.UP).basis.get_euler() 

	left_eye_tween.stop()
	right_eye_tween.stop()
	left_eye_tween.tween_property(left_eye, "set_rotation", des_rot_left, 0.5)
	right_eye_tween.tween_property(right_eye, "set_rotation", des_rot_right, 0.5)

func entered(body_that_entered):
	eyes_can_be_active = true
	inhabitants.append(body_that_entered)
	current_target = inhabitants[0]

func exited(body_that_left):
	inhabitants.erase(body_that_left)
	if inhabitants.size() <= 0:
		eyes_can_be_active = false
		current_target = null
	else:
		current_target = inhabitants[0]



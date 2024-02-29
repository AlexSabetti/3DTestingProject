class_name  SealLogicGDVer
extends InteractableObjectGDVer
signal finished_movement()
@export_group("Timing")
@export var movement_time: float = 3
@export_group("Move length")
@export var to_move_x: float = 0
@export var to_move_y: float = 10
@export var to_move_z: float = 0

var seal_door_col: CollisionShape3D
var movement_tween: Tween

func _ready():
	seal_door_col = get_node("SealDoorCol")

@warning_ignore("unused_parameter")
func activated(interactor: CharacterBody3D):
	if can_interact:
		movement_tween = create_tween()
		movement_tween.tween_property(self, "position", Vector3(position.x + to_move_x, position.y + to_move_y, position.z + to_move_z), 3)
		
		movement_tween.tween_callback(Callable(self, "finish_seal"))
		can_interact = false
		movement_tween.play()
		return true

func finish_seal():
	movement_tween.stop()
	finished_movement.emit()

func get_hover_tip():
	var key_name = ""
	for this_input: InputEvent in InputMap.action_get_events(prompt_action):
		var specific_key: InputEventKey
		specific_key = this_input as InputEventKey

		key_name = OS.get_keycode_string(specific_key.keycode)
	var to_return = object_name + "\n[" + key_name + "]"
	if not can_interact:
		to_return = "In Use"
	return to_return


class_name CompassItemGDVer
extends HeldItemGDVer

var lying_to_player: bool = false
@onready var needle_object: StaticBody3D = get_node("CompassBody/Needle")
@export_group("Pointing Direction")
@export var simulated_north: Vector3 = Vector3(0,0,1)
@export_group("Needle Settings")
@export var turning_speed: float
@export_group("Mischief Chance")
@export_range(0,100) var chance_to_invert: int = 10
var is_active: bool = false #this is slightly different than is_item_held, because the latter is used for switching logic, but this will be used for the physics process

func _ready():
	idle_held_anim = "Compass_Idle"
	clamp(simulated_north.x, -1, 1)
	#the y coord must be 0 no matter what
	simulated_north.y = 0
	clamp(simulated_north.z, -1, 1)
	clamp(turning_speed, 0,5)
	connect(visibility_changed.get_name(), toggle_compass())

func equip_item():
	var manager: PlayerAnimation_ManagerGDVer
	manager = player.player_animation
	if manager.current_state == idle_held_anim:
		#make sure that this item recognizes that it is the one equipped
		item_is_held = true
		#return true to let the caller know that it has been equipped
		return true
	
	if manager.current_state == "Nothing_Held_Idle":
		manager.set_target_animation("Compass_Equip")
	#return false to let the caller know that it wasn't equipped prior
	return false

func unequip_item():
	var manager: PlayerAnimation_ManagerGDVer
	manager = player.player_animation
	if manager.current_state == idle_held_anim:
		manager.set_target_animation("Compass_Unequip")
	
	if manager.current_state == "Nothing_Held_Idle":
		item_is_held = false
		return true
	
	return false

@warning_ignore("unused_parameter") #Going to have to change this later? Maybe? Want to use delta to make it animate properly, but Tween might tax the system
func _physics_process(delta):
	if is_active:
		var cur_rot = needle_object.transform.basis.get_rotation_quaternion()
		var target_rot
		if lying_to_player:
			target_rot = Quaternion(-1 * simulated_north, PI / 2)

		else:
			target_rot = Quaternion(simulated_north, PI / 2)
		
		var smooth_rot = cur_rot.slerp(target_rot, turning_speed)
		needle_object.rotation = smooth_rot

func toggle_compass():
	if item_is_held:
		prepare_compass()
	else:
		remove_compass()

func prepare_compass():
	print("Compass has been prepared")
	if randi_range(0, 100) <= chance_to_invert:
		lying_to_player = true
		needle_object.look_at(-1 * simulated_north)
	else:
		lying_to_player = false
		needle_object.look_at(simulated_north)
	is_active = true

func remove_compass():
	print("Compass has been removed")
	is_active = false

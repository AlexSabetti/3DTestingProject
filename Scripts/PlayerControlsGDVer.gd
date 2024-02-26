
class_name PlayerControlsGDVer

extends CharacterBody3D
@export_group("Speed Properties")
@export var base_speed: float = 6
@export var max_speed: float = 8
@export var run_speed_mod: float = 1.5
@export var bonus_speed_from_jump: float = 1.1
@export var crouch_speed_penalty: float = 0.3
@export var crouch_max_air_speed: float = 4
@export_group("Mouse Properties")
@export var mouse_sens: float = 0.003
@export_group("Air Movement Properties")
@export var personal_grav: float = 18.8
@export var jump_speed: float = 6
@export var air_control_penalty: float = 0.03
@export_group("")

@onready var pivot = get_node("Pivot")
@onready var char_cam: Camera3D = get_node("Pivot/MyEyes")

var vel = Vector3()

var cur_item_held = 'Nothing'
var item_held_num = 3 #3 stands for "Nothing
var items = {'Nothing': Node3D, 'Compass': Node3D, 'Flashlight': Node3D, 'Map': Node3D}
var number_to_item = {0: 'Compass', 1: 'Flashlight', 2: 'Map', 3: 'Nothing'}
var item_to_number = {'Compass': 0, 'Flashlight': 1, "Map": 2, 'Nothing': 3}
var item_to_change_to := 'Nothing'
var changing_held: bool = false

@onready var player_animation: AnimationPlayer = get_node("Pivot/Model/AnimationPlayer")

var currently_running: bool = false
var currently_crouching: bool = false

func _ready():
	Input.mouse_mode = Input.MOUSE_MODE_CAPTURED
	char_cam.make_current()
	air_control_penalty = clampf(air_control_penalty, -2, 2)

func control(delta) -> void:
	var walk_speed_mod = base_speed

	if not is_on_floor():
		#This simulates gravity
		vel.y -= personal_grav * delta
		#This sets up our movement
		var movement = Input.get_vector("left", "right", "forward", "back")
		var movement_dir = transform.basis * Vector3(movement.x, 0, movement.y).normalized()
		
		if Input.is_action_pressed("sprint"):
			walk_speed_mod *= run_speed_mod
			currently_running = true
			currently_crouching = false
			
			#This makes it so you have less control on where you are moving after you jump
			walk_speed_mod *= air_control_penalty
			#this is some magic I had to cook up to get momentum working.
			vel.x += movement_dir.x * walk_speed_mod
			vel.z += movement_dir.z * walk_speed_mod

			if movement_dir.x == 0:
				vel.x = clampf(vel.x, -1 * max_speed, max_speed)
			else:
				vel.x = clampf(vel.x, absf(movement_dir.x) * - 1 * max_speed, absf(movement_dir.x) * max_speed)
			if movement_dir.z == 0:
				vel.z = clampf(vel.z, -1 * max_speed, max_speed)
			else:
				vel.z = clampf(vel.z, absf(movement_dir.z) * - 1 * max_speed, absf(movement_dir.z) * max_speed)

			vel.y = clampf(vel.y, -14.0, 1000)
			velocity = vel
		elif Input.is_action_pressed("crouch"):
			walk_speed_mod *= crouch_speed_penalty
			currently_crouching = true
			
			#This makes it so you have less control on where you are moving after you jump
			walk_speed_mod *= air_control_penalty
			#this is some magic I had to cook up to get momentum working.
			vel.x += movement_dir.x * walk_speed_mod
			vel.z += movement_dir.z * walk_speed_mod

			if movement_dir.x == 0:
				vel.x = clampf(vel.x, -1 * crouch_max_air_speed, crouch_max_air_speed)
			else:
				vel.x = clampf(vel.x, absf(movement_dir.x) * - 1 * crouch_max_air_speed, absf(movement_dir.x) * crouch_max_air_speed)
			if movement_dir.z == 0:
				vel.z = clampf(vel.z, -1 * crouch_max_air_speed, crouch_max_air_speed)
			else:
				vel.z = clampf(vel.z, absf(movement_dir.z) * - 1 * crouch_max_air_speed, absf(movement_dir.z) * crouch_max_air_speed)

			vel.y = clampf(vel.y, -14.0, 1000)
			velocity = vel
		else:
			currently_crouching = false
			currently_running = false
			walk_speed_mod *= air_control_penalty

			vel.x += movement_dir.x * walk_speed_mod
			vel.z += movement_dir.z * walk_speed_mod

			if movement_dir.x == 0:
				vel.x = clampf(vel.x, -1 * base_speed, base_speed)
			else:
				vel.x = clampf(vel.x, absf(movement_dir.x) * - 1 * base_speed, absf(movement_dir.x) * base_speed)
			if movement_dir.z == 0:
				vel.z = clampf(vel.z, -1 * base_speed, base_speed)
			else:
				vel.z = clampf(vel.z, absf(movement_dir.z) * - 1 * base_speed, absf(movement_dir.z) * base_speed)

			vel.y = clampf(vel.y, -14.0, 1000)
			velocity = vel
	else:
		var movement = Input.get_vector("left", "right", "forward", "back")
		var movement_dir = transform.basis * Vector3(movement.x, 0, movement.y).normalized()
		
		if Input.is_action_pressed("sprint"):
			currently_running = true
			walk_speed_mod *= run_speed_mod
		else:
			currently_running = false
		
		if is_on_floor() and Input.is_action_just_pressed("jump"):
			if currently_running == true:
				vel.y = jump_speed - (jump_speed / 5)
			else:
				vel.y = jump_speed

			walk_speed_mod *= bonus_speed_from_jump

		vel.x = movement_dir.x * walk_speed_mod
		vel.z = movement_dir.z * walk_speed_mod
		
		velocity = vel
	move_and_slide()

@warning_ignore("unused_parameter")
func handle_input(delta: float):
	#originally, item_held_num was a local var that was initially set to -1
	#I don't know *why* I did this. It's stupid. handle_input is going to be called every physics process call,
	#which means that with this set to -1 every function call, it will be clamped to 0, and return to holding the map,
	# *unless* you were to tap another key every physics call. Yeesh.
	# var item_held_num = -1

	#Key inputs for items 
	if Input.is_action_just_pressed("map"):
		item_held_num = 0
	if Input.is_action_just_pressed("flashlight"):
		item_held_num = 1
	if Input.is_action_just_pressed("compass"):
		item_held_num = 2
	if Input.is_action_just_pressed("put_away"):
		item_held_num = 3
	
	#Scroll inputs for items
	if Input.is_action_just_pressed("shift_item_positive"):
		item_held_num += 1
	if Input.is_action_just_pressed("shift_item_negative"):
		item_held_num -= 1
	
	#Clamp the result so we don't scroll off into non-existence
	item_held_num = clamp(item_held_num, 0, number_to_item.size() - 1)

	if not changing_held:
		if number_to_item.get(item_held_num) != cur_item_held:
			item_to_change_to = number_to_item.get(item_held_num)
	
	#if Input.is_action_just_pressed("interact"):
@warning_ignore("unused_parameter")
func handle_changing(delta: float):
	var unequipped = false
	#There is actually an easier way to do this, when I was researching how to do all this, there was a guide for this using GDscript.
	#However, I needed to make it work in C#, so this is the GDScript version of the C# translation of the gdscript method. Yippee
	var held: HeldItemGDVer = items.get(cur_item_held) as HeldItemGDVer
	#if the item we are holding doesn't exist
	if held == null:
		#Then we must have "unequipped" it
		unequipped = true
	else:
		#However, if the item exists, and it recognizes it is held
		if held.item_is_held:
			#unequip it
			unequipped = held.unequip_item()
		else:
			#If it isn't being held, then it must be unequipped
			unequipped = true
	#if 
	if unequipped:
		var now_equipped = false
		var to_equip: HeldItemGDVer = items.get(item_to_change_to)

		#if the item we want doesn't exist
		if to_equip == null:
			#Then we can't equip it
			now_equipped = false
		else:
			#if item is not equipped, equip it you idiot
			if not to_equip.item_is_held:
				now_equipped = to_equip.equip_item()
			else:
				#if it is equipped, then why is it not set to equipped
				now_equipped = true
		
		#check if item was equipped
		if now_equipped:
			#we don't need to change anymore
			changing_held = false
			#set the current item to the item we were changing to
			cur_item_held = item_to_change_to
			#reset the placeholder for item to change to
			item_to_change_to = ""

	


func _unhandled_input(event):
	if event is InputEventMouseMotion and Input.mouse_mode == Input.MOUSE_MODE_CAPTURED:
		var move_pass = event as InputEventMouseMotion
		camera_rotate(move_pass.relative * mouse_sens)
		
func camera_rotate(move: Vector2):
	rotate_y( - move.x)
	pivot.rotate_x( - move.y)
	pivot.rotation = Vector3(clampf(pivot.rotation.x, -deg_to_rad(70), deg_to_rad(70)), 0, 0)

func _unhandled_key_input(event):
	if event.is_action_pressed("ui_cancel"):
		get_tree().quit()

func _physics_process(delta):
	control(delta)

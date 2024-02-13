extends CharacterBody3D

@onready var NavAgent = $NavigationAgent3D
@onready var raycasts = $Raycasts
const SPEED = 2
const accel = 2
var random = Vector3()
var count = 0
var direction = Vector3()
var chase_mode = false
var collide = null

#get gravity
var gravity = ProjectSettings.get_setting("physics/3d/default_gravity")

func _ready():
	random = get_random_point()
	print(random)
	for child in raycasts.get_children():
		child.enabled = false
	
func _process(delta):
	var found_player = false
	for child in raycasts.get_children():
		if child.is_colliding():
			get_player(child)
			found_player = true
			break
	if not found_player:
		chase_mode = false
		collide = null


func _physics_process(delta):
	if chase_mode == false: #get a new point to move
		if global_position.distance_to(random) < 1.5:
			random = get_random_point()
		
		NavAgent.target_position = random
	else:
		NavAgent.target_position = collide.global_position
	
	direction = NavAgent.get_next_path_position() - global_position
	direction = direction.normalized()
	velocity = velocity.lerp(direction*SPEED,accel * delta)
		
	if not is_on_floor():
		#adding gravity 
		velocity.y -= gravity * delta
	move_and_slide()

func get_random_point():
	#get random point on the map
	return Vector3(randf()*10,0.83,randf()*10)

func get_player(child):
	collide = child.get_collider()
	if collide.is_in_group("player"):
		chase_mode = true
		print("Chasing")


func _on_enable_ray_cast_body_entered(body):
	if body.is_in_group("player"):
		for child in raycasts.get_children():
			child.enabled = true #Enable the raycast for the ai


func _on_enable_ray_cast_body_exited(body):
	if body.is_in_group("player"):
		for child in raycasts.get_children():
			child.enabled = false #Disable the raycast for the ai

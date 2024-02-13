extends CharacterBody3D

@onready var NavAgent = $NavigationAgent3D
@onready var raycast = $RayCast3D
@onready var timer = $Timer
const SPEED = 20
const accel = 2
var random = Vector3()
var count = 0
var direction = Vector3()
var player = null



# Get the gravity from the project settings to be synced with RigidBody nodes.
var gravity = ProjectSettings.get_setting("physics/3d/default_gravity")

func _ready():
	player =(get_node("../Player"))
	random = get_random_point()
	print(random)


func _physics_process(delta):
	
	
	NavAgent.target_position = random
	
	direction = NavAgent.get_next_path_position() - global_position
	direction = direction.normalized()
	velocity = velocity.lerp(direction*SPEED,accel * delta)
	
	if not is_on_floor():
		velocity.y -= gravity * delta
	move_and_slide()


func get_random_point():
	#get random point on the map
	return player.global_position + Vector3(randf()*10,0.83,randf()*10)


func _on_timer_timeout():
	timer.wait_time = randi_range(5,15)
	print(timer.wait_time)
	#Every time the timer runs out it restarts with a random value and checks if it needs to move closer to the player
	if global_position.distance_to(player.global_position) >10:
		random = get_random_point() 

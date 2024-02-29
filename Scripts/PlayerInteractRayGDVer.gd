class_name PlayerInteractRayGDVer
extends RayCast3D

#NOTE: I learned this way of doing interaction from the youtuber Nagi, the video link is provided below:
	#https://youtu.be/An3uHrAoHRw?si=CzLhaeNTjGiajpCP
	#Thanks for the tutorial!
var proper_owner: CharacterBody3D
var what_am_i_looking_at: Label

func _ready():
	proper_owner = get_parent().get_parent()
	#print(proper_owner)
	what_am_i_looking_at = proper_owner.get_node("Pivot/MyEyes/MyThoughts/CenterPrompt")
	#print(what_am_i_looking_at)
	add_exception(proper_owner)

@warning_ignore("unused_parameter")
func _physics_process(delta):
	if is_colliding():
		
		var collision = get_collider()
		if collision is InteractableObjectGDVer:
			var obj: InteractableObjectGDVer = collision
			what_am_i_looking_at.text = obj.get_hover_tip()

			#print(what_am_i_looking_at.text)
			#print(obj.get_hover_tip())

			if Input.is_action_just_pressed("Interact") and obj.can_interact:
				obj.activated(proper_owner)
				print("activated")
		else: 
			what_am_i_looking_at.text = ""
	else: 
		what_am_i_looking_at.text = ""

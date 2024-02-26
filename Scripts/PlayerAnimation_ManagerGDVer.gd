class_name PlayerAnimation_ManagerGDVer

extends AnimationPlayer

var holding_anims: Dictionary = { 
	'Nothing_Held_Idle' : ["Nothing_Held_Idle", "Compass_Equip", "Flashlight_Equip", "Map_Equip"], 
	'Compass_Equip' : ["Compass_Idle"], "Compass_Idle" : ["Compass_Uneqip", "Compass_Idle"],
	'Compass_Unequip' : ["Nothing_Held_Idle"],
	'Flashlight_Equip' : ["Flashlight_Idle"],
	'Flashlight_Idle' : ["Flashlight_Toggle", "Flashlight_Unequip", "Flashlight_Idle"],
    'Flashlight_Toggle' :  ["Flashlight_Idle"],
    'Flashlight_Unequip' :  ["Nothing_Held_Idle"],
    'Map_Equip' : ["Map_Idle"],
    'Map_Idle' : ["Map_Unequip", "Map_Idle"],
    'Map_Unequip' : ["Nothing_Held_Idle"]
}

var anim_speeds: Dictionary = {
	#Default / Empty-Handed animation time
        'Nothing_Held_Idle' : 1.0,
        #Compass animation time
        'Compass_Equip' : 0.7,
        'Compass_Idle' : 1.0,
        'Compass_Unequip' : 0.7,
		#Flashlight animation time
        'Flashlight_Equip' : 0.7,
        'Flashlight_Idle': 1.0,
        'Flashlight_Toggle' : 0.2,
        'Flashlight_Unequip' : 0.7,
        #Map animation time
        'Map_Equip' : 0.7,
        'Map_Idle' : 1.0,
        'Map_Unequip' : 0.7
}

@export var current_state: String = ""

func _ready():
	set_target_animation("Nothing_Held_Idle")
	animation_finished.connect(animation_ended(current_state))
	
func set_target_animation(anim_name: String):
	#If we are already in the animation that is being called
	if anim_name == current_state:
		print("PlayerAnimation_ManagerGDVer.gd -- WARNING: animation is already set to " + anim_name)
		return true
	if has_animation(anim_name):
		#checks if the current state we are in is valid
		if current_state != "":
			#get all of the animation paths we can choose to go down from the one where we are at
			var anim_paths = holding_anims.get(current_state)
			#check if the animation we are calling exists as a possible path
			if anim_paths.has(anim_name):
				current_state = anim_name

				#Activate the animation for that name

				return true
			else:
				print("PlayerAnimation_ManagerGDVer.gd -- WARNING: I cannot change to {0} from  {1} as {1} does not have a path to it.".format([anim_name, current_state]))
				return false
		else:
			current_state = anim_name
			#play animation by that name

			return true
	return false
@warning_ignore("unused_parameter")
func animation_ended(anim_name: String):
	if current_state == "Nothing_Held_Idle":
		print("Nothing_Held_Idle animation has finished.") #subject to change
	#Compass animations
	elif current_state == "Compass_Equip":
		set_target_animation("Compass_Idle")
	elif current_state == "Compass_Idle":
		print("Compass_Idle animation has finished.")
	elif current_state == "Compass_Unequip":
		set_target_animation("Nothing_Held_Idle")
	#Flashlight animations
	elif current_state == "Flashlight_Equip":
		set_target_animation("Flashlight_Idle")
	elif current_state == "Flashlight_Idle":
		print("Flashlight_Idle animation has finished.")
	elif current_state == "Flashlight_Toggle":
		set_target_animation("Flashlight_Idle")
	elif current_state == "Flashlight_Unequip":
		set_target_animation("Nothing_Held_Idle")
	#Map animations
	elif current_state == "Map_Equip":
		set_target_animation("Map_Idle")
	elif current_state == "Map_Idle":
		print("Map_Idle animation has finished.")
	elif current_state == "Map_Unequip":
		set_target_animation("Nothing_Held_Idle")
	else:
		print("PlayerAnimation_ManagerGDVer.gd -- WARNING: The current animation '" + current_state +  "' has ended, but I do not have a case that recognizes it.")
		set_target_animation("Nothing_Held_Idle")
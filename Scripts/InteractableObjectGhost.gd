class_name InteractableObjectGhost
extends Area3D

@export var object_name: String
@export var prompt_action: String
@export var can_interact: bool = true
@export var interact_prompt: String

@warning_ignore("unused_parameter")
func activated(interactor: CharacterBody3D) -> bool:
	return false
	

func get_hover_tip() -> String:
	return "";
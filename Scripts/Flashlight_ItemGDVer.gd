class_name Flashlight_ItemGDVer
extends HeldItemGDVer

var toggle_on:bool = false
var toggle_light_anim:String = "Flashlight_Toggle"
var manager: PlayerAnimation_ManagerGDVer
var light_source: SpotLight3D

func _ready():
	player = get_parent().get_parent().get_parent().get_parent()
	light_source = get_node("Light")
	idle_held_anim = "Flashlight_Idle"
	manager = player.player_animation

func equip_item()->bool:
	
	if manager.current_state == idle_held_anim:
		item_is_held = true
		return true
	
	if manager.current_state == "Nothing_Held_Idle":
		manager.set_target_animation("Flashlight_Equip")
	return false

func unequip_item()->bool:
	if manager.current_state == idle_held_anim:
		manager.set_target_animation("Flashlight_Unequip")
	if manager.current_state == "Nothing_Held_Idle":
		item_is_held = false
		return true
	return false

func activate():
	if manager.current_state == idle_held_anim:
		manager.set_target_animation(toggle_light_anim)
		toggle_on = !toggle_on
		if toggle_on:
			light_source.visible = true
		else:
			light_source.visible = false



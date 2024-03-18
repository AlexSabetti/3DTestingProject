class_name HeldItemGDVer
extends Node3D
@export var item_name: String
var idle_held_anim: String
var item_is_held: bool = false
var player: PlayerControlsGDVer
signal on_unequip(this_item: HeldItemGDVer)

func equip_item()->bool:
	return false

func unequip_item()->bool:
	return false
func activate():
	pass

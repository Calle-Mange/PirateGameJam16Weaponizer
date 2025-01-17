extends CharacterBody2D

const SPEED = 300.0

@onready var animated_sprite_2d: AnimatedSprite2D = $AnimatedSprite2D

func _physics_process(delta: float) -> void:

	var input_direction = Input.get_vector("move_left", "move_right", "move_up", "move_down")
	velocity = input_direction * SPEED
	
	# Get input dir
	print(input_direction)
	
	if input_direction.x == 0 && input_direction.y == 0:
		animated_sprite_2d.play("idle")
	elif input_direction.x != 0 || input_direction.y != 0:
		animated_sprite_2d.play("move_vertical")

	move_and_slide()

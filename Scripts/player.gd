extends CharacterBody2D

const SPEED = 100.0
const JUMP_VELOCITY = -400.0

@onready var animated_sprite_2d: AnimatedSprite2D = $AnimatedSprite2D

func _physics_process(delta: float) -> void:

	if not is_on_floor():
		velocity += get_gravity() * delta

	var input_direction = Input.get_vector("move_left", "move_right", "move_up", "move_down")
	
	# Get input dir
	print(animated_sprite_2d.rotation)
	
	# idle
	if input_direction.x == 0 && input_direction.y == 0:
		animated_sprite_2d.play("idle")
		animated_sprite_2d.rotation_degrees = 0
		velocity.x = move_toward(velocity.x, 0, SPEED)
		
		
	# move right
	elif input_direction.x > 0 && input_direction.y == 0:
		animated_sprite_2d.play("move_straight")
		animated_sprite_2d.rotation_degrees = 90
		animated_sprite_2d
		
	# move left
	elif input_direction.x < 0 && input_direction.y == 0:
		animated_sprite_2d.play("move_straight")
		animated_sprite_2d.rotation_degrees = -90

	# move up
	elif input_direction.x == 0 && input_direction.y < 0:
		animated_sprite_2d.play("move_straight")
		animated_sprite_2d.rotation_degrees = 0
		
	# move down
	elif input_direction.x == 0 && input_direction.y > 0:
		animated_sprite_2d.play("move_straight")
		animated_sprite_2d.rotation_degrees = 180
		
	# move north west
	elif input_direction.x < 0 && input_direction.y < 0:
		print("nw")
		animated_sprite_2d.flip_h = false
		animated_sprite_2d.flip_v = false
		animated_sprite_2d.play("move_diagonal")
		animated_sprite_2d.rotation_degrees = 0
	# move north east
	elif input_direction.x > 0  && input_direction.y < 0:
		print("ne")
		animated_sprite_2d.flip_h = true
		animated_sprite_2d.flip_v = false
		animated_sprite_2d.play("move_diagonal")
		animated_sprite_2d.rotation_degrees = 0
	# move south west
	elif input_direction.x < 0 && input_direction.y > 0:
		print("sw")
		animated_sprite_2d.flip_h = false
		animated_sprite_2d.flip_v = true
		animated_sprite_2d.play("move_diagonal")
		animated_sprite_2d.rotation_degrees = 0
	# move south east
	elif input_direction.x > 0 && input_direction.y > 0:
		print("se")
		animated_sprite_2d.flip_h = false
		animated_sprite_2d.flip_v = false
		animated_sprite_2d.play("move_diagonal")
		animated_sprite_2d.rotation_degrees = 0

	move_and_slide()

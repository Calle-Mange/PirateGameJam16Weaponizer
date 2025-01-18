extends CharacterBody2D

const SPEED = 100.0
const rotation_speed = 3
var _theta : float

@onready var animated_sprite_2d: AnimatedSprite2D = $AnimatedSprite2D

func _physics_process(delta: float) -> void:

	var input_direction = Input.get_vector("move_left", "move_right", "move_up", "move_down")
	var rotation_direction = Input.get_axis("move_left", "move_right")

	#rotation += clamp(rotation_speed * delta, 0, abs(_theta)) * sign(_theta)  
	_theta = wrapf(atan2(input_direction.y, input_direction.x) - rotation, -PI, PI)
	
	velocity = input_direction * SPEED
	
	# Get input dir
	print(animated_sprite_2d.rotation)
	
	# idle
	if input_direction.x == 0 && input_direction.y == 0:
		animated_sprite_2d.play("idle")
		animated_sprite_2d.rotation_degrees = 0
		
		
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

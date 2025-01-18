extends CharacterBody2D

const SPEED = 100.0
const rotation_speed = 8

@onready var animated_sprite_2d: AnimatedSprite2D = $AnimatedSprite2D

func _physics_process(delta: float) -> void:

	var input_direction = Input.get_vector("move_left", "move_right", "move_up", "move_down")
	var rotation_direction_x = Input.get_axis("move_left", "move_right")
	var rotation_direction_y = Input.get_axis("move_up", "move_down")
	
	velocity = input_direction * SPEED
	
	# Get input dir
	print(rotation)
	print(snapped(rotation, 1.5))
	
	# idle
	if input_direction.x == 0 && input_direction.y == 0:
		animated_sprite_2d.play("idle")
		
		# behöver nog komma åt animationen utanför fysiklopen?
		if rotation != 0:
			rotation += rotation_direction_x * rotation_speed * delta
	# move right
	elif input_direction.x > 0 && input_direction.y == 0:
		animated_sprite_2d.play("move_straight")
		
		if rotation < 1.5:
			rotation += rotation_direction_x * rotation_speed * delta
	# move left
	elif input_direction.x < 0 && input_direction.y == 0:
		animated_sprite_2d.play("move_straight")
		
		if rotation > -1.5:
			rotation += rotation_direction_x * rotation_speed * delta
	# move up
	elif input_direction.x == 0 && input_direction.y < 0:
		animated_sprite_2d.play("move_straight")
		
		
		if rotation < 1: 
			rotation += rotation_direction_y * rotation_speed * delta
	# move down
	elif input_direction.x == 0 && input_direction.y > 0:
		animated_sprite_2d.play("move_straight")
		
		#if rotation < 0:
			#rotation += rotation_direction_y * rotation_speed * delta
	# move north west
	elif input_direction.x < 0 && input_direction.y < 0:
		print("nw")
		animated_sprite_2d.flip_h = false
		animated_sprite_2d.flip_v = false
		animated_sprite_2d.play("move_diagonal")
	# move north east
	elif input_direction.x > 0  && input_direction.y < 0:
		print("ne")
		animated_sprite_2d.flip_h = true
		animated_sprite_2d.flip_v = false
		animated_sprite_2d.play("move_diagonal")
	# move south west
	elif input_direction.x < 0 && input_direction.y > 0:
		print("sw")
		animated_sprite_2d.flip_h = false
		animated_sprite_2d.flip_v = true
		animated_sprite_2d.play("move_diagonal")
	# move south east
	elif input_direction.x > 0 && input_direction.y > 0:
		print("se")
		animated_sprite_2d.flip_h = false
		animated_sprite_2d.flip_v = false
		animated_sprite_2d.play("move_diagonal")

	move_and_slide()

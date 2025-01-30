extends CanvasLayer
@onready
var continueButton = $MarginContainer/PanelContainer/MarginContainer/Node2D/ContinueButton
@onready
var exitButton = $MarginContainer/PanelContainer/MarginContainer/Node2D/ExitButton
@onready var level_manager = $"/root/LevelManager"

var currentSelector = 0
var selectColor = Color(1, 0.953, 0);
var NoSelectColor = Color(0.875, 0.875, 0.875);
var audioLock: bool = true

func _ready() -> void:
	set_current_selector(currentSelector)

func _on_continue_button_pressed() -> void:
	print("continue pressed")
	level_manager.StartFirstLevel()


func _on_exit_button_pressed() -> void:
	print("exit pressed")
	level_manager.ChangeSceneToFile("res://Scenes/Menu/main_menu.tscn")

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(_delta: float) -> void:	
	if (Input.is_action_just_pressed("ui_down") && currentSelector != 1):
		print("down")
		currentSelector += 1
		$SelectAudio.play()
		set_current_selector(currentSelector)
	if (Input.is_action_just_pressed("ui_up") && currentSelector != 0):
		print("up")
		currentSelector -= 1
		$SelectAudio.play()
		set_current_selector(currentSelector)
	if (Input.is_action_just_pressed("ui_accept")):
		$PickAudio.play()
		await get_tree().create_timer(2).timeout 
		print("ok")
		select_current_option(currentSelector)

func set_current_selector(_currentSelector) -> void:
	#continueButton.add_theme_color_override("font_color", NoSelectColor)
	#exitButton.add_theme_color_override("font_color", NoSelectColor)
	if _currentSelector == 0:
		continueButton.add_theme_color_override("font_color", selectColor)
		exitButton.add_theme_color_override("font_color", NoSelectColor)
	elif _currentSelector == 1:
		exitButton.add_theme_color_override("font_color", selectColor)
		continueButton.add_theme_color_override("font_color", NoSelectColor)
		
func select_current_option(_currentSelector) -> void:
	if _currentSelector == 0:
		level_manager.StartFirstLevel()
		queue_free()
		
	elif _currentSelector == 1:
		level_manager.ChangeSceneToFile("res://Scenes/Menu/main_menu.tscn")
		queue_free()

func _on_continue_button_mouse_entered() -> void:
	continueButton.add_theme_color_override("font_color", selectColor)
	exitButton.add_theme_color_override("font_color", NoSelectColor)

func _on_exit_button_mouse_entered() -> void:
	continueButton.add_theme_color_override("font_color", NoSelectColor)
	exitButton.add_theme_color_override("font_color", selectColor)

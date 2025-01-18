extends MarginContainer
@onready
var startSelector = $CenterContainer/VBoxContainer/CenterContainer2/VBoxContainer/CenterContainer/HBoxContainer/StartSelector
@onready
var exitSelector = $CenterContainer/VBoxContainer/CenterContainer2/VBoxContainer/CenterContainer2/HBoxContainer/ExitSelector
var currentSelector = 0
# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	set_current_selector(currentSelector)

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:	
	if (Input.is_action_just_pressed("ui_down") && currentSelector != 1):
		currentSelector += 1
		set_current_selector(currentSelector)
	if (Input.is_action_just_pressed("ui_up") && currentSelector != 0):
		currentSelector -= 1
		set_current_selector(currentSelector)
	if (Input.is_action_just_pressed("ui_accept")):
		select_current_option(currentSelector)

func set_current_selector(_currentSelector) -> void:
	startSelector.text = ""
	exitSelector.text = ""
	
	if _currentSelector == 0:
		startSelector.text = ">"
	elif _currentSelector == 1:
		exitSelector.text = ">"

func select_current_option(_currentSelector) -> void:
	if _currentSelector == 0:
		print("Start!") #Change scene here
	elif _currentSelector == 1:
		get_tree().quit()
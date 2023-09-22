extends Panel

@export_group("UI Elements")
@export_subgroup("Buttons")
@export var logout_button: Button
@export var save_button: Button

@export_subgroup("Controllers")
@export var audio_slider: HSlider
@export var sfx_slider: HSlider
@export_group("")

@onready var api_url = Env.get_value("API_URL")
@onready var auth: Auth = preload("res://scripts/classes/auth.gd").new()

var jwt_token: String = ""

signal logout_clicked

func _ready():
	

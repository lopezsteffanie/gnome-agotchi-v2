extends Panel

@export_group("UI Elements")
@export_subgroup("Buttons")
@export var logout_button: Button
@export var save_button: Button

@export_group("")
@export var logout_request: HTTPRequest

@onready var api_url = Env.get_value("API_URL")
@onready var auth: Auth = preload("res://scripts/classes/auth.gd").new()
@onready var config: Config = preload("res://scripts/classes/config.gd").new()
@onready var api: Api = preload("res://scripts/classes/api.gd").new()

var jwt_token: String = ""
var audio_bus_index: int
var sfx_bus_index: int

signal logout_completed
signal save_clicked

func _ready():
	audio_bus_index = AudioServer.get_bus_index("music")
	sfx_bus_index = AudioServer.get_bus_index("sfx")
	logout_button.pressed.connect(_on_logout_button_pressed)
	save_button.pressed.connect(_on_save_button_pressed)
	logout_request.request_completed.connect(_on_logout_request_completed)
	jwt_token = config.load_token_from_file()
	
func _on_logout_button_pressed():
	var url: String = api_url + "/auth/logout"
	auth.logout(url, jwt_token, logout_request)

func _on_save_button_pressed():
	save_clicked.emit()
	
func _on_logout_request_completed(result, response_code, headers, body):
	var response: Dictionary = api.get_response(body)
	if api.is_response_valid(response_code):
		print(response)
		logout_completed.emit()
	else:
		printerr(response.error)

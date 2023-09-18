extends Control

@onready var api_url = Env.get_value("API_URL")
@onready var auth = preload("res://scripts/classes/auth.gd").new()
@onready var settings_panel = $SettingsPanel
@onready var settings_btn = $SettingsButton
@onready var logout_req = $LogoutRequest

var jwt_token = ""

func _ready():
	settings_panel.visible = false
	
	# Load JWT token from file
	load_token_from_file()
	
func _on_settings_button_pressed():
	settings_panel.visible = true
	settings_btn.disabled = true

func _on_logout_button_pressed():
	var url = api_url + "/api/auth/logout"
	settings_panel.visible = false
	auth.logout(url, jwt_token, logout_req)

func _on_close_settings_button_pressed():
	settings_btn.disabled = false
	settings_panel.visible = false
	
func _on_logout_request_request_completed(result, response_code, headers, body):
	var response = JSON.parse_string(body.get_string_from_utf8())
	if (response_code == 200):
		print(response)
		get_tree().change_scene_to_file("res://scenes/login.tscn")
	else:
		printerr(response)
		
func load_token_from_file():
	var config = ConfigFile.new()
	
	# Load the configuration file
	if config.load("user://token.cfg") == OK:
		jwt_token = config.get_value("auth", "token")
	else:
		print("Failed to load config file.")

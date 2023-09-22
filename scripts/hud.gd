extends Control

@export_group("UI Elements")
@onready var settings_panel = $SettingsPanel
@onready var settings_btn = $SettingsButton
@onready var logout_req = $LogoutRequest
@onready var stats_panel = $CharacterStatsPanel
@onready var stats_btn = $CharacterStatsButton
@onready var close_stats_btn = $CharacterStatsPanel/ClosePanelButton

@onready var api_url = Env.get_value("API_URL")
@onready var auth = preload("res://scripts/classes/auth.gd").new()

var jwt_token = ""
var gnome_data = ""

func _ready():
	settings_panel.visible = false
	stats_panel.visible = false
	# Load JWT token from file
	load_token_from_file()
	
func _on_settings_button_pressed():
	settings_panel.visible = true
	settings_btn.disabled = true
	
func _on_character_stats_button_pressed():
	stats_btn.disabled = true
	stats_panel.visible = true

func _on_logout_button_pressed():
	var url = api_url + "/api/auth/logout"
	settings_panel.visible = false
	auth.logout(url, jwt_token, logout_req)

func _on_close_settings_button_pressed():
	settings_btn.disabled = false
	settings_panel.visible = false
	
func _on_close_panel_button_pressed():
	stats_btn.disabled = false
	stats_panel.visible = false

	
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

func _on_save_button_pressed():
	save_gnome_data_to_file(gnome_data);
	
func save_gnome_data_to_file(gnome_data):
	var config = ConfigFile.new()
	
#	Update with Save Gnome Data
#	config.set_value("gnome", "color", col)
#	config.set_value("gnome", "personality", pers)
	config.save("user://gnome.cfg")

func _on_audio_slider_value_changed(value):
	update_audio_volume()

# Update with actual function
func update_audio_volume():
	pass

func _on_sfx_slider_value_changed(value):
	update_sfx_volume()
		
# TODO: Update with actual function
func update_sfx_volume():
	pass
	
# TODO: Add logic for updating character stat bars

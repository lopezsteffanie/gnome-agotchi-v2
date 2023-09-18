extends Control

@onready var api_url = Env.get_value("API_URL")
@onready var prompt = $Prompt
@onready var display_name = $DisplayName
@onready var name_input = $NameYourGnome
@onready var submit_button = $SubmitName
@onready var reset_button = $ResetName
@onready var start_button = $StartGame
@onready var error_message = $ErrorMessage
@onready var getGnomeId = $GetGnomeIdRequest
@onready var nameGnomeRequest = $NameGnomeRequest

var gnome_name = ""
var jwt_token = ""
var gnome_id = ""

func _ready():
	display_name.visible = false
	reset_button.visible = false
	start_button.disabled = true
	error_message.visible = false
	submit_button.pressed.connect(_on_submit_pressed)
	reset_button.pressed.connect(_on_reset_pressed)
	start_button.pressed.connect(_on_start_pressed)
	name_input.focus_entered.connect(_on_input_text)
	
	getGnomeId.request_completed.connect(_on_get_gnome_id_request_completed)
	nameGnomeRequest.request_completed.connect(_on_name_gnome_id_request_completed)
	
	# Load JWT token from file
	load_token_from_file()
	
func _on_get_gnome_id_request_completed(result, response_code, headers, body):
	var response = JSON.parse_string(body.get_string_from_utf8())
	if (response_code == 200):
		gnome_id = response.gnome_id
	else:
		printerr(response)
func _on_name_gnome_id_request_completed(result, response_code, headers, body):
	var response = JSON.parse_string(body.get_string_from_utf8())
	if (response_code == 200):
		print("Name Gnome Response: ", response)
		get_tree().change_scene_to_file("res://scenes/game.tscn")
	else:
		printerr(response)
	
func _on_submit_pressed():
	get_gnome_id()
	if name_input.text == "" || name_input.text == null:
		error_message.visible = true
	else:
		gnome_name = name_input.text
		display_name.text = "Your gnome's name: " + gnome_name
		prompt.visible = false
		name_input.visible = false
		display_name.visible = true
		submit_button.visible = false
		reset_button.visible = true
		start_button.disabled = false
		name_input.text = ""
		
func _on_reset_pressed():
	gnome_name = ""
	prompt.visible = true
	name_input.visible = true
	display_name.visible = false
	submit_button.visible = true
	reset_button.visible = false
	start_button.disabled = true

func _on_start_pressed():
	await name_gnome(gnome_id)

func _on_input_text():
	error_message.visible = false
	
func get_gnome_id():
	var url = api_url + "/api/gnome/get-current-user-gnome-id"
	if jwt_token != "":
		var headers = ["Authorization: " + jwt_token]
		
		# Send the request with the JWT token in the headers
		var error = await getGnomeId.request(url, headers, HTTPClient.METHOD_GET, "")
	else:
		print("JWT token is missing or invalid. Cannot make the request")
		
func name_gnome(gnome_id):
	var url = api_url + "/api/gnome/name/" + gnome_id
	var body = JSON.new().stringify({'name': gnome_name})
	if jwt_token != "":
		var headers = ["Authorization: " + jwt_token, "Content-Type: application/json"]
		
		# Send the request with the JWT token in the headers
		var error = await nameGnomeRequest.request(url, headers, HTTPClient.METHOD_POST, body)
	else:
		print("JWT token is missing or invalid. Cannot make the request")
		
func load_token_from_file():
	var config = ConfigFile.new()
	
	# Load the configuration file
	if config.load("user://token.cfg") == OK:
		jwt_token = config.get_value("auth", "token")
	else:
		print("Failed to load config file.")

func save_gnome_name_to_file(name):
	var config = ConfigFile.new()
	
	# Load the configuration file
	if config.load("user://gnome.cfg") == OK:
		config.set_value("gnome", "name", name)
		config.save("user://gnome.cfg")
	else:
		print("Failed to load config file.")

extends Node2D

# Define API URL
@onready var api_url = Env.get_value("API_URL")

# Reference to HTTP requests
@onready var createGnome = $CreateGnomeRequest
@onready var getGnomeId = $GetGnomeIdRequest
@onready var getGnomeRequest = $GetGnomeRequest

# Gnome sprite animation
@onready var animated_sprite = $CharacterBody2D/AnimatedSprite2D

# Gnome attributes
var color = ""
var personality = ""
var jwt_token = ""

func _ready():
	# Initialize the scene
	animated_sprite.visible = false
	
	# Connect request completion signals
	createGnome.request_completed.connect(_on_create_gnome_request_completed)
	getGnomeId.request_completed.connect(_on_get_gnome_id_request_completed)
	getGnomeRequest.request_completed.connect(_on_get_gnome_request_completed)
	
	# Load JWT token from file
	load_token_from_file()
	
	# Attempt to retrieve the gnome ID
	get_gnome_id()

# Event handlers	
func _on_create_gnome_request_completed(result, response_code, headers, body):
	var response = JSON.parse_string(body.get_string_from_utf8())
	if (response_code == 201):
		color = response.color
		personality = response["personality"]["name"]
		print("Personality: ", personality)
		set_sprite_color(color)
		save_gnome_data_to_file(color, personality)
	else:
		printerr(response)

func _on_get_gnome_id_request_completed(result, response_code, headers, body):
	var response = JSON.parse_string(body.get_string_from_utf8())
	if (response_code == 200):
		var gnome_id = response.gnome_id
		get_gnome_from_id(gnome_id)
	else:
		printerr(response)
		generate_gnome()
		
func _on_get_gnome_request_completed(result, response_code, headers, body):
	var response = JSON.parse_string(body.get_string_from_utf8())
	if (response_code == 200):
		var gnome = response.gnome
		color = gnome["color"]
		personality = gnome["personality"]["name"]
		print("Personality: ", personality)
		set_sprite_color(color)
		save_gnome_data_to_file(color, personality)
	else:
		printerr(response)
		
# Helper functions
func load_token_from_file():
	var config = ConfigFile.new()
	
	# Load the configuration file
	if config.load("user://token.cfg") == OK:
		jwt_token = config.get_value("auth", "token")
	else:
		print("Failed to load config file.")
		
func generate_gnome():
	var url = api_url + "/api/gnome/create"
	if jwt_token != "":
		var headers = ["Authorization: " + jwt_token]
			
		# Send the request with the JWT token in the headers
		var error = await createGnome.request(url, headers, HTTPClient.METHOD_POST, "")
	else:
		print("JWT token is missing or invalid. Cannot make the request")

func set_sprite_color(col: String):
	if col != "":
		animated_sprite.play(col)
	else:
		# Default to red
		animated_sprite.play("red")
		print("Could not get color, default to red")
	animated_sprite.visible = true

func save_gnome_data_to_file(col, pers):
	var config = ConfigFile.new()
	
	config.set_value("gnome", "color", col)
	config.set_value("gnome", "personality", pers)
	config.save("user://gnome.cfg")
	
func get_gnome_id():
	var url = api_url + "/api/gnome/get-current-user-gnome-id"
	if jwt_token != "":
		var headers = ["Authorization: " + jwt_token]
		
		# Send the request with the JWT token in the headers
		var error = await getGnomeId.request(url, headers, HTTPClient.METHOD_GET, "")
	else:
		print("JWT token is missing or invalid. Cannot make the request")
		
func get_gnome_from_id(gnome_id):
	var url = api_url + "/api/gnome/get-gnome/" + gnome_id
	if jwt_token != "":
		var headers = ["Authorization: " + jwt_token]
		
		# Send the request with the JWT token in the headers
		var error = await getGnomeRequest.request(url, headers, HTTPClient.METHOD_GET, "")
	

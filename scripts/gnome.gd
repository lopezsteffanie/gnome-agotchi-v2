extends Node2D

@export_group("HTTPRequests")
@export var create_gnome_request: HTTPRequest
@export var get_gnome_id_request: HTTPRequest
@export var get_gnome_request: HTTPRequest
@export_group("")

@export_group("Gnome")
@export var animated_sprite: AnimatedSprite2D
@export_group("")

@onready var api_url = Env.get_value("API_URL")
@onready var gnome_api: GnomeApi = preload("res://scripts/classes/gnome_api.gd").new()
@onready var api: Api = preload("res://scripts/classes/api.gd").new()
@onready var config: Config = preload("res://scripts/classes/config.gd").new()

# Gnome attributes
var color: String = ""
var personality: String = ""
var jwt_token: String = ""
var age: int

func _ready():
	# Initialize the scene
	animated_sprite.visible = false
	
	# Connect request completion signals
	create_gnome_request.request_completed.connect(_on_create_gnome_request_completed)
	get_gnome_id_request.request_completed.connect(_on_get_gnome_id_request_completed)
	get_gnome_request.request_completed.connect(_on_get_gnome_request_completed)
	
	# Load JWT token from file
	jwt_token = config.load_token_from_file()
	
	# Attempt to retrieve the gnome ID
	var url: String = api_url + "/gnome/get-current-user-gnome-id"
	gnome_api.get_gnome_id(url, jwt_token, get_gnome_id_request)

# Event handlers	
func _on_create_gnome_request_completed(result, response_code, headers, body):
	var response = api.get_response(body)
	if api.is_response_valid(response_code):
		print("Gnome: " response)
		color = response.color
		personality = response["personality"]["name"]
		load_and_save_gnome(color, personality)
	else:
		printerr(response)

func _on_get_gnome_id_request_completed(result, response_code, headers, body):
	var response = api.get_response(body)
	if api.is_response_valid(response_code):
		var gnome_id: String = response.gnome_id
		var url: String = api_url + "/gnome/get-gnome/" + gnome_id
		gnome_api.get_gnome_from_id(url, gnome_id, jwt_token, get_gnome_request)
	else:
		printerr(response)
		var url: String = api_url + "/gnome/create"
		gnome_api.generate_gnome(url, jwt_token, create_gnome_request)
		
func _on_get_gnome_request_completed(result, response_code, headers, body):
	var response: Dictionary = api.get_response(body)
	if api.is_response_valid(response_code):
		var gnome: Dictionary = response.gnome
		color = gnome["color"]
		personality = gnome["personality"]["name"]
		age = gnome["age"]
		load_and_save_gnome(color, personality)
	else:
		printerr(response)
		
# Helper functions	
func load_and_save_gnome(gnome_color: String, gnome_personality: String):
	if gnome_color != "":
		animated_sprite.play(gnome_color)
	else:
		# Default to red
		animated_sprite.play("red")
		print("Could not get color, default to red")
	animated_sprite.visible = true
	config.save_gnome_data_to_file(gnome_color, gnome_personality)

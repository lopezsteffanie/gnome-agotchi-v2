extends Node2D

@onready var api_url = Env.get_value("API_URL")
@onready var http = $HTTPRequest
@onready var animated_sprite = $CharacterBody2D/AnimatedSprite2D

var color
var personality
var jwt_token

func _ready():
	http.request_completed.connect(_on_http_request_request_completed)
	load_token_from_file()
	generate_gnome()
	
func _on_http_request_request_completed(result, response_code, headers, body):
	var response = JSON.parse_string(body.get_string_from_utf8())
	# If request is ok
	if (response_code == 200 || response_code == 201):
		print(response)
		set_sprite_color(response.color)
	else:
		printerr(response)
		
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
		var error = await http.request(url, headers, HTTPClient.METHOD_POST, "")
	else:
		print("JWT token is missing or invalid. Cannot make the request")

func set_sprite_color(color: String):
	if color != "":
		animated_sprite.play(color)
	else:
		# Default to red
		animated_sprite.play("red")
		print("Could not get color, default to red")
	animated_sprite.visible = true

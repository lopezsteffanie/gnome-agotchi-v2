extends Control

@export_group("UI Elements")
@export var error_message: Label

@export_subgroup("Buttons")
@export var login_button: Button
@export var signup_button: Button
@export var forgot_password_button: Button

@export_subgroup("LineEdits")
@export var input_email: LineEdit
@export var input_password: LineEdit
@export_group("")

@export_group("HTTPRequests")
@export var login_request: HTTPRequest
@export var gnome_id_request: HTTPRequest
@export var gnome_request: HTTPRequest
@export_group("")

@onready var api_url = Env.get_value("API_URL")
@onready var auth: Auth = preload("res://scripts/classes/auth.gd").new()
@onready var gnome_api: GnomeApi = preload("res://scripts/classes/gnome_api.gd").new()
@onready var config: Config = preload("res://scripts/classes/config.gd").new()
@onready var api: Api = preload("res://scripts/classes/api.gd").new()

var jwt_token: String = ""

signal login_completed
signal signup_completed
signal clicked_on_signup
signal clicked_on_forgot_password

func _ready():
	login_button.pressed.connect(_on_login_pressed)
	signup_button.pressed.connect(_on_signup_pressed)
	forgot_password_button.pressed.connect(_on_forgotpassword_pressed)
	login_request.request_completed.connect(_on_login_request_completed)
	gnome_id_request.request_completed.connect(_on_get_gnome_id_request_completed)
	gnome_request.request_completed.connect(_on_get_gnome_request_completed)
	input_email.focus_entered.connect(_on_input_text)
	input_password.focus_entered.connect(_on_input_text)
	error_message.text = ""

func _on_login_request_completed(result, response_code, headers, body):
	var response: Dictionary = api.get_response(body)
	
	if api.is_response_valid(response_code):
		jwt_token = response.token
		config.save_token_to_file(jwt_token)
		error_message.text = ""
		input_email.text = ""
		input_password.text = ""
		
		# Attempt to retrieve the gnome ID
		var url: String = api_url + "/gnome/get-current-user-gnome-id"
		gnome_api.get_gnome_id(url, jwt_token, gnome_id_request)
	else:
		printerr(response)
		error_message.text = response.error
		
func _on_get_gnome_id_request_completed(result, response_code, headers, body):
	var response: Dictionary = api.get_response(body)
	
	if api.is_response_valid(response_code):
		var gnome_id: String = response.gnome_id
		var url: String = api_url + "/gnome/get-gnome/" + gnome_id
		gnome_api.get_gnome_from_id(url, gnome_id, jwt_token, gnome_request)
	else:
		printerr(response)
		signup_completed.emit()
		
func _on_get_gnome_request_completed(result, response_code, headers, body):
	var response: Dictionary = api.get_response(body)
	
	if api.is_response_valid(response_code):
		var gnome: Dictionary = response.gnome
		if gnome["name"]:
			print("Name: ", gnome["name"])
			login_completed.emit()
		else:
			signup_completed.emit()
	else:
		printerr(response)
		error_message.text = response.error

func _on_login_pressed():
	var url: String = api_url + "/auth/login"
	auth.login_signup(url, input_email.text, input_password.text, login_request)

func _on_signup_pressed():
	clicked_on_signup.emit()
	
func _on_forgotpassword_pressed():
	clicked_on_forgot_password.emit()

func _on_input_text():
	error_message.text = ""

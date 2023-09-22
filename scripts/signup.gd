extends Control

@export_group("UI Elements")
@export var error_message: Label

@export_subgroup("Line Edits")
@export var input_email: LineEdit
@export var input_password: LineEdit
@export var input_confirm_password: LineEdit

@export_subgroup("Buttons")
@export var back_button: Button
@export var register_button: Button
@export_group("")

@export_group("HTTP Requests")
@export var signup_request: HTTPRequest
@export_group("")

@onready var api_url = Env.get_value("API_URL")
@onready var auth: Auth = preload("res://scripts/classes/auth.gd").new()
@onready var api: Api = preload("res://scripts/classes/api.gd").new()

signal signup_completed

func _ready():
	error_message.text = ""
	signup_request.request_completed.connect(_on_http_request_request_completed)
	input_email.focus_entered.connect(_on_input_text)
	input_password.focus_entered.connect(_on_input_text)
	back_button.pressed.connect(_on_back_pressed)
	register_button.pressed.connect(_on_register_pressed)

func _on_http_request_request_completed(result, response_code, headers, body):
	var response: Dictionary = api.get_response(body)
	# If request is ok
	if api.is_response_valid(response_code):
		error_message.text = ""
		input_email.text = ""
		input_password.text = ""
		input_confirm_password.text = ""
		# FIXME
		signup_completed.emit()
	else:
		error_message.text = response.error
	
func _on_register_pressed():
	if input_confirm_password.text == "" || input_confirm_password.text == null:
		error_message.text = "Please confirm your password"
	elif input_confirm_password.text != input_password.text:
		error_message.text = "Your passwords don't match"
	else:
		var url: String = api_url + "/auth/register"
		auth.login_signup(url, input_email.text, input_password.text, signup_request)

func _on_input_text():
	error_message.text = ""
	
func _on_back_pressed():
	# FixME
	signup_completed.emit()

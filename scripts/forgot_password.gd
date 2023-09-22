extends Control

@export_group("UI Elements")
@export var error_message: Label
@export var input_email: LineEdit

@export_subgroup("Buttons")
@export var back_button: Button
@export var submit_button: Button
@export_group("")

@export_group("HTTPRequests")
@export var forgot_password_request: HTTPRequest

@onready var api_url = Env.get_value("API_URL")
@onready var auth: Auth = preload("res://scripts/classes/auth.gd").new()
@onready var api: Api = preload("res://scripts/classes/api.gd").new()

signal left_page

func _ready():
	error_message.text = ""
	forgot_password_request.request_completed.connect(_on_http_request_request_completed)
	input_email.focus_entered.connect(_on_input_text)
	back_button.pressed.connect(_on_back_pressed)
	submit_button.pressed.connect(_on_submit_pressed)

func _on_http_request_request_completed(result, response_code, headers, body):
	var response: Dictionary = api.get_response(body)
	if api.is_response_valid(response_code):
		error_message.text = ""
		input_email.text = ""
		left_page.emit()
	else:
		error_message.text = response.error

func _on_submit_pressed():
	if input_email.text == "" || input_email.text == null:
		error_message.text = "Please inpupt your email to reset your password."
	else:
		var url: String = api_url + "/auth/forgot-password"
		auth.forgot_password(url, input_email.text, forgot_password_request)

func _on_input_text():
	error_message.text = ""
	
func _on_back_pressed():
	left_page.emit()

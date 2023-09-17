extends Control

@onready var api_url = Env.get_value("API_URL")
@onready var error_message = $ErrorMessage
@onready var login_button = $Buttons/AuthButtons/Login
@onready var signup_button = $Buttons/AuthButtons/Signup
@onready var forgot_password_button = $"Buttons/Forgot Password"
@onready var http = $HTTPRequest
@onready var input_email = $LineEdits/Email
@onready var input_password = $LineEdits/Password
@onready var auth = preload("res://scripts/classes/auth.gd").new()

func _ready():
	login_button.pressed.connect(_on_login_pressed)
	signup_button.pressed.connect(_on_signup_pressed)
	forgot_password_button.pressed.connect(_on_forgotpassword_pressed)
	http.request_completed.connect(_on_http_request_request_completed)
	input_email.focus_entered.connect(_on_input_text)
	input_password.focus_entered.connect(_on_input_text)

func _on_http_request_request_completed(result, response_code, headers, body):
	var response = JSON.parse_string(body.get_string_from_utf8())
	# If request is ok
	if (response_code == 200 || response_code == 201):
		print(response)
		error_message.text = ""
		input_email.text = ""
		input_password.text = ""
		get_tree().change_scene_to_file("res://scenes/game.tscn")
	else:
		printerr(response.error)
		error_message.text = response.error

func _on_login_pressed():
	var url = api_url + "/api/auth/login"
	auth.loginSignup(url, input_email.text, input_password.text, http)

func _on_signup_pressed():
	get_tree().change_scene_to_file("res://scenes/signup.tscn")
	
func _on_forgotpassword_pressed():
	get_tree().change_scene_to_file("res://scenes/forgot_password.tscn")

func _on_input_text():
	error_message.text = ""

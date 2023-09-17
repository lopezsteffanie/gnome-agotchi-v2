extends Control

@onready var api_url = Env.get_value("API_URL")
@onready var error_message = $ErrorMessage
@onready var http = $HTTPRequest
@onready var input_email = $LineEdits/Email
@onready var back_button = $Buttons/Back
@onready var submit_button = $Buttons/Submit
@onready var auth = preload("res://scripts/classes/auth.gd").new()

func _ready():
	http.request_completed.connect(_on_http_request_request_completed)
	input_email.focus_entered.connect(_on_input_text)
	back_button.pressed.connect(_on_back_pressed)
	submit_button.pressed.connect(_on_submit_pressed)

func _on_http_request_request_completed(result, response_code, headers, body):
	var response = JSON.parse_string(body.get_string_from_utf8())
	# If request is ok
	if (response_code == 200 || response_code == 201):
		print(response)
		error_message.text = ""
		input_email.text = ""
		get_tree().change_scene_to_file("res://scenes/login.tscn")
	else:
		error_message.text = response.error

func _on_submit_pressed():
	if input_email.text == "" || input_email.text == null:
		error_message.text = "Please inpupt your email to reset your password."
	else:
		var url = api_url + "/api/auth/forgot-password"
		auth.forgotPassword(url, input_email.text, http)

func _on_input_text():
	error_message.text = ""
	
func _on_back_pressed():
	get_tree().change_scene_to_file("res://scenes/login.tscn")

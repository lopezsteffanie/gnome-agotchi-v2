extends Control

var api_url

func _ready():
	# Get the value of the API_URL && API_Kenvironment variables
	api_url = Env.get_value("API_URL")
	
	print("API_URL is not set") if api_url == "" else print("API_URL: is set")

#login and signup function
func _loginSignup(url: String, email: String, password: String):
	var http = $HTTPRequest
	var jsonObject = JSON.new()
	var body = jsonObject.stringify({'email': email, 'password': password})
	var headers = ['Content-Type: application/json']
	var error = await http.request(url, headers, HTTPClient.METHOD_POST, body)
	


func _on_http_request_request_completed(result, response_code, headers, body):
	var response = JSON.parse_string(body.get_string_from_utf8())
	# If request is ok
	if (response_code == 200):
		print(response)
	else:
		print(response.error)


func _on_login_pressed():
	var url = api_url + "/api/auth/login"
	var email = $LineEdits/Email.text
	var password = $LineEdits/Password.text
	_loginSignup(url, email, password)

class_name Auth

var headers = ['Content-Type: application/json']

func loginSignup(url: String, email: String, password: String, httpRequest: HTTPRequest):
	var body = JSON.new().stringify({'email': email, 'password': password})
	var error = await httpRequest.request(url, headers, HTTPClient.METHOD_POST, body)

func forgotPassword(url: String, email: String, httpRequest: HTTPRequest):
	var body = JSON.new().stringify({'email': email})
	var error = await httpRequest.request(url, headers, HTTPClient.METHOD_POST, body)

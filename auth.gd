class_name Auth

func loginSignup(url: String, email: String, password: String, httpRequest: HTTPRequest):
	var body = JSON.new().stringify({'email': email, 'password': password})
	var headers = ['Content-Type: application/json']
	var error = await httpRequest.request(url, headers, HTTPClient.METHOD_POST, body)

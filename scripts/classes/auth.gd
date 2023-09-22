class_name Auth

var headers = ['Content-Type: application/json']

func login_signup(url: String, email: String, password: String, httpRequest: HTTPRequest):
	var body = JSON.new().stringify({'email': email, 'password': password})
	var error = await httpRequest.request(url, headers, HTTPClient.METHOD_POST, body)

func forgot_password(url: String, email: String, httpRequest: HTTPRequest):
	var body = JSON.new().stringify({'email': email})
	var error = await httpRequest.request(url, headers, HTTPClient.METHOD_POST, body)

func logout(url: String, jwt_token: String, httpRequest: HTTPRequest):
	if jwt_token != "":
		var headers = ["Authorization: " + jwt_token]
		
		# Send the request with the JWT token in the headers
		var error = await httpRequest.request(url, headers,  HTTPClient.METHOD_POST, "")
	else:
		print("JWT token is missing or invalid. Cannot make the request")

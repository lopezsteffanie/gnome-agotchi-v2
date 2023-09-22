class_name GnomeApi

func get_gnome_id(url: String, jwt_token: String, http: HTTPRequest):
	if jwt_token != "":
		var headers = ["Authorization: " + jwt_token]
		# Send the request with the JWT token in the headers
		var error = await http.request(url, headers, HTTPClient.METHOD_GET, "")
	else:
		print("JWT token is missing or invalid. Cannot make the request")


func get_gnome_from_id(url: String, gnome_id: String, jwt_token: String, http: HTTPRequest):
	if jwt_token != "":
		var headers = ["Authorization: " + jwt_token]
		
		# Send the request with the JWT token in the headers
		var error = await http.request(url, headers, HTTPClient.METHOD_GET, "")

func generate_gnome(url: String, jwt_token: String, http: HTTPRequest):
	if jwt_token != "":
		var headers = ["Authorization: " + jwt_token]
			
		# Send the request with the JWT token in the headers
		var error = await http.request(url, headers, HTTPClient.METHOD_POST, "")
	else:
		print("JWT token is missing or invalid. Cannot make the request")

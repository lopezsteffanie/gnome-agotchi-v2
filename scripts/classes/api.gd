class_name Api

func get_response(body: PackedByteArray):
	return JSON.parse_string(body.get_string_from_utf8())

func is_response_valid(response_code: int):
	return response_code == 200 || response_code == 201

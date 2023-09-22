class_name Config

func save_token_to_file(jwt_token: String):
	var config = ConfigFile.new()
	
	config.set_value("auth", "token", jwt_token)
	config.save("user://token.cfg")

func load_token_from_file():
	var config = ConfigFile.new()
	
	# Load the configuration file
	if config.load("user://token.cfg") == OK:
		return config.get_value("auth", "token")
	else:
		print("Failed to load config file.")

func save_gnome_data_to_file(color: String, personality: String):
	var config = ConfigFile.new()
	
	config.set_value("gnome", "color", color)
	config.set_value("gnome", "personality", personality)
	config.save("user://gnome.cfg")

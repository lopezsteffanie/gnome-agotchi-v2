extends Node

@onready var login_scene: PackedScene = preload("res://scenes/login.tscn")
@onready var gnome_scene: PackedScene = preload("res://scenes/gnome.tscn")
@onready var name_gnome_scene: PackedScene = preload("res://scenes/name_gnome.tscn")
@onready var signup_scene: PackedScene = preload("res://scenes/signup.tscn")
@onready var forgot_password_scene: PackedScene = preload("res://scenes/forgot_password.tscn")

var login_instance: Node
var gnome_instance: Node
var name_gnome_instance: Node
var signup_instance: Node
var forgot_password_instance: Node
# Called when the node enters the scene tree for the first time.
func _ready():
	add_login_instance()

func _on_login_completed():
	add_gnome_instance()
	remove_child(login_instance)
	
func _on_signup_completed():
	add_gnome_instance()
	
	name_gnome_instance = name_gnome_scene.instantiate()
	add_child(name_gnome_instance)
	remove_child(login_instance)
	
func _on_signup_clicked():
	signup_instance = signup_scene.instantiate()
	add_child(signup_instance)
	signup_instance.signup_completed.connect(_on_left_signup_page)
	remove_child(login_instance)
	
func _on_forgot_password_clicked():
	forgot_password_instance = forgot_password_scene.instantiate()
	add_child(forgot_password_instance)
	forgot_password_instance.left_page.connect(_on_left_forgot_password_page)
	remove_child(login_instance)
	
func _on_left_forgot_password_page():
	add_login_instance()
	remove_child(forgot_password_instance)
	
func _on_left_signup_page():
	add_login_instance()
	remove_child(signup_instance)
	
func add_login_instance():
	login_instance = login_scene.instantiate()
	add_child(login_instance)
	
	login_instance.login_completed.connect(_on_login_completed)
	login_instance.signup_completed.connect(_on_signup_completed)
	login_instance.clicked_on_signup.connect(_on_signup_clicked)
	login_instance.clicked_on_forgot_password.connect(_on_forgot_password_clicked)
	
func add_gnome_instance():
	gnome_instance = gnome_scene.instantiate()
	add_child(gnome_instance)

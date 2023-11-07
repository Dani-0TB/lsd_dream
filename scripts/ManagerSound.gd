@tool
extends Node

#static func player_music(track:AudioStream)->MusicPlayer:
#	var res:MusicPlayer = MusicPlayer.new()
#	res.set_bus("music")
#	res.set_stream(track)
#	return res
#static func player_sfx(track:AudioStream)->MusicPlayer:
#	var res:MusicPlayer = MusicPlayer.new()
#	res.set_bus("SFX")
#	res.set_stream(track)
#	return res
#
#signal main_faded_in
#signal main_faded_out
#signal main_fading_in
#signal main_fading_out
#var transition_time:float = ProjectSettings.get("custom_settings/world_transition_time")
#var player_main:MusicPlayer setget set_player_main, get_player_main
#func set_player_main(p:MusicPlayer):
#	player_main = p
#	player_main.connect("faded_in", self, "emit_signal",	["player_main_faded_in"])
#	player_main.connect("faded_out", self, "emit_signal",	["player_main_faded_out"])
#	player_main.connect("fading_in", self, "emit_signal",	["player_main_fading_in"])
#	player_main.connect("fading_out", self, "emit_signal",	["player_main_fading_out"])
#func get_player_main()->MusicPlayer:return player_main
#func music_main_request(stream:AudioStream, transition:float = transition_time):
#	# warning-ignore:return_value_discarded
#	# warning-ignore:return_value_discarded
#	if(is_instance_valid(player_main)):
#		if(player_main.get_stream() == stream):return
#		if(player_main.is_connected("tree_exited", self, "music_main_force")):
#			player_main.disconnect("tree_exited", self, "music_main_force")
#		if(!player_main.is_connected("faded_out", player_main, "queue_free")):
#			player_main.connect("faded_out", player_main, "queue_free")
#		player_main.connect("tree_exited", self, "music_main_force", [stream])
#		player_main.fade_out(transition)
#	else:
#		music_main_force(stream)
#func music_main_force(stream:AudioStream, transition:float = transition_time):
#	player_main = player_music(stream)
#	add_child(player_main)
#	player_main.fade_in(transition)
#
#signal second_fade_in
#signal second_fade_out
#signal second_fading_in
#signal second_fading_out
#var player_second:MusicPlayer setget set_player_second, get_player_second
#func set_player_second(p:MusicPlayer):
#	player_second = p
#	player_second.connect("faded_in", self, "emit_signal",	["player_second_faded_in"])
#	player_second.connect("faded_out", self, "emit_signal",	["player_second_faded_out"])
#	player_second.connect("fading_in", self, "emit_signal",	["player_second_fading_in"])
#	player_second.connect("fading_out", self, "emit_signal",["player_second_fading_out"])
#func get_player_second()->MusicPlayer:return player_second
#func music_second_request(stream:AudioStream, transition:float = transition_time):
#	if(!is_instance_valid(player_main)):music_main_request(stream)
#	elif(is_instance_valid(player_second)):
#		if(player_second.get_stream() == stream):return
#		if(player_second.is_connected("tree_exited", self, "music_second_request")):
#			player_second.disconnect("tree_exited", self, "music_second_request")
#		if(player_second.is_connected("tree_exited", player_main, "fade_in")):
#			player_second.disconnect("tree_exited", player_main, "fade_in")
#		# warning-ignore:return_value_discarded
#		player_second.connect("tree_exited", self, "music_second_request", [stream])
#		player_second.fade_out(transition)
#	else:
#		music_second_force(stream)
#func music_second_force(stream:AudioStream, transition:float = transition_time):
#	# warning-ignore:return_value_discarded
#	# warning-ignore:return_value_discarded
#	player_second = player_music(stream)
#	player_second.connect("tree_exited", player_main, "fade_in", [transition])
#	player_second.connect("faded_out", player_second, "queue_free")
#	add_child(player_second)
#	player_second.fade_in(transition)
#func get_playing()->MusicPlayer:
#	assert(is_instance_valid(player_main) || is_instance_valid(player_second),"no <MusicPlayer> to play")
#	if(is_instance_valid(player_second)):return player_second
#	elif(is_instance_valid(player_main)):return player_main
#	return null
#
#func play(transition:float = transition_time):
#	assert(is_instance_valid(player_main)||is_instance_valid(player_second),"no <MusicPlayer> to play")
#	if(is_instance_valid(player_second)):player_second.fade_out(transition)
#	elif(is_instance_valid(player_main)):player_main.fade_in(transition)
#func mute(transition:float = transition_time):
#	if(is_instance_valid(player_second)):
#		if(player_second.is_connected("faded_out", player_main, "fade_in")):
#			player_second.disconnect("faded_out", player_main, "fade_in")
#		player_second.fade_out(transition)
#	elif(is_instance_valid(player_main)):
#		player_main.fade_out(transition)
#func stop():get_playing()._set_playing(false)
#func resume():get_playing()._set_playing(true)
#
#func request_sfx(stream:AudioStream):
#	var p:MusicPlayer = player_sfx(stream)
## warning-ignore:return_value_discarded
#	p.connect("finished",p,"queue_free")
#	add_child(p)
#	p.play()

func _init():
	set_process_mode(Node.PROCESS_MODE_ALWAYS)
class music_layer:
	var player:MusicPlayer
	var music:AudioStream: set = set_music
	var fade_time:float
	func set_music(m:AudioStream):
		music = m
		player = MusicPlayer.new()
		player.set_stream(music)
		# warning-ignore:return_value_discarded
		# warning-ignore:return_value_discarded
		# warning-ignore:return_value_discarded
		# warning-ignore:return_value_discarded
		# warning-ignore:return_value_discarded
		# warning-ignore:return_value_discarded
		# warning-ignore:return_value_discarded
		player.connect("fading_out", Callable(self, "emit_signal").bind("stopping"))
		player.connect("fading_out", Callable(self, "set_fading").bind(true))
		player.connect("fading_in", Callable(player, "play").bind(), CONNECT_DEFERRED)
		player.connect("faded_out", Callable(self, "emit_signal").bind("stopped"))
		player.connect("faded_out", Callable(self, "set_fading").bind(false))
		player.connect("faded_out", Callable(player, "stop").bind(), CONNECT_DEFERRED)
		player.connect("tree_exited", Callable(self, "emit_signal").bind("exited"), CONNECT_ONE_SHOT)
		player.set_bus("music")
	var fading:bool = false: get = get_fading, set = set_fading
	func set_fading(f:bool):fading = f
	func get_fading()->bool:return fading
	# warning-ignore:unused_signal
	# warning-ignore:unused_signal
	# warning-ignore:unused_signal
	signal stopping
	signal stopped
	signal exited
	static func create(m:AudioStream, t:float)->music_layer:
		var res:music_layer = music_layer.new()
		res.set_music(m)
		assert(t>=0, 'attempted fade time <%s> is less than zero'%t)
		res.fade_time = t
		return res
	func play()->MusicPlayer:
		player.fade_in(fade_time)
		return player
	func stop():
		player.fade_out()
	func mark_for_deletion():
		# warning-ignore:return_value_discarded
		player.connect("faded_out", Callable(player, "queue_free"))
var music_layers:Array = [null,null,null]
var current:music_layer = null: set = set_current
func set_current(m:music_layer):
	current = m
	if(current):add_child(current.play())
func music_update():
	var updated:music_layer = null
	for stream in music_layers:
		if(stream != null):updated = stream
	if(current == null):set_current(updated)
	elif(current.get_fading()):
		if(!current.is_connected("stopped", Callable(self, "music_update"))):
			# warning-ignore:return_value_discarded
			current.connect("stopped", Callable(self, "music_update").bind(), CONNECT_ONE_SHOT)
	elif(updated != current):
		# warning-ignore:return_value_discarded
		current.connect("stopped", Callable(self, "set_current").bind(updated), CONNECT_ONE_SHOT)
		current.stop()
enum LAYER {WORLD, SCENE, DIALOG}
func play_world(s:AudioStream, instant:bool = false):
	var layer:music_layer = music_layer.create(s, float(!instant)) if (s!=null) else null
#	if(layer != null):
#		# warning-ignore:return_value_discarded
#		layer.connect("exited", self, "play_world", [null])
	if(music_layers[LAYER.WORLD] != null):music_layers[LAYER.WORLD].mark_for_deletion()
	music_layers[LAYER.WORLD] = layer
	music_update()
func play_scene(s:AudioStream, instant:bool = false):
	var layer:music_layer = music_layer.create(s, float(!instant)) if (s!=null) else null
#	if(layer != null):
#		# warning-ignore:return_value_discarded
#		layer.connect("exited", self, "play_scene", [null])
	if(music_layers[LAYER.SCENE]!=null):music_layers[LAYER.SCENE].mark_for_deletion()
	music_layers[LAYER.SCENE] = layer
	music_update()
func play_dialog(s:AudioStream, instant:bool = false):
	var layer:music_layer = music_layer.create(s, float(!instant)) if (s!=null) else null
#	if(layer != null):
#		# warning-ignore:return_value_discarded
#		layer.connect("exited", self, "play_dialog", [null])
	if(music_layers[LAYER.DIALOG]!=null):music_layers[LAYER.DIALOG].mark_for_deletion()
	music_layers[LAYER.DIALOG] = layer
	music_update()

func play_sfx(s:AudioStream):
	var sfx:AudioStreamPlayer = AudioStreamPlayer.new()
	sfx.set_bus('SFX')
	sfx.set_stream(s)
	# warning-ignore:return_value_discarded
	sfx.connect("finished", Callable(sfx, "queue_free"))
	add_child(sfx)
	sfx.play()

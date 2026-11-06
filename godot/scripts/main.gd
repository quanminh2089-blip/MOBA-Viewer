extends Node3D

var camera: Camera3D
var focus := Vector3.ZERO
var distance := 45.0
var yaw := 0.0
var pitch := 0.85

func _ready() -> void:
    var world := WorldEnvironment.new()
    var env := Environment.new()
    env.background_mode = Environment.BG_COLOR
    env.background_color = Color(0.12, 0.17, 0.19)
    env.ambient_light_source = Environment.AMBIENT_SOURCE_COLOR
    env.ambient_light_color = Color.WHITE
    env.ambient_light_energy = 0.85
    world.environment = env
    add_child(world)

    var sun := DirectionalLight3D.new()
    sun.rotation_degrees = Vector3(-55, 30, 0)
    sun.light_energy = 1.3
    add_child(sun)

    var model = load("res://map/1V1.glb")
    if model is PackedScene:
        var map_node = model.instantiate()
        add_child(map_node)
        _fit_map(map_node)
    else:
        push_error("Cannot load map GLB")

    camera = Camera3D.new()
    camera.current = true
    camera.fov = 55.0
    camera.far = 10000.0
    add_child(camera)
    _update_camera()
    _make_ui()

func _fit_map(root: Node) -> void:
    var meshes: Array[MeshInstance3D] = []
    _collect_meshes(root, meshes)
    var bounds := AABB()
    var found := false
    for mesh in meshes:
        if mesh.mesh == null:
            continue
        var local_box := mesh.mesh.get_aabb()
        for x in [0, 1]:
            for y in [0, 1]:
                for z in [0, 1]:
                    var p := local_box.position + Vector3(
                        local_box.size.x * x,
                        local_box.size.y * y,
                        local_box.size.z * z
                    )
                    p = mesh.global_transform * p
                    if not found:
                        bounds = AABB(p, Vector3.ZERO)
                        found = true
                    else:
                        bounds = bounds.expand(p)
    if found:
        focus = bounds.get_center()
        distance = maxf(bounds.size.length() * 0.8, 15.0)

func _collect_meshes(node: Node, result: Array[MeshInstance3D]) -> void:
    if node is MeshInstance3D:
        result.append(node)
    for child in node.get_children():
        _collect_meshes(child, result)

func _update_camera() -> void:
    var direction := Vector3(
        sin(yaw) * cos(pitch),
        sin(pitch),
        cos(yaw) * cos(pitch)
    )
    camera.global_position = focus + direction * distance
    camera.look_at(focus, Vector3.UP)

func _unhandled_input(event: InputEvent) -> void:
    if event is InputEventScreenDrag:
        yaw -= event.relative.x * 0.006
        pitch = clampf(pitch + event.relative.y * 0.004, 0.3, 1.45)
        _update_camera()
    elif event is InputEventMouseMotion and Input.is_mouse_button_pressed(MOUSE_BUTTON_LEFT):
        yaw -= event.relative.x * 0.006
        pitch = clampf(pitch + event.relative.y * 0.004, 0.3, 1.45)
        _update_camera()
    elif event is InputEventMouseButton and event.pressed:
        if event.button_index == MOUSE_BUTTON_WHEEL_UP:
            _zoom(0.85)
        elif event.button_index == MOUSE_BUTTON_WHEEL_DOWN:
            _zoom(1.15)

func _zoom(factor: float) -> void:
    distance = clampf(distance * factor, 3.0, 5000.0)
    _update_camera()

func _make_ui() -> void:
    var layer := CanvasLayer.new()
    add_child(layer)

    var ui := Control.new()
    ui.set_anchors_and_offsets_preset(Control.PRESET_FULL_RECT)
    ui.mouse_filter = Control.MOUSE_FILTER_IGNORE
    layer.add_child(ui)

    var panel := VBoxContainer.new()
    panel.set_anchors_and_offsets_preset(Control.PRESET_CENTER_RIGHT)
    panel.position = Vector2(-105, -95)
    panel.add_theme_constant_override("separation", 12)
    ui.add_child(panel)

    var zoom_in := Button.new()
    zoom_in.text = "+"
    zoom_in.custom_minimum_size = Vector2(85, 85)
    zoom_in.add_theme_font_size_override("font_size", 32)
    zoom_in.pressed.connect(func(): _zoom(0.8))
    panel.add_child(zoom_in)

    var zoom_out := Button.new()
    zoom_out.text = "-"
    zoom_out.custom_minimum_size = Vector2(85, 85)
    zoom_out.add_theme_font_size_override("font_size", 32)
    zoom_out.pressed.connect(func(): _zoom(1.25))
    panel.add_child(zoom_out)

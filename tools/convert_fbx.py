import bpy
import os

source = os.path.abspath(
    "Assets/Art/SceneModel/1V1.FBX"
)
output = os.path.abspath("output/1V1.glb")

os.makedirs("output", exist_ok=True)

bpy.ops.object.select_all(action="SELECT")
bpy.ops.object.delete(use_global=False)

bpy.ops.import_scene.fbx(filepath=source)

print("Imported objects:", len(bpy.data.objects))

bpy.ops.export_scene.gltf(
    filepath=output,
    export_format="GLB",
    export_apply=True
)

print("Exported:", output)

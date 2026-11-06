using UnityEngine;
using UnityEngine.SceneManagement;

public class MapViewer : MonoBehaviour
{
    Camera cam;
    Vector3 center;
    float distance = 35f, yaw = 0f, pitch = 55f;
    float lastPinch;
    void Start()
    {
        var renderers = FindObjectsOfType<Renderer>();
        Bounds bounds = new Bounds(Vector3.zero, new Vector3(20, 1, 20));
        bool first = true;
        foreach (var r in renderers) { if (!r.enabled) continue; if(first) { bounds=r.bounds; first=false; } else bounds.Encapsulate(r.bounds); }
        center=bounds.center;
        distance=Mathf.Max(15f, Mathf.Max(bounds.size.x,bounds.size.z)*1.15f);
        var go=new GameObject("Offline Viewer Camera");
        cam=go.AddComponent<Camera>(); cam.tag="MainCamera"; cam.fieldOfView=55;cam.nearClipPlane=0.1f;cam.farClipPlane=2000f;
        cam.clearFlags=CameraClearFlags.Skybox;
        if(FindObjectsOfType<Light>().Length==0){var l=new GameObject("Sun").AddComponent<Light>();l.type=LightType.Directional;l.intensity=1.2f;l.transform.rotation=Quaternion.Euler(50,-30,0);}
        UpdateCamera();
    }
    void Update()
    {
        if(Input.touchCount==1){var t=Input.GetTouch(0);if(t.phase==TouchPhase.Moved){yaw+=t.deltaPosition.x*.16f;pitch=Mathf.Clamp(pitch-t.deltaPosition.y*.12f,20,85);}}
        if(Input.touchCount==2){var a=Input.GetTouch(0);var b=Input.GetTouch(1);float d=Vector2.Distance(a.position,b.position);if(lastPinch>0)distance=Mathf.Clamp(distance-(d-lastPinch)*.05f,3,600);lastPinch=d;}else lastPinch=0;
        if(Input.GetMouseButton(0)){yaw+=Input.GetAxis("Mouse X")*2;pitch=Mathf.Clamp(pitch-Input.GetAxis("Mouse Y")*2,20,85);}
        distance=Mathf.Clamp(distance-Input.mouseScrollDelta.y*2,3,600);
        UpdateCamera();
    }
    void UpdateCamera(){if(!cam)return;var rot=Quaternion.Euler(pitch,yaw,0);cam.transform.position=center+rot*new Vector3(0,0,-distance);cam.transform.LookAt(center);}
    void OnGUI()
    {
        GUI.skin.button.fontSize=Mathf.RoundToInt(Mathf.Clamp(Screen.width/34f,14,32));
        if(GUI.Button(new Rect(12,12,Screen.width*.27f,Screen.height*.09f),"MAP 101"))SceneManager.LoadScene("map_101");
        if(GUI.Button(new Rect(Screen.width*.29f,12,Screen.width*.27f,Screen.height*.09f),"MAP 102"))SceneManager.LoadScene("map_102");
        GUI.Label(new Rect(12,Screen.height-50,Screen.width-20,45),"Vuot: xoay | 2 ngon: zoom");
    }
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Init(){new GameObject("Map Viewer Controller").AddComponent<MapViewer>();}
}

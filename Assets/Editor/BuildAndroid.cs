using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.IO;
public static class BuildAndroid
{
    public static void Build()
    {
        string[] paths={"Assets/Resources/ResScenes/map_101.unity","Assets/Resources/ResScenes/map_102.unity"};
        foreach(string p in paths)
        {
            Scene scene=EditorSceneManager.OpenScene(p,OpenSceneMode.Single);
            foreach(GameObject root in scene.GetRootGameObjects())Clean(root);
            EditorSceneManager.SaveScene(scene);
        }
        PlayerSettings.productName="MOBA Map Viewer";
        PlayerSettings.companyName="MinhQuan";
        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android,"com.minhquan.mobamapviewer");
        PlayerSettings.Android.minSdkVersion=AndroidSdkVersions.AndroidApiLevel23;
        PlayerSettings.defaultInterfaceOrientation=UIOrientation.LandscapeLeft;
        PlayerSettings.Android.targetArchitectures=AndroidArchitecture.ARM64;
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android,ScriptingImplementation.IL2CPP);
        Directory.CreateDirectory("Build");
        var opts=new BuildPlayerOptions{scenes=paths,locationPathName="Build/MOBA-Map-Viewer.apk",target=BuildTarget.Android,options=BuildOptions.None};
        BuildReport report=BuildPipeline.BuildPlayer(opts);
        if(report.summary.result!=BuildResult.Succeeded)throw new System.Exception("Unity Android build failed: "+report.summary.result);
    }
    static void Clean(GameObject go)
    {
        GameObjectUtility.RemoveMonoBehavioursWithMissingScript(go);
        foreach(Transform child in go.transform)Clean(child.gameObject);
    }
}

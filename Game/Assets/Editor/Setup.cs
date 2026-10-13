using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using CoopCombat;
public static class Setup {
 public static void Build(){
  EditorSettings.serializationMode=SerializationMode.ForceText;EditorSettings.externalVersionControl="Visible Meta Files";
  var scene=EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);
  var ground=GameObject.CreatePrimitive(PrimitiveType.Cube);ground.name="Ground";ground.transform.position=new Vector3(0,-.5f,0);ground.transform.localScale=new Vector3(25,1,25);ground.GetComponent<Renderer>().material.color=new Color(.2f,.3f,.2f);
  var camera=new GameObject("Main Camera");camera.tag="MainCamera";camera.AddComponent<Camera>();camera.AddComponent<AudioListener>();camera.transform.position=new Vector3(0,8,-10);camera.transform.LookAt(Vector3.zero);
  var light=new GameObject("Sun");light.AddComponent<Light>().type=LightType.Directional;light.transform.rotation=Quaternion.Euler(50,-30,0);
  var session=new GameObject("Session");session.AddComponent<SessionView>();Time.fixedDeltaTime=.05f;
  System.IO.Directory.CreateDirectory("Assets/Scenes");EditorSceneManager.SaveScene(scene,"Assets/Scenes/Combat.unity");EditorBuildSettings.scenes=new[]{new EditorBuildSettingsScene("Assets/Scenes/Combat.unity",true)};AssetDatabase.SaveAssets();
 }
}

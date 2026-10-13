using UnityEditor;
using UnityEditor.Build.Reporting;
public static class PlayerBuild {
 public static void Run(){var report=BuildPipeline.BuildPlayer(new BuildPlayerOptions{scenes=new[]{"Assets/Scenes/Combat.unity"},locationPathName="../.local/player/CoopCombat.exe",target=BuildTarget.StandaloneWindows64,options=BuildOptions.Development});if(report.summary.result!=BuildResult.Succeeded)throw new System.Exception("Player build failed: "+report.summary.result);}
}

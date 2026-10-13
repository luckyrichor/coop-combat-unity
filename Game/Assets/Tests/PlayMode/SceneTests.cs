using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using CoopCombat;
public class SceneTests {
 [UnityTest] public IEnumerator SceneHostInputToJudgementAndVisual(){
  yield return SceneManager.LoadSceneAsync("Combat");
  yield return new WaitForSeconds(.3f);
  var view=Object.FindObjectOfType<SessionView>();Assert.IsNotNull(view);Assert.IsNotNull(Camera.main);
  Assert.IsNotNull(GameObject.Find("Ground"));var player=GameObject.Find("Player-0");Assert.IsNotNull(player);
  float before=player.transform.position.z;view.QueueSkill(1);yield return new WaitForSeconds(.15f);
  Assert.Greater(player.transform.position.z,before+1);Assert.IsNotNull(GameObject.Find("Sphere"));
  view.QueueSkill(2);yield return new WaitForSeconds(.1f);Assert.IsNotNull(GameObject.Find("Sphere"));
 }
}

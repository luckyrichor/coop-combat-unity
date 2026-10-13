using System.Collections.Generic;
using UnityEngine;
namespace CoopCombat {
 public sealed class SessionView:MonoBehaviour {
  public bool Host=true;public int Port=45002;public float CameraDistance=7;
  LocalServer server;LocalClient client; readonly Dictionary<int,GameObject> visuals=new Dictionary<int,GameObject>();readonly Dictionary<int,int> events=new Dictionary<int,int>();
  float yaw=0,pitch=35;byte pendingSkill;bool probe;float exitSeconds;long loggedTick;
  [System.Serializable] class StateLog {public long tick;public PlayerState[] players;}
  public void QueueSkill(byte skill){if(skill<1||skill>2)throw new System.ArgumentOutOfRangeException("skill");pendingSkill=skill;}
  void Start(){Time.fixedDeltaTime=.05f;var args=System.Environment.GetCommandLineArgs();if(System.Array.IndexOf(args,"--join")>=0)Host=false;probe=System.Array.IndexOf(args,"--probe")>=0;int exitIndex=System.Array.IndexOf(args,"--exit-seconds");if(exitIndex>=0&&exitIndex+1<args.Length)float.TryParse(args[exitIndex+1],out exitSeconds);if(Host)server=new LocalServer(Port);client=new LocalClient(Port);}
  void Update(){if(Input.GetKeyDown(KeyCode.Space))pendingSkill=1;if(Input.GetKeyDown(KeyCode.F))pendingSkill=2;yaw+=Input.GetAxis("Mouse X")*3;pitch=Mathf.Clamp(pitch-Input.GetAxis("Mouse Y")*2,15,70);client.Poll();if(probe&&client.Tick!=loggedTick){loggedTick=client.Tick;Debug.Log("COOP_STATE "+JsonUtility.ToJson(new StateLog{tick=client.Tick,players=client.States}));}if(exitSeconds>0&&Time.time>exitSeconds)Application.Quit();
   foreach(var p in client.States){GameObject body;if(!visuals.TryGetValue(p.Id,out body)){body=GameObject.CreatePrimitive(PrimitiveType.Capsule);body.name="Player-"+p.Id;body.GetComponent<Renderer>().material.color=p.Id==client.Id?Color.cyan:Color.red;visuals.Add(p.Id,body);events[p.Id]=0;}
    body.transform.position=Vector3.Lerp(body.transform.position,new Vector3(p.X,1,p.Z),1-Mathf.Exp(-Time.deltaTime*18));
    if(p.SkillEvent!=events[p.Id]){events[p.Id]=p.SkillEvent;var fx=GameObject.CreatePrimitive(PrimitiveType.Sphere);fx.transform.position=body.transform.position;fx.transform.localScale=Vector3.one*2;fx.GetComponent<Renderer>().material.color=Color.yellow;Destroy(fx.GetComponent<Collider>());Destroy(fx,0.25f);}
   }
   GameObject target;if(visuals.TryGetValue(client.Id,out target)&&Camera.main){var rotation=Quaternion.Euler(pitch,yaw,0);Camera.main.transform.position=target.transform.position+rotation*new Vector3(0,0,-CameraDistance);Camera.main.transform.LookAt(target.transform.position);}
  }
  void FixedUpdate(){var movement=Quaternion.Euler(0,yaw,0)*new Vector3(Input.GetAxis("Horizontal"),0,Input.GetAxis("Vertical"));if(probe)movement=Host?Vector3.right:Vector3.forward;client.Send(movement.x,movement.z,pendingSkill);pendingSkill=0;if(server!=null)server.Pump(Time.fixedDeltaTime);}
  void OnGUI(){GUI.Label(new Rect(10,10,650,30),(Host?"HOST":"CLIENT")+" WASD move / mouse orbit / Space dash / F pulse / authoritative tick "+client.Tick);int y=45;foreach(var p in client.States){GUI.Label(new Rect(10,y,400,30),"Player "+p.Id+" HP "+p.Health);y+=30;}}
  void OnDestroy(){if(client!=null)client.Dispose();if(server!=null)server.Dispose();}
 }
}

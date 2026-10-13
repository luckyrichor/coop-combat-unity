using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
namespace CoopCombat {
 public sealed class LocalServer:IDisposable {
  readonly UdpClient socket;readonly Dictionary<string,int> peers=new Dictionary<string,int>();readonly Dictionary<string,IPEndPoint> endpoints=new Dictionary<string,IPEndPoint>();
  public readonly CombatModel Model=new CombatModel(); public int Port {get{return ((IPEndPoint)socket.Client.LocalEndPoint).Port;}}
  public LocalServer(int port=0){socket=new UdpClient(new IPEndPoint(IPAddress.Loopback,port));socket.Client.ReceiveTimeout=50;}
  public void Pump(float dt){
   var inputs=new List<InputCommand>();
   for(int n=0;n<64&&socket.Available>0;n++){
    IPEndPoint ep=new IPEndPoint(IPAddress.Loopback,0);var packet=socket.Receive(ref ep);if(packet.Length!=18)continue;
    using(var stream=new MemoryStream(packet))using(var reader=new BinaryReader(stream)){
     if(reader.ReadByte()!=1)continue;string key=ep.ToString();int id;
     if(!peers.TryGetValue(key,out id)){if(peers.Count>=4)continue;id=peers.Count;peers[key]=id;endpoints[key]=ep;Model.Add(id);}
     inputs.Add(new InputCommand{Player=id,Sequence=reader.ReadInt64(),X=reader.ReadSingle(),Z=reader.ReadSingle(),Skill=reader.ReadByte()});
    }
   }
   Model.Step(inputs,dt);var states=Model.Snapshot();
   foreach(var pair in endpoints){using(var stream=new MemoryStream())using(var writer=new BinaryWriter(stream)){
    writer.Write((byte)2);writer.Write(Model.Tick);writer.Write((byte)peers[pair.Key]);writer.Write((byte)states.Length);
    foreach(var p in states){writer.Write((byte)p.Id);writer.Write(p.X);writer.Write(p.Z);writer.Write(p.Health);writer.Write(p.SkillEvent);}
    var bytes=stream.ToArray();socket.Send(bytes,bytes.Length,pair.Value);
   }}
  }
  public void Dispose(){socket.Close();}
 }
 public sealed class LocalClient:IDisposable {
  readonly UdpClient socket;long seq; public long Tick {get;private set;} public int Id {get;private set;}=-1;
  public PlayerState[] States {get;private set;}=new PlayerState[0];
  public LocalClient(int port){socket=new UdpClient(new IPEndPoint(IPAddress.Loopback,0));socket.Connect(IPAddress.Loopback,port);socket.Client.ReceiveTimeout=100;}
  public void Send(float x,float z,byte skill){using(var stream=new MemoryStream())using(var w=new BinaryWriter(stream)){w.Write((byte)1);w.Write(++seq);w.Write(x);w.Write(z);w.Write(skill);var bytes=stream.ToArray();socket.Send(bytes,bytes.Length);}}
  public void Poll(){for(int n=0;n<64&&socket.Available>0;n++){
   IPEndPoint ep=new IPEndPoint(IPAddress.Loopback,0);var bytes=socket.Receive(ref ep);if(bytes.Length<11)continue;
   using(var stream=new MemoryStream(bytes))using(var r=new BinaryReader(stream)){
    if(r.ReadByte()!=2)continue;long tick=r.ReadInt64();int id=r.ReadByte(),count=r.ReadByte();if(count>4||bytes.Length!=11+17*count||tick<=Tick)continue;
    var states=new PlayerState[count];for(int i=0;i<count;i++)states[i]=new PlayerState{Id=r.ReadByte(),X=r.ReadSingle(),Z=r.ReadSingle(),Health=r.ReadInt32(),SkillEvent=r.ReadInt32()};
    Tick=tick;Id=id;States=states;
   }
  }}
  public void Dispose(){socket.Close();}
 }
}

using System;
using System.Collections.Generic;
namespace CoopCombat {
 public struct InputCommand {public int Player; public long Sequence; public float X,Z; public byte Skill;}
 [Serializable] public sealed class PlayerState {public int Id; public float X,Z; public int Health=100; public long Sequence=-1; public int SkillEvent; public double DashReady,PulseReady; public PlayerState Copy(){return (PlayerState)MemberwiseClone();}}
 public sealed class CombatModel {
  readonly SortedDictionary<int,PlayerState> players=new SortedDictionary<int,PlayerState>();
  public double Time {get;private set;} public long Tick {get;private set;}
  public void Add(int id){if(players.Count>=4||players.ContainsKey(id))throw new InvalidOperationException("player capacity");players.Add(id,new PlayerState{Id=id,X=id*2});}
  public PlayerState[] Snapshot(){var result=new List<PlayerState>();foreach(var p in players.Values)result.Add(p.Copy());return result.ToArray();}
  public void Step(IEnumerable<InputCommand> commands,float dt){
   if(dt<=0||dt>0.1f||float.IsNaN(dt))throw new ArgumentOutOfRangeException("dt");
   Time+=dt;Tick++;
   var latest=new Dictionary<int,InputCommand>();foreach(var c in commands)if(!latest.ContainsKey(c.Player)||latest[c.Player].Sequence<c.Sequence)latest[c.Player]=c;
   foreach(var c in latest.Values){PlayerState p;if(!players.TryGetValue(c.Player,out p)||c.Sequence<=p.Sequence||float.IsNaN(c.X)||float.IsInfinity(c.X)||float.IsNaN(c.Z)||float.IsInfinity(c.Z))continue;
    p.Sequence=c.Sequence;if(p.Health<=0)continue;float length=(float)Math.Sqrt(c.X*c.X+c.Z*c.Z);float x=c.X/Math.Max(1,length),z=c.Z/Math.Max(1,length);
    p.X=Math.Max(-10,Math.Min(10,p.X+x*4*dt));p.Z=Math.Max(-10,Math.Min(10,p.Z+z*4*dt));
    if(c.Skill==1&&Time>=p.DashReady){p.X=Math.Max(-10,Math.Min(10,p.X+(length>0?x:0)*3));p.Z=Math.Max(-10,Math.Min(10,p.Z+(length>0?z:1)*3));p.DashReady=Time+2;p.SkillEvent++;}
    if(c.Skill==2&&Time>=p.PulseReady){foreach(var target in players.Values)if(target.Id!=p.Id&&(target.X-p.X)*(target.X-p.X)+(target.Z-p.Z)*(target.Z-p.Z)<=9)target.Health=Math.Max(0,target.Health-20);p.PulseReady=Time+3;p.SkillEvent++;}
   }
  }
 }
}

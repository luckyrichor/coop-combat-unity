using System;
using System.Threading;
using NUnit.Framework;
using CoopCombat;
public class CombatTests {
 [Test] public void NormalizedAndFloodLimited(){var model=new CombatModel();model.Add(0);model.Step(new[]{new InputCommand{Player=0,Sequence=1,X=1,Z=1},new InputCommand{Player=0,Sequence=2,X=1,Z=1}},.05f);var p=model.Snapshot()[0];Assert.That(Math.Sqrt(p.X*p.X+p.Z*p.Z),Is.EqualTo(.2).Within(.0001));}
 [Test] public void DashCooldownAndPulseJudgement(){var model=new CombatModel();model.Add(0);model.Add(1);model.Step(new[]{new InputCommand{Player=0,Sequence=1,Skill=2}},.05f);Assert.That(model.Snapshot()[1].Health,Is.EqualTo(80));model.Step(new[]{new InputCommand{Player=0,Sequence=2,Skill=2}},.05f);Assert.That(model.Snapshot()[1].Health,Is.EqualTo(80));model.Step(new[]{new InputCommand{Player=0,Sequence=3,Skill=1,Z=1}},.05f);Assert.That(model.Snapshot()[0].Z,Is.EqualTo(3.2).Within(.0001));}
 [Test] public void ReplayAndNonFiniteIgnored(){var model=new CombatModel();model.Add(0);model.Step(new[]{new InputCommand{Player=0,Sequence=2,X=1}},.05f);model.Step(new[]{new InputCommand{Player=0,Sequence=1,X=1},new InputCommand{Player=0,Sequence=3,X=float.NaN}},.05f);Assert.That(model.Snapshot()[0].X,Is.EqualTo(.2f).Within(.0001));}
 [Test] public void TwoUdpClientsReceiveSameAuthoritativeState(){using(var server=new LocalServer())using(var a=new LocalClient(server.Port))using(var b=new LocalClient(server.Port)){
  for(int tick=0;tick<10;tick++){a.Send(1,0,0);b.Send(0,1,0);Thread.Sleep(15);server.Pump(.05f);Thread.Sleep(15);a.Poll();b.Poll();}
  Assert.That(a.Tick,Is.EqualTo(b.Tick));Assert.That(a.States.Length,Is.EqualTo(2));Assert.That(a.Id,Is.Not.EqualTo(b.Id));for(int i=0;i<2;i++){Assert.That(a.States[i].X,Is.EqualTo(b.States[i].X));Assert.That(a.States[i].Z,Is.EqualTo(b.States[i].Z));Assert.That(a.States[i].Health,Is.EqualTo(b.States[i].Health));}
 }}
 [Test] public void ReturnedSnapshotIsCopy(){var model=new CombatModel();model.Add(0);model.Snapshot()[0].Health=0;Assert.That(model.Snapshot()[0].Health,Is.EqualTo(100));}
}

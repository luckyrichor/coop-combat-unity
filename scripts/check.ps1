param([switch]$SkipPlayer)
$ErrorActionPreference='Stop'
$root=Split-Path $PSScriptRoot -Parent
$editor='F:\Unity_Hub\2022.3.38f1c1\Editor\Unity.exe'
New-Item "$root\.local" -ItemType Directory -Force | Out-Null
function RunEditor([string]$labArguments) {
 $p=Start-Process -FilePath $editor -ArgumentList $labArguments -Wait -PassThru
 if($p.ExitCode -ne 0) {throw "Unity exited $($p.ExitCode)"}
}
foreach($platform in @('EditMode','PlayMode')) {
 RunEditor "-batchmode -nographics -projectPath $root\Game -runTests -testPlatform $platform -testResults $root\.local\final-$platform.xml -logFile $root\.local\final-$platform.log"
 [xml]$report=Get-Content "$root\.local\final-$platform.xml"
 if([int]$report.'test-run'.total -lt 1 -or [int]$report.'test-run'.failed -ne 0 -or $report.'test-run'.result -ne 'Passed') {throw "$platform did not pass"}
 Write-Output "$platform passed: $($report.'test-run'.passed)"
}
if($SkipPlayer) {exit 0}
RunEditor "-batchmode -nographics -quit -projectPath $root\Game -executeMethod PlayerBuild.Run -logFile $root\.local\player-build.log"
$exe="$root\.local\player\CoopCombat.exe"
$hostProcess=Start-Process $exe -ArgumentList "--probe --exit-seconds 9 -batchmode -nographics -logFile $root\.local\host-player.log" -PassThru
Start-Sleep -Seconds 2
$joinProcess=Start-Process $exe -ArgumentList "--join --probe --exit-seconds 5 -batchmode -nographics -logFile $root\.local\join-player.log" -PassThru
try {
 if(-not $hostProcess.WaitForExit(60000)) {throw 'Host timeout'}
 if(-not $joinProcess.WaitForExit(60000)) {throw 'Join timeout'}
 if($hostProcess.ExitCode -ne 0 -or $joinProcess.ExitCode -ne 0) {throw 'Player failed'}
 function States([string]$path) {
  $map=@{}
  foreach($line in [IO.File]::ReadAllLines($path)) {
   if($line -match 'COOP_STATE (\{.*\})') {$state=$Matches[1] | ConvertFrom-Json;if($state.players.Count -eq 2){$map[[string]$state.tick]=$state}}
  }
  return $map
 }
 $hostStates=States "$root\.local\host-player.log"; $joinStates=States "$root\.local\join-player.log"
 $shared=@($hostStates.Keys | Where-Object {$joinStates.ContainsKey($_)})
 if($shared.Count -lt 50) {throw 'Too few shared two-player ticks'}
 foreach($tick in $shared) {if(($hostStates[$tick]|ConvertTo-Json -Depth 5 -Compress) -ne ($joinStates[$tick]|ConvertTo-Json -Depth 5 -Compress)){throw "Mismatch at tick $tick"}}
 $summary=@{shared_two_player_ticks=$shared.Count;states_equal=$true;host_exit=$hostProcess.ExitCode;join_exit=$joinProcess.ExitCode;transport='two separate Windows players, loopback UDP';fields=@('position','health','skill event')}
 [IO.File]::WriteAllText("$root\.local\network-summary.json",($summary|ConvertTo-Json -Depth 5),(New-Object Text.UTF8Encoding($false)))
 Write-Output "Equal snapshots: $($shared.Count)"
} finally {foreach($p in @($hostProcess,$joinProcess)){if(-not $p.HasExited){$p.Kill()}}}
